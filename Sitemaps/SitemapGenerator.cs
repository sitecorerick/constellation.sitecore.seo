namespace Spark.Sitecore.Seo.Sitemaps
{
	using System;
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Reflection;
	using System.Web;
	using System.Xml;

	using global::Sitecore.Data.Items;
	using global::Sitecore.Sites;

	/// <summary>
	/// Handles the creation of a sitemap.xml document based upon the current request parameters.
	/// </summary>
	public static class SitemapGenerator
	{
		#region Fields
		/// <summary>
		/// Internal (application lifetime) type to use for SitemapNode.
		/// </summary>
		private static Type runtimeType;
		#endregion

		/// <summary>
		/// Creates the sitemap.xml document, ready for streaming to the response.
		/// </summary>
		/// <param name="request">
		/// The .NET HttpRequest object.
		/// </param>
		/// <returns>
		/// A sitemap.xml document as an XmlDocument object.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "The objective is to create exactly this object, which includes streamwriting functionality.")]
		public static XmlDocument Generate(HttpRequest request)
		{
			return Generate(request, true);
		}

		/// <summary>
		/// Creates the sitemap.xml document, ready for streaming to the response.
		/// </summary>
		/// <param name="request">
		/// The .NET HttpRequest object.
		/// </param>
		/// <param name="useCachedCopyIfAvailable">
		/// Use the cached version of the output?
		/// </param>
		/// <returns>
		/// A sitemap.xml document as an XmlDocument object.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "The objective is to create exactly this object, which includes streamwriting functionality.")]
		public static XmlDocument Generate(HttpRequest request, bool useCachedCopyIfAvailable)
		{
			// Every Site in the installation will get its own sitemap.xml document.
			var key = request.Url.GetLeftPart(UriPartial.Path);

			var doc = HttpRuntime.Cache.Get(key) as XmlDocument;
			if (doc == null || !useCachedCopyIfAvailable)
			{
				doc = InitializeDocument();
				var site = SiteContextFactory.GetSiteContext(request.Url.Host, request.Url.LocalPath, request.Url.Port);

				// Regardless of whether we find a site, we will return a document, it will be empty if Sitecore can't figure out what host to map to.
				if (site != null)
				{
					// start recursing through the site's items
					var usethiscrawler = Type.GetType(Configuration.Settings.Crawler);

					if (usethiscrawler == null)
					{
						throw new ConfigurationErrorsException("web.config /configuration/spark/sitemapGenerator @crawler must contain a valid class identifier.");
					}

					var mycrawler = Activator.CreateInstance(usethiscrawler) as ICrawler;

					if (mycrawler == null)
					{
						throw new ConfigurationErrorsException("web.config /configuration/spark/sitemapGenerator @crawler must contain a valid class identifier.");
					}

					mycrawler.Crawl(site, doc);
				}

				/* 
				 * Because crawlers are fairly arbitrary and the document provides internal recommendations for freshness,
				 * we can use a simpler absolute timeout rather than slaving to Sitecore Publishing. I selected 45min because
				 * it is under the first time-based change frequency (hourly) reported by the document.
				 */
				int cachetime = int.Parse(Configuration.Settings.CacheTimeout);
				HttpRuntime.Cache.Insert(key, doc, null, DateTime.Now.AddMinutes(cachetime), System.Web.Caching.Cache.NoSlidingExpiration);
			}

			return doc;
		}

		#region Data Retrieval

		/// <summary>
		/// Creates an instance of ISitemapNode from the Type defined in the Sitemap Configuration Section
		/// of the config file.
		/// </summary>
		/// <param name="item">The Sitecore Item to use when initializing the ISitemapNode.</param>
		/// <returns>An instance of ISitemapNode based on the supplied Item and the node Type specified in the config file.</returns>
		public static ISitemapNode CreateNode(Item item)
		{
			EnsureRuntimeTypeIsSet();

			var args = new object[] { item };

			var node = Activator.CreateInstance(runtimeType, args);

			return node as ISitemapNode;
		}

		/// <summary>
		/// Creates an instance of ISitemapNode from the Type defined in the Sitemap Configuration Section
		/// of the config file.
		/// </summary>
		/// <param name="item">
		/// The Sitecore Item to use when initializing the ISitemapNode.
		/// </param>
		/// <param name="siteContext">
		/// The site context.
		/// </param>
		/// <returns>
		/// An instance of ISitemapNode based on the supplied Item and the node Type specified in the config file.
		/// </returns>
		public static ISitemapNode CreateNode(Item item, SiteContext siteContext)
		{
			EnsureRuntimeTypeIsSet();

			var args = new object[] { item, siteContext };

			var node = Activator.CreateInstance(runtimeType, args);

			return node as ISitemapNode;
		}

		/// <summary>
		/// Creates a sitemap.xml fragment and appends it to the sitemap.xml document in progress.
		/// </summary>
		/// <param name="doc">The sitemap.xml document in progress.</param>
		/// <param name="node">The node to process.</param>
		public static void AppendUrlElement(XmlDocument doc, ISitemapNode node)
		{
			var url = doc.CreateElement("url");

			var locElement = doc.CreateElement("loc");
			locElement.InnerText = node.Location;

			var lastModElement = doc.CreateElement("lastmod");
			lastModElement.InnerText = node.LastModified.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

			var changeFreqElement = doc.CreateElement("changefreq");
			changeFreqElement.InnerText = node.ChangeFrequency.ToString().ToLower(CultureInfo.InvariantCulture);

			var priorityElement = doc.CreateElement("priority");
			priorityElement.InnerText = node.Priority.ToString(CultureInfo.InvariantCulture);

			url.AppendChild(locElement);
			url.AppendChild(lastModElement);
			url.AppendChild(changeFreqElement);
			url.AppendChild(priorityElement);

			// ReSharper disable PossibleNullReferenceException
			doc.DocumentElement.AppendChild(url);
			// ReSharper restore PossibleNullReferenceException
		}

		/// <summary>
		/// Retrieves the type of ISitemapNode to use for this application from the config file. If the
		/// config file does not contain a type reference, the DefaultSitemapNode type will be used.
		/// </summary>
		private static void EnsureRuntimeTypeIsSet()
		{
			if (runtimeType == null)
			{
				var type = Configuration.Settings.SitemapNodeType;

				try
				{
					// ReSharper disable AccessToStaticMemberViaDerivedType
					runtimeType = TypeDelegator.GetType(type);
					// ReSharper restore AccessToStaticMemberViaDerivedType
				}
				catch (BadImageFormatException ex)
				{
					/* 
					 * The assembly or one of its dependencies is not valid.
					 * 
					 * or
					 * 
					 * the common language runtime is currently loaded, but the
					 * assembly was compiled with a later version.
					 */
					global::Sitecore.Diagnostics.Log.Error("SitemapNodeType was invalid: " + ex.Message, typeof(SitemapGenerator));
					runtimeType = typeof(DefaultSitemapNode);
				}
			}
		}
		#endregion

		#region Xml Management
		/// <summary>
		/// Creates a new instance of XmlDocument with the appropriate declaration and root node.
		/// </summary>
		/// <returns>A new empty sitemap.xml document.</returns>
		private static XmlDocument InitializeDocument()
		{
			var doc = new XmlDocument();
			var declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
			doc.AppendChild(declaration);

			var urlset = doc.CreateElement("urlset");
			doc.AppendChild(urlset);

			var xmlns = doc.CreateAttribute("xmlns");
			xmlns.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
			urlset.Attributes.Append(xmlns);

			return doc;
		}
		#endregion
	}
}
