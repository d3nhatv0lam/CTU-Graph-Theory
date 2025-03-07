using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface IAllVertexRun : IVertexRun
    {
        public void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices);
        public void ContinueAlgorithmWithAllVertex(CustomGraph graph);
    }
}
