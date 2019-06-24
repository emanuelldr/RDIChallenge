using System;
using System.ComponentModel.DataAnnotations;

namespace RDI.Domain.Token.Entities
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }
        public long CardNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
