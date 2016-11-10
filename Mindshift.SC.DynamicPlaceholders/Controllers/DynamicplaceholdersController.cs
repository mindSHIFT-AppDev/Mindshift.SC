using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Reflection;
using System.Xml;
using Sitecore.Layouts;
using System.Text;
using Sitecore.Data;
using System.Collections.ObjectModel;
using Sitecore.Data.Managers;
using Mindshift.SC.Common.Models;
using System.Web.Http;
using System.Collections;
using Mindshift.SC.Common.Controllers;

namespace Mindshift.SC.DynamicPlaceholders.Controllers {

	//[RoutePrefix("mindshiftAPI/{controller}")]
	[RoutePrefix("mindshiftAPI/Dynamicplaceholders")]
	public class DynamicplaceholdersController : ControllerBase {

		private List<DeviceItem> _devices = null;
		private List<DeviceItem> devices {
			get {
				if (_devices == null) {
					var master = Sitecore.Data.Database.GetDatabase("master"); // master will always have the Default Device.
					_devices = new List<DeviceItem>();
					// TODO: don't know a better way
					var deviceList = master.GetItem("/sitecore/layout/Devices").GetChildren();
					foreach (Item device in deviceList) {
						_devices.Add((DeviceItem)device);

					}
				}
				return _devices;
			}

		}



		[HttpGet]
		//mindshiftAPI/{controller}/{action}/{itemid}/{database}
		[Route("GetRenderings/{itemid}/{database}")]
		//[ResponseFilter]
		public DynamicResponse GetRenderings([FromUri]DynamicGetRequest request) {
			var ret = new DynamicResponse(request.ItemId.Replace("{", "").Replace("}", "").ToLower(), request.Database);

			// TODO: don't just do the DEFAULT

			//Response.Clear();




			var db = Sitecore.Data.Database.GetDatabase(request.Database);
			var item = db.GetItem(request.ItemId);

			// TODO: if item is null!

			//ID layoutFieldId = null;
			//switch (tab) {
			//	case "final":
			//		layoutFieldId = Sitecore.FieldIDs.FinalLayoutField;
			//		break;
			//	case "shared":
			//		layoutFieldId = Sitecore.FieldIDs.LayoutField;
			//		break;
			//	default:
			//		layoutFieldId = Sitecore.FieldIDs.LayoutField; // shouldn't be needed, but just in case
			//		break;
			//}


			ret.AddLayoutType("shared", "Shared");
			ret.AddLayoutType("final", "Final");


			foreach (var responseLayoutType in ret.LayoutTypes) {
				ID layoutFieldId = null;
				switch (responseLayoutType.Name) {
					case "final":
						layoutFieldId = Sitecore.FieldIDs.FinalLayoutField;
						break;
					case "shared":
						layoutFieldId = Sitecore.FieldIDs.LayoutField;
						break;
					default:
						layoutFieldId = Sitecore.FieldIDs.LayoutField; // shouldn't be needed, but just in case
						break;
				}

				// yes, there are two things holding the layout type - but that is because this one on the next line is for Response only
				//var responseLayoutType = ret.AddLayoutType(layoutTypeName);

				Sitecore.Data.Fields.LayoutField layoutField = item.Fields[layoutFieldId];
				var layoutDefinition = Sitecore.Layouts.LayoutDefinition.Parse(item[layoutFieldId]);

				int i = 0;
				foreach (var device in devices) {
					i++;
					// this holds a flat dictionary of all renderings so we can place our parent/child relationships correctly
					var allRenderings = new Dictionary<string, ResponseRendering>();
					var allRenderingsReversed = new Dictionary<string, List<ResponseRendering>>();



					var responseDevice = responseLayoutType.AddDevice(item, layoutDefinition, device, layoutField.GetLayoutID(device));


					//Sitecore.Layouts.DeviceDefinition device =	 layout.Devices[i] as Sitecore.Layouts.DeviceDefinition; 
					

					Sitecore.Layouts.RenderingReference[] renderings = layoutField.GetReferences(device);

					// if (renderings == null) return; // this item doesn't have any renderings under the Default device.

					//string layoutXml = layoutField.Value;

					//responseDevice.Xml = layoutField.Value;
					if (renderings != null) {
						// TODO: NEST! that's the WHOLE THING!
						foreach (var rendering in renderings) {
							var responseRendering = new ResponseRendering(rendering, layoutDefinition, item);// responseDevice.AddRendering(rendering); // TODO: no response needed!
							// add to the proper rendering. Two cases here, depending on which comes first.
							if (!string.IsNullOrEmpty(responseRendering.ParentUniqueId) && allRenderings.ContainsKey(responseRendering.ParentUniqueId)) {
								allRenderings[responseRendering.ParentUniqueId].AddRendering(item, layoutDefinition, responseRendering);
							} else if (allRenderingsReversed.ContainsKey(responseRendering.UniqueId)) {
								//responseRendering.RemoveRenderingRange(allRenderingsReversed);
								responseRendering.AddRenderingRange(item, layoutDefinition, allRenderingsReversed[responseRendering.UniqueId]); // could have collected a bunch by now
							}

							if (string.IsNullOrEmpty(responseRendering.ParentUniqueId)) { // this means the path wasn't built with a GUID, so I couldn't find it. TODO: Fix it anyway!
								responseDevice.AddRendering(item, layoutDefinition, responseRendering);
							}

							// TODO: but I need to remove it when this happens. So don't add the ones with parents, duh
							// - they DEFINITELY don't go in the maincontent then lol (at least, not right now)

							allRenderings.Add(responseRendering.UniqueId, responseRendering); // always add it to the flat list!

							if (!string.IsNullOrEmpty(responseRendering.ParentUniqueId)) {
								if (!allRenderingsReversed.ContainsKey(responseRendering.ParentUniqueId)) {
									allRenderingsReversed.Add(responseRendering.ParentUniqueId, new List<ResponseRendering>());
								}
								allRenderingsReversed[responseRendering.ParentUniqueId].Add(responseRendering);
							}
						}

						// TODO: loop through and "arange"

					}
				}
			}

			return ret;

		}

