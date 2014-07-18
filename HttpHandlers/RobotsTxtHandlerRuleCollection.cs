namespace Constellation.Sitecore.HttpHandlers
{
	using System.Configuration;

	/// <summary>
	/// The robots txt handler rule collection.
	/// </summary>
	[ConfigurationCollection(typeof(RobotsTxtHandlerRule), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class RobotsTxtHandlerRuleCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// The this.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="RobotsTxtHandlerRule"/>.
		/// </returns>
		public RobotsTxtHandlerRule this[int index]
		{
			get
			{
				return (RobotsTxtHandlerRule)BaseGet(index);
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
		/// The <see cref="RobotsTxtHandlerRule"/>.
		/// </returns>
		public new RobotsTxtHandlerRule this[string key]
		{
			get
			{
				return (RobotsTxtHandlerRule)BaseGet(key);
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
			return new RobotsTxtHandlerRule();
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
			return ((RobotsTxtHandlerRule)element).AgentName;
		}
	}
}
