namespace Spark.Sitecore.Seo.HttpHandlers
{
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class RobotsTxtConfiguration : ConfigurationSection
	{
		/// <summary>
		/// Internal instance of the configuration section to simplify access to its properties.
		/// </summary>
		private static RobotsTxtConfiguration config = null;

		/// <summary>
		/// Gets the configuration settings for the Sitemap Generator.
		/// </summary>
		public static RobotsTxtConfiguration Settings
		{
			get
			{
				if (config == null)
				{
					config = ConfigurationManager.GetSection("spark/robotsTxt") as RobotsTxtConfiguration;
				}

				return config;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether robots.
		/// </summary>
		[ConfigurationProperty("allowed", DefaultValue = true, IsRequired = true)]
		public bool Allowed
		{
			get { return (bool)this["allowed"]; }
			set { this["allowed"] = value; }
		}

		/// <summary>
		/// Gets the agents.
		/// </summary>
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public UserAgentsCollection Agents
		{
			[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
			get
			{
				return (UserAgentsCollection)base[""];
			}
		}
	}
}
