using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
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
    public partial class NhanVienOfferControl : SiteModuleControl
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
            NhanVien nv = null;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<NhanVienOffer></NhanVienOffer>");
            XmlElement root = doc.DocumentElement;
            XmlHelper.AddNode(doc, root, "ModuleTitle", "Mua sản phẩm");
            int proid = WebUtils.ParseInt32FromQueryString("proid", -1);
            int affid = WebUtils.ParseInt32FromQueryString("affid", -1);
            int userid = SiteUtils.GetCurrentSiteUser().UserId;
            XmlHelper.AddNode(doc, root, "affid", affid.ToString());
            XmlHelper.AddNode(doc, root, "userid", userid.ToString());
            nv = new NhanVien(proid);
            if (nv != null)
            {
                XmlElement NhanVienXml = doc.CreateElement("NhanVien");
                root.AppendChild(NhanVienXml);
                XmlHelper.AddNode(doc, NhanVienXml, "NVId", nv.ID.ToString());
                XmlHelper.AddNode(doc, NhanVienXml, "Id", nv.Code);
                XmlHelper.AddNode(doc, NhanVienXml, "Name", nv.Name);
                XmlHelper.AddNode(doc, NhanVienXml, "DateOfBirth", nv.DateOfBirth.ToString());
                XmlHelper.AddNode(doc, NhanVienXml, "BirthPlace", nv.BirthPlace);
                XmlHelper.AddNode(doc, NhanVienXml, "Address", nv.Address);
                XmlHelper.AddNode(doc, NhanVienXml, "DateStart", nv.DateStart.ToString());
                XmlHelper.AddNode(doc, NhanVienXml, "Salary", ProductHelper.FormatPrice(nv.Salary));
                XmlHelper.AddNode(doc, NhanVienXml, "CitizenIdentification", nv.CitizenIdentification);
                XmlHelper.AddNode(doc, NhanVienXml, "Image", nv.Image);

                XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("phongban", ModuleConfiguration.XsltFileName), doc);
            }
        }


    }
}