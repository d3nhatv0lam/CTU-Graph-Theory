using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class ViewLocatorNode
    {
        // node ID <=> ViewModel ID
        public int Id { get; }
        public Material.Icons.MaterialIconKind Kind { get; }
        public string Title { get; }
        
        public ViewLocatorNode() { }

        public ViewLocatorNode(int id,Material.Icons.MaterialIconKind kind,string title) 
        {
            Id = id;
            Kind = kind;
            Title = title;
        }
    }
}
