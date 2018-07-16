using WF.CustomeControl;
using WF.iOS.CustomeControl;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomListViewRenderer), typeof(CustomListView))]
namespace WF.iOS.CustomeControl
{
    public class CustomListView : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.CellLayoutMarginsFollowReadableWidth = false;
                Control.ShowsVerticalScrollIndicator = false;
            }
        }

    }
}