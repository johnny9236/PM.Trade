using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSBCMerchant;
using PM.Utils.WebUtils;
using PM.Utils;
using System.Xml.Linq;

using javax.crypto.spec;
using System.Web;
using javax.crypto;
using java.security;
using java.net;
using sun.misc;
using netpay.merchant.crypto;
using PM.Utils.WCF;
using System.Globalization;
using System.Linq.Expressions;


namespace TradeTest
{

    class Program
    {
        static void Main(string[] args)
        {

        
            //ParameterExpression param2 = Expression.Parameter(typeof(int));
            //BlockExpression block2 = Expression.Block(
            //    new[] { param2 },
            //    Expression.AddAssign(param2, Expression.Constant(20)),
            //    param2
            //    );

            //try
            //{
                //LoopExpression loop = Expression.Loop(
                // Expression.Call(
                //null,
                //typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                //Expression.Constant("Hello"))
                //);

                //// 创建一个代码块表达式包含我们上面创建的loop表达式
                //BlockExpression block = Expression.Block(loop);

                //// 将我们上面的代码块表达式
                //Expression<Action> lambdaExpression = Expression.Lambda<Action>(block);
                //lambdaExpression.Compile().Invoke();
                //ParameterExpression number = Expression.Parameter(typeof(int), "number");

                //BlockExpression myBlock = Expression.Block(
                //    new[] { number },
                //    Expression.Assign(number, Expression.Constant(2)),
                //    Expression.AddAssign(number, Expression.Constant(6)),
                //    Expression.DivideAssign(number, Expression.Constant(2)));

                //Expression<Func<int>> myAction = Expression.Lambda<Func<int>>(myBlock);
                //Console.WriteLine(myAction.Compile()());
                // 4
                //ParameterExpression arrayExpr = Expression.Parameter(typeof(int[]), "Array");

                //ParameterExpression indexExpr = Expression.Parameter(typeof(int), "Index");

                //ParameterExpression valueExpr = Expression.Parameter(typeof(int), "Value");

                //Expression arrayAccessExpr = Expression.ArrayAccess(
                //    arrayExpr,
                //    indexExpr
                //); 
                //NewExpression newDictionaryExpression = Expression.New(typeof(Dictionary<int, string>));
                //Console.WriteLine(newDictionaryExpression.ToString());

                 

                 
            //}
            //catch (Exception ex)
            //{
            //    Console.Write(ex);
            //}



            string pwd = "lAAHebRE7uby3TlAa1YX";// "hyxcqzd201";
            string projectID = "7400C186-A082-4F2E-B14A-EA44A281888";
            int wdID = 87731;
            string dptID = "90";

            string url = @"http://218.108.28.44:80/zjkservice/service.asmx";
            string projectXml =
            @"<?xml version='1.0' encoding='gb2312' ?><data><project   projName='海盐测试002'    investType=''  projArea='330000'    investAmount='0'  ></project><invite   inviteName='海盐测试002'    inviteKind=''  investPart='10'    evaluatePlace='330000'    startTime='2014-02-20'  evaTime='2014-02-20'   endTime='2014-02-20'   expertNumber='10'   linkMan=''   linkmanIdCard=''    linkmanPhone=''  agentorgan='cs1209010'   submitMedia=''    alreadyDraw=''  deptID='10'    demo=''          ></invite><owner   ownerName='lxj_hy_011001'    ownerPhone=''  address=''   ></owner></data>";


            //@"<?xml version='1.0' encoding='gb2312' ?><data> <project projName='海盐demo' projNumber='zjtest001' investType='政府投资项目'  projArea='330000' investAmount='55'>       </project> 
            // <invite inviteName='海盐专家demo' inviteKind='公开招标'  investPart='100' evaluatePlace='330000'  startTime='2014-01-8'  evaTime='2014-01-10'  endTime='2014-01-15'  expertNumber='5' linkMan=''  linkmanIdCard='' linkmanPhone='057156298425' agentorgan='海盐测试代理'  submitMedia='媒体公司'     	alreadyDraw='0'  deptID='123'		demo='remark'     ></invite> 
            //  <owner ownerName='海盐专家测试业主' ownerPhone='05718954561' address='天堂' ></owner> </data>";


            //@"<?xml version='1.0' encoding='gb2312' ?><data>  <project projName='海盐测试' projNumber='pfhao' investType='政府投资项目'  projArea='330000' investAmount='55'></project><invite inviteName='海盐测试' inviteKind='公开招标'  investPart='100'     evaluatePlace='330000'     startTime='20141210'    evaTime='20141001'     endTime='20141010'  expertNumber='5'    linkMan=''linkmanIdCard=''linkmanPhone='057156298425'   agentorgan='海盐测试代理'    submitMedia='媒体公司'     	alreadyDraw='0'  deptID='主管部门'		demo='remark'  ></invite><owner ownerName='海盐测试业主'  ownerPhone='05718954561' address='天堂' ></owner></data>";
            string supManXml =
                @"<?xml version='1.0' encoding='gb2312' ?><SupMan   Name=''    Phone=''  Unit=''   ></SupMan>";

            //@"<?xml version='1.0' encoding='gb2312' ?><SupMan><Name>监督单位hy01</Name><Phone>13600010987</Phone><Unit> 监督单位hy</Unit></SupMan>";

            object[] parm = new object[] { pwd, projectXml, supManXml, "", wdID, projectID };
            //项目导入
            ////depid=178
            var rtnObj = WebServiceHelper.InvokeWebService(url, "inputProject", parm);

            string ttr = rtnObj.ToString();
            Console.Write("ddd");

            ////return;
            //string avodXml = @"<?xml version='1.0' encoding='gb2312' ?><data><unit  unitID='589612F7-CF5A-4779-B063-9F3C58A4C1B2'  unitName='浙江卧龙集团2'      eludeType='3'  ></unit><unit  unitID='589612F7-CF5A-4779-B063-9F3C58A4C1B4'  unitName='浙江卧龙集团3'      eludeType='2'  ></unit></data>";
            //object[] parmAvod = new object[] { pwd, avodXml, projectID };
            //var rstAvodObj = WebServiceHelper.InvokeWebService(url, "inputUnit", parmAvod);
            //var ss = rstAvodObj.ToString();





            ////返回结果
            //object[] rstParm = new object[] { pwd, dptID, projectID };
            //var rstObj = WebServiceHelper.InvokeWebService(url, "GetProjExpertListStr", rstParm);
            //var tt = rstObj.ToString();

            //Console.Write(tt);
            //return;


            //var model2 = new BOCQueryAccountDtlModel();
            //model2.BusType = "11";
            //model2.Direction = "2";


            //  var AptitudeCode = new AptitudeCode();

            //var EntrpriseInfo = new EntrpriseInfo(); 
            //var EnterpriseSpeciality = new EnterpriseSpeciality();
            //var Personnel = new Personnel();
            //var PersonnelSpeciality = n// DateTime.ParseExact(s, "yyyyMMddHHmmssfff", culture);ew PersonnelSpeciality();

            var newICBCRtnQueryInfo = new TradeTest.model.ICBCRtnQueryInfo();
      //      var newICBCQueryInfo = new TradeTest.model.ICBCQueryInfo();


            using (CodeFirstDbContext context = new CodeFirstDbContext())
            {
                //context.AptitudeCode.Add(AptitudeCode);
                //context.EntrpriseInfo.Add(EntrpriseInfo);
                //context.EnterpriseSpeciality.Add(EnterpriseSpeciality);
                //context.Personnel.Add(Personnel);
                //context.PersonnelSpeciality.Add(PersonnelSpeciality);

               // context.ICBCQueryInfo.Add(newICBCQueryInfo);
                context.ICBCRtnQueryInfo.Add(newICBCRtnQueryInfo);
                context.SaveChanges();
            };

            Console.WriteLine("成功添加用户，接下来将获取。。。");
            Console.ReadKey();

            #region  //黄梅
            //string transName = "EDFR";
            //string Plain = "MercCode=9999998885000013300|BeginDate=20130904|EndDate=20130904|AcctNo=100075230360016668";
            //string Signature = SignatureService.sign(Plain);
            //string xmlString = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><packet><transName>{0}</transName><Plain>{1}</Plain><Signature>{2}</Signature></packet>", transName, Plain, Signature);
            //string urlStr = @"https://pbank.psbc.com/payment/main";// @"http://103.22.255.193:7003/payment/main";
            //string contentStr = "text/xml";//"Content-Type=text/xml";


            ////string sendInfo = " transName=EDFR&Plain=" + Plain + "&Signature=" + Signature;
            ////contentStr = "";
            ////var rtnString = HttpTransfer.RequestPost(urlStr, contentStr, sendInfo, );
            //var rtnString = HttpTransfer.RequestPost(urlStr, contentStr, xmlString, Encoding.UTF8);


            //var tt=    PM.Utils.StringHelper.MD5String("abcdefA");
            //Console.Write(tt);


            //var key = "48060ab8d0a827b9adba32d9020111";
            //var ecs = "T4NJx%2FVgocRsLyQnrMZLyuQQkFzMAxQjdqyzf6pM%2Fcg%3D";
            //var source = "建设银行|4367888888888888888";
            //Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            //Encoding bg2312 = Encoding.GetEncoding("gb2312");
            //var tempString = HttpUtility.UrlDecode(ecs, iso);
            //String basedString = tempString.Replace(",", "+");
            //byte[] tempBytes = Convert.FromBase64String(basedString); 
            //////
            //var escKey = key.Substring(0, 8);//key
            //var wrapKey = iso.GetBytes(escKey); 
            //MCipherDecode mcd = new MCipherDecode(key);//设置密钥
            //var decodedString = mcd.getDecodeString(ecs);//解密
            //byte[] tempByte = iso.GetBytes(decodedString);
            //var a = Encoding.GetEncoding("GBK").GetString(tempByte);
            //string url = "http://192.168.0.10:9008/PaymentService";
            //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
            //var model = new PM.PaymentModel.PayStartModel();
            //model.BusinessFunNo = "1111";// "LPSBBCB2CPay";//
            //model.InstitutionID = "000280";
            //model.BusnissID = "BusnissID";
            //model.OrderNo = "0000000000";
            //model.PayAccountDbBank = "";
            //model.PayAcountName = "";
            //model.PayAcountNo = "";
            //model.PayBankAccountType = "12";
            //model.PayBankID = "700";
            //model.PayCity = "";
            //model.PayCur = "Cur";
            //model.PayerID = "b58e5414-66fb-4831-b5c9-d87b5334e5c2";
            //model.PayerName = "xxx公司";
            //model.PayMoney = 100;
            //model.PayOpenBankNo = "1234567890";
            //model.PayProvince = "";
            //model.PaySettingAccNo = "0001";
            //model.RateMoney = 0;
            //model.ReceiptAccountDbBank = "交通银行金华分行";
            //model.ReceiptAcountName = "代理公共资源投标席位费清算户";
            //model.ReceiptAcountNo = "337001012620196003899";
            //model.ReceiptBankAccountType = "12";
            //model.ReceiptBankID = "700";
            //model.ReceiptCity = "";
            //model.ReceiptCur = "";
            //model.ReceiptOpenBankNo = "301338000023";
            //model.ReceiptProvince = "";
            //model.ReceiptSettingAccNo = "0001";
            //model.Remark = "demo";
            //model.TradeInfo = "测试费用";
            //model.PayFee = 0;

            //var pay = info.DoPay(model);
            //Console.WriteLine(pay);
            //Console.ReadLine();
            #endregion
        }

    }