		[HttpPost]
		[Route("mindshiftAPI/DynamicPlaceholders/SaveRenderings/{itemid}/{database}")]
		public DynamicResponse SaveRenderings(DynamicSaveRequest request) {

			var ret = GetRenderings(request);

			// TODO: compare (or keep a log of changes to send on the front end?)

			// this first iteration assumes the same structure with only updates.

			// TODO: SAVE it!
			// wait - we don't care about hierarchy! we only care about updating the placeholder!

			// start with the placeholer path and the datasource field...
			// which needs to be reverse mapped, maybe
			var db = Sitecore.Data.Database.GetDatabase(request.Database);
			var item = db.GetItem(request.ItemId);


			foreach (var layoutType in request.LayoutTypes) {
				ID layoutFieldId = null;
				switch (layoutType.Name) {
					case "final":
						layoutFieldId = Sitecore.FieldIDs.FinalLayoutField;
						break;
					case "shared":
						layoutFieldId = Sitecore.FieldIDs.LayoutField;
						break;
					default:
						layoutFieldId = Sitecore.FieldIDs.LayoutField; // shouldn't be needed, but just in case
						break;
				}

				Sitecore.Data.Fields.LayoutField layoutField = item.Fields[layoutFieldId];
				var layoutDefinition = Sitecore.Layouts.LayoutDefinition.Parse(item[layoutFieldId]);

				string layoutXml = layoutField.Value;


				var xmlLayoutDoc = new XmlDocument();
				xmlLayoutDoc.LoadXml(layoutXml);

				foreach (var device in layoutType.Devices) {

					UpdateRendering(device.Placeholders, xmlLayoutDoc, db, device);

				}


				item.Editing.BeginEdit();
				layoutField.Value = xmlLayoutDoc.OuterXml;
				//var test = xmlLayoutDoc.OuterXml;
				item.Editing.EndEdit();


			}
			return GetRenderings(request);
		}


		private void UpdateRendering(List<RequestPlaceholder> requestPlaceholders, XmlDocument xmlLayoutDoc, Database db, RequestDevice device) {
			foreach (var requestPlaceholder in requestPlaceholders) {
				foreach (var requestRendering in requestPlaceholder.Renderings) {
					var renderingElement = xmlLayoutDoc.SelectSingleNode("//r[@uid='{" + requestRendering.UniqueId.ToUpper() + "}']") as XmlElement;

					if (renderingElement == null) { // this is new!
						var deviceElement = xmlLayoutDoc.SelectSingleNode("//d[@id='{" + device.Id.ToUpper() + "}']") as XmlElement;
						renderingElement = xmlLayoutDoc.CreateElement("r");
						renderingElement.SetAttribute("uid", "{" + requestRendering.UniqueId.ToUpper() + "}");
						renderingElement.SetAttribute("id", "{" + requestRendering.ItemId.ToUpper() + "}");
						deviceElement.AppendChild(renderingElement);
					} 

					if (requestRendering.PlaceholderPath == "pinwheel_5~092f656c00904d798cb0961dd31cafd8") {
						// TODO: build out this if irenderingElement is NULL
						// - but it needs to go into the correct device.
						// - and ORDERING is important!
						// - will be hard to "insert after"
						// - let's do create first
						//  <r uid="{68B1BCEA-107F-48E5-9862-B2BCC93782CB}" ds="{569F1634-3A8B-43B9-8D42-4E8DA5122F3C}" id="{7B927EB1-2CB6-4F00-8E09-E2CB9438F54C}" ph="maincontent" /> 

					}
					// TODO: rebuild this path... from parents
					renderingElement.SetAttribute("ph", requestRendering.PlaceholderPath); // TODO: should I use requestPlaceholder and not assume the child has been updated? (it's a lot to keep synced)

					string ds = "";
					if (!string.IsNullOrWhiteSpace(requestRendering.DataSourcePath)) {
						var item = db.GetItem(requestRendering.DataSourcePath);
						if (item != null) {
							ds = item.ID.ToString();
						}
					}

					renderingElement.SetAttribute("ds", ds);
					
					UpdateRendering(requestRendering.Placeholders, xmlLayoutDoc, db, device);
				}
			}
		}
		//[HttpGet]
		//public SelectRenderingResponse GetSelectRenderings([FromUri]GetSelectRenderingRequest request) {




		//}

	}
}