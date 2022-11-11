using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineGrocerySopping.Models
{
    public class loginDetail
    {
        [Required(ErrorMessage = "User Name is required")]
        [EmailAddress(ErrorMessage = "Invalid User Name")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class RegistrationDetails
    {
        [Required(ErrorMessage = "User Name is required")]
        public string name { get; set; }
    
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
    
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
    
        [Required(ErrorMessage = "Gender is required")]
        public string gender { get; set; }
    
        [Required(ErrorMessage = "Mobile is required")]
        public string mobile { get; set; }
    
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string cpassword { get; set; }
    }

    public class PaymentDetail
    {
        public string Product { get; set; }
        public string Amount { get; set; }
        public string Quantity { get; set; }
        public string PayMode { get; set; }
        public string OrdId { get; set; }
}
    }
