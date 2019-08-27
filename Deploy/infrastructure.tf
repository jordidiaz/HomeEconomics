terraform {
  backend "azure" {

  }
}

locals {
  location            = "West Europe"
  resource_group_name = "HomeEconomics"
  prefix              = "hm"
}

provider "azurerm" {
  version         = "=1.33.0"
  subscription_id = "${var.subscriptionId}"
}

resource "random_string" "randomString" {
  length  = 8
  special = false
  upper   = false
}

resource "azurerm_resource_group" "resourceGroup" {
  name     = "${local.resource_group_name}"
  location = "${local.location}"
}

resource "azurerm_sql_server" "sqlServer" {
  name                         = "${local.prefix}-sqlserver-${random_string.randomString.result}"
  resource_group_name          = "${azurerm_resource_group.resourceGroup.name}"
  location                     = "${local.location}"
  version                      = "12.0"
  administrator_login          = "${var.sqlServerAdministratorLogin}"
  administrator_login_password = "${var.sqlServerAdministratorPassword}"
}

resource "azurerm_sql_database" "sqlDatabase" {
  name                = "${local.prefix}-sqldatabase"
  resource_group_name = "${azurerm_resource_group.resourceGroup.name}"
  location            = "${local.location}"
  server_name         = "${azurerm_sql_server.sqlServer.name}"
  edition             = "Basic"
}

resource "azurerm_application_insights" "applicationInsights" {
  name                = "${local.prefix}-appinsights"
  location            = "${local.location}"
  resource_group_name = "${azurerm_resource_group.resourceGroup.name}"
  application_type    = "web"
}
