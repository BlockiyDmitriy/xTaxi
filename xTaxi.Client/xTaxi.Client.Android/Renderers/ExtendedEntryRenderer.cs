using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
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

                SetColor(Android.Graphics.Color.Transparent);

                this.EditText.FocusChange += (sender, ee) => {
                    bool hasFocus = ee.HasFocus;
                    if (hasFocus)
                    {
                        SetColor(Android.Graphics.Color.Blue);
                    }
                    else
                    {
                        SetColor(Android.Graphics.Color.Transparent);
                    }
                };
            }
        }
        void SetColor(Android.Graphics.Color color)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(color);
            }
            else
            {
                Control.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}