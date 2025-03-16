using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class TarjanSCC : AbstractAlgorithm,IVertexRun, IAllVertexRun, IAlgorithmRequirement
    {
        public List<RequestOfAlgorithm> Requirements { get; }

        private enum RecursiveState
        {
            Entry,
            Exit,
        }
        private readonly Stack<(Vertex vertex, int selectedNeighbourIndex, RecursiveState state)> funtionStack;
        private readonly Dictionary<Vertex, int> num, min_num;
        private readonly Dictionary<Vertex, List<Vertex>> Neighbours;
        private readonly Stack<Vertex> SCCStack;
        private int k;
        public ObservableCollection<ObservableCollection<string>> Result { get; }

        public TarjanSCC()
        {
            AlgorithmName = "SCC - Bộ phận liên thông mạnh";
            Requirements = new();
            num = new();
            min_num = new();
            funtionStack = new();
            Neighbours = new();
            SCCStack = new();
            Result = new ObservableCollection<ObservableCollection<string>>();
            k = 0;
            FillPseudoCode();
            FillIAlgorithmRequirement();
        }
        protected override void FillPseudoCode()
        {
            string[] line_codes = new string[]
            {
                "SCC(u) {",
                "    num[u] = min_num[u] = k; k++;",
                "    thêm u vào stack",
                "    for (v là các đỉnh kề của u)",
                "        if (v chưa duyệt) {",
                "            SCC(v);",
                "            min_num[u] = min(min_num[u], min_num[v]);",
                "        } else if (v còn trên stack)",
                "            min_num[u] = min(min_num[u], num[v]);",
                "   if (num[u] == min_num[u]) {",
                "       Tìm thấy một bộ phận liên thông mạnh!",
                "       Rút các đỉnh ra khỏi stack đến khi gặp u",
                "   }",
                "}"
            };
            foreach (string line in line_codes)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }
        }

        public void FillIAlgorithmRequirement()
        {
            if (Requirements.Count != 0) return;
            Requirements.Add(new RequestOfAlgorithm("Phải là đồ thị có hướng"));
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            Requirements[0].IsDoneRequest = graph.IsDirectedGraph();

            return Requirements.All(request => request.IsDoneRequest == true);
        }

        private void CleanAlgorithm()
        {
            num.Clear();
            num.TrimExcess();
            min_num.Clear();
            min_num.TrimExcess();
            funtionStack.Clear();
            funtionStack.TrimExcess();
            Neighbours.Clear();
            Neighbours.TrimExcess();
            SCCStack.Clear();
            SCCStack.TrimExcess();
            k = 0;
        }

        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;
            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        public async void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;

            await RunLoop(graph, token);
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                var startVertex = QueueVertices.Dequeue();

                if (startVertex.IsVisited == true) continue;

                if (startVertex != null)
                    funtionStack.Push((startVertex, 0, RecursiveState.Entry));
                await RunLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        public async void RunAlgorithm(CustomGraph graph,Vertex startVertex)
        {
            var token = cts.Token;
            base.BaseRunAlgorithm(graph);
            foreach (var item in Result)
            {
                item.Clear();
            }
            Result.Clear();
            await PrepareState();
            funtionStack.Push((startVertex, 0,RecursiveState.Entry));
            await RunLoop(graph, token);
            EndAlgorithmState(graph);
        }

        public  async void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {

            base.BaseRunAlgorithm(graph);
            QueueVertices.Clear();
            foreach (var vertex in vertices)
                QueueVertices.Enqueue(vertex);

            foreach (var item in Result)
            {
                item.Clear();
            }
            Result.Clear();

            var token = cts.Token;
            await PrepareState();
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                Vertex startVertex  = QueueVertices.Dequeue();

                if (startVertex.IsVisited == true) continue;

                funtionStack.Push((startVertex, 0, RecursiveState.Entry));
                await RunLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        private Task PrepareState()
        {
            CleanAlgorithm();
            return Task.FromResult(0);
        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (QueueVertices.Count == 0 && funtionStack.Count == 0) OnCompletedAlgorithm();
        }

        private async Task RunLoop(CustomGraph graph, CancellationToken token)
        {
            while(funtionStack.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(TimeDelayOfLineCode);
                    return;
                }

                var (u, neighBourIndex, state) = funtionStack.Pop();
                u.SetPointTo();
                if (state == RecursiveState.Entry)
                {
                    await JoinFuntionState();

                    if (!u.IsVisited)
                    {
                        await SetNum_MinNumState();

                        if (!num.ContainsKey(u))
                            num.Add(u, k);
                        else num[u] = k;
                        if (!min_num.ContainsKey(u))
                            min_num.Add(u, k);
                        else min_num[u] = k;
                        k++;
                        u.SetVitsited();
                        SCCStack.Push(u);
                        u.SetPending();
                    }

                    if (!Neighbours.ContainsKey(u))
                        Neighbours.Add(u, graph.NeighboursOfVertex(u));
                    u.UnSetPointedTo();
                    if (neighBourIndex < Neighbours[u].Count)
                    {
                        await ForLoopState();
                        funtionStack.Push((u, neighBourIndex + 1, RecursiveState.Entry));

                        Vertex v = Neighbours[u][neighBourIndex];
                        v.SetPointTo();
                        await IfNotVisitedState();
                        if (!v.IsVisited)
                        {
                            await JoinNewFuntionState();
                            funtionStack.Push((v, 0, RecursiveState.Entry));
                        }
                        else
                        {
                            await IfRemainingOnStackState();
                            if (v.IsPending)
                            {
                                await SetMinnumByOnStackState();
                                // nhìn lên trái phải
                                min_num[u] = Math.Min(min_num[u], num[v]);
                            }
                        }
                        v.UnSetPointedTo();
                    }
                    else
                    {
                        funtionStack.Push((u, neighBourIndex, RecursiveState.Exit));
                    }
                }
                else if (state == RecursiveState.Exit)
                {
                    if (funtionStack.Count > 0 && Neighbours[u].Count > 0)
                    {
                        var (parent_u, _, _) = funtionStack.Peek();
                        if (parent_u != null)
                        {
                            await SetMinnumBySCCState();
                            min_num[parent_u] = Math.Min(min_num[parent_u], min_num[u]);
                        }
                    }
                    await IfMinnumEqualNumState();
                    if (min_num[u] == num[u])
                    {
                        await SCCFoundState();
                        List<Vertex> SCCVertex = new List<Vertex>();
                        Vertex v;
                        Result.Add(new ObservableCollection<string>());
                        do
                        {
                            v = SCCStack.Pop();
                            Result.Last().Insert(0, v.Title);
                            v.UnSetPending();
                            SCCVertex.Add(v);
                        } while (v != u);
                        graph.ColoredAllEdgeOfVertices(SCCVertex);
                    }
                }
                u.UnSetPointedTo();
            }
        }

        private async Task JoinFuntionState()
        {
            Pseudocodes[0].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;
        }

        private async Task SetNum_MinNumState()
        {
            Pseudocodes[1].IsSelectionCode = true;
            Pseudocodes[2].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[1].IsSelectionCode = false;
            Pseudocodes[2].IsSelectionCode = false;
        }

        private async Task ForLoopState()
        {
            Pseudocodes[3].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[3].IsSelectionCode = false;
        }
        private async Task IfNotVisitedState()
        {
            Pseudocodes[4].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[4].IsSelectionCode = false;
        }
        private async Task JoinNewFuntionState()
        {
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;
        }
        private async Task IfRemainingOnStackState()
        {
            Pseudocodes[7].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
        }
        private async Task SetMinnumByOnStackState() 
        {
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }
        
        private async Task SetMinnumBySCCState()
        {
            Pseudocodes[6].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[6].IsSelectionCode = false;
        }

       private async Task IfMinnumEqualNumState()
        {
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[9].IsSelectionCode = false;
        }

        private async Task SCCFoundState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
            Pseudocodes[11].IsSelectionCode = false;
        }

    }
}
