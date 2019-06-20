namespace ManageSystemPMSBE.Models
{
    public class NotificationSystemLanguage
    {
        public int NotificationSystemLanguageId { get; set; }
        public int LanguageId { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}