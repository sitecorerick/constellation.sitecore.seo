namespace Constellation.Sitecore.HttpHandlers.RobotsTxt
{
	using System.Configuration;

	/// <summary>
	/// A group of User Agent definitions for the RobotsTxt configuration section.
	/// </summary>
	[ConfigurationCollection(typeof(UserAgentConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class UserAgentsConfigurationElementCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// The this.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="UserAgentConfigurationElement"/>.
		/// </returns>
		public UserAgentConfigurationElement this[int index]
		{
			get
			{
				return (UserAgentConfigurationElement)this.BaseGet(index);
			}

			set
			{
				if (this.BaseGet(index) != null)
				{
					this.BaseRemoveAt(index);
				}

				this.BaseAdd(index, value);
			}
		}

		/// <summary>
		/// The this.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// The <see cref="UserAgentConfigurationElement"/>.
		/// </returns>
		public new UserAgentConfigurationElement this[string key]
		{
			get
			{
				return (UserAgentConfigurationElement)this.BaseGet(key);
			}
		}

		/// <summary>
		/// The create new element.
		/// </summary>
		/// <returns>
		/// The <see cref="ConfigurationElement"/>.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new UserAgentConfigurationElement();
		}

		/// <summary>
		/// The get element key.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// The <see cref="object"/>.
		/// </returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((UserAgentConfigurationElement)element).Name;
		}
	}
}
