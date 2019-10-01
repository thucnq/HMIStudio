using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HMIStudio.Shared.Model
{
    public class Component
    {
        private int status;
        private string name;
        private List<Component> childs = new List<Component>();
        private List<int> modals = new List<int>();
        public int Status
        {
            get => status;
            set => status = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public List<int> Modals
        {
            get => modals;
            set => modals = value;
        }
        public List<Component> Childs
        {
            get => childs;
            set => childs = value;
        }

        public Component()
        {
            status = 0;
        }
        public void Appear()
        {
            ShowEventArgs args = new ShowEventArgs();
            args.Name = name;
            OnShow(args);
        }
        protected virtual void OnShow(ShowEventArgs e)
        {
            EventHandler<ShowEventArgs> handler = Show;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public event EventHandler<ShowEventArgs> Show;
    }

    public class ShowEventArgs : EventArgs
    {
        public int Status { get; set; }
        public string Name { get; set; }
        public List<Component> Childs { get; set; }
    }

    class StructureDesign
    {


        public StructureDesign()
        {

        }
    }
}
