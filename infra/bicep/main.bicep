param serverfarms_plan_adonamingtool_name string = 'plan-adonamingtool-${uniqueString(resourceGroup().id)}'
param sites_devopsabcs_adonamingtool_name string = 'app-adonamingtool-${uniqueString(resourceGroup().id)}'
param storageAccounts_stadonamingtool_name string = 'stadonaming${uniqueString(resourceGroup().id)}'
param registries_devopsabcsadonamingtool_name string = 'cradonamingtool${uniqueString(resourceGroup().id)}'

param location string = resourceGroup().location

var filesharename = 'adonamingtooldata'

var imagereponame = 'devopsabcsadonamingtool/azuredevopsnamingtool'
var imagetag = 'latest'

resource registries_devopsabcsadonamingtool_name_resource 'Microsoft.ContainerRegistry/registries@2023-06-01-preview' = {
  name: registries_devopsabcsadonamingtool_name
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    policies: {
      quarantinePolicy: {
        status: 'disabled'
      }
      trustPolicy: {
        type: 'Notary'
        status: 'disabled'
      }
      retentionPolicy: {
        days: 7
        status: 'disabled'
      }
      exportPolicy: {
        status: 'enabled'
      }
      azureADAuthenticationAsArmPolicy: {
        status: 'enabled'
      }
      softDeletePolicy: {
        retentionDays: 7
        status: 'disabled'
      }
    }
    encryption: {
      status: 'disabled'
    }
    dataEndpointEnabled: false
    publicNetworkAccess: 'Enabled'
    networkRuleBypassOptions: 'AzureServices'
    zoneRedundancy: 'Disabled'
    anonymousPullEnabled: false
  }
}

resource storageAccounts_stadonamingtool_name_resource 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccounts_stadonamingtool_name
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    dnsEndpointType: 'Standard'
    defaultToOAuthAuthentication: false
    publicNetworkAccess: 'Enabled'
    allowCrossTenantReplication: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: true
    allowSharedKeyAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      requireInfrastructureEncryption: false
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }

  resource Microsoft_Storage_storageAccounts_fileServices_storageAccounts_stadonamingtool_name_default 'fileServices@2023-01-01' = {
    name: 'default'
    properties: {
      shareDeleteRetentionPolicy: {
        enabled: true
        days: 7
      }
    }

    resource storageAccounts_stadonamingtool_name_default_adonamingtooldata 'shares@2023-01-01' = {
      name: filesharename
      properties: {
        accessTier: 'TransactionOptimized'
        shareQuota: 5120
        enabledProtocols: 'SMB'
      }
    }
  }
}

resource serverfarms_plan_adonamingtool_name_resource 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: serverfarms_plan_adonamingtool_name
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: true
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

resource sites_devopsabcs_adonamingtool_name_resource 'Microsoft.Web/sites@2022-09-01' = {
  name: sites_devopsabcs_adonamingtool_name
  location: location
  kind: 'app,linux,container'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${sites_devopsabcs_adonamingtool_name}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${sites_devopsabcs_adonamingtool_name}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_plan_adonamingtool_name_resource.id
    reserved: true
    isXenon: false
    hyperV: false
    vnetRouteAllEnabled: false
    vnetImagePullEnabled: false
    vnetContentShareEnabled: false
    siteConfig: {
      numberOfWorkers: 1
      linuxFxVersion: 'DOCKER|${registries_devopsabcsadonamingtool_name}.azurecr.io/${imagereponame}:${imagetag}'
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_devopsabcs_adonamingtool_name_web 'Microsoft.Web/sites/config@2022-09-01' = {
  parent: sites_devopsabcs_adonamingtool_name_resource
  name: 'web'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
      'hostingstart.html'
    ]
    netFrameworkVersion: 'v4.0'
    linuxFxVersion: 'DOCKER|${registries_devopsabcsadonamingtool_name}.azurecr.io/${imagereponame}:${imagetag}'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$${sites_devopsabcs_adonamingtool_name}'
    scmType: 'GitHubAction'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    publicNetworkAccess: 'Enabled'
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    elasticWebAppScaleLimit: 0
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 0
    azureStorageAccounts: {
      adonamingtooldata: {
        type: 'AzureFiles'
        accountName: storageAccounts_stadonamingtool_name
        shareName: filesharename
        mountPath: '/app/settings'
        accessKey: storageAccounts_stadonamingtool_name_resource.listKeys().keys[0].value
      }
    }
  }
}
