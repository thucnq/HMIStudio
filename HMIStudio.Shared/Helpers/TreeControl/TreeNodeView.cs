﻿using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using HMIStudio.Shared.TreeView.Collections;

namespace HMIStudio.Shared.TreeView.Controls
{
    // analog to ITreeNode<T>
    public partial class TreeNodeView : StackLayout
    {
        Grid MainLayoutGrid;
        ContentView HeaderView;
        StackLayout ChildrenStackLayout;

        TreeNodeView ParentTreeNodeView { get; set; }

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create("IsExpanded", typeof(bool), typeof(TreeNodeView), true, BindingMode.TwoWay, null,
            (bindable, oldValue, newValue) =>
            {
                var node = bindable as TreeNodeView;

                if (oldValue == newValue || node == null)
                    return;

                node.BatchBegin();
                try
                {
                    // show or hide all children
                    node.ChildrenStackLayout.IsVisible = node.IsExpanded;
                }
                finally
                {
                    // ensure we commit
                    node.BatchCommit();
                }
            }
            , null, null);

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public View HeaderContent
        {
            get { return HeaderView.Content; }
            set { HeaderView.Content = value; }
        }

        public IEnumerable<TreeNodeView> ChildTreeNodeViews
        {
            get
            {
                foreach (TreeNodeView view in ChildrenStackLayout.Children)
                    yield return view;

                yield break;
            }
        }

        protected void DetachVisualChildren()
        {
            var views = ChildrenStackLayout.Children.OfType<TreeNodeView>().ToList();

            foreach (TreeNodeView nodeView in views)
            {
                ChildrenStackLayout.Children.Remove(nodeView);
                nodeView.ParentTreeNodeView = null;
            }
        }

        protected override void OnBindingContextChanged()
        {
            // prevent exceptions for null binding contexts
            // and during startup, this node will inherit its BindingContext from its Parent - ignore this
            if (BindingContext == null || (Parent != null && BindingContext == Parent.BindingContext))
                return;

            var node = BindingContext as ITreeNode;
            if (node == null)
                throw new InvalidOperationException("TreeNodeView currently only supports TreeNode-derived data binding sources.");

            base.OnBindingContextChanged();

            // clear out any existing child nodes - the new data source replaces them
            // make sure we don't do this if BindingContext == null
            DetachVisualChildren();

            // build the new visual tree
            BuildVisualChildren();
        }

        Func<View> _HeaderCreationFactory;
        public Func<View> HeaderCreationFactory
        {
            // [recursive up] inherit property value from parent if null
            get
            {
                if (_HeaderCreationFactory != null)
                    return _HeaderCreationFactory;

                if (ParentTreeNodeView != null)
                    return ParentTreeNodeView.HeaderCreationFactory;

                return null;
            }
            set
            {
                if (value == _HeaderCreationFactory)
                    return;

                _HeaderCreationFactory = value;
                OnPropertyChanged("HeaderCreationFactory");

                // wait until both factories are assigned before constructing the visual tree
                if (_HeaderCreationFactory == null || _NodeCreationFactory == null)
                    return;

                BuildHeader();
                BuildVisualChildren();
            }
        }

        Func<TreeNodeView> _NodeCreationFactory;
        public Func<TreeNodeView> NodeCreationFactory
        {
            // [recursive up] inherit property value from parent if null
            get
            {
                if (_NodeCreationFactory != null)
                    return _NodeCreationFactory;

                if (ParentTreeNodeView != null)
                    return ParentTreeNodeView.NodeCreationFactory;

                return null;
            }
            set
            {
                if (value == _NodeCreationFactory)
                    return;

                _NodeCreationFactory = value;
                OnPropertyChanged("NodeCreationFactory");

                // wait until both factories are assigned before constructing the visual tree
                if (_HeaderCreationFactory == null || _NodeCreationFactory == null)
                    return;

                BuildHeader();
                BuildVisualChildren();
            }
        }

        protected void BuildHeader()
        {
            // the new HeaderContent will inherit its BindingContext from this.BindingContext [recursive down]
            if (HeaderCreationFactory != null)
                HeaderContent = HeaderCreationFactory.Invoke();
        }

