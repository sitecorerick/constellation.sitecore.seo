namespace Constellation.Sitecore.HttpHandlers.RobotsTxt
{
	using System.Configuration;

	/// <summary>
	/// Defines a User Agent in the RobotsTxt configuration section.
	/// </summary>
	public class UserAgentConfigurationElement : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets the name of the User Agent.
		/// </summary>
		[ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}

			set
			{
				this["name"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user agent is allowed to crawl.
		/// </summary>
		[ConfigurationProperty("allowed", DefaultValue = true, IsRequired = true)]
		public bool Allowed
		{
			get
			{
				return (bool)this["allowed"];
			}

			set
			{
				this["allowed"] = value;
			}
		}
	}
}
