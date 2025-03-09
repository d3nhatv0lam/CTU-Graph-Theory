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
    public class TarjanSCCViewModel : ViewModelBase, IAlgorithmViewModel, IAlgorithmRequirementViewModel , IAllVertexRun
    {
        private TarjanSCC _scc;
        public string AlgorithmName
        {
            get => _scc.AlgorithmName;
        }
        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _scc.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; }

        public List<RequestOfAlgorithm> Requirements
        {
            get => _scc.Requirements;
        }

        public TarjanSCCViewModel()
        {
            _scc = new TarjanSCC();
            IsSetCompletedAlgorithm = false;
        }


        public void RunAlgorithm(CustomGraph graph,Vertex startVertex)
        {
            _scc.RunAlgorithm(graph,startVertex);
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

        public bool CheckRequirements(CustomGraph graph)
        {
            return _scc.CheckRequirements(graph);
        }
    }
}
