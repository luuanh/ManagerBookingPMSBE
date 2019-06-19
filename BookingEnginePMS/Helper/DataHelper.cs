using BookingEnginePMS.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookingEnginePMS.Helper
{
    public class DataHelper
    {
        public static string key = "a92e31049c3986a9f77289fc997a0dcf";
        public static string RandomString(int numberChar = 10)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < numberChar; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return RandomString(numberChar);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        public static string CreateMD5(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = ASCIIEncoding.Default.GetBytes(value);
            byte[] encoded = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
                sb.Append(encoded[i].ToString("x2"));
            return sb.ToString();
        }
        public static string Encrypt(string toEncrypt)
        {
            if (toEncrypt is null)
                toEncrypt = "";
            if (toEncrypt == "") return "";
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string toDecrypt)
        {
            if (toDecrypt is null || toDecrypt == "") return "";
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string convertToUnSign3(string value)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = value.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static float CurrencyConvertor(string typeCurrency)
        {
            var result = "1";
            try
            {
                var load = XDocument.Load(@"http://www.vietcombank.com.vn/ExchangeRates/ExrateXML.aspx");
                var xElement = load.Element("ExrateList");
                if (xElement != null)
                {
                    var usds = xElement.Elements("Exrate");
                    foreach (var element in usds.Where(element => element.Attribute("CurrencyCode").Value == typeCurrency))
                    {
                        result = element.Attribute("Sell").Value;
                    }
                }
            }
            catch (Exception)
            {
                result = "1";
            }
            return float.Parse(result);
        }
        public static int RangeDate(DateTime fromDate, DateTime toDate)
        {
            return Math.Abs((toDate - fromDate).Days);
        }

        public static Auth GetTokenKey()
        {
            string authString = "QVVZVzN0X2tzOVJlVkRONWNrbEdKR3R0ODRwTkV6LVcxRGt0UEtnN0VibnlGNndrMUlSY2hpOTBMY0JJb2hQeExmRzhTLVQ1VDZ1aHBQeEE6RUQ1TjlrV1RGXzhIamlabWhFeVJrWXdNcjZ3RGUtVGhGMGhKZUh4bEY0cUdBTFVTMFYyNjh3SHBXbGZWUGY0d05HbU9FMUdQN0U0MHE3Vkg=";
            var client = new RestClient("https://api.paypal.com/v1/oauth2/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "d3327fe0-ab88-46f2-b7e0-bdddcb31c42d");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Basic " + authString);
            request.AddHeader("PayPal-Partner-Attribution-Id", "IITTechnology_STP_VN");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type=client_credentials", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Auth auth = JsonConvert.DeserializeObject<Auth>(response.Content);
            return auth;
        }
    }
}


//string authString = "QWZIRkVuWllmdEVYenFGWUhWaUowQVozTEtyaE53aUVCOTZqaXpkZEoyczllVjh4dWF6S01kVllUdFZONDAyWkZLVGZFcGQ5RjJxeDYtel86RUloYWl5X294clNqaWhyMEdBNHRLMWF5VlhwS2lncUdoMngyVGJ4SmxQQkdzZHVzNWxRZlI2b0NFOTV6enczMVNhVnJub0h1SWpBU2lXelM=";
//var client = new RestClient("https://api.sandbox.paypal.com/v1/oauth2/token");
//var request = new RestRequest(Method.POST);
//request.AddHeader("Postman-Token", "59f6bf15-305f-427a-8241-58c1f0a4cf97");
//            request.AddHeader("Cache-Control", "no-cache");
//            request.AddHeader("Authorization", "Basic " + authString);
//            request.AddHeader("PayPal-Partner-Attribution-Id", "IITTechnology_STP_VN");
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.AddParameter("undefined", "grant_type=client_credentials", ParameterType.RequestBody);
//            IRestResponse response = client.Execute(request);
//Auth auth = JsonConvert.DeserializeObject<Auth>(response.Content);
//            return auth;