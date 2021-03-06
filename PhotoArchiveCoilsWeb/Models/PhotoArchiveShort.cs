using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoArchiveCoilsWeb.Models
{
    public class PhotoArchiveShort
    {
        public Guid Id { get; set; }
        public string Cam { get; set; }
        public string Code { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
    }
}
