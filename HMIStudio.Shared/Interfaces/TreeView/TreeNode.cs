using System.Windows.Input;
using Xamarin.Forms;
using HMIStudio.Shared.TreeView.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using StructureDesignViewModel = HMIStudio.Shared.ViewModel.StructureDesignViewModel;

namespace HMIStudio.Shared.Interfaces
{
    public class TreeNode : TreeNode<TreeNode>
    {
        public ICommand ToggleIsExpandedCommand { protected set; get; }
        public ICommand Appear { protected set; get; }
        public ICommand ShowModal1 { protected set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        // normal view model properties provide the content as well as the visual state

        bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                Set("IsExpanded", ref isExpanded, value);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
                //WhenPropertyChanged();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        // we're 100% in control of the indentation level, if any, that we use in rendering our tree nodes
        public double IndentWidth { get { return (double)(Depth * 30); } }

        string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                Set("Name", ref name, value);
            }
        }
        public List<int> Modals = new List<int>();
        int status = 0;
        public int Status
        {
            get { return status; }
            set { Set("Status", ref status, value); }
        }

        public string StatusColor
        {
            get
            {
                if (status == 0)
                {
                    return "Green";
                }
                else
                {
                    return "Red";
                }
            }
        }

        public bool IsFather
        {
            get
            {
                if (this.Children.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool HaveModal1
        {
            get
            {
                if (this.Modals.Find(x => x == 1) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool HaveModal2
        {
            get
            {
                if (this.Modals.Find(x => x == 2) == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool HaveModal3
        {
            get
            {
                if (this.Modals.Find(x => x == 3) == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string CollapseSymbol
        {
            get
            {
                if (isExpanded == true)
                {
                    return "-";
                }
                else
                {
                    return "+";
                }
            }
            set => CollapseSymbol = value;
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Depth")
            {
                base.OnPropertyChanged("IndentWidth");
            }
            else if (propertyName == "IsExpanded")
            {
                    base.OnPropertyChanged("CollapseSymbol");
            }
        }
        public TreeNode()
        {
            ToggleIsExpandedCommand = new Command(obj => IsExpanded = !IsExpanded);
            //ToggleIsExpandedCommand = new Command((obj) => Collapse(obj));
            Appear = new Command<string>((x) => ShowUp(x));
            ShowModal1 = new Command<string>((x) => ShowUpModal1(x));
        }
        //public void ShowUp(string x)
        //{
        //    //List<Component> source = (List<Component>)ComponentData.Components;
        //    //Component selectedComponent = source.Find(component => component.Name == x);
        //    Component selectedComponent = new Component { Name = x };
        //    selectedComponent.Show += ShowingComponent_Show;
        //    selectedComponent.Appear();

        //}
        public static void ShowUp(string name)
        {
            StructureDesignViewModel instance = new StructureDesignViewModel();
            StructureDesignViewModel.HandleSelectedComponent delegateIns = new StructureDesignViewModel.HandleSelectedComponent(instance.SetSelectedComponent);
            delegateIns(name);
        }
        public void ShowUpModal1(string name)
        {
            StructureDesignViewModel instance = new StructureDesignViewModel();
            StructureDesignViewModel.ShowModal1 delegateIns = new StructureDesignViewModel.ShowModal1(instance.SetModal1);
            delegateIns(name);
        }
        //void collapseChilds(Object o)
        //{
        //    if (Children.Count > 0)
        //    {
        //        foreach (Object node in Children)
        //        {
        //            IsExpanded = false;
        //            collapseChilds(node);
        //        }
        //    }
        //}
        //public void Collapse(Object obj)
        //{
        //    IsExpanded = !IsExpanded;
            
        //    if(IsExpanded == false)
        //    {
        //        collapseChilds(obj);
        //    }
            
        //}
        //static void ShowingComponent_Show(object sender, ShowEventArgs e)
        //{
        //    Console.WriteLine("Call api get component {0}", e.Name);
        //}
        public override string ToString()
        {
            return string.Format("TreeNode: Name={3}, Status={4}, IsExpanded={1}, IndentWidth={2} " + base.ToString(), ToggleIsExpandedCommand, IsExpanded, IndentWidth, Name, Status);
        }
    }
}