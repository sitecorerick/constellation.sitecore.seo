namespace Constellation.Sitecore.HttpHandlers
{
	using System;
	using System.Configuration;

	/// <summary>
	/// The sitemap xml handler configuration.
	/// </summary>
	public class SitemapXmlHandlerConfiguration : ConfigurationSection
	{
		/* <sitemapXmlHandler
		 *		crawlerType="Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultCrawler, Constellation.Sitecore.Seo"
		 *		sitemapNodeType="Constellation.Sitecore.HttpHandlers.SitemapXml.DefaultSitemapNode, Constellation.Sitecore.Seo"
		 *		cacheTimeoutMinutes="45" />
		 */

		/// <summary>
		/// The settings.
		/// </summary>
		private static SitemapXmlHandlerConfiguration settings;

		/// <summary>
		/// Gets the settings.
		/// </summary>
		public static SitemapXmlHandlerConfiguration Settings
		{
			get
			{
				if (settings == null)
				{
					var config = ConfigurationManager.GetSection("sitemapXmlHandler");
					settings = config as SitemapXmlHandlerConfiguration;
				}

				return settings;
			}
		}


		/// <summary>
		/// Gets the crawler type.
		/// </summary>
		[ConfigurationProperty("crawlerType", IsRequired = true)]
		public Type CrawlerType
		{
			get
			{
				var type = (string)base["crawlerType"];
				return Type.GetType(type);
			}
		}

		/// <summary>
		/// Gets the sitemap node type.
		/// </summary>
		[ConfigurationProperty("sitemapNodeType", IsRequired = true)]
		public Type SitemapNodeType
		{
			get
			{
				var type = (string)base["sitemapNodeType"];
				return Type.GetType(type);
			}
		}

		/// <summary>
		/// Gets the cache timeout minutes.
		/// </summary>
		[ConfigurationProperty("cacheTimeoutMinutes", DefaultValue = "45", IsRequired = true)]
		public int CacheTimeoutMinutes
		{
			get
			{
				var minutes = (string)base["cacheTimeoutMinutes"];
				int value;

				if (int.TryParse(minutes, out value))
				{
					return value;
				}

				return 45;
			}
		}
	}
}
