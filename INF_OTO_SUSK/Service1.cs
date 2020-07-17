using INF_OTO_SUSK_BusinessLayer;
using INF_OTO_SUSK_DataAccessLayer;
using INF_OTO_SUSK_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace INF_OTO_SUSK
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        
        InnerOperation obj = new InnerOperation();
        
        protected override void OnStart(string[] args)
        {
            obj.Start();
        }
        

        protected override void OnStop()
        {
            obj.Stop();
        }
    }
}