        // [recursive down] create item template instances, attach and layout, and set descendents until finding overrides
        protected void BuildVisualChildren()
        {
            var bindingContextNode = (ITreeNode)BindingContext;
            if (bindingContextNode == null)
                return;

            // STEP 1: remove child visual tree nodes (TreeNodeViews) that don't correspond to an item in our data source

            var nodeViewsToRemove = new List<TreeNodeView>();

            var bindingChildList = new List<ITreeNode>(bindingContextNode != null ? bindingContextNode.ChildNodes : null);

            // which ChildTreeNodeViews are in the visual tree... ?
            foreach (TreeNodeView nodeView in ChildTreeNodeViews)
                // but missing from the bound data source?
                if (!bindingChildList.Contains(nodeView.BindingContext))
                    // tag them for removal from the visual tree
                    nodeViewsToRemove.Add(nodeView);

            BatchBegin();
            try
            {
                // perform removal in a batch
                foreach (TreeNodeView nodeView in nodeViewsToRemove)
                    MainLayoutGrid.Children.Remove(nodeView);
            }
            finally
            {
                // ensure we commit
                BatchCommit();
            }

            // STEP 2: add visual tree nodes (TreeNodeViews) for children of the binding context not already associated with a TreeNodeView

            if (NodeCreationFactory != null)
            {
                var nodeViewsToAdd = new Dictionary<TreeNodeView, ITreeNode>();

                foreach (ITreeNode node in bindingContextNode.ChildNodes)
                {
                    if (!ChildTreeNodeViews.Any(nodeView => nodeView.BindingContext == node))
                    {
                        var nodeView = NodeCreationFactory.Invoke();
                        nodeView.ParentTreeNodeView = this;

                        if (HeaderCreationFactory != null)
                            nodeView.HeaderContent = HeaderCreationFactory.Invoke();

                        // the order of these may be critical
                        nodeViewsToAdd.Add(nodeView, node);
                    }
                }

                BatchBegin();
                try
                {
                    // perform the additions in a batch
                    foreach (KeyValuePair<TreeNodeView, ITreeNode> nodeView in nodeViewsToAdd)
                    {
                        // only set BindingContext after the node has Parent != null
                        nodeView.Key.BindingContext = nodeView.Value;
                        nodeView.Value.ExpandAction = () => nodeView.Key.BuildVisualChildren();
                        nodeView.Key.ChildrenStackLayout.IsVisible = nodeView.Key.IsExpanded;
                        ChildrenStackLayout.Children.Add(nodeView.Key);

                        ChildrenStackLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsExpanded", BindingMode.TwoWay));

                        // TODO: make sure to unsubscribe elsewhere
                        nodeView.Value.PropertyChanged += HandleListCountChanged;
                    }
                }
                finally
                {
                    // ensure we commit
                    BatchCommit();
                }
            }
        }

        void HandleListCountChanged(object sender, PropertyChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (e.PropertyName == "Count")
                {
                    var nodeView = ChildTreeNodeViews.Where(nv => nv.BindingContext == sender).FirstOrDefault();
                    if (nodeView != null)
                        nodeView.BuildVisualChildren();
                }
            });
        }

        public void InitializeComponent()
        {
            IsExpanded = true;

            MainLayoutGrid = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 2
            };
            MainLayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainLayoutGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainLayoutGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            HeaderView = new ContentView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = this.BackgroundColor
            };
            MainLayoutGrid.Children.Add(HeaderView);

            ChildrenStackLayout = new StackLayout
            {
                Orientation = this.Orientation,
                Spacing = 0
            };
            MainLayoutGrid.Children.Add(ChildrenStackLayout, 0, 1);

            Children.Add(MainLayoutGrid);

            Spacing = 0;
            Padding = new Thickness(0);
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Start;
        }

        public TreeNodeView() : base()
        {
            InitializeComponent();

            Debug.WriteLine("new TreeNodeView");
        }
    }
}