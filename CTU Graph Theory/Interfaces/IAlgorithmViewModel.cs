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
        public string AlgorithmName { get; }
        public Vertex? StartVertex { get; set; }
        public bool IsSetCompletedAlgorithm { get; set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; }
        public void TransferStartVertex(Vertex startVertex);
        public void RunAlgorithm(CustomGraph graph);
        public void RunAlgorithmWithAllVertex(CustomGraph graph,ObservableCollection<Vertex> vertices);
        public void PauseAlgorithm();
        public void ContinueAlgorithm(CustomGraph garph);
        public void ContinueAlgorithmWithAllVertex(CustomGraph graph);
        public void StopAlgorithm(CustomGraph graph);
        public void SetRunSpeed(int speedUp);
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState);
    }
}
