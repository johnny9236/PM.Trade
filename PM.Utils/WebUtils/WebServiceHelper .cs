using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.IO;
using System.Net;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Microsoft.CSharp;
namespace PM.Utils.WebUtils
{
    /// <summary>
    ///WebServiceHelper 的摘要说明
    /// </summary>
    public class WebServiceHelper
    {
        public WebServiceHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary> 
        /// 动态调用WebService 需要Soap验证的
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="classname">类名</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object GetCALogin(string url, string uid, string SYS_ID, string SYS_SN, string MethName, object[] args)
        {

            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            string classname = WebServiceHelper.GetClassName(url);

            //获取服务描述语言(WSDL) 
            WebClient wc = new WebClient();

            Stream stream = wc.OpenRead(url + "?wsdl");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码 
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            ICodeCompiler icc = csc.CreateCompiler();
            //设定编译器的参数 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            // cplist.ReferencedAssemblies.Add("System.Web.Services.Protocols.dll");
            ////编译代理类 
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }

            try
            {
                //生成代理实例,并调用方法 
                //实例化服务
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);

                object obj = Activator.CreateInstance(t);

                //实例化CredentialSoapHeader
                Type head = assembly.GetType(@namespace + "." + "CredentialSoapHeader", true, true);
                object obj1 = Activator.CreateInstance(head);
                System.Reflection.FieldInfo ApplicationID = head.GetField("ApplicationID");
                System.Reflection.FieldInfo ApplicationSN = head.GetField("ApplicationSN");
                System.Reflection.FieldInfo userID = head.GetField("UserName");
                ApplicationID.SetValue(obj1, SYS_ID);
                ApplicationSN.SetValue(obj1, SYS_SN);
                userID.SetValue(obj1, uid);
                //获取服务CredentialSoapHeaderValue属性并赋值    
                System.Reflection.FieldInfo mi1 = t.GetField("CredentialSoapHeaderValue");
                mi1.SetValue(obj, obj1);
                //调用方法
                System.Reflection.MethodInfo mi = t.GetMethod(MethName);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 动态调用WebService 
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }
        /// <summary> 
        /// 动态调用WebService 需要soap验证的
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object InvokeWebService(string url, string UsName, string UsPass, string methodname, object[] args)
        {
            return InvokeWebService(url, UsName, UsPass, null, methodname, args);
        }
        /// <summary> 
        /// 动态调用WebService 
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="classname">类名</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (classname == null || classname == "")
            {
                classname = WebServiceHelper.GetClassName(url);
            }
            //获取服务描述语言(WSDL) 
            WebClient wc = new WebClient();

            Stream stream = wc.OpenRead(url + "?wsdl");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码 
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            ICodeCompiler icc = csc.CreateCompiler();
            //设定编译器的参数 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            ////编译代理类 
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            try
            {
                //生成代理实例,并调用方法 
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary> 
        /// 动态调用WebService 需要Soap验证的
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="classname">类名</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object InvokeWebService(string url, string UsName, string UsPass, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (classname == null || classname == "")
            {
                classname = WebServiceHelper.GetClassName(url);
            }
            //获取服务描述语言(WSDL) 
            WebClient wc = new WebClient();

            Stream stream = wc.OpenRead(url + "?wsdl");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码 
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            ICodeCompiler icc = csc.CreateCompiler();
            //设定编译器的参数 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            // cplist.ReferencedAssemblies.Add("System.Web.Services.Protocols.dll");
            ////编译代理类 
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            try
            {
                //生成代理实例,并调用方法 
                //实例化服务
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);

                //实例化CredentialSoapHeader
                Type head = assembly.GetType(@namespace + "." + "CredentialSoapHeader", true, true);
                object obj1 = Activator.CreateInstance(head);
                System.Reflection.FieldInfo UserName = head.GetField("UserName");
                System.Reflection.FieldInfo UserPass = head.GetField("UserPass");
                UserName.SetValue(obj1, UsName);
                UserPass.SetValue(obj1, UsPass);

                //获取服务CredentialSoapHeaderValue属性并赋值    
                System.Reflection.FieldInfo mi1 = t.GetField("CredentialSoapHeaderValue");
                mi1.SetValue(obj, obj1);

                //调用方法
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static string GetClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}

