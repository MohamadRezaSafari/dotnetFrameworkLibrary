﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Providers
{
    public static class StaticFile
    {
        public static string Version(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                var absolutePath = HostingEnvironment.MapPath(rootRelativePath);
                var lastChangedDateTime = File.GetLastWriteTime(absolutePath);

                if (rootRelativePath.StartsWith("~"))
                {
                    rootRelativePath = rootRelativePath.Substring(1);
                }

                var versionedUrl = rootRelativePath + "?v=" + lastChangedDateTime.Ticks;

                HttpRuntime.Cache.Insert(rootRelativePath, versionedUrl, new CacheDependency(absolutePath));
            }

            return HttpRuntime.Cache[rootRelativePath] as string;
        }
    }
}