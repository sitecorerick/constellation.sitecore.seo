namespace Constellation.Sitecore.Renderings
{
	using Constellation.Html;
	using System.Web.UI;

	/// <summary>
	/// Renders an image tag using the designated static file host.
	/// </summary>
	public class StaticImage : StaticFileReference
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StaticImage"/> class.
		/// </summary>
		public StaticImage()
		{
			// ReSharper disable DoNotCallOverridableMethodsInConstructor
			this.Cacheable = true;
			this.VaryByData = true;
			this.EnableViewState = false;
			// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}

		/// <summary>
		/// Gets or sets the alternate text for the image file.
		/// </summary>
		public string Alt { get; set; }

		/// <summary>
		/// Gets or sets the relative path to the image file.
		/// </summary>
		public string Src { get; set; }

		/// <summary>
		/// Renders the output of the control
		/// </summary>
		/// <param name="output">The response writer.</param>
		protected override void DoRender(HtmlTextWriter output)
		{
			output.RenderImg(this.GetAbsoluteUrl(this.Src), this.Alt);
		}
	}
}
