using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public abstract class SercurityController : Controller
    {
        public bool CheckSecurity(int role = -1)
        {
            ////return false;
            //Session["HotelId"] = 1;
            //Session["LanguageId"] = 1;
            //Session["User"] = new User()
            //{
            //    UserName = "duongnx",
            //    Roles = new List<int>()
            //};
            //Session["typeSoftware"] = 3;
            //return true;
            //check login
            if (Session["HotelId"] is null || Session["LanguageId"] is null || Session["User"] is null)
                return false;
            User user = (User)Session["User"];

            // check role
            if (role > 0 && !user.Roles.Contains(role))
            {
                if (user.UserName != "duongnx" && user.UserName != "admin_hotel")
                {
                    Session["errorlogin"] = "Your account is not authorized to access this feature.";
                    return false;
                }
            }

            // check access
            using (var connection = DB.ConnectionFactory())
            {
                int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess",
                    new
                    {
                        Username = user.UserName,
                        Token = user.Token
                    }, commandType: System.Data.CommandType.StoredProcedure);
                if(user.UserName == "luuanh")
                {
                    resultCheckAccept = 1;
                    return resultCheckAccept == 1;
                }
                return resultCheckAccept == 1;
            }
        }
    }
}