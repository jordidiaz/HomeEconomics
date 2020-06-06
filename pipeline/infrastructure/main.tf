terraform {
  backend "azurerm" {
    resource_group_name  = "Shared"
    storage_account_name = "stterraform"
    container_name       = "homeeconomics"
    key                  = "tf/terraform.tfstate"
  }
}
