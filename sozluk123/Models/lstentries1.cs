using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sozluk123.Models
{
    public class lstentries1
    {
        public IEnumerable<yazar> yazars1 { get; set; }
        public IEnumerable<entry> entries1 { get; set; }

        public IEnumerable<baslik> baslik1 { get; set; }
        public IEnumerable<entry_yazar> entry1yazar1 { get; set; }

        public IEnumerable<Users> Users1 { get; set; }
    }
}