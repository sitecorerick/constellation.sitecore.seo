namespace Constellation.Sitecore.Renderings
{
	using global::Sitecore.Web.UI;
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;
	using System.Web;

	/// <summary>
	/// Base class for renderings that turn local static file urls into absolute
	/// URLs to enhance the load time of static assets like CSS files.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public abstract class StaticFileReference : WebControl
	{
		/// <summary>
		/// return the hostname to use based upon the Sitecore context site definition.
		/// </summary>
		/// <returns>the hostname to use.</returns>
		protected string GetHostname()
		{
			return global::Sitecore.Context.Site.Properties["staticHostName"];
		}

		/// <summary>
		/// Gets the specified hostname for the installation and prefixes the supplied
		/// file source with it.
		/// </summary>
		/// <param name="path">The relative path.</param>
		/// <returns>An absolute URL, including scheme.</returns>
		protected string GetAbsoluteUrl(string path)
		{
			var hostName = this.GetHostname();

			if (string.IsNullOrEmpty(hostName))
			{
				return path;
			}

			if (string.IsNullOrEmpty(path))
			{
				return path;
			}

			var builder = new StringBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Scheme));
			builder.Append(hostName);

			if (!path.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
			{
				builder.Append("/");
			}

			builder.Append(path);

			return builder.ToString();
		}
	}
}
