using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xTaxi.Client.Droid.Renderers;

[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(ExtendedDatePickerRenderer))]
namespace xTaxi.Client.Droid.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackground(null);
            }
        }
    }
}