using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xTaxi.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchContentView : ListView
    {
        public SearchContentView() => InitializeComponent();

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            SelectedItem = null;
        }
    }
}