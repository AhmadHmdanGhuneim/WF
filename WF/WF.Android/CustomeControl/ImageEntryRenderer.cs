
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;

using Android.Support.V4.Content;
using Android.Text;
using WF.CustomeControl;
using WF.Droid.CustomeControl;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Resource;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace WF.Droid.CustomeControl
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;
        public ImageEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            

            element = (ImageEntry)this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 25;
            Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            GradientDrawable gd = new GradientDrawable();

            //Below line is useful to give border color
            gd.SetColor(global::Android.Graphics.Color.WhiteSmoke);

            //gd.SetColor(Android.Resource.Color.FromHex("#e6e2e2d9"));

            Control.SetBackgroundDrawable(gd);
            Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            try
            {


                int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
                var drawable = ContextCompat.GetDrawable(this.Context, resID);
                var bitmap = ((BitmapDrawable)drawable).Bitmap;

                return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
            }
            catch (System.Exception)
            {

                throw;
            }

        }


    }
}