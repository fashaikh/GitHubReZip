using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace GitHubRezip
{
    public class RemoteZipManager : RemoteClientBase
    {
        public RemoteZipManager(string serviceUrl, ICredentials credentials = null, HttpMessageHandler handler = null)
            : base(serviceUrl, credentials, handler)
        {
        }

        public async Task<Stream> GetZipStream(string path)
        {
            return await Client.GetStreamAsync(path);
        }

        public void PutZipStream(string path, Stream zipFile)
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri(path, UriKind.Relative);
                request.Headers.IfMatch.Add(EntityTagHeaderValue.Any);
                request.Content = new StreamContent(zipFile);
                Client.SendAsync(request).Result.EnsureSuccessful();
            }
        }


        public void PutZipFile(string path, string localZipPath)
        {
            using (var stream = File.OpenRead(localZipPath))
            {
                PutZipStream(path, stream);
            }
        }
    }
}

