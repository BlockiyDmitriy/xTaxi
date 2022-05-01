using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xTaxi.Client.Droid.Renderers;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace xTaxi.Client.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackground(null);
            }
        }
    }
}