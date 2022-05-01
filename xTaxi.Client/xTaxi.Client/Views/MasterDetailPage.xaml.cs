using Xamarin.Forms;

namespace xTaxi.Client.Views
{
    [System.Obsolete]
    public partial class CustomMasterDetailPage : MasterDetailPage
    {
        public static CustomMasterDetailPage Current { get; set; }

        public CustomMasterDetailPage()
        {
            InitializeComponent();
            Current = this;
        }
    }
}