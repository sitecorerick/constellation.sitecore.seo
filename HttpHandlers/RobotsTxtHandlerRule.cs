namespace Constellation.Sitecore.HttpHandlers
{
	using System.Configuration;

	/// <summary>
	/// The robots txt handler rule.
	/// </summary>
	public class RobotsTxtHandlerRule : ConfigurationElement
	{
		// <add agentName="googlebot" allowed="true"/>

		/// <summary>
		/// Gets or sets the agent name.
		/// </summary>
		[ConfigurationProperty("agentName", IsKey = true, IsRequired = true)]
		public string AgentName
		{
			get { return (string)base["agentName"]; }
			set { base["agentName"] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether allowed.
		/// </summary>
		[ConfigurationProperty("allowed", IsRequired = true)]
		public bool Allowed
		{
			get { return (bool)base["allowed"]; }
			set { base["allowed"] = value; }
		}
	}
}
