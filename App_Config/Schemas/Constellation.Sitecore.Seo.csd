<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="9c27deec-1acb-4923-9c13-3b9e4632451f" namespace="Constellation.Sitecore" xmlSchemaNamespace="urn:Constellation.Sitecore" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <externalType name="Type" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSectionGroup name="Constellation">
      <configurationSectionProperties>
        <configurationSectionProperty>
          <containedConfigurationSection>
            <configurationSectionMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/SitemapXmlHandlerConfiguration" />
          </containedConfigurationSection>
        </configurationSectionProperty>
        <configurationSectionProperty>
          <containedConfigurationSection>
            <configurationSectionMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/RobotsTxtHandlerConfiguration" />
          </containedConfigurationSection>
        </configurationSectionProperty>
      </configurationSectionProperties>
    </configurationSectionGroup>
    <configurationSection name="SitemapXmlHandlerConfiguration" namespace="Constellation.Sitecore.Seo" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="sitemapXmlHandler">
      <attributeProperties>
        <attributeProperty name="SitemapNodeType" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="sitemapNodeType" isReadOnly="false" typeConverter="TypeNameConverter" defaultValue="&quot;Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultSitemapNode, Constellation.Sitecore.Seo&quot;">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="CrawlerType" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="crawlerType" isReadOnly="false" typeConverter="TypeNameConverter" defaultValue="&quot;Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultCrawler, Constellation.Sitecore.Seo&quot;">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/Type" />
          </type>
        </attributeProperty>
        <attributeProperty name="CacheTimeoutMinutes" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="cacheTimeoutMinutes" isReadOnly="false" defaultValue="30">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationSection>
    <configurationSection name="RobotsTxtHandlerConfiguration" namespace="Constellation.Sitecore.Seo" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="robotsTxtHandler">
      <attributeProperties>
        <attributeProperty name="Allowed" isRequired="false" isKey="false" isDefaultCollection="true" xmlName="allowed" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="RobotRules" isRequired="false" isKey="false" isDefaultCollection="true" xmlName="robotRules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/RobotRules" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="RobotRules" collectionType="AddRemoveClearMapAlternate" xmlItemName="robotRule" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/RobotRule" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="RobotRule">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Allowed" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="allowed" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/9c27deec-1acb-4923-9c13-3b9e4632451f/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>