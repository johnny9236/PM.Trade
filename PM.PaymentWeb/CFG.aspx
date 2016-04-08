<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CFG.aspx.cs" Inherits="CFG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/themes/Custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="js/json2.js" type="text/javascript"></script>
</head>
<body>
    <%--<form id="form1" runat="server">--%>
    <div style="margin: 10px 0;">
    </div>
    <table id="dg" class="easyui-datagrid" title="配置信息" style="width: 1024px; height: auto"
        data-options="
				iconCls: 'icon-edit',
				singleSelect: true,
				toolbar: '#tb',
				url: 'CfgHandler.ashx?action=List',
				onClickRow: onClickRow
			">
        <thead>
            <tr>
                <th data-options="field:'ID',width:0,hidden:true">
                    ID
                </th>
                <th data-options="field:'ProtocolsWayDes',width:0,hidden:true">
                    ProtocolsWayDes
                </th>
                <th data-options="field:'OprationTypeDes',width:0,hidden:true">
                    OprationTypeDes
                </th>
                <th data-options="field:'BusinessKindDes',width:0,hidden:true">
                    BusinessKindDes
                </th>
                <th data-options="field:'FileTypeDes',width:0,hidden:true">
                    FileTypeDes
                </th>
                <th data-options="field:'ActionTypeDes',width:0,hidden:true">
                    ActionTypeDes
                </th>
                <th data-options="field:'BusinessNo',width:100,editor:'text'">
                    业务号
                </th>
                <th data-options="field:'ProtocolsWay',width:60, formatter:function(value,row){   
                return       row.ProtocolsWayDes;
                 }, editor:{ type:'combobox', options:{ valueField:'protocolsWayID',
                    textField:'protocolsWayName', url:'CustomData/ProtocolsWay.ashx', required:true } }">
                    协议类型
                </th>
                <th data-options="field:'OprationType',width:60, formatter:function(value,row){ 
                return           row.OprationTypeDes;
                     }, editor:{ type:'combobox', options:{ valueField:'oprationTypeID',
                    textField:'oprationTypeName', url:'CustomData/OprationType.ashx', required:true } }">
                    操作类型
                </th>
                <th data-options="field:'ActionType',width:100, formatter:function(value,row){  
                return     row.ActionTypeDes; }, editor:{ type:'combobox', options:{ valueField:'actionTypeID',
                    textField:'actionTypeName', url:'CustomData/ActionType.ashx', required:true
                    } }">
                    动作类型
                </th>
                <th data-options="field:'BusinessKind',width:80, formatter:function(value,row){  
                return         row.BusinessKindDes; }, editor:{ type:'combobox', options:{ valueField:'businessTypeID',
                    textField:'businessTypeName', url:'CustomData/BusinessType.ashx', required:true
                    } }">
                    业务类型
                </th>
                <th data-options="field:'RequestURL',width:80,editor:'text'">
                    请求URL
                </th>
                <th data-options="field:'NotificationURL',width:80,editor:'text'">
                    通知URL
                </th>
                <th data-options="field:'NotificationBgURL',width:80,editor:'text'">
                   后台通知URL
                </th>
                <th data-options="field:'FileType',width:80, formatter:function(value,row){ 
                     return           row.FileTypeDes; }, editor:{ type:'combobox', options:{ valueField:'fileTypeID',
                    textField:'fileTypeName', url:'CustomData/FileType.ashx' } }">
                    文件读取类型
                </th>
                <th data-options="field:'IP',width:60,editor:'text'">
                    IP
                </th>
                <th data-options="field:'Port',width:20,editor:'text'">
                    端口
                </th>
                <th data-options="field:'RootFilePath',width:60,editor:'text'">
                    访问文件目录
                </th>
                <th data-options="field:'FtpUserName',width:40,editor:'text'">
                    FTP用户名
                </th>
                <th data-options="field:'FtpUserPwd',width:40,editor:'text'">
                    FTP密码
                </th>
            </tr>
        </thead>
    </table>
    <div id="tb" style="height: auto">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true"
            onclick="append()">添加</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                data-options="iconCls:'icon-remove',plain:true" onclick="removed()">删除</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true"
            onclick="accept()">保存</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                data-options="iconCls:'icon-undo',plain:true" onclick="reject()">撤销</a>
    </div>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dg').datagrid('validateRow', editIndex)) {
                var edProtocolsWay = $('#dg').datagrid('getEditor', { index: editIndex, field: 'ProtocolsWay' });
                var protocolsWayID = $(edProtocolsWay.target).combobox('getValue');
                var protocolsWayDes = $(edProtocolsWay.target).combobox('getText');
                $('#dg').datagrid('getRows')[editIndex]['ProtocolsWay'] = protocolsWayID;
                $('#dg').datagrid('getRows')[editIndex]['ProtocolsWayDes'] = protocolsWayDes;

                var edOprationType = $('#dg').datagrid('getEditor', { index: editIndex, field: 'OprationType' });
                var oprationTypeID = $(edOprationType.target).combobox('getValue');
                var oprationTypeDes = $(edOprationType.target).combobox('getText');
                $('#dg').datagrid('getRows')[editIndex]['OprationType'] = oprationTypeID;
                $('#dg').datagrid('getRows')[editIndex]['OprationTypeDes'] = oprationTypeDes;

                var edBusinessKind = $('#dg').datagrid('getEditor', { index: editIndex, field: 'BusinessKind' });
                var businessTypeID = $(edBusinessKind.target).combobox('getValue');
                var businessTypeDes = $(edBusinessKind.target).combobox('getText');
                $('#dg').datagrid('getRows')[editIndex]['BusinessKind'] = oprationTypeID;
                $('#dg').datagrid('getRows')[editIndex]['BusinessKindDes'] = businessTypeDes;

                var edfileType = $('#dg').datagrid('getEditor', { index: editIndex, field: 'FileType' });
                var fileTypeID = $(edfileType.target).combobox('getValue');
                var fileTypeDes = $(edfileType.target).combobox('getText');
                $('#dg').datagrid('getRows')[editIndex]['FileType'] = fileTypeID;
                $('#dg').datagrid('getRows')[editIndex]['FileTypeDes'] = fileTypeDes;


                var edactionType = $('#dg').datagrid('getEditor', { index: editIndex, field: 'ActionType' });
                var actionTypeID = $(edactionType.target).combobox('getValue');
                var actionTypeDes = $(edactionType.target).combobox('getText');
                $('#dg').datagrid('getRows')[editIndex]['ActionType'] = actionTypeID;
                $('#dg').datagrid('getRows')[editIndex]['ActionTypeDes'] = actionTypeDes;




                $('#dg').datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function onClickRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dg').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
                    editIndex = index;
                } else {
                    $('#dg').datagrid('selectRow', editIndex);
                }
            }
        }
        function append() {
            if (endEditing()) {
                $('#dg').datagrid('appendRow', { ID: newGuid() });
                editIndex = $('#dg').datagrid('getRows').length - 1;
                $('#dg').datagrid('selectRow', editIndex)
						.datagrid('beginEdit', editIndex);
            }
        }
        function removed() { 
            if (editIndex == undefined) { return }
            $('#dg').datagrid('cancelEdit', editIndex)
					.datagrid('deleteRow', editIndex);
            editIndex = undefined;
        }
        function accept() {
            if (endEditing()) {
                //  $('#dg').datagrid('acceptChanges');
                if ($('#dg').datagrid('getChanges').length) {
                    //
                    var inserted = $('#dg').datagrid('getChanges', "inserted");
                    var deleted = $('#dg').datagrid('getChanges', "deleted");
                    var updated = $('#dg').datagrid('getChanges', "updated");
                    var effectRow = new Object();
                    if (inserted.length) {
                        effectRow["inserted"] = JSON.stringify(inserted);
                    }
                    if (deleted.length) {
                        effectRow["deleted"] = JSON.stringify(deleted);
                    }
                    if (updated.length) {
                        effectRow["updated"] = JSON.stringify(updated);
                    }
                    $.post("CfgHandler.ashx?action=Save", effectRow, function (rsp) {
                        if (rsp.toLocaleLowerCase() == "true") {
                            $.messager.alert("提示", "提交成功！");
                            $('#dg').datagrid('acceptChanges');
                        } else {
                            $.messager.alert("提示", "提交失败！");
                        }
                    }
                    ).error(function () {
                        $.messager.alert("提示", "提交错误了！");
                    });
                }
            }
        }
        function reject() {
            $('#dg').datagrid('rejectChanges');
            editIndex = undefined;
        }
        function getChanges() {
            var rows = $('#dg').datagrid('getChanges');
            alert(rows.length + ' rows are changed!');
        }

        function newGuid() {
            var guid = "";
            for (var i = 1; i <= 32; i++) {
                var n = Math.floor(Math.random() * 16.0).toString(16);
                guid += n;
                if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                    guid += "-";
            }
            return guid;
        }

        //菜单去掉
        //        $('#dg').datagrid({
        //            onRowContextMenu: function (e, rowIndex, field) {
        //                e.preventDefault();
        //                $('#dg').datagrid('selectRow', rowIndex)
        //                if (!$('#tmenu').length) {
        //                    createColumnMenu();
        //                }
        //                $('#tmenu').menu('show', {
        //                    left: e.pageX,
        //                    top: e.pageY
        //                });
        //            }
        //        });
        ////////////菜单
        function createColumnMenu() {
            var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
            var fields = $('#dg').datagrid('getColumnFields');
            $('<div iconCls="icon-ok"/>').html("测试").appendTo(tmenu);
            tmenu.menu({
                onClick: function (item) {
                    var selectRow = $('#dg').datagrid('getSelected');
                    alert(selectRow["ID"]);
                    $.post("CfgInfo.ashx?action=Test&ID=" + selectRow["ID"], function (rsp) {
                        if (rsp == "True") {
                            $.messager.alert("提示", "测试成功！");
                        } else {
                            $.messager.alert("提示", "测试失败！");
                        }
                    }
                    ).error(function () {
                        $.messager.alert("提示", "测试错误了！");
                    });
                    //
                }
            });
        }


    
    </script>
    <%--  </form>--%>
</body>
</html>
