using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public bool IsSetCompletedAlgorithm { get; set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; }
        
        public void TransferGraph(CustomGraph graph, Vertex vertex);
        public void RunAlgorithm();
        public void PauseAlgorithm();
        public void SetRunSpeed(int speedUp);
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState);
    }
}
