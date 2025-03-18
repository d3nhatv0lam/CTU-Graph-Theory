using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class Kruskal : AbstractAlgorithm, INonStartVertexRun, IAlgorithmRequirement
    {

        private int currentEdge;
        private List<ShowableEdge> SortedEdges;
        private Int64 _minWeightInt;

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

        public Kruskal()
        {
            AlgorithmName = "Kruskal - Cây khung vô hướng nhỏ nhất";
            Requirements = new();
            _minWeightInt = 0;
            SortedEdges = new();
            currentEdge = 0;
            FillPseudoCode();
            FillIAlgorithmRequirement();
        }
        protected override void FillPseudoCode()
        {
            string[] code = {
            "// Tìm gốc của đỉnh u",
            "FindRoot(Vertex u) {",
            "    while(p[u] != u)",
            "        u = p[u];",
            "    return u;",
            "}",
            "",
            "Kruskal() {",
            "    1. Sắp xếp các cung của đồ thị theo thứ tự trọng số tăng dần",
            "    2. Với mọi u, p[u] = u; // mỗi đỉnh u là một bộ phận liên thông",
            "    minWeight = 0 // trọng lượng nhỏ nhất của cây",
            "    For(cung e của đồ thị) {",
            "        u = e.u;",
            "        v = e.v;",
            "        w = e.w;",
            "        root_u = FindRoot(u);",
            "        root_v = FindRoot(v);",
            "        if (root_u != root_v)  {",
            "            p[root_v] = root_u; // gộp 2 bộ phận liên thông",
            "            minWeight = minWeight + w;",
            "        }",
            "    }",
            "    return minWeight;",
            "}"
            };
            foreach (var line in code)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }

        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm)
            {
                base.CleanGraphForAlgorithm(graph);
                CleanAlgorithm();
            }
            if (currentEdge == graph.EdgeCount)
                OnCompletedAlgorithm();
        }

        private void CleanAlgorithm()
        {
            currentEdge = 0;
            _minWeightInt = 0;
            OnUpdateMinWeight(_minWeightInt);
            SortedEdges.Clear();
            SortedEdges.TrimExcess();
        }

        public List<RequestOfAlgorithm> Requirements { get; }


        public void FillIAlgorithmRequirement()
        {
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải vô hướng"));
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải có đầy đủ trọng số"));
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải liên thông (chỉ có 1 bộ phận liên thông)"));
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            Requirements[0].IsDoneRequest = graph.IsUnDirectedGraph();
            Requirements[1].IsDoneRequest = graph.IsWeightGraph;
            Requirements[2].IsDoneRequest = graph.IsUnDirectedConnectedGraph;
            return Requirements.All(x => x.IsDoneRequest);
        }

        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;

            await DoTask(graph, token);

            EndAlgorithmState(graph);
        }


        public async void RunAlgorithm(CustomGraph graph)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;

            CleanAlgorithm();

            await EdgesSortState();
            SortedEdges = graph.Edges.Select(x => (ShowableEdge)x).Where(x => x.Label is Int64).ToList();
            SortedEdges.Sort((a, b) => (int)((Int64)a.Label - (Int64)b.Label));

            await InitParentUState();

            await InitMinWeightState();

            await DoTask(graph, token);

            EndAlgorithmState(graph);
        }

        private async Task EdgesSortState()
        {
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }
        private async Task InitParentUState()
        {
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[9].IsSelectionCode = false;
        }
        private async Task InitMinWeightState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
        }


        private async Task JoinFindRootState()
        {
            Pseudocodes[1].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[1].IsSelectionCode = false;
        }
        private async Task SeeParentState()
        {
            Pseudocodes[2].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[2].IsSelectionCode = false;
        }
        private async Task UpdateParentState()
        {
            Pseudocodes[3].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[3].IsSelectionCode = false;

        }
        private async Task ReturnUState()
        {
            Pseudocodes[4].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[4].IsSelectionCode = false;
        }
        private async Task<Vertex> FindRoot(Vertex u)
        {
            bool isAllowWhileState = false;
            await JoinFindRootState();
            u.SetVitsited();
            await SeeParentState();
            while (u.ParentVertex != null)
            {
                u.ParentVertex.SetVitsited();
                if (isAllowWhileState) await SeeParentState();
                else isAllowWhileState = true;
                    await UpdateParentState();
                u.UnSetVisited();
                u = u.ParentVertex;
            }
            u.UnSetVisited();
            await ReturnUState();
            return u;
        }

        private async Task DoTask(CustomGraph graph, CancellationToken token)
        {
            while (currentEdge < graph.EdgeCount)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }

                ShowableEdge edge = SortedEdges.ElementAt(currentEdge);
                if (edge.IsShowEdge == false)
                {
                    currentEdge++;
                    continue;
                }

                await LoopState();

                
                edge.IsPointTo = true;

                await GetEdgeDataState();

                await FindRootUState();
                Vertex root_u = await FindRoot((Vertex)edge.Tail);
                await FindRootVState();
                Vertex root_v = await FindRoot((Vertex)edge.Head);

                Int64 w = (Int64)edge.Label;

                await IfRootEqualState();
                if (root_u != root_v)
                {
                    await MeregeRootState();
                    root_v.ParentVertex = root_u;
                    await UpdateMinWeightState();
                    _minWeightInt += w;
                    edge.IsVisited = true;
                }
                edge.IsPointTo = false;
                currentEdge++;
            }
            await ReturnMinWeightState();
            OnUpdateMinWeight(_minWeightInt);
        }

        private async Task LoopState()
        {
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[11].IsSelectionCode = false;
        }
        private async Task GetEdgeDataState()
        {
            Pseudocodes[12].IsSelectionCode = true;
            Pseudocodes[13].IsSelectionCode = true;
            Pseudocodes[14].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[12].IsSelectionCode = false;
            Pseudocodes[13].IsSelectionCode = false;
            Pseudocodes[14].IsSelectionCode = false;
        }
        private async Task FindRootUState()
        {
            Pseudocodes[15].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[15].IsSelectionCode = false;
        }
        private async Task FindRootVState()
        {
            Pseudocodes[16].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[16].IsSelectionCode = false;
        }
        private async Task IfRootEqualState()
        {
            Pseudocodes[17].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[17].IsSelectionCode = false;
        }
        private async Task MeregeRootState()
        {
            Pseudocodes[18].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[18].IsSelectionCode = false;
        }
        private async Task UpdateMinWeightState()
        {
            Pseudocodes[19].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[19].IsSelectionCode = false;
        }
        private async Task ReturnMinWeightState()
        {
            Pseudocodes[22].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[22].IsSelectionCode = false;
        }
    }
}
