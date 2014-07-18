namespace Constellation.Sitecore.HttpHandlers
{
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// The robots txt handler configuration.
	/// </summary>
	public class RobotsTxtHandlerConfiguration : ConfigurationSection
	{
		/// <summary>
		/// The settings.
		/// </summary>
		private static RobotsTxtHandlerConfiguration settings;

		/// <summary>
		/// Gets the settings.
		/// </summary>
		public static RobotsTxtHandlerConfiguration Settings
		{
			get
			{
				if (settings == null)
				{
					var config = ConfigurationManager.GetSection("robotsTxtHandler");
					settings = config as RobotsTxtHandlerConfiguration;
				}

				return settings;
			}
		}

		/// <summary>
		/// Gets a value indicating whether any robots are allowed.
		/// </summary>
		[ConfigurationProperty("allowed", DefaultValue = false, IsRequired = true)]
		public bool Allowed
		{
			get
			{
				return (bool)base["allowed"];
			}
		}

		/// <summary>
		/// Gets the rules used to locate and link to shared content items
		/// across all sites.
		/// </summary>
		[ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
		public RobotsTxtHandlerRuleCollection Rules
		{
			[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
			get
			{
				return (RobotsTxtHandlerRuleCollection)base[""];
			}
		}
	}
}
