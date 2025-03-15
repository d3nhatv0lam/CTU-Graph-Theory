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
    public class PrimViewModel : ViewModelBase, IAlgorithmViewModel, IAlgorithmRequirementViewModel, IVertexRun
    {
        private Prim _prim;
        public string AlgorithmName => _prim.AlgorithmName;

        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes => _prim.Pseudocodes;

        public List<RequestOfAlgorithm> Requirements => _prim.Requirements;

        public PrimViewModel()
        {
            _prim = new();
            IsSetCompletedAlgorithm = false;
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            return _prim.CheckRequirements(graph);
        }

        public void ContinueAlgorithm(CustomGraph graph)
        {
            _prim.ContinueAlgorithm(graph);
        }

        public void PauseAlgorithm()
        {
            _prim.PauseAlgorithm();
        }

        public void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            _prim.RunAlgorithm(graph, startVertex);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (!IsSetCompletedAlgorithm)
            {
                _prim.CompletedAlgorithm += returnIsRunningState;
                IsSetCompletedAlgorithm = true;
            }
        }

        public void SetRunSpeed(int speedUp)
        {
            _prim.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _prim.StopAlgorithm(graph);
        }
    }
}
