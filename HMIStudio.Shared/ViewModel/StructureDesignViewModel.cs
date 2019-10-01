using HMIStudio.Shared.Helpers;
using HMIStudio.Shared.Interfaces;
using System.ComponentModel;
using Component = HMIStudio.Shared.Model.Component;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using HMIStudio.Shared.TreeView.Collections;

namespace HMIStudio.Shared.ViewModel
{
    public class StructureDesignViewModel : ObservableObject, INotifyPropertyChanged
    {
        private static readonly StructureDesignViewModel instance = new StructureDesignViewModel();
        //private StructureDesignViewModel() { }

        public static StructureDesignViewModel Instance
        {
            get
            {
                return instance;
            }
        }
        public static IList<Component> Components
        {
            get
            {
                return ComponentData.Components;
            }
        }
        public IList<Component> ReferenceComponents
        {
            get
            {
                return ComponentData.ReferenceComponents;
            }
        }
        TreeNode selectedGrandFatherComponent;
        TreeNode selectedFatherComponent;
        TreeNode selectedComponent;
        TreeNode selectedGrandFatherReferenceComponent;
        TreeNode selectedFatherReferenceComponent;
        TreeNode selectedReferenceComponent;
        string selectedComponentStructure = "hello";
        bool modal1IsVisible = false;
        string modal1Content = "hello";
        public bool Modal1IsVisible
        {
            get { return modal1IsVisible; }
            set
            {
                if (modal1IsVisible != value)
                {
                    modal1IsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Modal1Content
        {
            get { return modal1Content; }
            set
            {
                if (modal1Content != value)
                {
                    modal1Content = value;
                    OnPropertyChanged();
                }
            }
        }
        public TreeNode BaseTree
        {
            get; set;
        }
        public TreeNode ReferenceTree
        {
            get; set;
        }


        public TreeNode ComponentTree
        {
            get; set;
        }

        public TreeNode ReferenceComponentTree
        {
            get; set;
        }


        void GrowUp(TreeNode node, List<Component> components)
        {
            int len = components.Count;
            for (int i = 0; i < len; i++)
            {
                var currentNode = node.Children.Add(new TreeNode { Name = components[i].Name, Status = components[i].Status, IsExpanded = false, Modals = components[i].Modals });
                GrowUp((TreeNode)currentNode, components[i].Childs);
            }
        }
        public StructureDesignViewModel()
        {
            ComponentTree = new SimpleTreeView().ViewModel.MyTree;
            ComponentTree = new TreeNode { Name = "Component Root", Status = 0, IsExpanded = true };
            GrowUp(ComponentTree, (List<Component>)Components);

            ReferenceComponentTree = new SimpleTreeView().ViewModel.MyTree;
            ReferenceComponentTree = new TreeNode { Name = "Reference Component Root", Status = 0, IsExpanded = true };
            GrowUp(ReferenceComponentTree, (List<Component>)Components);
        }
        public TreeNode SelectedGrandFatherComponent
        {
            get { return selectedGrandFatherComponent; }
            set
            {
                if (selectedGrandFatherComponent != value)
                {
                    selectedGrandFatherComponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public TreeNode SelectedFatherComponent
        {
            get
            {
                if (selectedGrandFatherComponent?.Children.Count > 0)
                {
                    selectedFatherComponent = (HMIStudio.Shared.Interfaces.TreeNode)selectedGrandFatherComponent.Children[0];
                }
                return selectedFatherComponent;
            }
            set
            {
                if (selectedFatherComponent != value)
                {
                    selectedFatherComponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public TreeNode SelectedComponent
        {
            get
            {
                if (selectedFatherComponent?.Children.Count > 0)
                {
                    selectedComponent = (HMIStudio.Shared.Interfaces.TreeNode)selectedFatherComponent.Children[0];
                }
                return selectedComponent;
            }
            set
            {
                if (selectedComponent != value)
                {
                    selectedComponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public TreeNode SelectedGrandFatherReferenceComponent
        {
            get { return selectedGrandFatherReferenceComponent; }
            set
            {
                if (selectedGrandFatherReferenceComponent != value)
                {
                    selectedGrandFatherReferenceComponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public TreeNode SelectedFatherReferenceComponent
        {
            get
            {
                if (selectedGrandFatherReferenceComponent?.Children.Count > 0)
                {
                    selectedFatherReferenceComponent = (HMIStudio.Shared.Interfaces.TreeNode)selectedGrandFatherReferenceComponent.Children[0];
                }
                return selectedFatherReferenceComponent;
            }
            set
            {
                if (selectedFatherReferenceComponent != value)
                {
                    selectedFatherReferenceComponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public TreeNode SelectedReferenceComponent
        {
            get
            {
                if (selectedFatherReferenceComponent?.Children.Count > 0)
                {
                    selectedReferenceComponent = (HMIStudio.Shared.Interfaces.TreeNode)selectedFatherReferenceComponent.Children[0];
                }
                return selectedReferenceComponent;
            }
            set
            {
                if (selectedReferenceComponent != value)
                {
                    selectedReferenceComponent = value;
                    OnPropertyChanged();
                }
            }
        }
        //public string SelectedComponentStructure
        //{
        //    get
        //    {
        //        return selectedComponentStructure;
        //    }
        //    set
        //    {
        //        if (selectedComponentStructure != value)
        //        {
        //            selectedComponentStructure = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public string SelectedComponentStructure
        {
            get
            {
                return selectedComponentStructure;
            }
            set
            {
                //selectedComponentStructure = value;
                Set("SelectedComponentStructure", ref selectedComponentStructure, value);
            }

        }
        //protected void OnPropertyChanges([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //protected void SetPropertyValue<T>(ref T bakingFiled, T value, [CallerMemberName] string propertyName = null)
        //{
        //    bakingFiled = value;
        //    OnPropertyChanges(propertyName);

        //}
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                if (propertyName == "SelectedGrandFatherComponent")
                {
                    OnPropertyChanged(nameof(SelectedFatherComponent));
                }
                else if (propertyName == "SelectedFatherComponent")
                {
                    OnPropertyChanged(nameof(SelectedComponent));
                }
            }
        }
        public delegate void HandleSelectedComponent(string Name);
        public delegate void ShowModal1(string Name);
        public void SetSelectedComponent(string name)
        {
            Console.WriteLine("Showing selected component {0}", name);
            SelectedComponentStructure = name;
            //OnPropertyChanged(nameof(SelectedComponentStructure));

        }
        public void SetModal1(string name)
        {
            Console.WriteLine("Showing modal  {0}", name);
            Modal1Content = name;
            modal1IsVisible = true;

        }

        public void ExitModal(object sender, EventArgs e)
        {
            //(sender as Button).Text = "I was just clicked!";
            modal1IsVisible = false;
            Console.WriteLine(modal1IsVisible);
            Console.WriteLine(Modal1IsVisible);
        }
    }
}
