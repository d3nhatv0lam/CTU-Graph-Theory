using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.ViewModels
{
    public class TarjanSCCViewModel : ViewModelBase, IAlgorithmViewModel, IAlgorithmRequirementViewModel
    {
        private TarjanSCC _scc;
        public CustomGraph _Graph { get; set; }

        public string AlgorithmName
        {
            get => _scc.AlgorithmName;
        }

        public Vertex? StartVertex { get => _scc.StartVertex; set => _scc.StartVertex = value; }
        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _scc.Pseudocodes;
        }

        public ObservableCollection<RequestOfAlgorithm> Requirements
        {
            get => _scc.Requirements;
        }

        public TarjanSCCViewModel()
        {
            _scc = new TarjanSCC();
            IsSetCompletedAlgorithm = false;
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            return _scc.CheckRequirements(graph);
        }

        public void ContinueAlgorithm()
        {
            _scc.ContinueAlgorithm(_Graph);
        }

        public void ContinueAlgorithmWithAllVertex()
        {
            _scc.ContinueAlgorithmWithAllVertex(_Graph);
        }

        public void PauseAlgorithm()
        {
            _scc.PauseAlgorithm();
        }

        public void RunAlgorithm()
        {
            _scc.RunAlgorithm(_Graph);
        }

        public void RunAlgorithmWithAllVertex(ObservableCollection<Vertex> vertices)
        {
            _scc.RunAlgorithmWithAllVertex(_Graph, vertices);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (IsSetCompletedAlgorithm) return;
            _scc.CompletedAlgorithm += returnIsRunningState;
        }

        public void SetRunSpeed(int speedUp)
        {
            _scc.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm()
        {
            _scc.StopAlgorithm(_Graph);
        }

        public void TransferGraph(CustomGraph graph, Vertex startVertex)
        {
            _Graph = graph;
            StartVertex = startVertex;
        }
    }
}
