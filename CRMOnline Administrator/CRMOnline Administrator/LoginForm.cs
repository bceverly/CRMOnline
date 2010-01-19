using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CRMOnline_Administrator
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SecurityWebReference.Security foo = new CRMOnline_Administrator.SecurityWebReference.Security();
            MessageBox.Show(foo.Login("test", "test_pass"));
        }
    }
}
