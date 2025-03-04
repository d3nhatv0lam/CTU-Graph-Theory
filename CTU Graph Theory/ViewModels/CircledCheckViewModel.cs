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
        private AbstractAlgorithm _scc;
        public string AlgorithmName
        {
            get => _scc.AlgorithmName;
        }

        public Vertex? StartVertex
        {
            get => _scc.StartVertex;
            set
            {
                if (_scc.StartVertex == value) return;
                _scc.StartVertex = value;
            }
        }
        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _scc.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; } = false;

        public ObservableCollection<RequestOfAlgorithm> Requirements => throw new NotImplementedException();

        public CircledCheckViewModel()
        {
            _scc = new CircledCheck();
        }

        public void TransferStartVertex(Vertex? startVertex)
        {
            StartVertex = startVertex;
        }

        public void RunAlgorithm(CustomGraph graph)
        {
            _scc.RunAlgorithm(graph);
        }

        public void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            _scc.RunAlgorithmWithAllVertex(graph, vertices);
        }

        public void PauseAlgorithm()
        {
            _scc.PauseAlgorithm();
        }
        public void ContinueAlgorithm(CustomGraph graph)
        {
            _scc.ContinueAlgorithm(graph);
        }

        public void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            _scc.ContinueAlgorithmWithAllVertex(graph);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _scc.StopAlgorithm(graph);
        }
        public void SetRunSpeed(int speedUp)
        {
            _scc.SetRunSpeed(speedUp);
        }
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _scc.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }
    }
}
