using StackedDeck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<UploadedFile> Ads { get; set; }
    }
}