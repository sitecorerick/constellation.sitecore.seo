namespace Spark.Sitecore.Seo.Sitemaps
{
	using System.Xml;

	using global::Sitecore.Sites;

	/// <summary>
	/// The base contract for an object that represents a crawler in a sitemap.xml document.
	/// The crawler object is used to build the sitemap
	/// </summary>
	public interface ICrawler
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
		void Crawl(SiteContext site, XmlDocument doc);
	}
}
