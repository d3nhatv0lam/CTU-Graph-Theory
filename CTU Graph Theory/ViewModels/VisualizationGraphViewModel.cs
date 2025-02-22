
using Avalonia.Controls;
using AvaloniaGraphControl;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Models;
using DynamicData;

using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.ViewModels
{
    public class VisualizationGraphViewModel: ViewModelBase
    {
        private CustomGraph _mainGraph = new CustomGraph();
        private int _vertexCount = 0;
        private bool _isDirectedGraph = false;
        private string _graphData = string.Empty;
        private List<ViewModelBase> _algorithmList;
        private ViewModelBase _selectedAlgorithm;

        public CustomGraph MainGraph
        {
            get => _mainGraph;
            set => this.RaiseAndSetIfChanged(ref _mainGraph, value);
        }
        public int VertexCount
        {
            get => _vertexCount;
            set => this.RaiseAndSetIfChanged(ref _vertexCount, value);
        }
        public string GraphData
        {
            get => _graphData;
            set => this.RaiseAndSetIfChanged(ref _graphData, value);
        }

        public List<ViewModelBase> AlgorithmList
        {
            get 
            {
                if (_algorithmList == null) _algorithmList = new();
                return _algorithmList;
            }   
        }

        public ViewModelBase SelectedAlgorithm
        {
            get => _selectedAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
        }
     

        public ReactiveCommand<RadioButton, Unit> ChangeGraphTypeCommand { get; private set; }

        public VisualizationGraphViewModel()
        {
            //var v1 = new Vertex("1");
            //var v2 = new Vertex("2");
            //var v3 = new Vertex("3");
            //var v4 = new Vertex("4");


            //MainGraph.Edges.Add(new ShowableEdge(v1, v1, "aaa",ShowableEdge.Visible.Show, Edge.Symbol.None, Edge.Symbol.None));
            //MainGraph.Edges.Add(new ShowableEdge(v1, v2, "asdafdass", ShowableEdge.Visible.Show, Edge.Symbol.None, Edge.Symbol.None));
            //MainGraph.Edges.Add(new ShowableEdge(v3, v4, "a", ShowableEdge.Visible.Show, Edge.Symbol.None, Edge.Symbol.None));
            //MainGraph.Edges.Add(new ShowableEdge(v1, v4, "", ShowableEdge.Visible.Show, Edge.Symbol.None, Edge.Symbol.None));

            //MainGraph.Edges.Add(new ShowableEdge(new Vertex("alo"), Vertex.EmptyVertex, "alo", ShowableEdge.Visible.NotShow));
            CreateAlgorithList();
            InitCommand();
            InitObservable();
        }

        private void CreateAlgorithList()
        {
            AlgorithmList.Add(new BFSViewModel());
            AlgorithmList.Add(new DFSStackViewModel());
        }

        private void InitObservable()
        {
            // Create new graph from string input
            this.WhenAnyValue(x => x.GraphData).
                ObserveOn(RxApp.TaskpoolScheduler).
                Throttle(TimeSpan.FromMilliseconds(300)).
                Select(graphData => graphData.Trim().Split("\n").Select(line => line.Trim()).Where(line => line.Length != 0).ToList()).
                Select(graphData => CustomGraph.CreateNewGraphFromStringLineData(graphData, GetGraphType(_isDirectedGraph))).
                ObserveOn(RxApp.MainThreadScheduler).
                Subscribe(newGraph => { MainGraph = newGraph; VertexCount = MainGraph.VetexCount; });

            
        }

        private void InitCommand()
        {
            ChangeGraphTypeCommand = ReactiveCommand.Create<RadioButton>((radioButon) =>
            {
                if (radioButon == null) return;
                string radioButtonContent = radioButon.Tag as string;
                if (string.IsNullOrWhiteSpace(radioButtonContent)) return;
                 ChangeGraphType(radioButtonContent);
            });

        }

        private CustomGraph.GraphDirectType GetGraphType(bool isDirectedGraph)
        {
            if (isDirectedGraph) return CustomGraph.GraphDirectType.Directed;
            return CustomGraph.GraphDirectType.UnDirected;
        }

        public void UpdateEdgeColor(Connection edgeLine)
        {
            edgeLine.InvalidateMeasure();
        }

        private void ChangeGraphType(string newGraphType)
        {
            // if new graph is Direct
            if (newGraphType == "Directed")
            {
                // current graph is Directed
                if (_isDirectedGraph) return;
                // not Directed => change
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph,CustomGraph.GraphDirectType.Directed);
                _isDirectedGraph = true;
            }
            // UnDirected
            else
            {
                if (!_isDirectedGraph) return;
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph, CustomGraph.GraphDirectType.UnDirected);
                _isDirectedGraph = false;
            }

        }

    }
}
