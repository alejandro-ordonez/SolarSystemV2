using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Solar.CustomControls
{
    public class CustomPin:Pin
    {
        public static readonly BindableProperty IdPanelProperty =
            BindableProperty.Create("IdPanel", typeof(int), typeof(CustomPin), null);

        public int IdPanel
        {
            get { return (int)GetValue(IdPanelProperty); }
            set { SetValue(IdPanelProperty, value); }
        }
    }
}
