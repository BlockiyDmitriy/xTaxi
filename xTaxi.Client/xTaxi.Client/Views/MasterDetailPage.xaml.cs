using Xamarin.Forms;

namespace xTaxi.Client.Views
{
    public partial class CustomMasterDetailPage : Xamarin.Forms.MasterDetailPage
    {
        public static CustomMasterDetailPage Current { get; set; }

        public CustomMasterDetailPage()
        {
            InitializeComponent();
            Current = this;
        }
    }
}