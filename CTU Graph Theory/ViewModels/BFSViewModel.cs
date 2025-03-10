using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Xaml.Interactivity;
using AvaloniaGraphControl;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using ReactiveUI;

namespace CTU_Graph_Theory.ViewModels
{
    public class BFSViewModel: ViewModelBase, IAlgorithmViewModel , IAllVertexRun
    {
        private BFS _bfs;
        public string AlgorithmName
        {
            get => _bfs.AlgorithmName;
        }

        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _bfs.Pseudocodes;
        }

        public ObservableCollection<ObservableCollection<string>> Result
        {
            get
            {
                return _bfs.Result;
            }
        }



        public bool IsSetCompletedAlgorithm { get; set; }


        public BFSViewModel()
        {
            _bfs = new BFS();
            IsSetCompletedAlgorithm = false;
        }

        public void RunAlgorithm(CustomGraph graph,Vertex startVertex)
        {
            _bfs.RunAlgorithm(graph,startVertex);
        }

        public void RunAlgorithmWithAllVertex(CustomGraph graph,ObservableCollection<Vertex> vertices)
        {
            _bfs.RunAlgorithmWithAllVertex(graph, vertices);
        }

        public void PauseAlgorithm()
        {
            _bfs.PauseAlgorithm();
        }
        public void ContinueAlgorithm(CustomGraph graph)
        {
            _bfs.ContinueAlgorithm(graph);
        }

        public void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            _bfs.ContinueAlgorithmWithAllVertex(graph);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _bfs.StopAlgorithm(graph);
        }
        public void SetRunSpeed(int speedUp)
        {
            _bfs.SetRunSpeed(speedUp);
        }
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _bfs.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }
    }
}
