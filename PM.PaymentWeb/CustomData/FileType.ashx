<%@ WebHandler Language="C#" Class="FileType" %>

using System;
using System.Web;

public class FileType : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string fileTypeStr = "[{\"fileTypeID\":\"File\",\"fileTypeName\":\"File\"},{\"fileTypeID\":\"FTP\",\"fileTypeName\":\"FTP\"}]";
        context.Response.Write(fileTypeStr); 
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}