using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class Response
    {
        public bool IsSuccsess { get; set; }
        public string Message { get; set; }
        public bool IsFile { get; set; } = false;
    }
}
