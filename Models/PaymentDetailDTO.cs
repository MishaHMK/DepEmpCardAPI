namespace DepEmpCardAPI.Models
{
    public class PaymentDetailDTO
    {
        public int PaymentDetailId { get; set; }
        public int? CardOwnerId { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
    }
}
