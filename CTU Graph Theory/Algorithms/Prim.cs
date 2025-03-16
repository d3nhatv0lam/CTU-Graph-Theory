using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using Microsoft.Msagl.Layout.Incremental;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class Prim : AbstractAlgorithm, IVertexRun, IAlgorithmRequirement
    {
        public List<RequestOfAlgorithm> Requirements { get ;}
        private int loopTime;
        private Int64 _minWeight;
        private Dictionary<Vertex, Int64> Pi;

        private event EventHandler<Int64>? _updateMinWeight;
        public event EventHandler<Int64>? UpdateMinWeight
        {
            add => _updateMinWeight += value;
            remove => _updateMinWeight -= value;
        }
        private void OnUpdateMinWeight(Int64 minWeight)
        {
            if (_updateMinWeight != null)
            {
                _updateMinWeight(this, minWeight);
            }
        }
        public Prim()
        {
            AlgorithmName = "Prim - Cây khung nhỏ nhất vô hướng của một bộ phận liên thông";
            Pi = new();
            Requirements = new();
            _minWeight = 0;
            loopTime = 0;
            FillPseudoCode();
            FillIAlgorithmRequirement();
        }
        protected override void FillPseudoCode()
        {
            string[] lines_code =
            {
                "Biến hỗ trợ",
                "pi[u]: Khoảng cách gần nhất từ đỉnh u tới một trong các đỉnh đã duyệt",
                "mark[u]: đỉnh u chưa/đã được duyệt",
                "p[u]: đỉnh đã duyệt gần với u nhất",
                "Prim(Vertex s) {",
                "    Với mọi u:",
                "        pi[u] = oo;",
                "        p[u] = None;",
                "        mark[u] = false;",
                "    pi[s] = 0;",
                "    Lặp n-1 lần: (n là số lượng đỉnh trong đồ thị)",
                "        Tìm u có pi[u] nhỏ nhất chưa được duyệt",
                "        if(không tìm được u)",
                "            return; // không thể tìm tiếp nữa",
                "        mark[u] = true;",
                "        for(đỉnh kề v của u) {",
                "            if(v chưa được duyệt && Trọng số cung (u,v) < pi[v]){",
                "                pi[v] = w;",
                "                p[v] = u;",
                "            }",
                "        }",
                "}"
            };
            foreach (var line in lines_code)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }

        }

        public bool CheckRequirements(CustomGraph graph)
        {
            Requirements[0].IsDoneRequest = graph.IsUnDirectedGraph();
            Requirements[1].IsDoneRequest = graph.IsWeightGraph;
            return Requirements.All(x => x.IsDoneRequest);
        }


        public void FillIAlgorithmRequirement()
        {
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải vô hướng"));
            Requirements.Add(new RequestOfAlgorithm("Dồ thị phải có đầy đủ trọng số"));
        }

        private void CleanAlgorithm()
        {
            loopTime = 0;
            _minWeight = 0;
            OnUpdateMinWeight(_minWeight);
            Pi.Clear();
            Pi.TrimExcess();
        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm)
            {
                base.CleanGraphForAlgorithm(graph);
                CleanAlgorithm();
            }
            if (loopTime == graph.EdgeCount - 1)
                OnCompletedAlgorithm();
        }

        public async void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;

            CleanAlgorithm();
            await PrepareState(graph, startVertex);

            await DoTask(graph, token);

            EndAlgorithmState(graph);
        }

        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;

            await DoTask(graph, token);

            EndAlgorithmState(graph);
        }
        private async Task PrepareState(CustomGraph graph,Vertex startVertex)
        {
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;

            Pseudocodes[6].IsSelectionCode = true;
            Pseudocodes[7].IsSelectionCode = true;
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            foreach (var u in graph.Vertices)
            {
                Pi.Add(u, Int64.MaxValue);
            }
            Pseudocodes[6].IsSelectionCode = false;
            Pseudocodes[7].IsSelectionCode = false;
            Pseudocodes[8].IsSelectionCode = false;

            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[9].IsSelectionCode = false;

            Pi[startVertex] = 0;
        }

        private async Task DoTask(CustomGraph graph, CancellationToken token)
        {
            while ( loopTime < graph.VetexCount - 1)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }
                await WhileLoopState();

                Vertex? u = await FindUState(graph);

                await IfNotFoundUState();
                if (u == null)
                {
                    await ReturnState();
                    return;
                }
                u.SetPointTo();

                await MarkUState();
                u.SetVitsited();
                u.UnSetPointedTo();

                ShowableEdge? visitedEdge = graph.GetEdgeWithMinWeight(u.ParentVertex, u);
                if (visitedEdge != null)
                {
                    visitedEdge.IsVisited = true; 
                    _minWeight += (Int64)visitedEdge.Label;
                }

                foreach (var v in graph.NeighboursOfVertex(u))
                {
                    v.SetPointTo();
                    await ForLoopState();

                    ShowableEdge? edge = graph.GetEdgeWithMinWeight(u, v);
                    if (edge != null)
                    {
                        Int64 w = (Int64)edge.Label;
                        await IfSetPiState();
                        if (!v.IsVisited && w < Pi[v])
                        {
                            await SetPiState();
                            Pi[v] = w;
                            v.ParentVertex = u;
                        }
                    }
                    v.UnSetPointedTo();
                }
                loopTime++;
            }
            await FillLastVertexState(graph);
            
            OnUpdateMinWeight(_minWeight);
        } 

        private async Task WhileLoopState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
        }
        private async Task<Vertex?> FindUState(CustomGraph graph)
        {
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[11].IsSelectionCode = false;

            var pair = Pi.Where(x => x.Key.IsVisited == false && x.Value != Int64.MaxValue).OrderBy(x => x.Value).FirstOrDefault();

            if (pair.Equals(default(KeyValuePair<Vertex, Int64>))) // Kiểm tra cặp mặc định
                return null;

            return pair.Key;
        }
        private async Task IfNotFoundUState()
        {
            Pseudocodes[12].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[12].IsSelectionCode = false;
        }
        private async Task ReturnState()
        {
            Pseudocodes[13].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[13].IsSelectionCode = false;
        }
        private async Task MarkUState()
        {
            Pseudocodes[14].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[14].IsSelectionCode = false;
        }
       private async Task ForLoopState()
        {
            Pseudocodes[15].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[15].IsSelectionCode = false;
        }
        private async Task IfSetPiState()
        {
            Pseudocodes[16].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[16].IsSelectionCode = false;
        }
        private async Task SetPiState()
        {
            Pseudocodes[17].IsSelectionCode = true;
            Pseudocodes[18].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[17].IsSelectionCode = false;
            Pseudocodes[18].IsSelectionCode = false;
        }
        private async Task FillLastVertexState(CustomGraph graph)
        {
            Vertex? u = await FindUState(graph);
            if (u == null) return;
            u.SetVitsited();
            ShowableEdge? edgeColored = graph.GetEdgeWithMinWeight(u.ParentVertex, u);
            if (edgeColored != null)
            {
                edgeColored.IsVisited = true;
                _minWeight += (Int64)edgeColored.Label;
            }
        }
    }
}
