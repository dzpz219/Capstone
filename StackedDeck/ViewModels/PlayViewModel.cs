using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackedDeck.ViewModels
{
    public class PlayViewModel
    {
        public Double Credits { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Bet { get; set; }
    }
}