using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoArchiveCoilsWeb
{
    public class ApplicationConfiguration
    {
         public Cam[] Cams { get; set; }
        
        
    }

    public class Cam
    {
        public string Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
