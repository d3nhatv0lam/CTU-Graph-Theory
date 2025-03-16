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

namespace CTU_Graph_Theory.Algorithms
{
    public class CircledCheck: AbstractAlgorithm , IAllVertexRun
    {
        private enum FuntionState
        {
            Entry,
            Exit
        };
        private Stack<Vertex> FuntionStack;
        private Dictionary<Vertex, List<Vertex>> NeigboursOfVertex;

        private Stack<(Vertex, int, FuntionState)> FunctionStack;
        private bool _isHasCricled;
        private event EventHandler<bool?>? _returnIsCricleCheck;
        public event EventHandler<bool?> ReturnIsCricleCheck
        {
            add => _returnIsCricleCheck += value;
            remove => _returnIsCricleCheck -= value;
        }

        private void OnReturnIsCricleCheck(bool? isCricleCheck)
        {
            _returnIsCricleCheck?.Invoke(this, isCricleCheck);
        }

        public CircledCheck(): base()
        {
            AlgorithmName = "CircledCheck - Kiểm tra đồ thị chứa chu trình";
            FuntionStack = new();
            FunctionStack = new();
            NeigboursOfVertex = new Dictionary<Vertex, List<Vertex>>();
            _isHasCricled = false;
            FillPseudoCode();
        }
        protected override void FillPseudoCode()
        {
            string[] code_lines = {
            "Đen: Chưa duyệt",
            "Vàng: Đang duyệt",
            "Đỏ: Đã duyệt",
            "// Có hướng //",
            "CircleCheck_BaseDFS(Vertex u) {",
            "\tTô màu cho đỉnh u là vàng (đang duyệt)",
            "\tFor (Các đỉnh kề v của u)",
            "\t\tif (v chưa duyệt)",
            "\t\t\tCircleCheck_BaseDFS(v)",
            "\t\telse if (v đang được duyệt)",
            "\t\t\tĐã tìm thấy chu trình!",
            "\tTô màu đỉnh u là đỏ (đã duyệt)",
            "}",
            "// Vô hướng //",
            "CircleCheck_BaseDFS(Vertex u, Vertex parent_u) {",
            "\tTô màu cho đỉnh u là vàng (đang duyệt)",
            "\tFor (Các đỉnh kề v của u)",
            "\t\tif (v là parent_u)",
            "\t\t\tcontinue;",
            "\t\tif (v chưa duyệt)",
            "\t\t\tCircleCheck_BaseDFS(v,u)",
            "\t\telse if (v đang được duyệt)",
            "\t\t\tĐã tìm thấy chu trình!",
            "\tTô màu đỉnh u là đỏ (đã duyệt)",
            "}"
            };
            foreach (var line in code_lines)
                Pseudocodes.Add(new StringPseudoCode(line));
        }

        private void CleanDFS()
        {
            _isHasCricled = false;
            FunctionStack.Clear();
            FunctionStack.TrimExcess();
            FuntionStack.Clear();
            FuntionStack.TrimExcess();
            foreach (var (key,value) in NeigboursOfVertex)
            {
                value.Clear();
                value.TrimExcess();
            }
            NeigboursOfVertex.Clear();
            NeigboursOfVertex.TrimExcess();
        }

        private Task DoneAlgorithm()
        {
            CleanDFS();
            QueueVertices.Clear();
            return Task.FromResult(0);
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
            var token = cts.Token;
            base.BaseContinueAlgorithm(graph);
            await RunLoop(graph, token);
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                var startVertex = QueueVertices.Dequeue();
                if (startVertex.IsVisited == true || startVertex.IsPending == true) continue;

                //FuntionStack.Push(startVertex);
                FunctionStack.Push((startVertex, 0, FuntionState.Entry));
                await RunLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        public async void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;

            await PrepareState();
            //FuntionStack.Push(startVertex);
            FunctionStack.Push((startVertex, 0, FuntionState.Entry));
            await RunLoop(graph,token);
            EndAlgorithmState(graph);
        }

        public async void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            BaseRunAlgorithm(graph);
            QueueVertices.Clear();
            foreach (var vertex in vertices)
                QueueVertices.Enqueue(vertex);


            var token = cts.Token;
            await PrepareState();
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;


                var startVertex = QueueVertices.Dequeue();

                if (startVertex.IsVisited == true || startVertex.IsPending == true) continue;
                FunctionStack.Push((startVertex, 0, FuntionState.Entry));
                //FuntionStack.Push(startVertex);
                await RunLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (QueueVertices.Count == 0 && FuntionStack.Count == 0) OnCompletedAlgorithm();
            OnReturnIsCricleCheck(_isHasCricled);
        }

