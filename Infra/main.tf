# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
  subscription_id = "452059b4-1dbe-460c-9ef6-85738d616b22"
}

# Generate a random integer to create a globally unique name
resource "random_integer" "ri" {
  min = 100
  max = 999
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "upt-arg-${random_integer.ri.result}"
  location = "eastus"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "upt-asp-${random_integer.ri.result}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "webapp" {
  name                  = "upt-awa-${random_integer.ri.result}"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  depends_on            = [azurerm_service_plan.appserviceplan]
  //https_only            = true
  site_config {
    minimum_tls_version = "1.2"
    always_on = false
    application_stack {
      dotnet_version = "8.0"
    }
  }
}

# Recurso para configurar el control de código fuente en Azure App Service
resource "azurerm_app_service_source_control" "app_service_source_control" {
  app_id = azurerm_app_service.app_service.id  # ID de la App Service
  repo_url = "https://github.com/UPT-FAING-EPIS/lab-2024-rec-si982-u1-01-edwardapaza"  # URL del repositorio
  branch = "main"  # Rama a desplegar
  use_manual_integration = false  # Integración automática
  use_mercurial = false  # No usar Mercurial (Git es el predeterminado)
}