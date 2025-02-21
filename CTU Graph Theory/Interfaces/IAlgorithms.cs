using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface IAlgorithms
    {
        // cơ bản
        public string AlgorithmName { get; }
        public  List<StringPseudoCode> Pseudocodes { get; }

        public void RunAlgorithm(CustomGraph graph);
        public void PauseAlgorithm();

        protected virtual void FillPseudoCode()
        {

        }


    }
}
