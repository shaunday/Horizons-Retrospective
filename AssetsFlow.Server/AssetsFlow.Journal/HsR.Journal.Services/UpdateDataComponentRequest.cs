using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Services
{
    public class UpdateDataComponentRequest
    {
        public string Content { get; set; } = "";
        public string Info { get; set; } = "";
        public bool DenyActivation { get; set; } = false;
    }
}
