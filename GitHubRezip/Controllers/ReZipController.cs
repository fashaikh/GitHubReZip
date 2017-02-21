using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO.Compression;
using Ionic.Zip;

namespace GitHubRezip.Controllers
{
    public class ReZipController : ApiController
    {

        [HttpGet]
        // GET: /api/ReZip/Azure-Samples/functions-dotnet-sas-token/archive/master
        public async Task<HttpResponseMessage> GetZip( string user ,
            string project,
            string branch
            )
        {
            var githubZipUrl = $"https://github.com/{user}/{project}/archive/{branch}.zip";
            HttpResponseMessage response = Request.CreateResponse();
            var rzm = new RemoteZipManager(githubZipUrl);
            var zipstream = await rzm.GetZipStream(githubZipUrl);
            var zipName = $"{project}-{branch}.zip";
            response.Content = ZipStreamContent.Create(ref zipstream, ref zipName, zip =>
            {

                var ziparchive = new ZipArchive(zipstream);
                var rootFolderFound = false;
                var rootFolderName = String.Empty;
                foreach (ZipArchiveEntry e in ziparchive.Entries)
                {
                    if (!rootFolderFound && e.Length == 0)
                    {
                        rootFolderName = e.FullName;
                        rootFolderFound = true;
                    }
                    else
                    {
                        zip.AddFileContent(e.FullName.Replace(rootFolderName, String.Empty), e.Open());
                    }
                }

            });
            return response;
        }
        

    }
}
