using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using HMIStudio.Shared.TreeView.Collections;
using System.Globalization;
using System.Xml.Linq;

namespace HMIStudio.Shared.Interfaces
{
    public class IsActiveToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isActive = (bool)value;
            return isActive ? "Red" : "Green";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "Green") ? 0 : 1;
        }

    }
    public class TreeViewModel : ObservableObject
    {
        TreeNode _MyTree;
        public TreeNode MyTree
        {
            get { return _MyTree; }
            set { Set("MyTree", ref _MyTree, value); }
        }

        public ICommand AddNodeCommand { protected set; get; }

        static Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Day);

        public TreeViewModel()
        {


        }
        public async void InsertRandomNodes()
        {
            //insert 6 new nodes randomly into the tree, 1 every 5 seconds
            for (int i = 0; i < 6; i++)
            {
                await Task.Delay(1000);

                if (MyTree == null)
                    return;

                // pick a random node
                var randomIndex = random.Next(0, MyTree.Subtree.Count() - 1);
                var node = MyTree.Subtree.Skip(randomIndex).First();

                (node as TreeNode).Children.Add(new TreeNode { Name = GetRandomTitle(), Status = 0, IsExpanded = false });
            }

        }

        string GetRandomTitle()
        {
            var title = GetRandomAdjective() + " " + GetRandomWord() + " " + GetRandomWord() + " ";

            if (random.NextDouble() < 0.30)
                title += random.Next(3, 99).ToString() + "!";

            return title;
        }

        string GetRandomAdjective()
        {
            var adjs = new string[] { "happy", "fluffy", "short", "tall", "hard", "soft", "flat", "thick", "thin", "round", "square", "rambunctious", "titillating", "merry", "fried", "limber", "bellicose",
                "tired", "pretentious", "moody", "comical", "severe", "flabberghasted", "opinionated", "naive", "hungry", "bedazzled", "mendacious", "patient", "radical", "flummoxed", "snide", "petty" };
            var i = random.Next(0, adjs.Count() - 1);
            return adjs.Skip(i).First();
        }

        string GetRandomWord()
        {
            var words = new string[] { "bird", "hand", "dog", "fruit", "frog", "juice", "egg", "apple", "bottle", "cork", "wine", "hat", "glove", "moon", "tree", "hair", "house", "river", "flavor",
                "clown", "door", "phone", "clock", "bread", "candy", "shoe", "fish", "drink", "noodle", "ladder", "smile", "brandy", "corn", "cheese", "dog", "kitten" };
            var i = random.Next(0, words.Count() - 1);
            return words.Skip(i).First();
        }
    }
}