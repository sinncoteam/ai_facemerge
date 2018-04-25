using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AICore.Utils;
namespace AICore.Api.FaceMerge
{
    public class FaceMergeModel
    {
        public int ret { get; set; }
        public string msg { get; set; }
        public FaceMergeModelData data { get; set; }
    }

    public class FaceMergeModelData
    {
        public string image { get; set; }
    }
}