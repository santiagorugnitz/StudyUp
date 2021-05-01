using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class MessageStructure
    {
        public string[] registration_ids { get; set; }
        public object data { get; set; }
        public object notification { get; set; }
    }
}
