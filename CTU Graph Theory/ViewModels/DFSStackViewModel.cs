using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Algorithms.Base;

namespace CTU_Graph_Theory.ViewModels
{
    class DFSStackViewModel: ViewModelBase, IAlgorithmViewModel , IAllVertexRun
    {
        private DFSStack _dfsStack;
        public string AlgorithmName
        {
            get => _dfsStack.AlgorithmName;
        }



        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _dfsStack.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<ObservableCollection<string>> Result
        {
            get
            {
                return _dfsStack.Result;
            }
        }

        public DFSStackViewModel()
        {
            _dfsStack = new DFSStack();
            IsSetCompletedAlgorithm = false;    
        }


        public void RunAlgorithm(CustomGraph graph,Vertex startVertex)
        {
            _dfsStack.RunAlgorithm(graph, startVertex);
        }

        public void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            _dfsStack.RunAlgorithmWithAllVertex(graph, vertices);
        }

        public void PauseAlgorithm()
        {
            _dfsStack.PauseAlgorithm();
        }
        public void ContinueAlgorithm(CustomGraph graph)
        {
            _dfsStack.ContinueAlgorithm(graph);
        }

        public void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            _dfsStack.ContinueAlgorithmWithAllVertex(graph);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _dfsStack.StopAlgorithm(graph);
        }
        public void SetRunSpeed(int speedUp)
        {
            _dfsStack.SetRunSpeed(speedUp);
        }
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _dfsStack.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }
    }
}
