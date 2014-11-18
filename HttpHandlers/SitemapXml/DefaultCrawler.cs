namespace Constellation.Sitecore.HttpHandlers.SitemapXml
{
	using global::Sitecore.Sites;
	using System.Text;
	using System.Xml;

	/// <summary>
	/// Default Crawler Class for sitemaps
	/// </summary>
	public class DefaultCrawler : ICrawler
	{
		/// <summary>
		/// The crawl.
		/// </summary>
		/// <param name="site">
		/// The site.
		/// </param>
		/// <param name="doc">
		/// The doc.
		/// </param>
		public void Crawl(SiteContext site, XmlDocument doc)
		{
			/*
			 *  We're going to crawl the site layer-by-layer which will put the upper levels
			 *  of the site nearer the top of the sitemap.xml document as opposed to crawling
			 *  the tree by parent/child relationships, which will go deep on each branch before
			 *  crawling the entire site.
			 */
			var max = global::Sitecore.Configuration.Settings.MaxTreeDepth;

			var query = new StringBuilder("fast:" + site.StartPath);

			for (var i = 0; i < max; i++)
			{
				var items = site.Database.SelectItems(DatasourceResolver.EncodeQuery(query.ToString()));

				if (items != null)
				{
					foreach (var item in items)
					{
						var node = SitemapGenerator.CreateNode(item, site);

						if (node.IsPage && node.IsListedInNavigation && node.ShouldIndex)
						{
							SitemapGenerator.AppendUrlElement(doc, node);
						}
					}
				}

				query.Append("/*");
			}
		}
	}
}
