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
    public class MooreDijkstraViewModel : ViewModelBase, IAlgorithmViewModel, IAlgorithmRequirementViewModel, IVertexRun
    {
        private MooreDijkstra _mooreDijkstra;
        public string AlgorithmName
        {
            get => _mooreDijkstra.AlgorithmName;
        }

        public bool IsSetCompletedAlgorithm { get; set;}

        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _mooreDijkstra.Pseudocodes;
        }

        public List<RequestOfAlgorithm> Requirements
        {
            get => _mooreDijkstra.Requirements;
        }

        public MooreDijkstraViewModel()
        {
            _mooreDijkstra = new MooreDijkstra(); 
            IsSetCompletedAlgorithm = false;
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            return _mooreDijkstra.CheckRequirements(graph);
        }

        public void ContinueAlgorithm(CustomGraph graph)
        {
            _mooreDijkstra.ContinueAlgorithm(graph);
        }

        public void PauseAlgorithm()
        {
            _mooreDijkstra.PauseAlgorithm();
        }

        public void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            _mooreDijkstra.RunAlgorithm(graph, startVertex);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _mooreDijkstra.CompletedAlgorithm += returnIsRunningState;
        }

        public void SetRunSpeed(int speedUp)
        {
            _mooreDijkstra.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _mooreDijkstra.StopAlgorithm(graph);
        }
    }
}
