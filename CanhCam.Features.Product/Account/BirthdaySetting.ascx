<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BirthdaySetting.ascx.cs" Inherits="CanhCam.Web.AccountUI.BirthdaySetting" %>

<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <div class="row">
			<div class="col-xs-12 col-lg-8">
				<div class="birthday-wrap">
					<div class="row">
						<div class="year col-xs-12 col-md-4">
							<asp:DropDownList ID="ddYear" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddYear_SelectedIndexChanged" AutoPostBack="True">
							</asp:DropDownList>
						</div>
						<div class="month col-xs-12 col-md-4">
							<asp:DropDownList ID="ddMonth" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged" AutoPostBack="True">
							</asp:DropDownList>
						</div>
						<div class="day col-xs-12 col-md-4">
							<asp:DropDownList ID="ddDay" AppendDataBoundItems="true" runat="server">
							</asp:DropDownList>
						</div>
					</div>
				</div>
			</div>
        </div>
        <asp:RequiredFieldValidator ControlToValidate="ddDay" InitialValue="0"
			ValidationGroup="profile" SkinID="Registration"
			ID="reqDay" Display="Dynamic" SetFocusOnError="true" runat="server" />
     </ContentTemplate>   
</asp:UpdatePanel>