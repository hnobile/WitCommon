<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WIT.Common.AddThis._Default" %>
<%@ Register Assembly="WIT.Common.AddThis" Namespace="WIT.Common.AddThis.Controls" TagPrefix="AddThisControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <AddThisControls:AddThisButton ID="AddThisButton1" runat="server" 
            Separator="|" TextMore="More" SelectedLanguage="en" 
            Title="Rodrigo">
        </AddThisControls:AddThisButton>
        
        <AddThisControls:AddThisButton ID="AddThisButton2" runat="server" 
            Separator="->" TextMore="Mas" SelectedLanguage="en" 
            Title="Rodrigo">
        </AddThisControls:AddThisButton>
    </div>
    </form>
</body>
</html>
