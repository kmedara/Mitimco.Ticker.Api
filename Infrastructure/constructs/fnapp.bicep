/*
User Defined Type, import from another file doesn't work yet (in preview)
*/
type AppSettings = {
  ApiConfigurationOptions__AlphaApiConfigurationOptions__BaseUrl: string
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Value: string
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Identifier : string
  // }
  // {
  //   name: 'ApiConfigurationOptions__AlphaApiConfigurationOptions__BaseUrl'
  //   value: 'https://www.alphavantage.co'
  // }
  // {
  //   name: 'ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Value'
  //   value: '9DW5RE6WRK0N4D63'
  // }
  // {
  //   name: 'ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Identifier'
  //   value: 'apikey'
  // }
}

@description('Application Settings (Environment Variables)')
param appSettings AppSettings

@description('The name of the function app that you wish to create.')
param name string

@description('Storage Account type')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'

@description('Location for all resources.')
param location string = resourceGroup().location

 @description('')
param subnetId string

@description('The language worker runtime to load in the function app.')
@allowed([
  'node'
  'dotnet'
  'java'
])
param runtime string = 'node'

var functionAppName = name
var hostingPlanName = name

@description('all hyphens are removed and name is chopped to 20 chars if needed\n appended with')
var storageAccountName = substring(replace(name, '-', ''), 0, 24)
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
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}
var environmentVariables = [for item in items(appSettings): {
          name: item.key
          value: item.value
        }]

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    virtualNetworkSubnetId: subnetId
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
