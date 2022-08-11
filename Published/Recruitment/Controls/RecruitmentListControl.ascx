<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecruitmentListControl.ascx.cs" Inherits="CanhCam.Web.RecruitmentUI.RecruitmentListControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<Site:RecruitmentDisplaySettings ID="displaySettings" runat="server" />
<asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
<div id="divPager" runat="server" class="pages productpager">
    <portal:gbCutePager ID="pgr" runat="server" />
</div>