namespace Constellation.Sitecore.HttpHandlers.RobotsTxt
{
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Configuration : ConfigurationSection
	{
		/// <summary>
		/// Internal instance of the configuration section to simplify access to its properties.
		/// </summary>
		private static Configuration config = null;

		/// <summary>
		/// Gets the configuration settings for the Sitemap Generator.
		/// </summary>
		public static Configuration Settings
		{
			get
			{
				if (config == null)
				{
					config = ConfigurationManager.GetSection("constellation/robotsTxtHandler") as Configuration;
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
		public UserAgentsConfigurationElementCollection Agents
		{
			[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
			get
			{
				return (UserAgentsConfigurationElementCollection)base[""];
			}
		}
	}
}
