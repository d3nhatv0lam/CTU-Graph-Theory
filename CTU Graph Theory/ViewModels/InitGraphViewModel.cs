
using Avalonia.Controls;
using AvaloniaEdit;
using AvaloniaGraphControl;
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
    public class InitGraphViewModel: ViewModelBase
    {
        private CustomGraph _mainGraph = new CustomGraph();
        private int _vertexCount = 0;
        private bool _isDirectedGraph = false;
        private string _graphData = string.Empty;
        
        public ReactiveCommand<RadioButton, Unit> ChangeGraphTypeCommand { get; private set; }

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


        public InitGraphViewModel()
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
          
            InitCommand();
            InitObservable();
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

        private CustomGraph.GraphType GetGraphType(bool isDirectedGraph)
        {
            if (isDirectedGraph) return CustomGraph.GraphType.Directed;
            return CustomGraph.GraphType.UnDirected;
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
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph,CustomGraph.GraphType.Directed);
                _isDirectedGraph = true;
            }
            // UnDirected
            else
            {
                if (!_isDirectedGraph) return;
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph, CustomGraph.GraphType.UnDirected);
                _isDirectedGraph = false;
            }

        }

    }
}
