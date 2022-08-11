<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PageNotFound.aspx.cs" Inherits="CanhCam.Web.PageNotFoundPage" %>
 
    <asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
    <asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

        <style>
            #notfound {
                display: flex;
                justify-content: center;
                vertical-align: middle;
                text-align: center;
                padding: 80px 0;
                color: #000;
                font-size: 0.875rem;
                font-weight: 700;
                margin-bottom: 20px;
                margin-top: 0px;
                line-height: 1.5;
            }

            .notfound .notfound-404 h1 {
                font-size: 8rem;
                margin: 0px;
                font-weight: 900;
                background: linear-gradient(90deg,#f05a94,#bc0049);
                -webkit-background-clip: text;
                -webkit-text-fill-color: transparent;
                background-size: cover;
                background-position: center;
            }

            .notfound h2 {
                color: #000;
                font-size: 2.2rem;
                font-weight: 700;
                text-transform: uppercase;
                margin-top: 0;
            }

            .notfound a {
                margin-top: 15px;
                font-size: 1.25rem;
                text-decoration: none;
                text-transform: uppercase;
                background: linear-gradient(90deg,#f05a94,#bc0049);
                display: inline-block;
                padding: 15px 30px;
                border-radius: 40px;
                color: #fff;
                font-weight: 700;
                -webkit-box-shadow: 0px 4px 15px -5px #bc0049;
                box-shadow: 0px 4px 15px -5px #bc0049;
            }

            .notfound a:hover,
            .notfound a:focus {
                color: #fff;
            }
        </style>
        <div class="container">
            <div id="notfound">
                <div class="notfound">
                    <div class="notfound-404">
                        <h1>Oops!</h1>
                    </div>
                    <h2>
                        <asp:Literal runat="server" Text="<%$Resources:Resource, PageNotFoundLabel %>" />
                    </h2>
                    <p>
                        <asp:Literal runat="server" Text="<%$Resources:Resource, PageNotFoundNoteLabel %>" />
                    </p>
                    <a href="/">
                        <asp:Literal runat="server" Text="<%$Resources:Resource, PageNotFoundButton %>" />
                    </a>
                </div>
            </div>
        </div>



        <div class="errorPage" style="display:none;">
            <p class="name">404</p>
            <p class="description">
                <asp:Literal ID="litTitle" runat="server" />
            </p>
            <p>
                <asp:HyperLink ID="hplHomepage" CssClass="btn btn-warning" runat="server" />
                <asp:Literal ID="litErrorMessage" Visible="false" runat="server" EnableViewState="false" />
            </p>
        </div>
    </asp:Content>
    <asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
    <asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />