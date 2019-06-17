using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SolarApp.Droid.CustomRenderers.JustifiedLabel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SolarApp.CustomRederers.JustifiedLabel), typeof(JustifiedLabelRenderer))]
namespace SolarApp.Droid.CustomRenderers.JustifiedLabel
{
    public class JustifiedLabelRenderer: LabelRenderer
    {
        public JustifiedLabelRenderer(Context context):base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.JustificationMode = Android.Text.JustificationMode.InterWord;
            }
        }
    }
}