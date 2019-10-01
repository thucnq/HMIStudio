using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMIStudio.Shared.Helpers;
using HMIStudio.Shared.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MQTTnet;
using MQTTnet.Server;

namespace HMIStudio.UI
{
   
    public partial class MainPage : TabbedPage
    {
        public static (bool start, float mins) OpeningArgs { get; set;  }
        public static TimerViewModel DownVM { get; set; }
        public MainPage()
        {
            InitializeComponent();

            TabItemStructureDesign.BindingContext = new StructureDesignViewModel();
            TabItemDown.BindingContext = DownVM = new TimerViewModel(Constants.Countdown, OpeningArgs.start, OpeningArgs.mins);
            TabItemUp.BindingContext = new TimerViewModel(Constants.Countup);
            TabItemGiveaway.BindingContext = new TimerViewModel(Constants.Giveaway);

            MQTTLibrary.StartServer();
        }
    }
}
