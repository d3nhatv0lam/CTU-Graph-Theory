
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaGraphControl;
using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Interfaces;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.ViewModels
{
    public class VisualizationGraphViewModel: ViewModelBase
    {
        private CustomGraph _mainGraph = new CustomGraph();
        private int _vertexCount = 0;
        private int _edgeCount = 0;
        private CustomGraph.GraphType _graphType;
        private bool _isDirectedGraph = false;
        private string _graphData = string.Empty;
        private List<IAlgorithmViewModel> _algorithmList;
        private IAlgorithmViewModel? _selectedAlgorithm = null;
        private ObservableCollection<Vertex> _vertices;
        private Vertex? _startVertex = null;
        private bool _isAllowRunAlgorithm = false;
        private bool _isRunningAlgorithm = false;
        private bool _isPauseAlgorithm = false; 
        private int _multiplierSpeed = 1;

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
        public int EdgeCount
        {
            get => _edgeCount;
            set => this.RaiseAndSetIfChanged(ref _edgeCount, value);
        }
        public CustomGraph.GraphType GraphType
        {
            get => _graphType;
            set => this.RaiseAndSetIfChanged(ref  this._graphType, value);
        }
        public bool IsDirectedGraph
        {
            get => _isDirectedGraph;
            set => this.RaiseAndSetIfChanged(ref _isDirectedGraph, value);
        }
        public string GraphData
        {
            get => _graphData;
            set => this.RaiseAndSetIfChanged(ref _graphData, value);
        }
        public List<IAlgorithmViewModel> AlgorithmList
        {
            get 
            {
                if (_algorithmList == null) _algorithmList = new();
                return _algorithmList;
            }   
        }
        public IAlgorithmViewModel? SelectedAlgorithm
        {
            get => _selectedAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
            
        }
        public ObservableCollection<Vertex> Vertices
        {
            get => _vertices;
            set => this.RaiseAndSetIfChanged(ref _vertices, value);
        }
        public Vertex? StartVertex
        {
            get => _startVertex;
            set => this.RaiseAndSetIfChanged(ref _startVertex, value);
        }
        public bool IsAllowRunAlgorithm
        {
            get => _isAllowRunAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _isAllowRunAlgorithm, value);
        }
        public bool IsRunningAlgorithm
        {
            get => _isRunningAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _isRunningAlgorithm, value);
        }
        public bool IsPauseAlgorithm
        {
            get => _isPauseAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _isPauseAlgorithm, value);
        }
        public int MultiplierSpeed
        {
            get => _multiplierSpeed;
            set => this.RaiseAndSetIfChanged(ref _multiplierSpeed, value);
        }
     

        public ReactiveCommand<RadioButton, Unit> ChangeGraphTypeCommand { get; private set; }
        private IObservable<bool> CanRunAlgorithmCommand { get;  set; }
        public ReactiveCommand<Unit,Unit> RunAlgorithmCommand { get; private set; }
        private IObservable<bool> CanPauseAlgorithmCommand { get;  set; }
        public ReactiveCommand<Unit,Unit> PauseAlgorithmCommand { get; private set; }
        private IObservable<bool> CanContinueAlgorithmCommand { get; set; }
        public ReactiveCommand<Unit,Unit> ContinueAlgorithmCommand { get; private set; }
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
            InitObservable();
            InitCommand();
            GraphData = "1 2 1\r\n2 3 5\r\n3 1 2\r\n1 5 2\r\n2 4 3\r\n4 6 6";
        }

        private void CreateAlgorithList()
        {
            AlgorithmList.Add(new BFSViewModel());
            AlgorithmList.Add(new DFSStackViewModel());
            AlgorithmList.Add(new DFSRecursiveViewModel());
            AlgorithmList.Add(new CircledCheckViewModel());
            AlgorithmList.Add(new TarjanSCCViewModel());
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
                Subscribe(newGraph => {
                    MainGraph = newGraph; 
                    VertexCount = MainGraph.VetexCount; 
                    EdgeCount = MainGraph.EdgeCount; 
                    GraphType = MainGraph.TypeOfGraph;
                    IsDirectedGraph = MainGraph.IsDirectedGraph();
                    // Add Select all
                    if (MainGraph.Vertices.Count > 0)
                    {
                        Vertices = new ObservableCollection<Vertex>() { Vertex.EmptyVertex };
                        foreach (Vertex vertex in MainGraph.Vertices)
                        {
                            Vertices.Add(vertex);
                        }
                    }
                    StartVertex = null;
                });

            this.WhenAnyValue(x => x.MainGraph, x => x.SelectedAlgorithm).Where((tuple) => tuple.Item2 != null).Subscribe(tuple => { if (tuple.Item2 is IAlgorithmRequirementViewModel algorithm) IsAllowRunAlgorithm = algorithm.CheckRequirements(tuple.Item1); else IsAllowRunAlgorithm = true; });
            // change algorithm & first choose this algorithm
            this.WhenAnyValue(x => x.SelectedAlgorithm).Where(algorithm => algorithm?.IsSetCompletedAlgorithm == false).Subscribe(algorithm => algorithm?.SetCompletedAlgorithm(OnAlgorithmCompleted) );
            // update Algorithm graph && vertex
            this.WhenAnyValue(x => x.SelectedAlgorithm, x => x.StartVertex, (selectedAlgorithm, startVertex) => (selectedAlgorithm, startVertex)).Subscribe((tuple) => {tuple.selectedAlgorithm?.TransferGraph(MainGraph, tuple.startVertex); tuple.selectedAlgorithm?.SetRunSpeed(MultiplierSpeed);} ); 
            // set run speed
            this.WhenAnyValue(x => x.MultiplierSpeed).Subscribe(multiplierSpeed => SelectedAlgorithm?.SetRunSpeed(multiplierSpeed));
            // command check can activate
            CanRunAlgorithmCommand = this.WhenAnyValue(x => x.SelectedAlgorithm, x => x.StartVertex,x => x.IsAllowRunAlgorithm, (algorithm, startVertex,isAllowRun) => (algorithm != null) && (startVertex != null) && (isAllowRun == true));
            CanPauseAlgorithmCommand = this.WhenAnyValue(x => x.IsRunningAlgorithm, x => x.IsPauseAlgorithm , (isRunning,isPause) => isRunning == true && isPause == false);
            CanContinueAlgorithmCommand = this.WhenAnyValue(x => x.IsPauseAlgorithm, isPaused => isPaused == true);
        }

        private void InitCommand()
        {
            ChangeGraphTypeCommand = ReactiveCommand.Create<RadioButton>((radioButon) =>
            {
                if (radioButon == null) return;
                string radioButtonContent = radioButon.Tag as string;
                if (string.IsNullOrWhiteSpace(radioButtonContent)) return;
                 ChangeGraphType(radioButtonContent);
                // Change Direct of Graph => Update Algorithm graph
                SelectedAlgorithm?.TransferGraph(MainGraph, StartVertex);
                MainGraph.UnVisitAndClearParentAll();
            });
            
            RunAlgorithmCommand = ReactiveCommand.Create(() => 
            {
                if (IsRunningAlgorithm)
                {
                    IsRunningAlgorithm = false;
                    IsPauseAlgorithm = false;
                    SelectedAlgorithm?.StopAlgorithm();
                    return;
                }
                IsRunningAlgorithm = true;
                IsPauseAlgorithm = false;
                if (Vertex.IsVertexEqual(StartVertex, Vertex.EmptyVertex))
                    SelectedAlgorithm?.RunAlgorithmWithAllVertex(MainGraph.Vertices);
                else SelectedAlgorithm?.RunAlgorithm();     
                },CanRunAlgorithmCommand);
            PauseAlgorithmCommand = ReactiveCommand.Create(() => { SelectedAlgorithm?.PauseAlgorithm(); IsPauseAlgorithm = true;}, CanPauseAlgorithmCommand);

            ContinueAlgorithmCommand = ReactiveCommand.Create(() => 
            {
                if (Vertex.IsVertexEqual(StartVertex, Vertex.EmptyVertex))
                    SelectedAlgorithm?.ContinueAlgorithmWithAllVertex();
                else
                    SelectedAlgorithm?.ContinueAlgorithm();
                IsPauseAlgorithm = false; 
            },CanContinueAlgorithmCommand);
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
                if (IsDirectedGraph) return;
                // not Directed => change
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph, CustomGraph.GraphDirectType.Directed);
                IsDirectedGraph = true;
            }
            // UnDirected
            else
            {
                if (!IsDirectedGraph) return;
                MainGraph = CustomGraph.CreateNewGraphFromChangeGraphType(MainGraph, CustomGraph.GraphDirectType.UnDirected);
                IsDirectedGraph = false;
            }
        }

        private void OnAlgorithmCompleted(object? sender, EventArgs e)
        {
            IsRunningAlgorithm = false;
        }

    }
}
