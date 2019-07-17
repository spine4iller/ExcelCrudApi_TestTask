using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Web.Http.Results;
using DataLibrary;
using System.Net.Http.Headers;

namespace API.Controllers
{
   
    [RoutePrefix("api/excel")]
    public class ExcelController : ApiController
    {
       
        DataService ds = new DataService();
        [HttpPost, Route("edit")]
        public string editRow(ExcelDataRow row)
        {            
            return ds.edit(row);
        }

        [HttpGet, Route("listAll")]
        public List<RowInfo> listRows()
        {
            return ds.listAll();            
        }

[HttpGet, Route("delete/{id}")]
        public string delete(int id)
        {  
            return ds.delete(id);            
        }

        [HttpPost, Route("uploadFile")]
        public async Task            <JsonResult<string>>
            uploadFile()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                // throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                return Json("multipart expected");
            }
            
            var folderPart = DateTime.Today.ToShortDateString();
            string root = @"c:\upload\" + folderPart;//
           // HttpContext.Current.Server.MapPath("~/App_Data");
            Directory.CreateDirectory(root);
            var provider = new //MultipartFormDataStreamProvider(root);// 
            CustomMultipartFormDataStreamProvider(root);
            
            var fname = "";
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
             //   fs.logString(JsonConvert.SerializeObject(provider.FormData), MethodBase.GetCurrentMethod().Name);
               // desc.type = provider.FormData.GetValues("type")[0];

               // if (provider.FormData.AllKeys.Contains("isPotential") )
               
                MultipartFileData file = provider.FileData[0];
                
                     fname = Path.GetFileName(file.LocalFileName);                
               
                    var location = root + $@"\{fname}";
                ds.saveRowsFromExcel(location);
                return                Json("file upload success");
                
            }
            catch (Exception e)
            {  
                return Json($"file upload error:{e.Message}");
            }

        }
        class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {           
            public CustomMultipartFormDataStreamProvider(string path) : base(path)
            {

            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }
    }
}
