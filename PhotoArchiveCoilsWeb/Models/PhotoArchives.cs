using System;
using System.Collections.Generic;

namespace PhotoArchiveCoilsWeb.Models
{
    public partial class PhotoArchives
    {
        public Guid Id { get; set; }
        public string Cam { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
        public string Code { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
    }
}
