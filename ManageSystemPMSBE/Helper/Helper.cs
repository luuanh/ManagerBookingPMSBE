using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace ManageSystemPMSBE.Helper
{
    public class Helper
    {
        public static readonly string key = "a92e31049c3986a9f77289fc997a0dcf";
        private static readonly string[] sources = { "Walk-in / Telephone", "Online Booking Engine", "Booking.com", "Expedia", "Agoda", "Trip Connect", "AirBNB", "Hostelworld", "Myallocator", "Company", "Guest Member", "Owner", "Returning Guest", "Apartment", "Siteminder", "Other Travel Agency" };
        private static readonly string[] status = { "Mới", "Đã checkin", "Đã checkout", "Khách không đến", "Đã hủy", "Chuyển công nợ" };
        private static readonly string[] statusCash = { "Đóng", "Mở" };
        private static readonly string[] typeReservation = { "Ngày", "Giờ" };
        private static readonly List<string> _country = new List<string>() {
                "USA","Canada","Afghanistan","Albania","Algeria","American Samoa","Andorra","Angola","Anguilla","Antarctica",
                "Antigua and Barbuda","Argentina","Armenia","Aruba","Australia","Austria","Azerbaijan","Bahamas","Bahrain",
                "Bangladesh","Barbados","Belarus","Belgium","Belize","Benin","Bermuda","Bhutan","Bolivia","Bosnia and Herzegovina",
                "Botswana","Bouvet Island","Brazil","British Indian Ocean Territory","British Virgin Islands","Brunei","Bulgaria",
                "Burkina Faso","Burundi","Cambodia","Cameroon","Cape Verde","Cayman Islands","Central African Republic","Chad",
                "Chile","China","Chile","Cocos Islands","Colombia","Comoros","Congo","Cook Islands","Costa Rica","Croatia",
                "Cuba","Cyprus","Czech Republic","Denmark","Djibouti","Dominica","Dominican Republic","East Timor","Ecuador",
                "Egypt","El Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Falkland Islands","Faroe Islands",
                "Fiji","Finland","France","Gabon","Gambia","Georgia","Germany","Ghana","Gibraltar","Greece","Greenland",
                "Grenada","Guadeloupe","Guam","Guatemala","Guinea","Guinea-Bissau","Guyana","Haiti","Heard","Honduras",
                "Hong Kong","Hungary","Iceland","India","Indonesia","Iran","Iraq","Ireland","Israel","Italy","Ivory Coast",
                "Jamaica","Japan","Jordan","Kazakhstan","Kenya","Kiribati","Korea, North","Korea, South","Kuwait",
                "Kyrgyzstan","Laos","Latvia","Lebanon","Lesotho","Liberia","Libya","Liechtenstein","Lithuania",
                "Luxembourg","Macau","Macedonia","Madagascar","Malawi","Malaysia","Maldives","Mali","Malta","Marshall Islands",
                "Martinique","Mauritania","Mauritius","Mayotte","Mexico","Micronesia","Moldova","Monaco","Mongolia",
                "Montserrat","Morocco","Mozambique","Myanmar","Namibia","Nauru","Nepal","Netherlands","Netherlands Antilles",
                "New Caledonia","New Zealand","Nicaragua","Niger","Nigeria","Niue","Norfolk Island",
                "Northern Mariana Islands","Norway","Oman","Pakistan","Palau","Panama","Papua New Guinea","Paraguay","Peru",
                "Philippines", "Pitcairn Island", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania",
                "Russia", "Rwanda", "S.Georgia", "Saint Kitts Nevis", "Saint Lucia", "Saint Vincent", "Samoa",
                "San Marino", "Sao Tome", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore",
                "Slovakia", "Slovenia", "Somalia", "South Africa", "Spain", "Sri Lanka", "St. Helena", "St. Pierre",
                "Sudan", "Suriname", "Svalbard", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan",
                "Tajikistan", "Tanzania","Thailand","Togo","Tokelau","Tonga","Trinidad","Tunisia","Turkey",
                "Turkmenistan","Turks","Tuvalu","Uganda","Ukraine","United Arab Emirates","United Kingdom",
                "Uruguay","Uzbekistan","Vanuatu","Vatican City","Venezuela","Vietnam","Virgin Islands","Wallis",
                "Western Sahara","Yemen","Yugoslavia","Zaire","Zambia","Zimbabwe","Other"
            };

        public static string GetSourceReservation(int index)
        {
            return sources[index - 1];
        }
        public static string GetStatus(int index)
        {
            return status[index - 1];
        }
        public static string GetTypeReservation(int index)
        {
            return typeReservation[index];
        }
        public static string GetStatusCash(bool status)
        {
            return status ? statusCash[1] : statusCash[0];
        }
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
        public static DateTime DateTimeUTCNow()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            DateTime ngayhientai = DateTime.Parse(TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString());
            return ngayhientai;
        }
        public static List<string> GetAllCountry()
        {
            return _country;
        }
        public static string StringCutter(string str, int maxlength)
        {
            if (str.Length > maxlength)
                return str.Substring(0, maxlength) + "...";
            return str;
        }
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåąạậấầẫẩắằẵẳảăặ".Contains(s))
            {
                return "a";
            }
            else if ("èéêëęệếềệẹẻểễ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıị".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőðơờớợốộồỗổởọỏỡ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůưứừựụủửữũ".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿỹýỳỵ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if ("đđ".Contains(s))
            {
                return "d";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}