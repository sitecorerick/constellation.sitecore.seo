namespace Constellation.Sitecore.Pipelines.HttpRequest
{
	using global::Sitecore;
	using global::Sitecore.Globalization;
	using global::Sitecore.Pipelines.HttpRequest;
	using global::Sitecore.Web;
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using System.Web;

	/// <summary>
	/// Determines the language based upon the browser language, and previous page views
	/// </summary>
	public class LanguageResolver : HttpRequestProcessor
	{
		#region Methods
		/// <summary>
		/// Required by the HttpRequestProcessor contract. Called by Sitecore from the pipeline.
		/// </summary>
		/// <remarks>
		/// Language detection and setting is done from "most explicit" to "most implicit"
		///	Here is the order of determination:
		///	- Querystring (Explicit - typically a change request)
		///	- URL (Explicit - typically offered by the target content)
		///	- Sitecore Cookie (Explicit - typically set for the session)
		///	- Browser (Implicit - typically first visit)
		///	If no match is found from the above, the user is shunted to the site picker page.
		/// </remarks>
		/// <param name="args">The arguments for this request.</param>
		protected override void Execute(HttpRequestArgs args)
		{
			string explicitLanguage;

			if (Context.PageMode.IsNormal)
			{
				explicitLanguage = this.GetSupportableLanguage(this.GetQuerystringLanguage());
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetSupportableLanguage(this.GetFilePathLanguage());
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetSupportableLanguage(this.GetCookieLanguage());
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetSupportableBrowserLanguage();
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
				}
			}
			else
			{
				explicitLanguage = this.GetQuerystringLanguage();
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetFilePathLanguage();
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetCookieLanguage();
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				explicitLanguage = this.GetSupportableBrowserLanguage();
				if (!string.IsNullOrEmpty(explicitLanguage))
				{
					this.SetLanguage(explicitLanguage);
					return;
				}

				this.SetLanguage(Context.Site.Language);
			}
		}

		/// <summary>
		/// Runs the default Sitecore Language Resolver when operating within the various
		/// Sitecore Clients.
		/// </summary>
		/// <param name="args">The arguments for this request.</param>
		protected override void Defer(HttpRequestArgs args)
		{
			var resolver = new global::Sitecore.Pipelines.HttpRequest.LanguageResolver();

			resolver.Process(args);
		}
		#endregion

		#region Helpers
		#region Language Getters
		/// <summary>
		/// Retrieves the language code from a browser cookie.
		/// </summary>
		/// <returns>The language code or null.</returns>
		protected string GetCookieLanguage()
		{
			var key = Context.Site.GetCookieKey("lang");
			var cookie = HttpContext.Current.Request.Cookies[key];

			if (cookie == null)
			{
				return null;
			}

			return cookie.Value;
		}

		/// <summary>
		/// Retrieves the language code from the folder structure of the URL.
		/// </summary>
		/// <returns>The language code or null.</returns>
		protected string GetFilePathLanguage()
		{
			if (Context.Data.FilePathLanguage == null)
			{
				return null;
			}

			return Context.Data.FilePathLanguage.Name;
		}

		/// <summary>
		/// Retrieves the language code from the "sc_lang" querystring parameter.
		/// </summary>
		/// <returns>The language code or null.</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
		protected string GetQuerystringLanguage()
		{
			var lang = HttpContext.Current.Request.QueryString["sc_lang"];

			if (string.IsNullOrEmpty(lang))
			{
				lang = HttpContext.Current.Request.QueryString["_lang"];
			}

			return lang;
		}

		/// <summary>
		/// Returns the first supportable ISO code based upon the browser's accepted language
		/// and the languages supported by the current site.
		/// </summary>
		/// <returns>A best fit ISO Code or null.</returns>
		protected string GetSupportableBrowserLanguage()
		{
			var languages = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];

			if (string.IsNullOrEmpty(languages))
			{
				return Context.Site.Language;
			}

			var browserLanguages = StringUtil.Split(languages, ',', true);
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var browserCode in browserLanguages)
			// ReSharper restore LoopCanBeConvertedToQuery
			{
				var language = this.GetSupportableLanguage(TruncateBrowserLanguage(browserCode));
				if (!string.IsNullOrEmpty(language))
				{
					return language;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns a best-fit language code based on the supplied language code.
		/// </summary>
		/// <param name="langCode">
		/// The supplied code from browser, cookie, or querystring.
		/// </param>
		/// <returns>
		/// The best fit given the site's supported languages.
		/// </returns>
		protected string GetSupportableLanguage(string langCode)
		{
			if (string.IsNullOrEmpty(langCode))
			{
				return null;
			}

			var siteLanguages = this.GetSupportedSiteLanguages();

			// Exact match
			if (siteLanguages.Contains(langCode))
			{
				return langCode; // matches Sitecore code.
			}

			// Match against languages configured without dialect
			foreach (var siteCode in siteLanguages)
			{
				if (siteCode.Length == 2 && langCode.StartsWith(siteCode))
				{
					return siteCode; // use Sitecore code.
				}
			}

			// Match against a dialect when we ignore the browser dialect
			if (langCode.Length >= 2)
			{
				langCode = langCode.Substring(0, 2);

				// ReSharper disable LoopCanBeConvertedToQuery
				foreach (var siteCode in siteLanguages)
				// ReSharper restore LoopCanBeConvertedToQuery
				{
					if (siteCode.StartsWith(langCode))
					{
						return siteCode; // use Sitecore code.
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Retrieves the languages that are explicitly supported by the context site.
		/// </summary>
		/// <returns>An array of ISO Codes.</returns>
		protected string[] GetSupportedSiteLanguages()
		{
			var site = Context.Site;

			if (site == null)
			{
				return null;
			}

			var languagesProperty = Context.Site.Properties["supportedLanguages"];

			if (string.IsNullOrEmpty(languagesProperty))
			{
				return null;
			}

			return languagesProperty.Split('|');
		}
		#endregion

		#region Language Setters
		/// <summary>
		/// Ensures that all language states are configured appropriately for this request, and the next if possible.
		/// </summary>
		/// <param name="isoCode">The language to use.</param>
		protected void SetLanguage(string isoCode)
		{
			Language targetLanguage = this.ResolveLanguageFromIsoCode(isoCode);

			this.SetCookieLanguage(targetLanguage);
			this.SetContextLanguage(targetLanguage);
			this.SetThreadCulture(targetLanguage);
		}

		/// <summary>
		/// Sets the Sitecore language cookie for the session and our cookie for the next sessions.
		/// </summary>
		/// <param name="language">The language to use for subsequent requests.</param>
		protected void SetCookieLanguage(Language language)
		{
			var cookieName = Context.Site.GetCookieKey("lang");
			WebUtil.SetCookieValue(cookieName, language.Name, DateTime.MaxValue);
		}

		/// <summary>
		/// Sets the sitecore Context language for the request.
		/// </summary>
		/// <param name="language">The language to use for this request.</param>
		protected void SetContextLanguage(Language language)
		{
			Context.Language = language;
		}

		/// <summary>
		/// Sets the Microsoft Culture for the request.
		/// </summary>
		/// <param name="language">The Language to use as the Culture basis.</param>
		protected void SetThreadCulture(Language language)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language.Name);
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language.Name);
		}
		#endregion

		/// <summary>
		/// Uses an  ISO Code to obtain a Language instance from the Sitecore API.
		/// If no matching Language is found, the Site's default language is used.
		/// </summary>
		/// <param name="isoCode">The code to parse.</param>
		/// <returns>An instance of Sitecore.Globalization.Language.</returns>
		protected Language ResolveLanguageFromIsoCode(string isoCode)
		{
			Language language = Context.Database.Languages.FirstOrDefault(systemLanguage => systemLanguage.Name.Equals(isoCode, StringComparison.InvariantCultureIgnoreCase));

			if (language == null)
			{
				return this.ResolveLanguageFromIsoCode(Context.Site.Language);
			}

			return language;
		}

		/// <summary>
		/// Reduces the browser language string to the ISO Code.
		/// </summary>
		/// <param name="language">The string to parse.</param>
		/// <returns>The ISO code.</returns>
		private static string TruncateBrowserLanguage(string language)
		{
			if (language.IndexOf(';') > -1)
			{
				return language.Substring(0, language.IndexOf(';'));
			}

			return language;
		}
		#endregion
	}
}
