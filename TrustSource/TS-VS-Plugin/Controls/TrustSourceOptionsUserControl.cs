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
            txtTrustSourceApiKey.Text = optionsPage.ApiKey;
            chkAskOptional.Checked = optionsPage.AskOptional;
        }

        private void txtTrustSourceApiKey_TextChanged(object sender, EventArgs e)
        {
            optionsPage.ApiKey = txtTrustSourceApiKey.Text;
        }

        private void chkAskOptional_CheckedChanged(object sender, EventArgs e)
        {
            optionsPage.AskOptional = chkAskOptional.Checked;
        }
    }
}
