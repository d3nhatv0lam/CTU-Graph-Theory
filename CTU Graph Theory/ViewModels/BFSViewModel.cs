using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Xaml.Interactivity;
using AvaloniaGraphControl;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using ReactiveUI;

namespace CTU_Graph_Theory.ViewModels
{
    public class BFSViewModel: ViewModelBase, IAlgorithmViewModel
    {
        private BFS _bfs;
        private CustomGraph _graph;
        public CustomGraph _Graph
        {
            get => _graph;
            set => _graph = value;
        }
        public string AlgorithmName
        {
            get => _bfs.AlgorithmName;
        }

        public Vertex? StartVertex
        {
            get => _bfs.StartVertex;
            set 
            {
                if (_bfs.StartVertex == value) return;
                _bfs.StartVertex = value;
            }
        }
        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _bfs.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; } = false;

        public BFSViewModel()
        {
            _bfs = new BFS();
        }

        public void TransferGraph(CustomGraph graph,Vertex? startVertex)
        {
            _graph = graph;
            StartVertex = startVertex;
        }

        public void RunAlgorithm()
        {
            _bfs.RunAlgorithm(_graph);
        }

        public void PauseAlgorithm()
        {
            _bfs.PauseAlgorithm();
        }
        public void ContinueAlgorithm()
        {
            _bfs.ContinueAlgorithm(_Graph);
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
