namespace Constellation.Sitecore.Renderings
{
	using Constellation.Html;
	using System.Diagnostics.CodeAnalysis;
	using System.Web.UI;

	/// <summary>
	/// Renders a script tag for an external Javascript file with the correct static hostname.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class StaticJavascriptReference : StaticFileReference
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StaticJavascriptReference"/> class.
		/// </summary>
		public StaticJavascriptReference()
		{
			// ReSharper disable DoNotCallOverridableMethodsInConstructor
			this.Cacheable = true;
			this.VaryByData = true;
			this.EnableViewState = false;
			// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}

		/// <summary>
		/// Gets or sets the relative path to the Javascript source file.
		/// </summary>
		public string Src { get; set; }

		/// <summary>
		/// Renders the output of the control
		/// </summary>
		/// <param name="output">The response writer.</param>
		protected override void DoRender(HtmlTextWriter output)
		{
			using (output.RenderTag(
				HtmlTextWriterTag.Script,
				new HtmlAttribute(HtmlTextWriterAttribute.Type, "text/javascript"),
				new HtmlAttribute(HtmlTextWriterAttribute.Src, this.GetAbsoluteUrl(this.Src))))
			{
				// nothing needed inside.
			}
		}
	}
}
