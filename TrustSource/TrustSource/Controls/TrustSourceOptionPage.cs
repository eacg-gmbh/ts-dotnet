using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TrustSource
{
    [Guid("00000000-0000-0000-0000-000000000000")]
    public class TrustSourceOptionPage : DialogPage
    {
        protected override IWin32Window Window
        {
            get
            {
                TrustSourceOptionsUserControl page = new TrustSourceOptionsUserControl();
                page.optionsPage = this;
                page.Initialize();
                return page;
            }
        }

        private string _username { get; set; }
        private string _apikey { get; set; }

        [Category("TrustSource")]
        [DisplayName("Username")]
        [Description("TrustSource Username")]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [Category("TrustSource")]
        [DisplayName("Api Key")]
        [Description("TrustSource Api Key")]
        public string ApiKey
        {
            get { return _apikey; }
            set { _apikey = value; }
        }
    }
}
