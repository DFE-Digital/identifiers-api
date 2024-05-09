data "azurerm_virtual_network" "private_endpoints" {
  for_each = local.private_endpoint_configurations

  name                = each.value["vnet_name"]
  resource_group_name = each.value["vnet_resource_group_name"]
}

data "azurerm_route_table" "private_endpoints" {
  for_each = {
    for k, v in local.private_endpoint_configurations : k => v if v["subnet_route_table_name"] != null
  }

  name                = each.value["subnet_route_table_name"]
  resource_group_name = each.value["vnet_resource_group_name"]
}
