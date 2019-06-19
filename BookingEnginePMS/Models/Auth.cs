using System.Collections.Generic;

namespace BookingEnginePMS.Models
{
    public class Auth
    {
        public string scope { get; set; }
        public string nonce { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public long expires_in { get; set; }
    }
    public class Payer
    {
        public string payment_method { get; set; }
    }
    public class Amount
    {
        public double total { get; set; }
        public string currency { get; set; }
    }
    public class Payee
    {
        public string merchant_id { get; set; }
    }
    public class Payment_options
    {
        public string allowed_payment_method { get; set; }
    }
    public class Item_list
    {
        public List<Item> items { get; set; }
    }
    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double tax { get; set; }
        //public int sku { get; set; }
        public string currency { get; set; }
    }
    public class Sale
    {
        public string id { get; set; }
        public string state { get; set; }
    }
    public class Related_resource
    {
        public Sale sale { get; set; }
    }
    public class Transactions
    {
        public Amount amount { get; set; }
        public Payee payee { get; set; }
        public string custom { get; set; }
        public string invoice_number { get; set; }
        public Payment_options payment_options { get; set; }
        public Item_list item_list { get; set; }
    }
    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; } //GET,REDIRECT,POST
        public string description { get; set; }
    }
    public class Redirect_urls
    {
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
    public class Application_context
    {
        public string shipping_preference { get; set; }
        public string user_action { get; set; }
    }
    public class PaymentPaypal
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Application_context application_context { get; set; }
        public Redirect_urls redirect_urls { get; set; }
        public List<Transactions> transactions { get; set; }
        //public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
    }

    public class TransactionsResult
    {
        public Amount amount { get; set; }
        public Payee payee { get; set; }
        public string custom { get; set; }
        public string invoice_number { get; set; }
        public Payment_options payment_options { get; set; }
        public Item_list item_list { get; set; }
        public List<Related_resource> related_resources { get; set; }
    }
    public class PaymentPaypalResult
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Application_context application_context { get; set; }
        public Redirect_urls redirect_urls { get; set; }
        public List<TransactionsResult> transactions { get; set; }
        //public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
        public string AllDataJson { get; set; }
    }
    public class Partner_specific_identifier
    {
        public string type { get; set; }
        public string value { get; set; }
    }
    public class Business_address
    {
        public string country_code { get; set; }
    }
    public class Business_details
    {
        public Business_address business_address { get; set; }
    }
    public class Customer_data
    {
        public string customer_type { get; set; }
        public Business_details business_details { get; set; }
        public List<Partner_specific_identifier> partner_specific_identifiers { get; set; }
    }
    public class Rest_api_integration
    {
        public string integration_method { get; set; }
        public string integration_type { get; set; }
    }
    public class Rest_third_party_details
    {
        public string partner_client_id { get; set; }
        public List<string> feature_list { get; set; }
    }
    public class Api_integration_preference
    {
        public string partner_id { get; set; }
        public Rest_api_integration rest_api_integration { get; set; }
        public Rest_third_party_details rest_third_party_details { get; set; }
    }
    public class Requested_capabilities
    {
        public string capability { get; set; }
        public Api_integration_preference api_integration_preference { get; set; }
    }
    public class Collected_consents
    {
        public string type { get; set; }
        public bool granted { get; set; }
    }
    public class Web_experience_preference
    {
        public string partner_logo_url { get; set; }
        public string return_url { get; set; }
    }
    public class DataConnectPayPal
    {
        public Customer_data customer_data { get; set; }
        public List<Requested_capabilities> requested_capabilities { get; set; }
        public Web_experience_preference web_experience_preference { get; set; }
        public List<Collected_consents> collected_consents { get; set; }
        public List<string> products { get; set; }
    }
    public class ResultConnectPayPal
    {
        public List<Link> links { get; set; }
    }
}