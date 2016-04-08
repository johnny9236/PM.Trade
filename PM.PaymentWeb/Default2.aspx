<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
</head>
<body>
  <%--  <form id="form1" runat="server">--%>
    <div>
        <div id="show" runat="server">
        </div>
        <div>
            <textarea cols="10" rows="10" id="textareaDataContent"></textarea><br />
            <input type="button" name="btnGetData" id="btnGetData" value="Get Data From ASP.NET Web API" />
        </div>
        <script type="text/javascript">

//            $(document).ready(function () {
//                $("#btnGetData").bind("click", AjaxGetData());
//            });

            function AjaxGetData() {
                var hash = "8f9964a615a471be636b7c5bc68cc4ce:e10adc3949ba59abbe56e057f20f883e"

                var ticket = "Basic " + hash;
                $.ajax({
                    url: "http://localhost:10573/api/testjy/",
                    cache: false,
                    dataType:'JSONP',
//                    beforeSend: function (jqXHR, settings) {
//                        jqXHR.setRequestHeader('Authorization', ticket);
                    //                    },
                 headers : { Authorization : ticket } ,
                    statusCode: {
                        401: function () {
                            window.location.href = '/home/login';
                        },
                        200: function (data) {
                            $("#textareaDataContent").text(data);
                        }
                    }
                });
            }
        </script>

    </div>
  <%--  </form>--%>
</body>
</html>
