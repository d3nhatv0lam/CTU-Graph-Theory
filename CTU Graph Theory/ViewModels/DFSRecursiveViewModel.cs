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
    public class DFSRecursiveViewModel : ViewModelBase, IAlgorithmViewModel
    {
        private DFSRecursive _dfsRecursive;
        public CustomGraph _Graph { get; set; }

        public string AlgorithmName { get => _dfsRecursive.AlgorithmName; }

        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _dfsRecursive.Pseudocodes;
        }
        public Vertex? StartVertex 
        {
            get => _dfsRecursive.StartVertex; 
            set => _dfsRecursive.StartVertex = value;
        }

        public DFSRecursiveViewModel()
        {
            _dfsRecursive = new DFSRecursive();
            IsSetCompletedAlgorithm = false;
        }

        public void ContinueAlgorithm()
        {
            _dfsRecursive.ContinueAlgorithm(_Graph);
        }

        public void ContinueAlgorithmWithAllVertex()
        {
            _dfsRecursive.ContinueAlgorithmWithAllVertex(_Graph);
        }

        public void PauseAlgorithm()
        {
            _dfsRecursive.PauseAlgorithm();
        }

        public void RunAlgorithm()
        {
            _dfsRecursive.RunAlgorithm(_Graph);
        }

        public void RunAlgorithmWithAllVertex(ObservableCollection<Vertex> vertices)
        {
            _dfsRecursive.RunAlgorithmWithAllVertex(_Graph, vertices);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (IsSetCompletedAlgorithm) return;
            _dfsRecursive.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }

        public void SetRunSpeed(int speedUp)
        {
            _dfsRecursive.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm()
        {
            _dfsRecursive.StopAlgorithm(_Graph);
        }

        public void TransferGraph(CustomGraph graph, Vertex startVertex)
        {
            _Graph = graph;
            StartVertex = startVertex;
        }
    }
}
