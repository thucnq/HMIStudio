using System;
using System.Collections.Generic;
using HMIStudio.Shared.Model;

namespace HMIStudio.Shared.Helpers
{
    public class ComponentData
    {

        public static IList<Component> Components { get; private set; }
        public static IList<Component> ReferenceComponents { get; private set; }
        static ComponentData()
        {
            Random rnd = new Random();
            IList<Component> hypergrandchilds = new List<Component>();
            for (int i = 0; i < 273; i++)
            {
                hypergrandchilds.Add(new Component { });
                hypergrandchilds[i].Name = "Hyper Grand Child " + i.ToString();
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 0)
                {
                    hypergrandchilds[i].Status = 1;
                    if (rand % 2 == 0)
                    {
                        hypergrandchilds[i].Modals = new List<int> { 1, 2 };
                    }
                    else if (rand % 3 == 0)
                    {
                        hypergrandchilds[i].Modals = new List<int> { 1, 3 };
                    }
                    else if (rand % 4 == 0)
                    {
                        hypergrandchilds[i].Modals = new List<int> { 1 };
                    }
                }
            }

            IList<Component> grandchilds = new List<Component>();
            for (int i = 0; i < 81; i++)
            {
                grandchilds.Add(new Component { });
                grandchilds[i].Name = "Grand Child " + i.ToString();
                grandchilds[i].Childs = ((List<Component>)hypergrandchilds).GetRange(i * 3, 3);
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 1)
                {
                    hypergrandchilds[i].Status = 1;
                }
            }

            IList<Component> childs = new List<Component>();
            for (int i = 0; i < 27; i++)
            {
                childs.Add(new Component { });
                childs[i].Name = "Child " + i.ToString();
                childs[i].Childs = ((List<Component>)grandchilds).GetRange(i * 3, 3);
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 1)
                {
                    hypergrandchilds[i].Status = 1;
                }
            }

            IList<Component> fathers = new List<Component>();
            for (int i = 0; i < 9; i++)
            {
                fathers.Add(new Component { });
                fathers[i].Name = "Father " + i.ToString();
                fathers[i].Childs = ((List<Component>)childs).GetRange(i * 3, 3);
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 1)
                {
                    hypergrandchilds[i].Status = 1;
                }
            }

            Components = new List<Component>();
            for (int i = 0; i < 3; i++)
            {
                Components.Add(new Component { });
                Components[i].Name = "Grand Father " + i.ToString();
                Components[i].Childs = ((List<Component>)fathers).GetRange(i * 3, 3);
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 1)
                {
                    hypergrandchilds[i].Status = 1;
                }
            }

            ReferenceComponents = new List<Component>();
            for (int i = 0; i < 3; i++)
            {
                ReferenceComponents.Add(new Component { });
                ReferenceComponents[i].Name = "Grand Father Ref" + i.ToString();
                ReferenceComponents[i].Childs = ((List<Component>)fathers).GetRange(i * 3, 3);
                int rand = rnd.Next(1, 3);
                if (rand % 2 == 1)
                {
                    hypergrandchilds[i].Status = 1;
                }
            }
        }
    }
}
