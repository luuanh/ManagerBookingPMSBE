using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class ResponeConfirmPayPal
    {
        public string merchant_id { get; set; }
        public string tracking_id { get; set; }
        public bool payments_receivable { get; set; }
        public bool primary_email_confirmed { get; set; }
    }
    public class ConfigPaymentMethodController : SercurityController
    {
        // GET: Admin/ConfigPaymentMethod
        public ActionResult Index()
        {
            if (!CheckSecurity(47))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 47
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult ConfigVTCPay()
        {
            if (!CheckSecurity(47))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 47
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult ConfigPaypal()
        {
            if (!CheckSecurity(47))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            ConfigPayPal statusConnectPayPal = null;
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 47
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                statusConnectPayPal = connection.QuerySingleOrDefault<ConfigPayPal>("ConfigPayPal_GetStatusHotel",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure);
            }
            if (statusConnectPayPal is null) statusConnectPayPal = new ConfigPayPal() { HotelId = -1 };
            Auth auth = DataHelper.GetTokenKey();
            DataConnectPayPal dataConnectPayPal = new DataConnectPayPal()
            {
                collected_consents = new List<Collected_consents>()
                {
                    new  Collected_consents()
                    {
                        granted = true,
                        type = "SHARE_DATA_CONSENT"
                    }
                },
                customer_data = new Customer_data()
                {
                    customer_type = "MERCHANT",
                    business_details = new Business_details()
                    {
                      business_address = new Business_address()
                      {
                          country_code = "VN"
                      }  
                    },
                    partner_specific_identifiers = new List<Partner_specific_identifier>()
                    {
                        new Partner_specific_identifier()
                        {
                            type = "TRACKING_ID",
                            value = "Hotel_"+HotelId
                        }
                    }
                },
                products = new List<string>()
                {
                    "EXPRESS_CHECKOUT"
                },
                requested_capabilities = new List<Requested_capabilities>()
                {
                   new Requested_capabilities()
                   {
                        capability = "API_INTEGRATION",
                        api_integration_preference = new Api_integration_preference() 
                        {
                            partner_id = "ZVHJE736BERKQ",
                            rest_api_integration = new Rest_api_integration()
                            {
                                integration_method = "PAYPAL",
                                integration_type = "THIRD_PARTY"
                            },
                            rest_third_party_details = new Rest_third_party_details()
                            {
                                feature_list = new List<string>() { "PAYMENT", "REFUND" },
                                partner_client_id = "AUYW3t_ks9ReVDN5cklGJGtt84pNEz-W1DktPKg7EbnyF6wk1IRchi90LcBIohPxLfG8S-T5T6uhpPxA"
                            }
                        }
                   }
                },
                web_experience_preference = new Web_experience_preference()
                {
                    partner_logo_url = "http://iit.com.vn/files/images/logo-iit.png",
                    return_url = "http://pms.asiky.com/Admin/ConfigPaymentMethod/ResultPayPal"
                }
            };
            string jsonDataConnectPayPal = JsonConvert.SerializeObject(dataConnectPayPal);
            string jsonResult = GetConnectPayPal(jsonDataConnectPayPal, auth.access_token);
            ResultConnectPayPal result = JsonConvert.DeserializeObject<ResultConnectPayPal>(jsonResult);
            ViewData["Link"] = result.links.Find(x => x.rel == "action_url");
            ViewData["statusConnectPayPal"] = statusConnectPayPal;
            return View();
        }
        public ActionResult ResultPayPal(string merchantId, string merchantIdInPayPal, bool permissionsGranted, string accountStatus,
                                            bool consentStatus, string productIntentID, bool isEmailConfirmed, string returnMessage)
        {
            if (!CheckSecurity(47))
                return Redirect("/Admin/Login/Index");

            int resultPaypal = 1;
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 47
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                connection.Execute("Hotel_ConnectPayPal",
                         new
                         {
                             HotelId = merchantId.Split('_')[1],
                             MerchantId = merchantIdInPayPal,
                             EmailConfirm = isEmailConfirmed
                         }, commandType: CommandType.StoredProcedure);
                if (permissionsGranted && consentStatus && isEmailConfirmed)
                {
                    resultPaypal = 1;
                }
                else
                {
                    resultPaypal = -1;
                }
                if (!isEmailConfirmed)
                {
                    resultPaypal = -2;
                }
            }
            ViewBag.resultPaypal = resultPaypal;
            return View();
        }
        private string GetConnectPayPal(string dataPost, string token)
        {
            var client = new RestClient("https://api.paypal.com/v1/customer/partner-referrals");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "c1eb2f85-0119-4e65-a27c-7f737d2e2365");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", dataPost, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
        public JsonResult GetConfirmConnection()
        {
            if (!CheckSecurity(47))
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                ConfigPayPal statusConnectPayPal = connection.QuerySingleOrDefault<ConfigPayPal>("ConfigPayPal_GetStatusHotel",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure);
                Auth auth = DataHelper.GetTokenKey();
                var client = new RestClient("https://api.paypal.com/v1/customer/partners/ZVHJE736BERKQ/merchant-integrations/" + statusConnectPayPal.MerchantId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "639c4a48-4528-4c64-b038-d58a29d94fde");
                request.AddHeader("Authorization", "Bearer " + auth.access_token);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);
                int result = 1;
                try
                {
                    ResponeConfirmPayPal responeConfirmPayPal = JsonConvert.DeserializeObject<ResponeConfirmPayPal>(response.Content);
                    if (responeConfirmPayPal.primary_email_confirmed)
                    {
                        connection.Execute("Hotel_ConnectPayPal_UpdateLog",
                             new
                             {
                                 HotelId = HotelId,
                                 LogCallAPI = response.Content
                             }, commandType: CommandType.StoredProcedure);
                        result = 1;
                    }
                    else
                        result = 0;
                }
                catch (Exception ex)
                {
                    result = -1;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];

            using (var connection = DB.ConnectionFactory())
            {
                List<ConfigPaymentMethod> currencies = connection.Query<ConfigPaymentMethod>("ConfigPaymentMethod_Get",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                return Json(currencies, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(List<ConfigPaymentMethod> currencies)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            if (currencies is null) currencies = new List<ConfigPaymentMethod>();
            using (var connection = DB.ConnectionFactory())
            {
                currencies.ForEach(x =>
                {
                    connection.Execute("ConfigPaymentMethod_Put",
                        new
                        {
                            ConfigPaymentMethodId = x.ConfigPaymentMethodId,
                            RequireCard = x.RequireCard,
                            Active = x.Active,
                            Index = x.Index,
                            HotelId = HotelId,
                            ActiveInvoice = x.ActiveInvoice
                        }, commandType: CommandType.StoredProcedure);
                });
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPolicy(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("PaymentMethod_GetPolicy",
                    new
                    {
                        HotelId = HotelId,
                        ConfigPaymentMethodId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<PolicyPaymentMethod> paymentMethods = multi.Read<PolicyPaymentMethod>().ToList();
                    List<int> languagesId = multi.Read<int>().ToList();
                    if (paymentMethods is null) paymentMethods = new List<PolicyPaymentMethod>();
                    if (languagesId is null) languagesId = new List<int>();
                    List<PolicyPaymentMethod> paymentMethodsResult = new List<PolicyPaymentMethod>();
                    languagesId.ForEach(x =>
                    {
                        PolicyPaymentMethod paymentMethod = paymentMethods.Find(y => y.LanguageId == x);
                        if (paymentMethod is null)
                        {
                            paymentMethodsResult.Add(new PolicyPaymentMethod()
                            {
                                LanguageId = x,
                                ConfigPaymentMethodId = id,
                                Policy = ""
                            });
                        }
                        else
                            paymentMethodsResult.Add(paymentMethod);
                    });
                    return Json(paymentMethodsResult, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PutPolicy(int id, List<PolicyPaymentMethod> data)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (data is null) data = new List<PolicyPaymentMethod>();
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("PolicyPaymentMethod_Delete",
                        new
                        {
                            HotelId = HotelId,
                            ConfigPaymentMethodId = id
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    data.ForEach(x =>
                    {
                        connection.Execute("PolicyPaymentMethod_Post",
                        new
                        {
                            HotelId = HotelId,
                            ConfigPaymentMethodId = id,
                            Policy = x.Policy,
                            LanguageId = x.LanguageId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    });
                    transaction.Commit();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetConfigVTCPay()
        {
            if (!CheckSecurity(62))
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                ConfigVTCPay configVTCPay = connection.Query<ConfigVTCPay>("ConfigVTCPay_Get",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (configVTCPay is null) configVTCPay = new ConfigVTCPay();
                configVTCPay.ReceiverAccount = DataHelper.Decrypt(configVTCPay.ReceiverAccount);
                configVTCPay.WebSite = DataHelper.Decrypt(configVTCPay.WebSite);
                configVTCPay.SecurityKey = DataHelper.Decrypt(configVTCPay.SecurityKey);
                configVTCPay.WebsiteId = DataHelper.Decrypt(configVTCPay.WebsiteId);

                return Json(configVTCPay, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutConfigVTCPay(ConfigVTCPay configVTCPay)
        {
            if (!CheckSecurity(62))
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("ConfigVTCPay_Put",
                    new
                    {
                        HotelId = HotelId,
                        ReceiverAccount = DataHelper.Encrypt(configVTCPay.ReceiverAccount),
                        WebSite = DataHelper.Encrypt(configVTCPay.WebSite),
                        SecurityKey = DataHelper.Encrypt(configVTCPay.SecurityKey),
                        WebsiteId = DataHelper.Encrypt(configVTCPay.WebsiteId),
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}