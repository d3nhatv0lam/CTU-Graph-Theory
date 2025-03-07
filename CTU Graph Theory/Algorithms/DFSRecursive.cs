using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;

namespace CTU_Graph_Theory.Algorithms
{
    public class DFSRecursive : AbstractAlgorithm , IAllVertexRun
    {
        private Stack<Vertex> FuntionStack;

        public DFSRecursive() : base()
        {
            AlgorithmName = "DFS - Duyệt theo chiều sâu (đệ quy)";
            FuntionStack = new Stack<Vertex>();
            FillPseudoCode();
        }

        protected override void FillPseudoCode()
        {
            string[] code_lines =
            {
                "void DFS(int u) {",
                "   if (u đã duyệt)",
                "       return;",
                "   duyệt u",
                "   đánh dấu đỉnh u đã duyệt;",
                "   for (các đỉnh kề v của u)",
                "       DFS(v);",
                "}"
            };
            foreach (string line in code_lines)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }
        }
        private void CleanDFS()
        {
            FuntionStack.Clear();
            FuntionStack.TrimExcess();
        }
        public async void ContinueAlgorithm(CustomGraph graph)
        {
            base.BaseContinueAlgorithm(graph);
            var token = cts.Token;

            await RunDFSLoop(graph, token);
            EndAlgorithmState(graph);
        }

        public async void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            var token = cts.Token;
            base.BaseContinueAlgorithm(graph);
            await RunDFSLoop(graph, token);
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                var startVertex = QueueVertices.Dequeue();
                if (startVertex.IsVisited == true) continue;

                FuntionStack.Push(startVertex);

                await RunDFSLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        public async void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            base.BaseRunAlgorithm(graph);
            var token = cts.Token;

            await PrepareState();
            FuntionStack.Push(startVertex);
            await RunDFSLoop(graph,token);

            EndAlgorithmState(graph);
        }

        public async void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            base.BaseRunAlgorithm(graph);
            QueueVertices.Clear();
            foreach (var vertex in vertices)
                QueueVertices.Enqueue(vertex);

            var token = cts.Token;
            await PrepareState();
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;


                 var startVertex = QueueVertices.Dequeue();
                
                if (startVertex.IsVisited == true) continue;

                FuntionStack.Push(startVertex);
                await RunDFSLoop(graph, token);
            }
            EndAlgorithmState(graph);
        }

        private void EndAlgorithmState(CustomGraph graph)
        {
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (QueueVertices.Count == 0 && FuntionStack.Count == 0) OnCompletedAlgorithm();
        }

        private  Task PrepareState()
        {
            CleanDFS();
            return Task.FromResult(0);
        }

        private async Task RunDFSLoop(CustomGraph graph,CancellationToken token)
        {
            while (FuntionStack.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }
                await JoinFuntionState();
                // double check to avoid spam pause-continue
                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }
                Vertex u = await GetVertexState();
                // update Point to vertex into UI
                u.SetPointTo();

                bool isReturn = await IsVertexMarkedState(u);
                if (isReturn) 
                {
                    await ReturnState();
                    u.UnSetPointedTo();
                    continue;
                }

                
                await VisitVertexU(u);
                // draw adjacent => update into UI
                if (u.ParentVertex != null)
                {
                    ShowableEdge? AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
                    if (AdjacentEdge != null)
                        AdjacentEdge.IsVisited = true;
                }

                List<Vertex> reverseNeighBours = graph.NeighboursOfVertex(u);
                reverseNeighBours.Reverse();
                // need to push stack and Pause Algorithm
                foreach (Vertex v in reverseNeighBours)
                {
                    FuntionStack.Push(v);
                    v.ParentVertex = u;
                }
                await LoopState();

                await JoinNewFuntionState();
            }
        }

        private async Task JoinFuntionState()
        {
            Pseudocodes[0].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;
        }

        private Task<Vertex> GetVertexState()
        {
            Vertex u = FuntionStack.Pop();
            return Task.FromResult(u);
        }

        private async Task<bool> IsVertexMarkedState(Vertex u)
        {
            bool isMarked = false;
            Pseudocodes[1].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[1].IsSelectionCode = false;
            if (u.IsVisited)
                isMarked = true;
            return isMarked;
        }

        private async Task ReturnState()
        {
            Pseudocodes[2].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[2].IsSelectionCode = false;
        }

        private async Task VisitVertexU(Vertex u)
        {
            Pseudocodes[3].IsSelectionCode = true;
            Pseudocodes[4].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            u.SetVitsited();
            u.UnSetPointedTo();
            Pseudocodes[3].IsSelectionCode = false;
            Pseudocodes[4].IsSelectionCode = false;
        }

        private async Task LoopState()
        {
            Pseudocodes[5].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = false;
        }

        private async Task JoinNewFuntionState()
        {
            Pseudocodes[6].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[6].IsSelectionCode = false;
        }
    }
}
