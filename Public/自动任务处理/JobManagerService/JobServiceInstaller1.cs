using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace JobManagerService
{
    [RunInstaller(true)]
    public partial class JobServiceInstaller1 : Installer
    {
        public JobServiceInstaller1()
        {
            InitializeComponent();
        }
    }
}
