<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecruitmentViewControl.ascx.cs"
    Inherits="CanhCam.Web.RecruitmentUI.RecruitmentViewControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<portal:ContentExpiredLabel ID="expired" runat="server" EnableViewState="false" Visible="false" />
<asp:Panel ID="pnlInnerWrap" runat="server">
    <Site:RecruitmentDisplaySettings ID="displaySettings" runat="server" />
    <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
    <div id="divPager" runat="server" class="pages productdetailpager">
        <portal:gbCutePager ID="pgr" runat="server" />
    </div>
</asp:Panel>