locals {
  location            = "West Europe"
  resource_group_name = "HomeEconomics"
  prefix              = "hm"
}

provider "azurerm" {
  version         = "=1.33.0"
  subscription_id = "${var.subscriptionId}"
}

provider "random" {
  version = "~> 2.2"
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

output "ConnectionStrings_HomeEconomics" {
  value = "Server=tcp:${azurerm_sql_server.sqlServer.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.sqlDatabase.name};Persist Security Info=False;User ID=${azurerm_sql_server.sqlServer.administrator_login};Password=${azurerm_sql_server.sqlServer.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}

output "ApplicationInsights_InstrumentationKey" {
  value = "${azurerm_application_insights.applicationInsights.instrumentation_key}"
}
