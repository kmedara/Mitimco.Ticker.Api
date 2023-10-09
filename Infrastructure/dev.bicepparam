using './stacks/main.stack.bicep'

param division = 'Engineering'
param environment = 'dev'
param location = 'eastus'
param subscriptionId = 'b01224bb-8e1b-4caa-b917-f5287f06c078'
param vnetName = 'engineering-dev-east'
param vnetRg = 'EngineeringDevRG'
param fnAppSubnet = 'AppSub'
param appSettings = {
  ApiConfigurationOptions__AlphaApiConfigurationOptions__BaseUrl: 'https://www.alphavantage.co'
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Value: '9DW5RE6WRK0N4D63'
  ApiConfigurationOptions__AlphaApiConfigurationOptions__ApiKey__Identifier : 'apikey'
}
