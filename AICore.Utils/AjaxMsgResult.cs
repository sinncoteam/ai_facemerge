using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICore.Utils
{
    public class AjaxMsgResult
    {
        public int success { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
        public object source { get; set; }
    }
}
