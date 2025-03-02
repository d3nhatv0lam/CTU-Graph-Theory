using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms;

namespace CTU_Graph_Theory.ViewModels
{
    public class CircledCheckViewModel : IAlgorithmViewModel
    {
        private AbstractAlgorithm _circledCheckAlgorithm;
        public CustomGraph _Graph { get; set; }

        public string AlgorithmName
        {
            get => _circledCheckAlgorithm.AlgorithmName;
        }

        public Vertex? StartVertex {
            get => _circledCheckAlgorithm.StartVertex;
            set => _circledCheckAlgorithm.StartVertex = value;
        }
        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _circledCheckAlgorithm.Pseudocodes;
        }

        public CircledCheckViewModel()
        {
            _circledCheckAlgorithm = new CircledCheck();
            IsSetCompletedAlgorithm = false;
        }

        public void ContinueAlgorithm()
        {
            _circledCheckAlgorithm.ContinueAlgorithm(_Graph);
        }

        public void ContinueAlgorithmWithAllVertex()
        {
            _circledCheckAlgorithm.ContinueAlgorithmWithAllVertex(_Graph);
        }

        public void PauseAlgorithm()
        {
            _circledCheckAlgorithm.PauseAlgorithm();
        }

        public void RunAlgorithm()
        {
            _circledCheckAlgorithm.RunAlgorithm(_Graph);
        }

        public void RunAlgorithmWithAllVertex(ObservableCollection<Vertex> vertices)
        {
            _circledCheckAlgorithm.RunAlgorithmWithAllVertex(_Graph,vertices);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _circledCheckAlgorithm.CompletedAlgorithm += returnIsRunningState;
        }

        public void SetRunSpeed(int speedUp)
        {
            _circledCheckAlgorithm.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm()
        {
            _circledCheckAlgorithm.StopAlgorithm(_Graph);
        }

        public void TransferGraph(CustomGraph graph, Vertex startVertex)
        {
            _Graph = graph;
            StartVertex = startVertex;
        }
    }
}
