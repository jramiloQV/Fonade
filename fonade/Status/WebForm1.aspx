<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Fonade.Status.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>


            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upTest">
                <ProgressTemplate>
                Updating!
                </ProgressTemplate>
            </asp:UpdateProgress>


        <asp:UpdatePanel ID="upTest" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
            <ContentTemplate>


            <asp:Label ID="lblResults" runat="server"></asp:Label><br /><br />
            <asp:Button ID="btnAsync" Text="Asynch Post" runat="server" OnClick="btnAsync_Click" />
            <asp:Button ID="btnFullPost" Text="Full Post" runat="server" 
                OnClick="btnFullPost_Click" OnClientClick="ShowProgress();"/>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAsync" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnFullPost" />
            </Triggers>
        </asp:UpdatePanel>
    

    </form>
    

    <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(UpdateProgress1.ClientID); %>').style.display = "inline";
    }
    

    </script>
    

</body>
</html>
