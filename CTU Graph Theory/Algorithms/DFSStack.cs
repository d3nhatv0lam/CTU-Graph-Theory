using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Models;

namespace CTU_Graph_Theory.Algorithms
{
    public class DFSStack : AlgorithmBase
    {

        private Stack<Vertex> stack;
        public DFSStack() : base()
        {
            this.AlgorithmName = "BFS - Duyệt theo chiều sâu bằng Stack";
            stack = new Stack<Vertex>();
            FillPseudoCode();
        }

        protected override void FillPseudoCode()
        {
            string[] code_lines = {
                "Thêm đỉnh bất kỳ vào ngăn xếp",
                "while ngăn xếp chưa rỗng {",
                "    u = Lấy đỉnh ở đỉnh ngăn xếp ra",
                "    if (u đã duyệt)",
                "        continue;",
                "    Duyệt u",
                "    Đánh dấu u đã duyệt",
                "    for các đỉnh kề v của u",
                "        if (v chưa duyệt)",
                "            Đưa v vào ngăn xếp",
                "}"
            };

            foreach (var line in code_lines)
            {
                Pseudocodes.Add(new StringPseudoCode(line));
            }
        }

        private void CleanBFS(CustomGraph graph)
        {
            stack.Clear();
        }

        public override async void RunAlgorithm(CustomGraph graph)
        {
            var token = cts.Token;
            base.RunAlgorithm(graph);
            await PrepareDFSStackState(graph);
           
            stack.Push(StartVertex);

            await ChooseStartVertexState();

            // clone token để xóa biết đường tự hủy
            
            await RunDFSStackLoop(graph, token);


            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (stack.Count == 0) OnCompletedAlgorithm();
        }

        public override async void ContinueAlgorithm(CustomGraph graph)
        {
            base.ContinueAlgorithm(graph);
            var token = cts.Token;
            await RunDFSStackLoop(graph, token);
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (stack.Count == 0) OnCompletedAlgorithm();
        }

        public override async void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            base.RunAlgorithmWithAllVertex(graph, vertices);
            var token = cts.Token;
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                await PrepareDFSStackState(graph);
                await ChooseStartVertexState();

                StartVertex = QueueVertices.Dequeue();
                if (StartVertex.IsVisited == true) continue;

                stack.Push(StartVertex);
                await RunDFSStackLoop(graph, token);
            }
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (QueueVertices.Count == 0 && stack.Count == 0) OnCompletedAlgorithm();
        }
        public override async void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            var token = cts.Token;
            base.ContinueAlgorithmWithAllVertex(graph);
            await RunDFSStackLoop(graph, token);
            while (QueueVertices.Count > 0)
            {
                if (token.IsCancellationRequested) break;
                StartVertex = QueueVertices.Dequeue();
                if (StartVertex.IsVisited == true) continue;

                stack.Push(StartVertex);

                await RunDFSStackLoop(graph, token);
            }
            if (IsStopAlgorithm) base.CleanGraphForAlgorithm(graph);
            if (QueueVertices.Count == 0 && stack.Count == 0) OnCompletedAlgorithm();
        }

        private async Task PrepareDFSStackState(CustomGraph graph)
        {
            if (StartVertex == null) return;
            CleanBFS(graph);
        }

        private async Task ChooseStartVertexState()
        {
            Pseudocodes[0].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;
        }

        private async Task RunDFSStackLoop(CustomGraph graph, CancellationToken token)
        {
            while (stack.Count != 0)
            {
                await WhileState();

                if (token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                    return;
                }

                Vertex u = await ChooseVertexUState();
                u.SetPointTo();
                //visit vertex => update into UI
                bool isContinue = await IsContinueState(u);
                u.UnSetPointedTo();
                if (isContinue) continue;

                await MarkVertexState(u);
               
                // draw adjacent => update into UI
                if (u.ParentVertex != null)
                {
                    ShowableEdge? AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
                    if (AdjacentEdge != null)
                        AdjacentEdge.IsVisited = true;
                }


                foreach (var v in graph.NeighboursOfVertex(u))
                {
                    v.SetPointTo();
                    await ForLoopState();

                    await IfVisitedState();
                    if (v.IsVisited == false)
                    {
                        await AddVertexIntoStackState(v, u);
                    }
                    else v.UnSetPointedTo();
                    
                }
            }
        }

        private async Task WhileState()
        {
            Pseudocodes[1].IsSelectionCode = true;
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[1].IsSelectionCode = false;
        }

        private async Task<Vertex> ChooseVertexUState()
        {
            Pseudocodes[2].IsSelectionCode = true;
            Vertex u = stack.Pop();
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[2].IsSelectionCode = false;
            return u;
        }

        private async Task<bool> IsContinueState(Vertex u)
        {
            bool isContinue = false;
            Pseudocodes[3].IsSelectionCode = true;
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[3].IsSelectionCode = false;
            if (u.IsVisited == true)
            {
                Pseudocodes[4].IsSelectionCode = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[4].IsSelectionCode = false;
                isContinue = true;
            }
            return isContinue;
        }

        private async Task MarkVertexState(Vertex u)
        {
            Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = true;
            u.SetVitsited();
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = false;
        }
        private async Task ForLoopState()
        {
            Pseudocodes[7].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
        }
        private async Task IfVisitedState()
        {
            Pseudocodes[8].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }

        private async Task AddVertexIntoStackState(Vertex v, Vertex u)
        {
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            stack.Push(v);
            if (v.ParentVertex == null) v.ParentVertex = u;
            Pseudocodes[9].IsSelectionCode = false;
            v.UnSetPointedTo();
            v.SetPending();
            await Task.Delay(TimeDelayOfLineCode);
        }
    }
}
