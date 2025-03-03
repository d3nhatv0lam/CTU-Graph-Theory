using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Models;

namespace CTU_Graph_Theory.Interfaces
{
    public interface ITargetVertexViewModel: ITargetVertex
    {
        public void TransferTargetVertex(Vertex targetVertex);
    }
}
