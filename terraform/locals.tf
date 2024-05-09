locals {
  environment                                  = var.environment
  project_name                                 = var.project_name
  azure_location                               = var.azure_location
  tags                                         = var.tags
  virtual_network_address_space                = var.virtual_network_address_space
  enable_container_registry                    = var.enable_container_registry
  registry_admin_enabled                       = var.registry_admin_enabled
  registry_use_managed_identity                = var.registry_use_managed_identity
  registry_managed_identity_assign_role        = var.registry_managed_identity_assign_role
  image_name                                   = var.image_name
  image_tag                                    = var.image_tag
  container_command                            = var.container_command
  container_secret_environment_variables       = var.container_secret_environment_variables
  container_scale_http_concurrency             = var.container_scale_http_concurrency
  container_health_probe_protocol              = var.container_health_probe_protocol
  enable_dns_zone                              = var.enable_dns_zone
  dns_zone_domain_name                         = var.dns_zone_domain_name
  dns_ns_records                               = var.dns_ns_records
  dns_mx_records                               = var.dns_mx_records
  dns_txt_records                              = var.dns_txt_records
  enable_cdn_frontdoor                         = var.enable_cdn_frontdoor
  container_apps_allow_ips_inbound             = var.container_apps_allow_ips_inbound
  cdn_frontdoor_enable_rate_limiting           = var.cdn_frontdoor_enable_rate_limiting
  cdn_frontdoor_host_add_response_headers      = var.cdn_frontdoor_host_add_response_headers
  cdn_frontdoor_custom_domains                 = var.cdn_frontdoor_custom_domains
  cdn_frontdoor_origin_fqdn_override           = var.cdn_frontdoor_origin_fqdn_override
  enable_cdn_frontdoor_health_probe            = var.enable_cdn_frontdoor_health_probe
  cdn_frontdoor_origin_host_header_override    = var.cdn_frontdoor_origin_host_header_override
  cdn_frontdoor_forwarding_protocol            = var.cdn_frontdoor_forwarding_protocol
  key_vault_access_ipv4                        = var.key_vault_access_ipv4
  tfvars_filename                              = var.tfvars_filename
  enable_monitoring                            = var.enable_monitoring
  monitor_email_receivers                      = var.monitor_email_receivers
  enable_container_health_probe                = var.enable_container_health_probe
  container_health_probe_path                  = var.container_health_probe_path
  cdn_frontdoor_health_probe_path              = var.cdn_frontdoor_health_probe_path
  cdn_frontdoor_health_probe_protocol          = var.cdn_frontdoor_health_probe_protocol
  monitor_endpoint_healthcheck                 = var.monitor_endpoint_healthcheck
  existing_logic_app_workflow                  = var.existing_logic_app_workflow
  existing_network_watcher_name                = var.existing_network_watcher_name
  existing_network_watcher_resource_group_name = var.existing_network_watcher_resource_group_name
  statuscake_monitored_resource_addresses      = var.statuscake_monitored_resource_addresses
  statuscake_contact_group_name                = var.statuscake_contact_group_name
  statuscake_contact_group_integrations        = var.statuscake_contact_group_integrations
  statuscake_contact_group_email_addresses     = var.statuscake_contact_group_email_addresses

  resource_prefix                 = "${local.environment}${local.project_name}"
  private_endpoint_configurations = var.private_endpoint_configurations
}
