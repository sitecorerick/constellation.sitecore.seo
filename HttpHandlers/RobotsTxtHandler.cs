namespace Constellation.Sitecore.HttpHandlers
{
	using System.Text;
	using System.Web;

	/// <summary>
	/// Handles requests for robots.txt and response with a reference to the hostname-specific
	/// sitemap.xml file.
	/// </summary>
	public class RobotsTxtHandler : IHttpHandler
	{
		#region IHttpHandler Members
		/// <summary>
		/// Gets a value indicating whether this instance of the handler is reusable.
		/// Required by IHttpHandler, not used by developers in this case.
		/// </summary>
		public bool IsReusable
		{
			get { return false; }
		}

		/// <summary>
		/// Required by IHttpHandler, this is called by ASP.NET when an appropriate URL match is found.
		/// </summary>
		/// <param name="context">The current request context.</param>
		public void ProcessRequest(HttpContext context)
		{
			var builder = new StringBuilder();

			var rules = RobotsTxtHandlerConfiguration.Settings.Rules;

			for (int i = 0; i < rules.Count; i++)
			{
				var rule = rules[i];
				builder.AppendLine("UserAgent: " + rule.AgentName);

				if (!rule.Allowed)
				{
					builder.AppendLine("Disallow: /");
				}

			}

			builder.AppendLine("User-agent: *");

			if (!RobotsTxtHandlerConfiguration.Settings.Allowed)
			{
				builder.AppendLine("Disallow: /");
			}

			builder.AppendLine();
			builder.AppendLine("Sitemap: " + context.Request.Url.GetLeftPart(System.UriPartial.Authority) + "/sitemap.xml");

			context.Response.Clear();
			context.Response.ContentType = "text";
			context.Response.Write(builder.ToString());
			context.Response.End();
		}
		#endregion
	}
}
