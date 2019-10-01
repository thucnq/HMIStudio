using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]

namespace HMIStudio.UI
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            MainPage = new MainPage();   
        }
    }
}
