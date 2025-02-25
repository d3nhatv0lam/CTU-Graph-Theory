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
    class DFSStackViewModel: ViewModelBase, IAlgorithmViewModel
    {
        private DFSStack _dfsStack;
        public CustomGraph _Graph { get; set; }
        public string AlgorithmName
        {
            get => _dfsStack.AlgorithmName;
        }
        public Vertex? StartVertex
        {
            get => _dfsStack.StartVertex;
            set
            {
                if (_dfsStack.StartVertex == value) return;
                _dfsStack.StartVertex = value;
            }
        }
        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes 
        { 
            get => _dfsStack.Pseudocodes; 
        }

        public DFSStackViewModel()
        {
            _dfsStack = new DFSStack();
            IsSetCompletedAlgorithm = false;
        }

        public void TransferGraph(CustomGraph graph, Vertex vertex)
        {
            _Graph = graph;
            StartVertex = vertex;
        }

        public void RunAlgorithm()
        {
            _dfsStack.RunAlgorithm(_Graph);
        }

        public void PauseAlgorithm()
        {
            _dfsStack.PauseAlgorithm();
        }

        public void SetRunSpeed(int speedUp)
        {
            _dfsStack.SetRunSpeed(speedUp);
        }
        public void ContinueAlgorithm()
        {
            _dfsStack.ContinueAlgorithm(_Graph);
        }
        public void StopAlgorithm()
        {
            _dfsStack.StopAlgorithm(_Graph);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (IsSetCompletedAlgorithm) return;
      
               _dfsStack.CompletedAlgorithm += returnIsRunningState;
           
        }


        public void RunAlgorithmWithAllVertex(ObservableCollection<Vertex> vertices)
        {
            _dfsStack.RunAlgorithmWithAllVertex(_Graph,vertices);
        }

        public void ContinueAlgorithmWithAllVertex()
        {
            _dfsStack.ContinueAlgorithmWithAllVertex(_Graph);
        }
    }
}
