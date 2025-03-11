using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class BellmanFord : AbstractAlgorithm, IVertexRun, IAlgorithmRequirement
    {
        public List<RequestOfAlgorithm> Requirements { get; }
        private Dictionary<Vertex,Int64> Pi { get; }
        private int loopTime;

        public BellmanFord()
        {
            AlgorithmName = "Bellman - Ford Tìm đường đi ngắn nhất";
            Requirements = new List<RequestOfAlgorithm>();
            Pi = new();
            loopTime = 0;
            FillIAlgorithmRequirement();
            FillPseudoCode();
        }
        protected override void FillPseudoCode()
        {
            string[] line_codes =
            {
                "Biến hỗ trợ",
                "pi[i]: Khoảng cách từ đỉnh s đến i",
                "BellmanFord(s){",
                "    với mọi u, pi[u] = oo;",
                "    pi[s] = 0;",
                "    lặp n-1 lần:",
                "        for (cung e của đồ thị){",
                "            u = e.u;",
                "            v = e.v;",
                "            w = e.w;",
                "            if (pi[u] == oo)",
                "                continue;",
                "            if (pi[u] + w < pi[v]){",
                "                pi[v] = pi[u] + w",
                "                p[v] = u;",
                "           }",
                "        }",
                "}"
            };
            foreach (var line in line_codes)
            { 
                Pseudocodes.Add(new StringPseudoCode(line));
            }
        }
        public void FillIAlgorithmRequirement()
        {
            Requirements.Add(new RequestOfAlgorithm("Phải là đồ thị có hướng"));
            Requirements.Add(new RequestOfAlgorithm("Đồ thị phải có đầy đủ trọng số"));
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            Requirements[0].IsDoneRequest = graph.IsDirectedGraph();
            Requirements[1].IsDoneRequest = graph.IsWeightGraph;
            return Requirements.All(x => x.IsDoneRequest);
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

        private void CleanAlgorithm()
        {
            Pi.Clear();
            Pi.TrimExcess();
            loopTime = 0;
        }

        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;
            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        public async void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;
            await PrepareState();
            await InitState(graph, startVertex);
            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        private Task PrepareState()
        {
            CleanAlgorithm();
            return Task.FromResult(0);
        }

        private async Task InitState(CustomGraph graph, Vertex startVertex)
        {
            Pseudocodes[3].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[3].IsSelectionCode = false;
            foreach (Vertex u in graph.Vertices)
            {
                Pi.Add(u, Int64.MaxValue);
            }
            Pseudocodes[4].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[4].IsSelectionCode = false;
            Pi[startVertex] = 0;
        }

        private async Task RunLoop(CustomGraph graph,CancellationToken token)
        {
            while (loopTime < graph.Vertices.Count - 1)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }
                await LoopTimeState();

                foreach (ShowableEdge edge in graph.Edges)
                {
                    
                    await ForLoopEdgeState();
                    edge.IsPointTo = true;

                    await GetEdgeInformationState();
                    Vertex u = (Vertex)edge.Tail,
                            v = (Vertex)edge.Head;
                    Int64 w = (Int64)edge.Label;

                    await IfINFState();
                    if (Pi[u] == Int64.MaxValue)
                    {
                        await ContinueState();
                        edge.IsPointTo = false;
                        continue;
                    }
                    await IfFindPathState();
                    if (Pi[u] + w < Pi[v])
                    {
                        await FindPathState();
                        Pi[v] = Pi[u] + w;
                        List<ShowableEdge> previousEdges = graph.GetAllEdges(v.ParentVertex, v);
                        foreach (var item in previousEdges)
                        {
                            item.IsVisited = false;
                        }
                        edge.IsVisited = true;
                        v.ParentVertex = u;
                    }
                    edge.IsPointTo = false;
                }
                loopTime++;
            }
        }

        private async Task LoopTimeState()
        {
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;
        }
        private async Task ForLoopEdgeState()
        {
            Pseudocodes[6].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[6].IsSelectionCode = false;
        }
        private async Task GetEdgeInformationState()
        {
            Pseudocodes[7].IsSelectionCode = true;
            Pseudocodes[8].IsSelectionCode = true;
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
            Pseudocodes[8].IsSelectionCode = false;
            Pseudocodes[9].IsSelectionCode = false;
        }  
        private async Task IfINFState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
        }
        private async Task ContinueState()
        {
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[11].IsSelectionCode = false;
        }
        private async Task IfFindPathState()
        {
            Pseudocodes[12].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[12].IsSelectionCode = false;
        }
        private async Task FindPathState()
        {
            Pseudocodes[13].IsSelectionCode = true;
            Pseudocodes[14].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[13].IsSelectionCode = false;
            Pseudocodes[14].IsSelectionCode = false;
        }
    }
}
