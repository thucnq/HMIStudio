using System.Diagnostics;
using Xamarin.Forms;
using HMIStudio.Shared.TreeView.Controls;

namespace HMIStudio.Shared.Interfaces
{
    public class SimpleTreeView : HMIStudio.Shared.TreeView.Controls.TreeView
    {
        public TreeViewModel ViewModel { get; set; }

        public SimpleTreeView()
        {
            // these properties have to be set in a specific order, letting us know that we're doing some dumb things with properties and will need to 
            // TODO: fix this later

            ViewModel = new TreeViewModel();
            NodeCreationFactory =
                () => new TreeNodeView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start
                };

            HeaderCreationFactory =
                () => new HMIStudio.Shared.Interfaces.TreeView.TreeCardView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start
                };

            HeaderCreationFactory =
                () =>
                {
                    var result = new HMIStudio.Shared.Interfaces.TreeView.TreeCardView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    };
                    Debug.WriteLine("HeaderCreationFactory: new TreeCardView");
                    return result;
                };



            //ViewModel.InsertRandomNodes();
            //BindingContext = ViewModel.MyTree;
            
        }
    }
}