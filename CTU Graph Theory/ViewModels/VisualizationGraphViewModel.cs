
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
        private List<RequestOfAlgorithm> _algorithmRequiments;
        private ObservableCollection<Vertex> _vertices;
        private Vertex? _startVertex = null;
        private bool _isAllowSelectAllVertex = false;
        private bool _isSelectAllVertex = false;
        private bool _isAlgorithmRequirementAccept = true;
        private bool _isRunningAlgorithm = false;
        private bool _isPauseAlgorithm = false;
        private bool _isEnableStartVertexSelection = true;
        private bool _isNonSelectStartVertex = false;
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
        public List<RequestOfAlgorithm> AlgorithmRequiments
        {
            get
            {
                if (_algorithmRequiments == null) _algorithmRequiments = new() { new RequestOfAlgorithm("") { IsDoneRequest = true} }; 
                return _algorithmRequiments;
            }
            set => this.RaiseAndSetIfChanged(ref _algorithmRequiments, value);
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
        public bool IsAllowSelectAllVertex
        {
            get => _isAllowSelectAllVertex;
            set => this.RaiseAndSetIfChanged(ref _isAllowSelectAllVertex, value);
        }
        public bool IsSelectAllVertex
        {
            get => _isSelectAllVertex;
            set => this.RaiseAndSetIfChanged(ref _isSelectAllVertex, value);
        }
        public bool IsAlgorithmRequirementAccept
        {
            get => _isAlgorithmRequirementAccept;
            set => this.RaiseAndSetIfChanged(ref _isAlgorithmRequirementAccept, value);
        }
        public bool IsEnableStartVertexSelection
        {
            get => _isEnableStartVertexSelection;
            set => this.RaiseAndSetIfChanged(ref _isEnableStartVertexSelection, value);
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
        public bool IsNonSelectStartVertex
        {
            get => _isNonSelectStartVertex;
            set => this.RaiseAndSetIfChanged(ref _isNonSelectStartVertex, value);
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
            AlgorithmList.Add(new MooreDijkstraViewModel());
            AlgorithmList.Add(new BellmanFordViewModel());
            AlgorithmList.Add(new KruskalViewModel());
            AlgorithmList.Add(new PrimViewModel());
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
                Subscribe(newGraph => 
                {
                    MainGraph = newGraph; 
                    VertexCount = MainGraph.VetexCount; 
                    EdgeCount = MainGraph.EdgeCount; 
                    GraphType = MainGraph.TypeOfGraph;
                    IsDirectedGraph = MainGraph.IsDirectedGraph();
                    Vertices = MainGraph.Vertices;
                });

            // StartVertex Selectable
            this.WhenAnyValue(x => x.IsAllowSelectAllVertex, x => x.IsSelectAllVertex, x => x.IsRunningAlgorithm).Subscribe(tuple => IsEnableStartVertexSelection = !tuple.Item1 || (!tuple.Item2 && !tuple.Item3) );
            // Algorithm Changed Requirement
            this.WhenAnyValue(x => x.MainGraph, x => x.SelectedAlgorithm).Where((tuple) => tuple.Item2 != null).Subscribe(tuple => 
            { 
                if (tuple.Item2 is IAlgorithmRequirementViewModel algorithm)
                {
                    IsAlgorithmRequirementAccept = algorithm.CheckRequirements(tuple.Item1);
                    AlgorithmRequiments = algorithm.Requirements;
                }
                else
                {
                    // set all is true to Aniamte in UI
                    AlgorithmRequiments.ForEach(x => x.IsDoneRequest = true);
                    IsAlgorithmRequirementAccept = true;
                }
            });
            // change algorithm -> Check -> Check first choose this algorithm
            this.WhenAnyValue(x => x.SelectedAlgorithm)
                .Do(algorithm => 
                { 
                    if (algorithm is IAllVertexRun allowAlgorithmCheck) 
                        IsAllowSelectAllVertex = true; 
                    else IsAllowSelectAllVertex = false;
                    if (algorithm is INonStartVertexRun allowChooseStartVertex)
                        IsNonSelectStartVertex = true;
                    else IsNonSelectStartVertex = false;
                })
                .Where(algorithm => algorithm?.IsSetCompletedAlgorithm == false).Subscribe(algorithm => algorithm?.SetCompletedAlgorithm(OnAlgorithmCompleted));
            // set run speed when change Slider or change Selectedalgorithn
            this.WhenAnyValue(x => x.SelectedAlgorithm,x => x.MultiplierSpeed).Subscribe(tuple => tuple.Item1?.SetRunSpeed(tuple.Item2));
            // command check can activate
            CanRunAlgorithmCommand = this.WhenAnyValue(x => x.SelectedAlgorithm, x => x.StartVertex, x => x.IsSelectAllVertex, x => x.IsAlgorithmRequirementAccept,x => x.IsNonSelectStartVertex, (algorithm, startVertex, isSelectedAllVertex, requirementAccept, isNonSelectStartVertex) => (algorithm != null) &&
                                                                                                                                                                    (startVertex != null || isSelectedAllVertex == true || isNonSelectStartVertex == true) && (requirementAccept == true));
            CanPauseAlgorithmCommand = this.WhenAnyValue(x => x.IsRunningAlgorithm, x => x.IsPauseAlgorithm , (isRunning,isPause) => isRunning == true && isPause == false);
            CanContinueAlgorithmCommand = this.WhenAnyValue(x => x.IsPauseAlgorithm, isPaused => isPaused == true);
        }

        private void InitCommand()
        {
            ChangeGraphTypeCommand = ReactiveCommand.Create<RadioButton>((radioButon) =>
            {
                if (radioButon == null) return;
                string? radioButtonContent = radioButon.Tag as string;
                if (string.IsNullOrWhiteSpace(radioButtonContent)) return;
                 ChangeGraphType(radioButtonContent);
                MainGraph.UnVisitAndClearParentAll();
            });
            
            RunAlgorithmCommand = ReactiveCommand.Create(() => 
            {
                if (IsRunningAlgorithm)
                {
                    IsRunningAlgorithm = false;
                    IsPauseAlgorithm = false;
                    SelectedAlgorithm?.StopAlgorithm(MainGraph);
                    return;
                }
                IsRunningAlgorithm = true;
                IsPauseAlgorithm = false;

                if (SelectedAlgorithm is IAllVertexRun selectedAlgorithmAllVertexRun)
                {
                    if (IsSelectAllVertex) selectedAlgorithmAllVertexRun?.RunAlgorithmWithAllVertex(MainGraph, Vertices);
                    else selectedAlgorithmAllVertexRun?.RunAlgorithm(MainGraph, StartVertex);
                }
                else if (SelectedAlgorithm is IVertexRun selectedAlgorithmVertexRun)
                {
                    selectedAlgorithmVertexRun?.RunAlgorithm(MainGraph, StartVertex);
                }
                else if (SelectedAlgorithm is INonStartVertexRun selectedAlgorithmNonstartVertex)
                {
                    selectedAlgorithmNonstartVertex?.RunAlgorithm(MainGraph);
                }

                },CanRunAlgorithmCommand);
            PauseAlgorithmCommand = ReactiveCommand.Create(() => { SelectedAlgorithm?.PauseAlgorithm(); IsPauseAlgorithm = true;}, CanPauseAlgorithmCommand);

            ContinueAlgorithmCommand = ReactiveCommand.Create(() => 
            {
                if (SelectedAlgorithm is IAllVertexRun selectedAllVertexAlgorithm)
                {
                    if (IsSelectAllVertex) selectedAllVertexAlgorithm?.ContinueAlgorithmWithAllVertex(MainGraph);
                    else selectedAllVertexAlgorithm?.ContinueAlgorithm(MainGraph);
                }
                else if (SelectedAlgorithm is IVertexRun selectedVertexAlgorithm) selectedVertexAlgorithm.ContinueAlgorithm(MainGraph);
                else if (SelectedAlgorithm is INonStartVertexRun selectedAlgorithmNonstartVertex) selectedAlgorithmNonstartVertex.ContinueAlgorithm(MainGraph);
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
