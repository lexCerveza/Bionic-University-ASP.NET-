using System;
using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Purchase
    {
        public Guid PurchaseId { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string GuitarName { get; set; }

        [Required(ErrorMessage = "Your name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Your e-mail is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Your phone number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Your address is required")]
        public string ShippingAddress { get; set; }
    }
}