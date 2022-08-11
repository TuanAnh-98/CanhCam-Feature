using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.AffiliateUI
{
    public partial class ProductListControl : SiteModuleControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateList));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<ProductList></ProductList>");
            XmlElement root = doc.DocumentElement;
            XmlHelper.AddNode(doc, root, "ModuleTitle", "Danh sách sản phẩm");

            var prolist = AffiliateProduct.GetPageAdv();
            foreach (var item in prolist)
            {

                XmlElement ProductListXml = doc.CreateElement("Products");
                root.AppendChild(ProductListXml);
                XmlHelper.AddNode(doc, ProductListXml, "ProductID", item.ProductId.ToString());
                XmlHelper.AddNode(doc, ProductListXml, "Title", item.Title);
                XmlHelper.AddNode(doc, ProductListXml, "Code", item.Code);
                XmlHelper.AddNode(doc, ProductListXml, "StockQuantity", item.StockQuantity.ToString());
                XmlHelper.AddNode(doc, ProductListXml, "Price", item.Price.ToString());
                XmlHelper.AddNode(doc, ProductListXml, "OldPrice", item.OldPrice.ToString());
                XmlHelper.AddNode(doc, ProductListXml, "FileAttachment", item.FileAttachment);
            }
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("phongban", ModuleConfiguration.XsltFileName), doc);
        }
    }
}