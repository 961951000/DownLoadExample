using DownLoadExample.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DownLoadExample.Controllers
{
    public class FilesController : ApiController
    {
        public HttpResponseMessage GetExcelFromXlsx()
        {
            var file = new FileInfo(HostingEnvironment.MapPath("~/Tests/docs-20171110094051.xlsx"));
            var fileDownload = new FileDownloadStream(file);
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(fileDownload.WriteToStream, new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

            return response;
        }

        public HttpResponseMessage GetExcelFromXml()
        {
            var response = Request.CreateResponse();
            var file = new FileInfo(HostingEnvironment.MapPath("~/Tests/Sample.xml"));
            var fileDownload = new FileDownloadStream(file);
            response.Content = new PushStreamContent(fileDownload.WriteToStream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            };

            return response;
        }

        public HttpResponseMessage GetXml()
        {
            string filename = HostingEnvironment.MapPath("~/Tests/Sample.xml");
            var response = Request.CreateResponse();
            XDocument xDoc = XDocument.Load(filename, LoadOptions.None);
            response.Content = new PushStreamContent(
            (stream, content, context) =>
            {
                // After save we close the stream to signal that we are done writing.
                xDoc.Save(stream);
                stream.Close();
            },
            "application/xml");

            return response;
        }

        public HttpResponseMessage GetXml1()
        {
            string filename = HostingEnvironment.MapPath("~/Tests/Sample.xml");
            var response = Request.CreateResponse();
            response.Content = new StreamContent(File.OpenRead(filename));
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;
        }
    }
}
