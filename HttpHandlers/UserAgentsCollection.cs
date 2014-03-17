namespace Spark.Sitecore.Seo.HttpHandlers
{
	using System.Configuration;

	/// <summary>
	/// A group of User Agent definitions for the RobotsTxt configuration section.
	/// </summary>
	[ConfigurationCollection(typeof(UserAgentElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class UserAgentsCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// The this.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="UserAgentElement"/>.
		/// </returns>
		public UserAgentElement this[int index]
		{
			get
			{
				return (UserAgentElement)BaseGet(index);
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
		/// The <see cref="UserAgentElement"/>.
		/// </returns>
		public new UserAgentElement this[string key]
		{
			get
			{
				return (UserAgentElement)BaseGet(key);
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
			return new UserAgentElement();
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
			return ((UserAgentElement)element).Name;
		}
	}
}
