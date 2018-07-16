using Xamarin.Forms;
using WF.CustomeControl;
using WF.Droid.CustomeControl;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomListViewRenderer), typeof(CustomListView))]
namespace WF.Droid.CustomeControl
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomListView : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.VerticalScrollBarEnabled = false;
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete
}