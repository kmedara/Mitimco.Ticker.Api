//Managed IDentities only

/*
Function App

app subnet
private subnet 
private nsg

secure storage account with service endpoint
param department string = 'Engineering'
param environment string = 'dev'
param location string = 'eastus'
//param appvnet string = 'engineering-dev-east'

param vnetGroup = 'Engineer'
*/
// param appSettings AppSettings

param division string
param environment string
param location string

param subscriptionId string

@description('Application settings object\nSee fnApp construct for user defined environment variable object\nimporting from another file was not working (feature is in preview)')
param appSettings object

@description('function app subnet name, should match a subnet in the vnet')
param fnAppSubnet string
@description('Name of existing vnet function app resides in')
param vnetName string

@description('resource group of existing vnet function app resides in')
param vnetRg string

@description('prefix used for each resource created\nRuns toLower() on department, environment, and location\n using a hyphen as a delimiter')
var prefix = '${toLower(division)}-${toLower(environment)}-${toLower(location)}'

module vnet '../constructs/vnet.bicep' = { 
  params:{ name: vnetName } 
  name: '${deployment().name}-vnetDeploy'
  scope: resourceGroup(vnetRg)
}


module stackRg '../constructs/rg.bicep' = {
  name: '${deployment().name}-group'
  params: {
    name: prefix
    subscriptionId: subscriptionId
    location: location
  }
  scope: subscription(subscriptionId)
}

module fnApp '../constructs/fnapp.bicep' = {
  name: '${deployment().name}-fnAppDeploy'
  params: {
    name : prefix
    location: stackRg.outputs.location
    subnetId: first(filter(vnet.outputs.subnets, (subnet) => contains(subnet.name, fnAppSubnet) )).name
    appSettings: appSettings
  }
  scope: resourceGroup(stackRg.name)
}

/* API Management

web subnet 
Expose function app via api management
Add Cors
*/

