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
    public interface IAlgorithmViewModel
    {
        // cơ bản
        public CustomGraph _Graph { get; set; }
        public string AlgorithmName { get; }
        public bool IsSetCompletedAlgorithm { get; set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; }
        public void TransferGraph(CustomGraph graph, Vertex startVertex);
        public void RunAlgorithm();
        public void RunAlgorithmWithAllVertex(ObservableCollection<Vertex> vertices);
        public void PauseAlgorithm();
        public void ContinueAlgorithm();
        public void ContinueAlgorithmWithAllVertex();
        public void SetRunSpeed(int speedUp);
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState);
    }
}
