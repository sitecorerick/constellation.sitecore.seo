namespace Constellation.Sitecore.HttpHandlers.SitemapXml
{
	using System.Configuration;

	/// <summary>
	/// Specifies an app config section specific to the Sitemap Generator.
	/// </summary>
	public class Configuration : ConfigurationSection
	{
		/// <summary>
		/// Internal instance of the configuration section to simplify access to its properties.
		/// </summary>
		private static Configuration config;

		/// <summary>
		/// Gets the configuration settings for the Sitemap Generator.
		/// </summary>
		public static Configuration Settings
		{
			get
			{
				return config ?? (config = ConfigurationManager.GetSection("constellation/sitemapXmlHandler") as Configuration);
			}
		}

		/// <summary>
		/// Gets or sets the type to use when creating instances of ISitemapNode as part of a sitemap.xml generation process.
		/// </summary>
		[ConfigurationProperty("sitemapNodeType", DefaultValue = "Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultSitemapNode, Constellation.Sitecore.Seo", IsRequired = false)]
		public string SitemapNodeType
		{
			get { return (string)this["sitemapNodeType"]; }
			set { this["sitemapNodeType"] = value; }
		}

		/// <summary>
		/// Gets or sets the crawler.
		/// </summary>
		[ConfigurationProperty("crawler", DefaultValue = "Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultCrawler, Constellation.Sitecore.Seo", IsRequired = false)]
		public string Crawler
		{
			get { return (string)this["crawler"]; }
			set { this["crawler"] = value; }
		}

		/// <summary>
		/// Gets or sets the cache timeout in minutes.
		/// </summary>
		[ConfigurationProperty("cacheTimeout", DefaultValue = "45", IsRequired = false)]
		public string CacheTimeout
		{
			get { return (string)this["cacheTimeout"]; }
			set { this["cacheTimeout"] = value; }
		}
	}
}
