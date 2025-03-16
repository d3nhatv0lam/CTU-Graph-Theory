using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Algorithms.Base;
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
    public class DFSRecursiveViewModel : ViewModelBase, IAlgorithmViewModel , IAllVertexRun
    {

        private DFSRecursive _dfsRecursive;
        public string AlgorithmName
        {
            get => _dfsRecursive.AlgorithmName;
        }

        public ObservableCollection<ObservableCollection<string>> Result
        {
            get
            {
                return _dfsRecursive.Result;
            }
        }
        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _dfsRecursive.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; }



        public DFSRecursiveViewModel()
        {
            _dfsRecursive = new DFSRecursive();
            IsSetCompletedAlgorithm = false;
        }


        public void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            _dfsRecursive.RunAlgorithm(graph, startVertex);
        }

        public void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            _dfsRecursive.RunAlgorithmWithAllVertex(graph, vertices);
        }

        public void PauseAlgorithm()
        {
            _dfsRecursive.PauseAlgorithm();
        }
        public void ContinueAlgorithm(CustomGraph graph)
        {
            _dfsRecursive.ContinueAlgorithm(graph);
        }

        public void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            _dfsRecursive.ContinueAlgorithmWithAllVertex(graph);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _dfsRecursive.StopAlgorithm(graph);
        }
        public void SetRunSpeed(int speedUp)
        {
            _dfsRecursive.SetRunSpeed(speedUp);
        }
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _dfsRecursive.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }
    }
}
