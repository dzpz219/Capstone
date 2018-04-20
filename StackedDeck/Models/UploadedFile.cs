using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    public class UploadedFile
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int Index { get; set; }
    }
}