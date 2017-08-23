using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Helpers
{
    public static class GenericHelper
    {
        public static string EnsureSlashesAtEnd(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return path;
            }
            else
            {
                if (path.Substring(path.Length - 1) == "/")
                {
                    return path;
                }
                else
                {
                    return path + "/";
                }
            }
            
            
        }

        public static Sitecore.Web.SiteInfo GetCurrentSiteByURL()
        {

            var url = System.Web.HttpContext.Current.Request.Url;
            //var siteContext = Sitecore.Sites.SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery);

            //return siteContext;

            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
            string pattern = @"[^.\s]+\.mindshift\.com";
           
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);


            foreach (Sitecore.Web.SiteInfo siteInfo in siteInfoList)
            {
                var hostNames = siteInfo.HostName.Split("|".ToCharArray());
                foreach (var hostName in hostNames)
                {
                    Match m = r.Match(hostName);
                    if (url.Host.ToLower() == hostName.ToLower())
                    {
                        return siteInfo;
                    }
                    else if(m.Success)
                    {
                        return siteInfo;
                    }
                }
            }

            return null;

        }
    }
}
