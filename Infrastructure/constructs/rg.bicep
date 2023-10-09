@description('name of resource\n gets appended with -rg')
param name string

@description('Id of subscription to deploy group to')
param subscriptionId string

param location string

targetScope = 'subscription'

resource rg  'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: '${name}-rg'
  location: location
}

output location string = rg.location
