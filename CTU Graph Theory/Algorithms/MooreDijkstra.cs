using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CTU_Graph_Theory.Algorithms
{
    public class MooreDijkstra : AbstractAlgorithm,IAlgorithmRequirement, IVertexRun
    {
        public List<RequestOfAlgorithm> Requirements { get; }
        public Dictionary<Vertex,Int64> Pi;
        //public PriorityQueue<Vertex, int> Pi;
        private int loopTime;

        public MooreDijkstra()
        {
            AlgorithmName = "Moore Dijkstra - Tìm đường đi ngắn nhất";
            Requirements = new List<RequestOfAlgorithm>();
            Pi = new();
            loopTime = 0;
            FillPseudoCode();
            FillIAlgorithmRequirement();
        }

        protected override void FillPseudoCode()
        {
            string[] line_codes = {
                "Biến hỗ trợ:",
                "+ pi[i]: Trọng số khi đi từ đỉnh s->đỉnh i",
                "+ p[i]: Đỉnh cha của đỉnh i",
                "+ mark[i]: Trạng thái đánh dấu của dỉnh i",
                "Dijkstra(s: đỉnh bắt đầu) {",
                "    Khởi tạo tất cả các đỉnh đều chưa đánh dấu",
                "    Với mọi u != s, pi[u] = oo",
                "    pi[s] = 0",
                "    Lập từ 1 tới n-1 (n là số đỉnh có trong đồ thị):{",
                "        u = Đỉnh chưa đánh dấu có pi[u] nhỏ nhất.",
                "        if (không tìm được u)",
                "            return; // không tìm được đường đi nữa",
                "        Đánh dấu u",
                "        for (đỉnh kề v của u)",
                "            if (v chưa được đánh dấu && p[u] + trọng số cung(u, v) < p[v]) {",
                "                pi[v] = pi[u] + trọng số cung(u, v)",
                "                p[v] = u",
                "            }",
                "    }",
                "}"
            };
            foreach (var line in line_codes)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }
        }

        public void FillIAlgorithmRequirement()
        {
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải có đầy đủ trọng số"));
            Requirements.Add(new RequestOfAlgorithm("Đồ thị không được có trọng số âm"));
        }
        public bool CheckRequirements(CustomGraph graph)
        {
            Requirements[0].IsDoneRequest = graph.IsWeightGraph;
            Requirements[1].IsDoneRequest = !graph.IsHasNegativeWeight;
            return Requirements.All(x => x.IsDoneRequest);
        }
        private void CleanAlgorithm()
        {
            Pi.Clear();
            Pi.TrimExcess();
            loopTime = 0;
        }

        public async void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;
            await PrepareState();
            await InitState(graph,startVertex);
            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;

            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm)
            {
                base.CleanGraphForAlgorithm(graph);
                CleanAlgorithm();
            }
            if (loopTime == graph.VetexCount - 1) OnCompletedAlgorithm();
        }

        private Task PrepareState()
        {
            CleanAlgorithm();
            return Task.FromResult(0);
        }

        private async Task InitState(CustomGraph graph,Vertex startVertex)
        {
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;
            Pseudocodes[6].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[6].IsSelectionCode = false;
            foreach (Vertex u in graph.Vertices)
            {
                Pi.Add(u, Int64.MaxValue);
            }
            Pseudocodes[7].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
            Pi[startVertex] = 0;
        }

        private async Task RunLoop(CustomGraph graph,CancellationToken token)
        {
            while(loopTime < graph.VetexCount - 1)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }

                await LoopState();

                Vertex? u = await FindVertexState(graph);

                await IfDontFoundVertexState();
                if (u == null)
                {
                    await ReturnState();
                    loopTime = graph.VetexCount - 1;
                    return;
                }

                await MarkVertexState();
                u.SetVitsited();
                ShowableEdge? edgeColored = graph.GetEdgeWithMinWeight(u.ParentVertex, u);
                if (edgeColored != null)
                {
                    edgeColored.IsVisited = true;
                }
                
                foreach (Vertex v in graph.NeighboursOfVertex(u))
                {
                    await ForLoopState();
                    v.SetPointTo();
                    ShowableEdge? edge = graph.GetEdgeWithMinWeight(u, v);
                    if (edge == null) continue;
                    await IfReWritePathState();
                    if (!v.IsVisited && Pi[u] + (Int64)edge.Label < Pi[v])
                    {
                        await ReWritePathState();
                        Pi[v] = Pi[u] + (Int64)edge.Label;
                        v.ParentVertex = u;
                    }
                    v.UnSetPointedTo();
                }
                loopTime++;
            }
            await FillLastVertexState(graph);
        }

        private async Task LoopState()
        {
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }
        private async Task<Vertex?> FindVertexState(CustomGraph graph)
        {
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[9].IsSelectionCode = false;
            Vertex? u = null;
            Int64 minWeight = Int64.MaxValue;
            foreach (Vertex v in graph.Vertices)
            {
                if (v.IsVisited == false && Pi[v] < minWeight)
                {
                    u = v;
                    minWeight = Pi[v];
                }
            }
            return u;
        }

        private async Task IfDontFoundVertexState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
        }

        private async Task ReturnState()
        {
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[11].IsSelectionCode = false;
        }
        private async Task MarkVertexState()
        {
            Pseudocodes[12].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[12].IsSelectionCode = false;
        }
        private async Task ForLoopState()
        {
            Pseudocodes[13].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[13].IsSelectionCode = false;
        }
        private async Task IfReWritePathState()
        {
            Pseudocodes[14].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[14].IsSelectionCode = false;
        }
        private async Task ReWritePathState()
        {
            Pseudocodes[15].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[15].IsSelectionCode = false;
            Pseudocodes[16].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[16].IsSelectionCode = false;
        }

        private async Task FillLastVertexState(CustomGraph graph)
        {
            Vertex? u = await FindVertexState(graph);
            if (u == null) return;
            u.SetVitsited();
            ShowableEdge? edgeColored = graph.GetEdgeWithMinWeight(u.ParentVertex, u);
            if (edgeColored != null)
            {
                edgeColored.IsVisited = true;
            }
        }
    }
}