        private Task PrepareState()
        {
            CleanDFS(); 
            OnReturnIsCricleCheck(null);
            return Task.FromResult(0);
        }

        private async Task RunLoop(CustomGraph graph, CancellationToken token)
        {
            while(FunctionStack.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }

                var (u, loopIndex, state) = FunctionStack.Pop();

                if (!u.IsPending)
                {
                    if (graph.IsDirectedGraph())
                        await JoinDirectedFuntionState();
                    else await JoinUnDirectedFuintionState();

                    u.SetPointTo();
                }

                if (state == FuntionState.Entry)
                {
                    
                    if (!NeigboursOfVertex.ContainsKey(u))
                        NeigboursOfVertex.Add(u, graph.NeighboursOfVertex(u));

                    var neigbours = NeigboursOfVertex[u];

                    if (loopIndex < neigbours.Count)
                    {
                        if (!u.IsVisited && !u.IsPending)
                        {
                            if (graph.IsDirectedGraph())
                                await SetDirectedPendingState(u);
                            else await SetUnDirectedPendingState(u);

                            u.UnSetPointedTo();
                            u.SetPending();
                        }
                           
                        var v = neigbours[loopIndex];

                        v.SetPointTo();
                        if (graph.IsDirectedGraph())
                        {
                            await DirectedForLoopState();
                            await DirectedIfUnVisitedState();
                            if (v.IsVisited == false && v.IsPending == false)
                            {
                                v.ParentVertex = u;
                                FunctionStack.Push((u, loopIndex + 1, FuntionState.Entry));
                                FunctionStack.Push((v, 0, FuntionState.Entry));
                                await DirectedNextLoopState();
                                continue;
                            }
                            
                            if (v.IsVisited == false && v.IsPending == true)
                            {
                                v.UnSetPointedTo();
                                await DirectedCircleFoundState();
                                await DoneAlgorithm();
                                await ColoredFullCircle(graph, u, v);
                                OnReturnIsCricleCheck(_isHasCricled);
                                return;
                            }

                            // nếu 2 trường hợp trên k xảy ra thì phải tăng loopIndex
                            FunctionStack.Push((u, loopIndex + 1, FuntionState.Entry));
                            v.UnSetPointedTo();
                        }
                        else
                        {
                            await UnDirectedForLoopState();
                            await UnDirectedIfParentState();
                            if (v == u.ParentVertex)
                            {
                                await UnDirectedContinueState();
                                v.UnSetPointedTo();
                                FunctionStack.Push((u, loopIndex + 1, FuntionState.Entry));
                                continue;
                            }
                            await UnDirectedIfUnVisitedState();
                            if (v.IsVisited == false && v.IsPending == false)
                            {
                                await UnDirectedNextLoopState();
                                v.ParentVertex = u;
                                FunctionStack.Push((u, loopIndex + 1, FuntionState.Entry));
                                FunctionStack.Push((v, 0, FuntionState.Entry));
                                continue;
                            }
                            await UnDirectedIfCircleFoundState();
                            if (v.IsVisited == false && v.IsPending == true)
                            {
                                await UnDirectedCircleFoundState();
                                v.UnSetPointedTo(); 
                                await DoneAlgorithm();
                                await ColoredFullCircle(graph, u, v);
                                OnReturnIsCricleCheck(_isHasCricled);
                                return;
                            }
                            // nếu 2 trường hợp trên k xảy ra thì phải tăng loopIndex
                            FunctionStack.Push((u, loopIndex + 1, FuntionState.Entry));
                            v.UnSetPointedTo();
                        }
                    }
                    else
                    {
                        u.SetPointTo();
                        FunctionStack.Push((u, loopIndex, FuntionState.Exit));
                    }
                }
                else if (state == FuntionState.Exit)
                {
                    
                    if (graph.IsDirectedGraph())
                        await DirectedSetVisitedVertexUState(u);
                    else await UnDirectedSetVisitedVertexUState(u);
                    u.SetVitsited();
                }
                u.UnSetPointedTo();
            }
        }

        //private async Task RunLoop(CustomGraph graph, CancellationToken token)
        //{
        //    while (FuntionStack.Count > 0)
        //    {
        //        if (token.IsCancellationRequested)
        //        {
        //            await Task.Delay(100);
        //            return;
        //        }
        //        if (graph.IsDirectedGraph())
        //            await JoinDirectedFuntionState();
        //        else await JoinUnDirectedFuintionState();
        //        // double check to avoid spam pause-continue
        //        if (token.IsCancellationRequested)
        //        {
        //            await Task.Delay(100);
        //            return;
        //        }

        //        Vertex u = GetVertexState().Result;
        //        // update Point to vertex into UI
        //        u.SetPointTo();

        //        if (graph.IsDirectedGraph())
        //            await SetDirectedPendingState(u);
        //        else await SetUnDirectedPendingState(u);

        //        u.UnSetPointedTo();
        //        // draw adjacent => update into UI
        //        //if (u.ParentVertex != null)
        //        //{
        //        //    ShowableEdge? AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
        //        //    if (AdjacentEdge != null)
        //        //        AdjacentEdge.IsVisited = true;
        //        //}

        //        List<Vertex> reverseNeighBours = graph.NeighboursOfVertex(u);
        //        reverseNeighBours.Reverse();
        //        // need to push stack and Pause Algorithm
        //        foreach (Vertex v in reverseNeighBours)
        //        {
        //            FuntionStack.Push(v);
        //            if (graph.IsDirectedGraph() && v.ParentVertex == null) v.ParentVertex = u;
        //            if (graph.IsUnDirectedGraph() && v.IsVisited == false && v.IsPending == false) v.ParentVertex = u;
        //            //v.ParentVertex = u;
        //        }

        //        if (reverseNeighBours.Count == 0 && FuntionStack.Count > 0)
        //        {
        //            Vertex v = u, topOfFuntionStack = FuntionStack.Peek();
        //            if (graph.IsDirectedGraph())
        //            {
        //                await DirectedForLoopState();
        //                await DirectedSetVisitedVertexUState(u);
        //                while (v.ParentVertex != null && v.ParentVertex != topOfFuntionStack.ParentVertex)
        //                {
        //                    await DirectedSetVisitedVertexUState(v);
        //                    v.UnSetPointedTo();
        //                    v = v.ParentVertex;
        //                }
        //                await UnDirectedSetVisitedVertexUState(v);
        //                v.UnSetPointedTo();
        //            }  
        //            else
        //            {
        //                await UnDirectedForLoopState();
        //                await UnDirectedSetVisitedVertexUState(u);
        //                while (v.ParentVertex != null && v.ParentVertex != topOfFuntionStack.ParentVertex)
        //                {
        //                    await UnDirectedSetVisitedVertexUState(v);
        //                    v.UnSetPointedTo();
        //                    v = v.ParentVertex;
        //                }
        //                await UnDirectedSetVisitedVertexUState(v);
        //                v.UnSetPointedTo();
        //            }
        //            continue;
        //        }



        //        while (FuntionStack.Count > 0)
        //        {
        //            // try get to check in Pseucode
        //            Vertex nextFuntionVertex = FuntionStack.Pop();

        //            if (graph.IsDirectedGraph())
        //            {
        //                await DirectedForLoopState();
        //                nextFuntionVertex.SetPointTo();

        //                await DirectedIfUnVisitedState();
        //                if (nextFuntionVertex.IsVisited == false && nextFuntionVertex.IsPending == false)
        //                {
        //                    await DirectedNextLoopState();
        //                    // push again to Recursive
        //                    FuntionStack.Push(nextFuntionVertex);
        //                    break;
        //                }
        //                if (nextFuntionVertex.IsVisited == false && nextFuntionVertex.IsPending == true)
        //                {
        //                    await DirectedCircleFoundState();
        //                    await DoneAlgorithm();
        //                    nextFuntionVertex.UnSetPointedTo();
        //                    if (graph.GetEdge(u, nextFuntionVertex) == null)
        //                        await ColoredFullCircle(graph, nextFuntionVertex.ParentVertex, nextFuntionVertex);
        //                    else await ColoredFullCircle(graph, u, nextFuntionVertex);
                            
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                await UnDirectedForLoopState();
        //                nextFuntionVertex.SetPointTo();
        //                await UnDirectedIfParentState();
        //                if (nextFuntionVertex == u.ParentVertex)
        //                {
        //                    await UnDirectedContinueState();
        //                    nextFuntionVertex.UnSetPointedTo();
        //                    continue;
        //                }
        //                await UnDirectedIfUnVisitedState();
        //                if (nextFuntionVertex.IsVisited == false && nextFuntionVertex.IsPending == false)
        //                {
        //                    await UnDirectedNextLoopState();
        //                    // push again to Recursive 
        //                    FuntionStack.Push(nextFuntionVertex);
        //                    nextFuntionVertex.UnSetPointedTo();
        //                    break;
        //                }
        //                await UnDirectedIfCircleFoundState();
        //                if (nextFuntionVertex.IsPending == true)
        //                {
        //                    await UnDirectedCircleFoundState();
        //                    await DoneAlgorithm();
        //                    await ColoredFullCircle(graph, u, nextFuntionVertex);
        //                    nextFuntionVertex.UnSetPointedTo();
        //                    return;
        //                }
        //            }
        //            nextFuntionVertex.UnSetPointedTo();
        //            // when didn't do anything, check all
        //            bool isAllVisited = reverseNeighBours.All(vertex => vertex.IsVisited == true);
        //            if (isAllVisited)
        //            {
        //                if (graph.IsDirectedGraph())
        //                    await DirectedSetVisitedVertexUState(u);
        //                else await UnDirectedSetVisitedVertexUState(u);
        //            }
        //        }

  

        //        if (FuntionStack.Count == 0)
        //        {
        //            if (graph.IsDirectedGraph())
        //            {
        //                while (u != null)
        //                {
        //                    await DirectedSetVisitedVertexUState(u);
        //                    u.UnSetPointedTo();
        //                    u = u.ParentVertex;
        //                }
        //            }
        //            else
        //            {
        //                while (u != null)
        //                {
        //                    await UnDirectedSetVisitedVertexUState(u);
        //                    u.UnSetPointedTo();
        //                    u = u.ParentVertex;
        //                }
        //            }     
        //        }
        //        //if (graph.IsDirectedGraph())
        //        //    await DirectedSetVisitedVertexUState(u);
        //        //else await UnDirectedSetVisitedVertexUState(u);
        //    }
        //}

        // Share State //

        private Task<Vertex> GetVertexState()
        {
            Vertex u = FuntionStack.Pop();
            return Task.FromResult(u);
        }
        private Task ColoredFullCircle(CustomGraph graph,Vertex u,Vertex v)
        {
            _isHasCricled = true;
            ShowableEdge? AdjacentEdge = graph.GetEdge(u, v);
            if (AdjacentEdge != null)
                AdjacentEdge.IsVisited = true;
            do
            {
                AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
                if (AdjacentEdge != null)
                    AdjacentEdge.IsVisited = true;
                u = u.ParentVertex;
            } while (v != u);
            return Task.FromResult(0);
        }

        // Directed State //
        private async Task JoinDirectedFuntionState()
        {
            Pseudocodes[4].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[4].IsSelectionCode = false;
        }
        private async Task SetDirectedPendingState(Vertex u)
        {
            u.SetPending();
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;
        }
        private async Task DirectedForLoopState()
        {
            Pseudocodes[6].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[6].IsSelectionCode = false;
        }
        private async Task DirectedIfUnVisitedState()
        {
            Pseudocodes[7].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
        }
        private async Task DirectedNextLoopState()
        {
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }
        private async Task DirectedCircleFoundState()
        {
            Pseudocodes[10].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[10].IsSelectionCode = false;
        }
        private async Task DirectedSetVisitedVertexUState(Vertex u)
        {
            u.SetVitsited();
            Pseudocodes[11].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[11].IsSelectionCode = false;
        }

        // UnDirected State //
        private async Task JoinUnDirectedFuintionState()
        {
            Pseudocodes[14].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[14].IsSelectionCode = false;
        }
        private async Task SetUnDirectedPendingState(Vertex u)
        {
            u.SetPending();
            Pseudocodes[15].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[15].IsSelectionCode = false;
        }
        private async Task UnDirectedForLoopState()
        {
            Pseudocodes[16].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[16].IsSelectionCode = false;
        }

        private async Task UnDirectedIfParentState()
        {
            Pseudocodes[17].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[17].IsSelectionCode = false;
        }

        private async Task UnDirectedContinueState()
        {
            Pseudocodes[18].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[18].IsSelectionCode = false;
        }
        private async Task UnDirectedIfUnVisitedState()
        {
            Pseudocodes[19].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[19].IsSelectionCode = false;
        }
        private async Task UnDirectedNextLoopState()
        {
            Pseudocodes[20].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[20].IsSelectionCode = false;
        }
        private async Task UnDirectedIfCircleFoundState()
        {
            Pseudocodes[21].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[21].IsSelectionCode = false;
        }
        private async Task UnDirectedCircleFoundState()
        {
            Pseudocodes[22].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[22].IsSelectionCode = false;
        }
        private async Task UnDirectedSetVisitedVertexUState(Vertex u)
        {
            u.SetVitsited();
            Pseudocodes[23].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[23].IsSelectionCode = false;
        }

      
    }
}
