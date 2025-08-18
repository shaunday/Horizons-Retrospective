using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.UserService.Contracts
{
    public class UserServiceVersion
    {
        public string Version { get; }
        public UserServiceVersion(string version) => Version = version;
    }

}