    public class MCipherDecode
    {

        static MCipherDecode()
        {
            if (Security.getProvider("BC") == null)
            {
                //Security.addProvider(new  ());
            }
        }
        private String encryptKey = "12345678";

        public MCipherDecode(String key)
        {
            encryptKey = key.Substring(0, 8);
        }

        public String getEncryptKey()
        {
            return encryptKey;
        }

        public void setEncryptKey(String encryptKey)
        {
            this.encryptKey = encryptKey.Substring(0, 8);
        }

        private static byte[] getSrcBytes(byte[] srcBytes, byte[] wrapKey)
        {

            SecretKeySpec key = new SecretKeySpec(wrapKey, "DES");

            Cipher cipher = Cipher.getInstance("DES/ECB/PKCS5Padding", "BC");

            cipher.init(Cipher.DECRYPT_MODE, key);

            byte[] cipherText = cipher.doFinal(srcBytes);


            return cipherText;
        }





        public static byte[] DecodeBase64String(String base64Src)
        {
            BASE64Decoder de = new BASE64Decoder();
            byte[] base64Result = de.decodeBuffer(base64Src);
            return base64Result;

        }

        public String getDecodeString(String urlString)
        {
            String tempString = URLDecoder.decode(urlString, "ISO-8859-1");
            String basedString = tempString.Replace(",", "+");
            byte[] tempBytes = DecodeBase64String(basedString);
            byte[] tempSrcBytes = getSrcBytes(tempBytes, Encoding.GetEncoding("ISO-8859-1").GetBytes(encryptKey));
            return Encoding.GetEncoding("ISO-8859-1").GetString(tempSrcBytes);

        }

    }

    public class ExpDemo
    {
        public string Model { get; set; }

        public int FunNum { get; set; }
    }
}
