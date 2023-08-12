$subscription = "Marketing"
$resourceGroup = "rg-adonamingtool-tst-001"
$location = "canadacentral"
az login
az account set -s $subscription
az group create -n $resourceGroup --location $location
az deployment group create --resource-group $resourceGroup --name rollout01 `
    --template-file main.bicep