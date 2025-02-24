using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface IAlgorithmBase
    {
         protected void FillPseudoCode();
         protected void StartVetexChanged(CustomGraph graph);
         public void RunAlgorithm(CustomGraph graph);
         public void ContinueAlgorithm(CustomGraph graph);
    }
}
