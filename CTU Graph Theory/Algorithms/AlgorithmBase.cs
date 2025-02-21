using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class AlgorithmBase
    {
        // base
        public string AlgorithmName { get; protected set; }
        public List<StringPseudoCode> Pseudocodes { get; protected set; }
        public Vertex? StartVertex { get; set; } = null;

        // Animation
        protected CancellationTokenSource? cts = new();

        public AlgorithmBase() { }

        protected virtual void FillPseudoCode() { }
        protected virtual void StartVetexChanged(CustomGraph graph) { }
        public virtual void RunAlgorithm(CustomGraph graph) { }
        public virtual void PauseAlgorithm() 
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
        }

    }
}
