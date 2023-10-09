param prefix string
param vnetId string


resource appVnetIntegration 'Microsoft.Web/sites/virtualNetworkConnections@2022-09-01' = {
  name: '${prefix}/virtualNetworkConnections'
  properties: {
    vnetResourceId: vnetId
    
  }
}
