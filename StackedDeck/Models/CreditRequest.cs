using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    public class CreditRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Double Amount { get; set; }
        public Boolean Approved { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; }
    }
}