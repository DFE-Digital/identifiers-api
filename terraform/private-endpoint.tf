resource "azurerm_subnet" "private_endpoint" {
  for_each = local.private_endpoint_configurations

  name                                      = lower("${local.resource_prefix}-${each.value.subresource_name}privateendpoint")
  virtual_network_name                      = "${local.resource_prefix}default"
  resource_group_name                       = local.resource_prefix
  address_prefixes                          = [each.value["subnet_cidr"]]
  private_endpoint_network_policies_enabled = false
}

resource "azurerm_subnet_route_table_association" "private_endpoint" {
  for_each = {
    for k, v in local.private_endpoint_configurations : k => v if v["subnet_route_table_name"] != null
  }

  subnet_id      = azurerm_subnet.private_endpoint[each.key].id
  route_table_id = data.azurerm_route_table.private_endpoints[each.key].id
}

resource "azurerm_private_endpoint" "default" {
  for_each = local.private_endpoint_configurations

  name                = "${each.value.vnet_resource_group_name}.${azurerm_subnet.private_endpoint[each.key].name}"
  location            = data.azurerm_virtual_network.private_endpoints[each.key].location
  resource_group_name = each.value.vnet_resource_group_name
  subnet_id           = azurerm_subnet.private_endpoint[each.key].id

  custom_network_interface_name = "${local.resource_prefix}${each.key}-nic"

  private_service_connection {
    name                           = "${local.resource_prefix}${each.key}"
    private_connection_resource_id = each.value.target_resource_id
    subresource_names              = [each.value.subresource_name]
    is_manual_connection           = false
  }

  private_dns_zone_group {
    name                 = "${local.resource_prefix}${each.key}-${each.value.subresource_name}-private-link"
    private_dns_zone_ids = [azurerm_private_dns_zone.private_link[each.key].id]
  }

  tags = local.tags
}

resource "azurerm_private_dns_a_record" "private_link" {
  for_each = {
    for k, v in local.private_endpoint_configurations : k => v if v["create_private_dns_zone"]
  }

  name                = "@"
  zone_name           = azurerm_private_dns_zone.private_link[each.key].name
  resource_group_name = local.resource_prefix
  ttl                 = 300
  records             = [azurerm_private_endpoint.default[each.key].private_service_connection[0].private_ip_address]
  tags                = local.tags
}

resource "azurerm_private_dns_zone" "private_link" {
  for_each = {
    for k, v in local.private_endpoint_configurations : k => v if v["create_private_dns_zone"]
  }

  name                = each.value.private_dns_hostname
  resource_group_name = local.resource_prefix
  tags                = local.tags
}

resource "azurerm_private_dns_zone_virtual_network_link" "private_link" {
  for_each = local.private_endpoint_configurations

  name                  = "${local.resource_prefix}-${each.value.subresource_name}privatelink"
  resource_group_name   = local.resource_prefix
  private_dns_zone_name = each.value["create_private_dns_zone"] ? azurerm_private_dns_zone.private_link[each.key].name : each.value.private_dns_hostname
  virtual_network_id    = module.azure_container_apps_hosting.networking.vnet_id
  tags                  = local.tags
}
