﻿<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <pipelines>
      <initialize>
        <processor type="$rootNamespace$.Pipelines.Initialize.ConfigureWebApi, $assemblyName$"
                   patch:after="processor[@type='Sitecore.Services.Infrastructure.Sitecore.Pipelines.ServicesWebApiInitializer, Sitecore.Services.Infrastructure.Sitecore']" />
      </initialize>
    </pipelines>
  </sitecore>
</configuration>
