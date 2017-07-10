using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Diagnostics;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Template.Fields
{
    public class CustomGeneralLink : Sitecore.Shell.Applications.ContentEditor.Link
    {
        private string _itemid;

        public string ItemID
        {
            get
            {
                return StringUtil.GetString(new string[1]
                                            {
                                              this._itemid
                                            });
            }
            set
            {
                Assert.ArgumentNotNull((object)value, "value");
                this._itemid = value;
            }
        }

        private Sitecore.Web.SiteInfo CurrentSiteNode
        {
            get
            {

                return GenericHelper.GetCurrentSiteByURL();

                //var url = System.Web.HttpContext.Current.Request.Url;
                ////var siteContext = Sitecore.Sites.SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery);

                ////return siteContext;

                //var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

                //foreach (Sitecore.Web.SiteInfo siteInfo in siteInfoList)
                //{
                //    var hostNames = siteInfo.HostName.Split("|".ToCharArray());
                //    foreach (var hostName in hostNames)
                //    {
                //        if (url.Host.ToLower() == hostName.ToLower())
                //        {
                //            return siteInfo;
                //        }
                //    }
                //}

                //return null;
            }
        }


        public override string Source
        {
            get
            {
                return this.GetViewStateString("Source");
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                String newValue = value;
                if (value.ToLower().StartsWith("query:", StringComparison.InvariantCulture))
                {
                    
                    //base.SetViewStateString("Source", "/sitecore/content/Ricoh/Sites/USA/Home");
                    
                    string query = value.Substring(6);
                    bool flag = query.StartsWith("fast:", StringComparison.InvariantCulture);
                   
                    if(!flag)
                    {
                        Item item = Client.ContentDatabase.GetItem(this.ItemID);
                        if (item != null)
                        {
                            Item sourceItem = item.Axes.SelectSingleItem(value.Substring("query:".Length));
                            if (sourceItem != null)
                            {
                                base.SetViewStateString("Source", sourceItem.Paths.FullPath);
                            }
                        }
                    }
                    
                   
                }
                else if(value.ToLower().Contains("{sitename}"))
                {


                    base.SetViewStateString("Source", value.Replace("{sitename}", CurrentSiteNode.Name));
                   

                }
                else
                {
                    string str = MainUtil.UnmapPath(newValue);
                    if (str.EndsWith("/", StringComparison.InvariantCulture))
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    base.SetViewStateString("Source", str);
                }
            }
        }
    }
}
