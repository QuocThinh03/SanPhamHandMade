namespace QuocThinh.Pages
{
    public class PaymentInformationModel
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderDescription { get; set; }
        public string OrderType { get; set; }
        public string Url { get; set; }
        public string Name { get; internal set; }
    }
}