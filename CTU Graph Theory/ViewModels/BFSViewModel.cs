using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Models;
using ReactiveUI;

namespace CTU_Graph_Theory.ViewModels
{
    public class BFSViewModel: ViewModelBase
    {
        private BFS _bfs;

        private bool _isRunningAlgorithm = false;

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

        public bool IsRunningAlgorithm
        {
            get => _isRunningAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _isRunningAlgorithm, value);
        }

        public BFSViewModel()
        {
            _bfs = new BFS();
        }
    }
}
