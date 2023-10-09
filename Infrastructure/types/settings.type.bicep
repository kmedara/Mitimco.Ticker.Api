//https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/bicep-config#create-the-config-file-in-visual-studio-code
//https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/bicep-config#enable-experimental-features
///Need latest azure cli to use this feature
//@export()
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
