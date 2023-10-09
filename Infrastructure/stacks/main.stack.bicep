//TODO create 'UserAssigned' service principal with necessary role assignments for automated deployment

//TODO Expose Function App endpoints through API Management instance

//NOTE Function app vnet integration only works with plans higher than consumptions...awful

param project string
param environment string
param location string

param hostingSkuTier string = 'Dynamic'
@description('Application settings object\nSee fnApp construct for user defined environment variable object\nimporting from another file was not working (feature is in preview)')
param appSettings object

@description('function app subnet name, should match a subnet in the vnet')
param fnAppSubnet string
@description('Name of existing vnet function app resides in')
param vnetName string

@description('resource group of existing vnet function app resides in')
param vnetRg string

@description('prefix used for each resource created\nRuns toLower() on department, environment, and location\n using a hyphen as a delimiter')
var prefix = '${toLower(project)}-${toLower(environment)}-${toLower(location)}'

module vnet '../constructs/vnet.bicep' = { 
  params:{ name: vnetName } 
  name: '${deployment().name}-vnetDeploy'
  scope: resourceGroup(vnetRg)
}

targetScope = 'subscription'

resource rg  'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: '${prefix}-rg'
  location: location
}

var appSubnet = first(filter(vnet.outputs.subnets, (subnet) => contains(subnet.name, fnAppSubnet))).id
// output sub string = subnetid
module fnApp '../constructs/fnapp.bicep' = {
  name: '${deployment().name}-fnAppDeploy'
  params: {
    prefix : prefix
    location: rg.location
    subnetId: appSubnet
    appSettings: appSettings
    hostingSkuTier: hostingSkuTier
  }
  scope: rg
}

/*
Vnet integration is only allowed in higher tier hosting plans, none of which are truly serverless
Poor implementation by microsoft unfortunately
*/
module integ '../constructs/integration.bicep' = if (hostingSkuTier != 'Dynamic'){
  name: '${deployment().name}-appVnetIntegrationDeploy'
  params: {
    prefix: fnApp.outputs.appName
    vnetId: vnet.outputs.self.id
  }
  scope: rg
  dependsOn: [
    fnApp
  ]
}

/* API Management

web subnet 
Expose function app via api management
Add Cors
*/

