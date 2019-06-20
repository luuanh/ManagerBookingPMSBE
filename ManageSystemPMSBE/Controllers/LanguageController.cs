using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class LanguageController : SercurityController
    {
        public JsonResult GetActive()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Language> language = connection.Query<Language>("Language_GetActive"
                    , commandType: CommandType.StoredProcedure).ToList();
                return Json(language, JsonRequestBehavior.AllowGet);
            }
        }
    }
}