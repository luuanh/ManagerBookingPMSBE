using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Helper
{
    public class ConfigData
    {
        public static List<string> GetAllCountry()
        {
            List<string> country = new List<string>() {
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
            return country;
        }
    }
}