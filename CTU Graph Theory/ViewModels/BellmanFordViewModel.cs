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
    public class BellmanFordViewModel : ViewModelBase,IAlgorithmViewModel, IVertexRun, IAlgorithmRequirementViewModel
    {
        private BellmanFord _bellmanFord;
        public List<RequestOfAlgorithm> Requirements => _bellmanFord.Requirements;

        public string AlgorithmName => _bellmanFord.AlgorithmName;

        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes => _bellmanFord.Pseudocodes;

        public BellmanFordViewModel()
        {
            _bellmanFord = new(); IsSetCompletedAlgorithm = false;
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            return _bellmanFord.CheckRequirements(graph);
        }

        public void ContinueAlgorithm(CustomGraph graph)
        {
            _bellmanFord.ContinueAlgorithm(graph);
        }

        public void PauseAlgorithm()
        {
            _bellmanFord.PauseAlgorithm();
        }

        public void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            _bellmanFord.RunAlgorithm(graph, startVertex);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (!IsSetCompletedAlgorithm)
            {
                _bellmanFord.CompletedAlgorithm += returnIsRunningState;
                IsSetCompletedAlgorithm = true;
            }
        }

        public void SetRunSpeed(int speedUp)
        {
            _bellmanFord.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _bellmanFord.StopAlgorithm(graph);
        }
    }
}
