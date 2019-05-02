using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrustSource
{
    public partial class TrustSourceOptionsUserControl : UserControl
    {
        internal TrustSourceOptionPage optionsPage;

        public TrustSourceOptionsUserControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            txtTrustSourceUsername.Text = optionsPage.Username;
            txtTrustSourceApiKey.Text = optionsPage.ApiKey;
            chkAskOptional.Checked = optionsPage.AskOptional;
        }

        private void btnSave_MouseClick(object sender, MouseEventArgs e)
        {
            optionsPage.Username = txtTrustSourceUsername.Text;
            optionsPage.ApiKey = txtTrustSourceApiKey.Text;
            optionsPage.AskOptional = chkAskOptional.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            optionsPage.Username = txtTrustSourceUsername.Text;
            optionsPage.ApiKey = txtTrustSourceApiKey.Text;
            optionsPage.AskOptional = chkAskOptional.Checked;
        }
    }
}
