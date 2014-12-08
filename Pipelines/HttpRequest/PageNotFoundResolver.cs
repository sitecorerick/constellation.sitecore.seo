namespace Constellation.Sitecore.Pipelines.HttpRequest
{
	using global::Sitecore;
	using global::Sitecore.Data.Items;
	using global::Sitecore.Diagnostics;
	using global::Sitecore.IO;
	using global::Sitecore.Pipelines.HttpRequest;
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Web;

	/// <summary>
	/// In the instance where a URL represents an Item, but the Item does not exist,
	/// this handler will route the user to an Item defined in the content tree specifically
	/// to provide content around this condition. This handler will also set the Response header
	/// so that search engines won't index the page and remove the URL from their collection.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Getting this to work can be a bit tricky. Follow all steps below carefully.
	/// </para>
	/// <para>
	/// Step 1: Ensure you've got an Item to handle the 404 in your site. Typically it would be named
	/// <c>/not-found</c> or <c>/not-found.aspx</c>. If you have more than one site, you need one on each site, 
	/// since this handler can fire for all sites.
	/// </para>
	/// <para>
	/// Step 2: Create a Rendering (WebControl type) named "Not Found". Use the following WebControl:
	/// <c>Constellation.Sitecore.Renderings.HttpResponsesNotFound, Constellation.Sitecore.Seo</c>
	/// This control is necessary to set the 404 response code, which must be set during the rendering of the handling page.
	/// </para>
	/// <para>
	/// Step 3: Ensure each of your 404 pages has the "Not Found" rendering installed somewhere
	/// near the top of the Presentation Details. It doesn't matter where it goes. 
	/// I put it near the top so it's easy to spot when looking at the list of included Renderings.
	/// </para>
	/// <para>
	/// Step 4: Web.Config changes (there are quite a few of them)
	/// </para>
	/// <para>
	/// 4.A: Install the following Processor in the HttpRequestBegin pipeline immediately after ItemResolver.
	/// <c>&lt;processor type="Constellation.Sitecore.Pipelines.HttpRequest.PageNotFoundResolver, Constellation.Sitecore.Seo" /&gt;</c>
	/// </para>
	/// <para>
	/// 4.B: Change the <c>ItemNotFoundUrl</c> setting value to match the root-relative path to your not-found item, 
	/// ex: /not-found.aspx
	/// </para>
	/// <para>
	/// 4.C: Change the <c>LinkItemNotFoundUrl</c> setting value to match the root-relative path to your not-found item.
	/// ex: /not-found.aspx
	/// </para>
	/// <para>
	/// 4.D: Adjust your <c>customErrors</c> element to include a specific path for 404 errors, 
	/// referencing the root-relative path to your not-found page.
	/// ex: <c>&lt;customErrors mode="RemoteOnly"&gt;
	///			&lt;error statusCode="404" redirect="/not-found.aspx"/&gt;
	///		&lt;/customErrors&gt;</c>
	///	Alternately, set the redirect to "/default.aspx" which will cause IIS to shunt to Sitecore.
	///	</para>
	///	<para>
	///	4.E: To preserve the URL in the browser's location bar, tell Sitecore to shunt server-side errors 
	///	through Server.Transfer rather than Response.Redirect.
	///	This is done by setting the "<c>RequestErrors.UseServerSideRedirect</c>" setting's value to "<c>true</c>".
	/// </para>
	/// <para>
	/// Server Meltdown: Particularly relevant in multi-site installations, the target page for the 404s must exist, 
	/// and must be published, or you may experience a recursive loop condition.
	/// </para>
	/// <para>
	/// Expected Behavior: 
	/// <list type="bullet">
	///		<item><description>
	///		The URL displayed in the browser will be different for sites running in classic mode. 
	///		Server.Transfer doesn't seem to work correctly.
	///		</description></item>
	///		<item><description>
	///		If you set <c>ErrorMode</c> to <c>RemoteOnly</c>, 
	///		you may see the yellow 404 page when debugging locally.
	///		</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class PageNotFoundResolver : HttpRequestProcessor
	{
		#region Methods
		/// <summary>
		/// Required by the HttpRequestProcessor contract. Called by Sitecore from the pipeline.
		/// </summary>
		/// <param name="args">The arguments for this request.</param>
		protected override void Execute(HttpRequestArgs args)
		{
			if (Context.Item != null)
			{
				return;
			}

			var site = Context.Site;
			if (site == null)
			{
				Log.Warn("PageNotFoundREsolver has no Site for: " + HttpContext.Current.Request.RawUrl, this);
			}

			var siteInfo = site;
			if (siteInfo == null)
			{
				Log.Warn("PageNotFoundResolver has no siteInfo for: " + HttpContext.Current.Request.RawUrl, this);
				return;
			}

			var language = Context.Language;
			if (language == null)
			{
				Log.Warn("PageNotFoundResolver has no language for: " + HttpContext.Current.Request.RawUrl, this);
				return;
			}

			var page = this.Resolve404Item();

			if (page == null)
			{
				Log.Warn("PageNotFoundResolver has no matching 404 page for: " + args.Context.Request.RawUrl, this);
				return;
			}

			/*
			 * Don't forget to set the Response Status Code and Description
			 * using Layout or Rendering.
			 * 
			 * We can't set it here because IIS will intercept it and we'll never
			 * render the layout.
			 */

			var context = args.Context;
			context.Response.TrySkipIisCustomErrors = true;
			Context.Item = page;
		}

		/// <summary>
		/// Required by the HttpRequestProcessor contract. Called by Sitecore from the pipeline.
		/// </summary>
		/// <param name="args">The arguments for this request.</param>
		protected override void Defer(HttpRequestArgs args)
		{
			// No action required.
		}

		/// <summary>
		/// Uses the Context Site and a custom property on the site definition to locate the appropriate
		/// 404 page to make the context item.
		/// </summary>
		/// <returns>The Item to handle the request.</returns>
		protected virtual Item Resolve404Item()
		{
			var notFoundPageItemPath = Context.Site.Properties["notFoundPageItemPath"];

			if (!notFoundPageItemPath.StartsWith("/sitecore/content", StringComparison.CurrentCultureIgnoreCase))
			{
				notFoundPageItemPath = FileUtil.MakePath(Context.Site.RootPath, notFoundPageItemPath);
			}

			var pageNotFound = Context.Database.SelectSingleItem(notFoundPageItemPath);
			return pageNotFound;
		}
		#endregion
	}
}
