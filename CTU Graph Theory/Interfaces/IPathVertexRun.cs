using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface IPathVertexRun
    {
        public void RunAlgorithm(CustomGraph graph, Vertex startVertex, Vertex endVertex);
        public void ContinueAlgorithm(CustomGraph graph);
    }
}
