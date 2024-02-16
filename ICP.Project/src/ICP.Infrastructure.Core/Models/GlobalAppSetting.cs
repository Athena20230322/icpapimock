using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public class GlobalAppSetting
    {
        public Assembly Assembly
        {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }

        public string AssemblyName
        {
            get
            {
                return Assembly.FullName;
            }
        }

        public string RootPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public string ProcessId { get; set; }

        public long? UserID { get; set; }

        public void LoadUserID(long UserID)
        {
            this.UserID = UserID;
        }

        public static GlobalAppSetting Create(Func<string> funcGetProcessId)
        {
            return new GlobalAppSetting
            {
                ProcessId = funcGetProcessId()
            };
        }
    }
}
