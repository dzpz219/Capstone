using StackedDeck.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackedDeck.ViewModels
{
    public class AdminData
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<UploadedFile> Ads { get; set; }
        public IEnumerable<CreditRequest> Requests { get; set; }
        public IEnumerable<ReportedUser> Reports { get; set; }

        public Dictionary<string, string> CountryList { get; set; }
        public Dictionary<string, string> ProvinceList { get; set; }

        [DataType(DataType.Text)]
        public string Username { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "First Name:")]
        public string Firstname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name:")]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Question:")]
        public string RecoveryQuestion1 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Answer:")]
        public string RecoveryAnswer1 { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Phone:")]
        public string Phone { get; set; }

        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^([a-zA-Z]\d[a-zA-Z]( )?\d[a-zA-Z]\d)$", ErrorMessage = "Not a valid Postal Code")]
        [Display(Name = "Postal:")]
        public string Postal { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Address:")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Province:")]
        public string Province { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Country:")]
        public string Country { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Credits:")]
        public Double Credits { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Registered Date:")]
        public DateTime RegisterDate { get; set; }
    }
}