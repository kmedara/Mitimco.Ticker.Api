param name string 

@description('')
resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: name
}

output subnets array = vnet.properties.subnets
output location string = vnet.location
output self object = vnet
