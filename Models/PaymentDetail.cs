using System;
using System.Collections.Generic;

#nullable disable

namespace DepEmpCardAPI.Models
{
    public class PaymentDetail
    {
        public int PaymentDetailId { get; set; }
        public int? CardOwnerId { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }

        public virtual Employee CardOwner { get; set; }
    }
}
