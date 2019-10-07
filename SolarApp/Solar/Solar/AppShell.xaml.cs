using System;
using System.Collections.Generic;
using Solar.ViewModels;
using Xamarin.Forms;

namespace Solar
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            BindingContext = new ShellViewModel();
            InitializeComponent();
        }
    }
}
