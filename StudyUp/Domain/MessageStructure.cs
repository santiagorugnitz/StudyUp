namespace Domain
{
    public class MessageStructure
    {
        public string[] registration_ids { get; set; }
        public object data { get; set; }
        public object notification { get; set; }
        public string priority { get; set; }

    }
}
