/*
User Defined Type, import from another file doesn't work yet (in preview)
*/
type AppSettings = {
  ApiConfigurationOptions__AlphaApiConfigurationOptions__BaseUrl: string
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Value: string
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Identifier : string
}

@description('Application Settings (Environment Variables)')
param appSettings AppSettings

@description('resource prefix')
param prefix string

@description('Storage Account type')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'

param location string

 @description('subnet to allow access from (network injection is not allowed in consumption plans), however the storage account will still be set to only allow traffic from this subnet')
param subnetId string

@description('The language worker runtime to load in the function app.')
@allowed([
  'node'
  'dotnet'
  'java'
])
param runtime string = 'node'

param hostingSkuTier string = 'Dynamic'


var functionAppName = '${prefix}-fnapp'
var hostingPlanName = '${prefix}-plan'
var storageAccountName = '${take(replace(prefix, '-', ''), 21)}stg'
var functionWorkerRuntime = runtime

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'Storage'
  properties: {
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: true
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Deny'
      virtualNetworkRules: [ // enable the storage account service endpoint)
        {
          id: subnetId
          action: 'Allow'
        }
      ]
    }
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: hostingSkuTier
  }
  properties: {}
}

var environmentVariables = [for item in items(appSettings): {
          name: item.key
          value: item.value
        }]
output subnet string = subnetId
resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    virtualNetworkSubnetId: hostingPlan.sku.tier == 'Dynamic' ? null : subnetId
    siteConfig: {
      appSettings: flatten([
        environmentVariables
        [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION'
          value: '~14'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionWorkerRuntime
        }
      ]])
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

output appName string = functionApp.name
output hostingSku string = hostingPlan.sku.tier
