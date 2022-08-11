/// Created:			    2013-01-01
/// Last Modified:		    2013-01-01
/// primarily a base class for other panel controls
/// by sub classing we can configure properties on different sub classes differently from theme.skin file
/// this technique allows a nice way to use additional markup or less markup as needed
/// to support different versions of Artisteer skins, jqueryui skin, and makes it possible to use html 5 structural elements like article
/// and is intended to help us with our mobile strategy by making the markup rendering more flexible

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CanhCam.Web.Framework;

namespace CanhCam.Web.UI
{
    public class BasePanel : Panel
    {
        private string element = "div";
        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        private string extraCssClasses = string.Empty;
        public string ExtraCssClasses
        {
            get { return extraCssClasses; }
            set { extraCssClasses = value; }
        }

        private bool renderContentsOnly = false;
        public bool RenderContentsOnly
        {
            get { return renderContentsOnly; }
            set { renderContentsOnly = value; }
        }

        private string literalExtraTopContent = string.Empty;
        public string LiteralExtraTopContent
        {
            get { return literalExtraTopContent; }
            set { literalExtraTopContent = value; }
        }

        private string literalExtraBottomContent = string.Empty;
        public string LiteralExtraBottomContent
        {
            get { return literalExtraBottomContent; }
            set { literalExtraBottomContent = value; }
        }

        private bool detectSideColumn = false;
        public bool DetectSideColumn
        {
            get { return detectSideColumn; }
            set { detectSideColumn = value; }
        }

        private string columnId = UIHelper.CenterColumnId;

        private string sideColumnxtraCssClasses = string.Empty;
        public string SideColumnxtraCssClasses
        {
            get { return sideColumnxtraCssClasses; }
            set { sideColumnxtraCssClasses = value; }
        }

        private string sideColumnLiteralExtraTopContent = string.Empty;
        public string SideColumnLiteralExtraTopContent
        {
            get { return sideColumnLiteralExtraTopContent; }
            set { sideColumnLiteralExtraTopContent = value; }
        }

        private string sideColumnLiteralExtraBottomContent = string.Empty;
        public string SideColumnLiteralExtraBottomContent
        {
            get { return sideColumnLiteralExtraBottomContent; }
            set { sideColumnLiteralExtraBottomContent = value; }
        }

        private bool renderId = true;
        public bool RenderId
        {
            get { return renderId; }
            set { renderId = value; }
        }

        private bool dontRender = false;
        public bool DontRender
        {
            get { return dontRender; }
            set { dontRender = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (detectSideColumn)
            {
                columnId = this.GetColumnId();

                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:

                        //if (sideColumnxtraCssClasses.Length > 0)
                        //{
                            extraCssClasses = sideColumnxtraCssClasses;
                        //}

                        //if (sideColumnLiteralExtraTopContent.Length > 0)
                        //{
                            literalExtraTopContent = sideColumnLiteralExtraTopContent;
                        //}

                        //if (sideColumnLiteralExtraBottomContent.Length > 0)
                        //{
                            literalExtraBottomContent = sideColumnLiteralExtraBottomContent;
                        //}

                        break;

                    case UIHelper.CenterColumnId:
                    default:
                        // nothing to do here

                        break;
                }
            }
            
            if (extraCssClasses.Length > 0)
            {
                if (CssClass.Length > 0)
                {
                    //CssClass += " " + extraCssClasses;
                    if (!CssClass.Contains(extraCssClasses))
                    {
                        CssClass = extraCssClasses + " " + CssClass;
                    }
                }
                else
                {
                    CssClass = extraCssClasses;
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (dontRender) { return; }

            if (!renderContentsOnly)
            {
                if (renderId)
                {
                    //writer.Write("<" + element + " id='" + this.ClientID + "' class='" + CssClass + "'>\n");
                    writer.Write("<");
                    writer.Write(element);
                    writer.Write(" id='" + this.ClientID + "' class='" + CssClass + "'>\n");
                }
                else
                {
                    //writer.Write("<" + element + " class='" + CssClass + "'>\n");
                    writer.Write("<");
                    writer.Write(element);
                    writer.Write(" class='" + CssClass + "'>\n");
                    
                    //writer.WriteBeginTag(element);
                    //writer.WriteAttribute("class", CssClass);
                }
            }

            if (literalExtraTopContent.Length > 0)
            {
                writer.Write(literalExtraTopContent);
            }

            base.RenderContents(writer);

            if (literalExtraBottomContent.Length > 0)
            {
                writer.Write(literalExtraBottomContent);
            }

            if (!renderContentsOnly)
            {
                writer.Write("\n</" + element + ">");
                //writer.WriteEndTag(element);
            }
        }
    }
}