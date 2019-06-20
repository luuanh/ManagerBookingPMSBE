using System.Web;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Models
{
    public class CustomerAuth : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string userName = (string)HttpContext.Current.Session["UserName"];
            // kiểm tra với username này có những quyền với method gì (AcceptMethod)
            // this.Roles
            base.OnAuthorization(filterContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            //filterContext.Result = new ViewResult() { ViewName = "ahjihi" };
        }
    }
}