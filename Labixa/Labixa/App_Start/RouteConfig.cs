using Labixa.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Labixa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("profile", "thong-tin-ca-nhan", new { controller = "Profile", action = "Index", slug = UrlParameter.Optional });
            routes.MapRoute("bds", "bat-dong-san", new { controller = "Profile", action = "MyProperty", slug = UrlParameter.Optional });
            routes.MapRoute("csh", "chu-so-huu", new { controller = "Owner", action = "Index", slug = UrlParameter.Optional });
            routes.MapRoute("ctcsh", "chi-tiet-chu-so-huu/{Id}", new { controller = "Owner", action = "EditHolder", Id = UrlParameter.Optional });
            routes.MapRoute("dscsh", "danh-sach-san-pham/{Id}", new { controller = "Owner", action = "ListProperty", Id = UrlParameter.Optional });
            routes.MapRoute("tcsh", "tao-chu-so-huu", new { controller = "Owner", action = "CreateHolder", slug = UrlParameter.Optional });
            routes.MapRoute("bdscd", "bat-dong-san-cho-duyet", new { controller = "Profile", action = "WaitingProperty", slug = UrlParameter.Optional });
            routes.MapRoute("editbds", "dang-bat-dong-san/{Id}", new { controller = "Profile", action = "SubmitProperty", Id = UrlParameter.Optional });
            routes.MapRoute("search", "tim-kiem/{slug}", new { controller = "Home", action = "FindProduct", slug = UrlParameter.Optional });
            routes.MapRoute("submitbds", "dang-bat-dong-san", new { controller = "Profile", action = "SubmitProperty", slug = UrlParameter.Optional });
            routes.MapRoute("chothue", "cho-thue/{slug}", new { controller = "ProductHome", action = "ChoThueQuan", slug = UrlParameter.Optional });
            routes.MapRoute("choban", "cho-ban/{slug}", new { controller = "ProductHome", action = "ChoBanQuan", slug = UrlParameter.Optional });
            routes.MapRoute("chitiet", "san-pham/{slug}", new { controller = "ProductHome", action = "SanPham", slug = UrlParameter.Optional });
            routes.MapRoute("tintuc", "tin-tuc", new { controller = "BlogHome", action = "Index" });
            routes.MapRoute("chitiettintuc", "tin-tuc/{slug}", new { controller = "BlogHome", action = "Detail", slug = UrlParameter.Optional });
            routes.MapRoute("timkiem", "tim-kiem", new { controller = "Product", action = "Search", slug = UrlParameter.Optional });
            routes.MapRoute("lienhe", "lien-he", new { controller = "Home", action = "ContactUs", slug = UrlParameter.Optional });
            routes.MapRoute("vechungtoi", "chung-toi", new { controller = "Home", action = "AboutUs", slug = UrlParameter.Optional });
            routes.MapRoute("trangchu", "", new { controller = "Home", action = "Index", slug = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }


    }
}
