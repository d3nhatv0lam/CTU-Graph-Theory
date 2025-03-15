using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface INonStartVertexRun
    {
        public void RunAlgorithm(CustomGraph graph);
        public void ContinueAlgorithm(CustomGraph graph);
    }
}
