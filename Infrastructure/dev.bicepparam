using './stacks/main.stack.bicep'

param project = 'ticker-api'
param environment = 'dev'
param location = 'eastus'
param vnetName = 'engineering-dev-east'
param vnetRg = 'EngineeringDevRG'
param fnAppSubnet = 'AppSub'

///These would be passed in via pipeline instead of in source
param appSettings = {
  ApiConfigurationOptions__AlphaApiConfigurationOptions__BaseUrl: 'https://www.alphavantage.co'
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Value: '9DW5RE6WRK0N4D63'
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Identifier : 'apikey'
}
