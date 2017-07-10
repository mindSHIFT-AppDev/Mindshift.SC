using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Layouts;


namespace Mindshift.SC.Common.Mapper.Helpers
{
	public class BranchHooks
	{
		private static readonly List<Sitecore.Data.ID> _inProcess = new List<Sitecore.Data.ID>();

		public string Database { get; set; }


		//item:copied
		public void OnItemCopied(object sender, EventArgs args)
		{
			Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
			Sitecore.Diagnostics.Assert.IsNotNull(eventArgs, "eventArgs");
			Sitecore.Data.Items.Item sourceItem = eventArgs.Parameters[0] as Sitecore.Data.Items.Item;
			Sitecore.Diagnostics.Assert.IsNotNull(sourceItem, "item");

			Sitecore.Data.Items.Item targetItem = eventArgs.Parameters[1] as Sitecore.Data.Items.Item;
			Sitecore.Diagnostics.Assert.IsNotNull(targetItem, "item");

			if (_inProcess.Contains(sourceItem.ID))
			{
				return;
			}

			if (sourceItem.Name == "__Standard Values") return;

			// TODO: uncomment next line to not allow this functionality during copying and only allow duplicate.
			//if (!sourceItem.Parent.ID.Equals(targetItem.Parent.ID)) return;

			UpdateDataSourcesWithDescendants(targetItem, sourceItem.Paths.FullPath);

		}


		//item:added
		public void OnItemAdded(object sender, EventArgs args)
		{
			Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
			Sitecore.Diagnostics.Assert.IsNotNull(eventArgs, "eventArgs");
			Sitecore.Data.Items.Item item = eventArgs.Parameters[0] as Sitecore.Data.Items.Item;
			Sitecore.Diagnostics.Assert.IsNotNull(item, "item");

			if (_inProcess.Contains(item.ID))
			{
				return;
			}

			if (item.Name == "__Standard Values") return;

			if (item.Branch == null) return;

			if (item.Branch == null || item.Branch.InnerItem == null) return; // only branch templates, please.

			UpdateDataSourcesWithDescendants(item, item.Branch.InnerItem.Paths.FullPath + "/$name");
		}


		private void UpdateDataSourcesWithDescendants(Item item, string sourcePath)
		{
			UpdateDataSources(item, sourcePath);

			// Note: wondered what could be done to limit the items here. It doesn't hurt anything to loop through items with no presentation and any filtering of this list might have side effects.
			var ancestorItemList = item.Axes.GetDescendants();

			foreach (var ancestorItem in ancestorItemList)
			{
				var newSourcePath = ancestorItem.Paths.FullPath.Replace(item.Paths.FullPath, sourcePath); // get the source of the ancestor item. Basically reverse replacing what we do inside the UpdateDataSource method.
				UpdateDataSources(ancestorItem, newSourcePath);
			}


		}

		private DeviceItem _defaultDevice = null;
		private DeviceItem defaultDevice
		{
			get
			{
				if (_defaultDevice == null)
				{
					var master = Sitecore.Data.Database.GetDatabase("master"); // master will always have the Default Device.
					_defaultDevice = master.GetItem("/sitecore/layout/Devices/Default"); 
				}
				return _defaultDevice;
			}

		}

		private void UpdateDataSources(Item item, string sourcePath)
		{
			try
			{
				_inProcess.Add(item.ID);

				Sitecore.Data.Fields.LayoutField layoutField = item.Fields[Sitecore.FieldIDs.FinalLayoutField];
				Sitecore.Layouts.RenderingReference[] renderings = layoutField.GetReferences(defaultDevice);

				if (renderings == null) return; // this item doesn't have any renderings under the Default device.

				string layoutXml = layoutField.Value;

				foreach (var rendering in renderings)
				{
					if (!string.IsNullOrEmpty(rendering.Settings.DataSource))
					{
						var dataSourceItem = item.Database.GetItem(rendering.Settings.DataSource);
						if (dataSourceItem != null)
						{
							if (dataSourceItem.Paths.FullPath.Contains(sourcePath)) // Only modify paths pointed into the original location
							{
								var newPath = dataSourceItem.Paths.FullPath.Replace(sourcePath, item.Paths.FullPath);
								var newDataSourceItem = item.Database.GetItem(newPath);
								if (newDataSourceItem != null)
								{
									layoutXml = layoutXml.Replace(rendering.Settings.DataSource, newDataSourceItem.ID.ToString());
								}
							}
						}
					}
				}

				if (!layoutField.Value.Equals(layoutXml))
				{
					item.Editing.BeginEdit();
					layoutField.Value = layoutXml;
					item.Editing.EndEdit();
				}
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error("UpdateDataSources. Error: " + ex.Message + "Stack Trace: " + ex.StackTrace, new object());
				throw ex;
			}
			finally
			{
				if (item != null) _inProcess.Remove(item.ID);
			}

		}

	}
}
