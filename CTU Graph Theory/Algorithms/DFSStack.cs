using System;
using System.Collections;
using System.Collections.Generic;
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
            List<string> code_lines = new List<string>() {
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

        protected override void StartVetexChanged(CustomGraph graph)
        {
            base.StartVetexChanged(graph);
            CleanBFS(graph);
        }

        public override async void RunAlgorithm(CustomGraph graph)
        {
            base.RunAlgorithm(graph);

            if (StartVertex == null) return;

            graph.UnVisitAndClearParentAll();
            CleanBFS(graph);
            stack.Push(StartVertex);
            IsStartVertexChanged = false;

            Pseudocodes[0].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;

            // clone token để xóa biết đường tự hủy
            var token = cts.Token;
            RunDFSStackLoop(graph, token);
        }

        public override void ContinueAlgorithm(CustomGraph graph)
        {
            base.ContinueAlgorithm(graph);
            var token = cts.Token;
            RunDFSStackLoop(graph, token);
        }

        private async void RunDFSStackLoop(CustomGraph graph, CancellationToken token)
        {
            while (stack.Count != 0)
            {
                Pseudocodes[1].IsSelectionCode = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[1].IsSelectionCode = false;


                if (token.IsCancellationRequested)
                    return;

                Pseudocodes[2].IsSelectionCode = true;
                Vertex u = stack.Pop();
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[2].IsSelectionCode = false;
                //visit vertex => update into UI
                Pseudocodes[3].IsSelectionCode = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                if (u.IsVisited == true)
                {
                    Pseudocodes[3].IsSelectionCode = false;
                    Pseudocodes[4].IsSelectionCode = true;
                    await Task.Delay(this.TimeDelayOfLineCode);
                    Pseudocodes[4].IsSelectionCode = false;
                    continue;
                }
                Pseudocodes[3].IsSelectionCode = false;

                Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = true;
                u.IsPending = false;
                u.IsVisited = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = false;
                // draw adjacent => update into UI
                if (u.ParentVertex != null)
                {
                    ShowableEdge? AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
                    if (AdjacentEdge != null)
                        AdjacentEdge.IsVisited = true;
                }

                foreach (var v in graph.NeighboursOfVertex(u))
                {
                    Pseudocodes[7].IsSelectionCode = true;
                    await Task.Delay(TimeDelayOfLineCode);
                    Pseudocodes[7].IsSelectionCode = false;
                    Pseudocodes[8].IsSelectionCode = true;
                    await Task.Delay(TimeDelayOfLineCode);
                    if (v.IsVisited == false)
                    {
                        Pseudocodes[8].IsSelectionCode = false;

                        Pseudocodes[9].IsSelectionCode = true;
                        await Task.Delay(TimeDelayOfLineCode);
                        stack.Push(v);
                        if (v.ParentVertex == null) v.ParentVertex = u;
                        Pseudocodes[9].IsSelectionCode = false;
                        v.IsPending = true;
                        await Task.Delay(TimeDelayOfLineCode);
                    }
                    Pseudocodes[8].IsSelectionCode = false;
                }
            }
            Pseudocodes[1].IsSelectionCode = false;
            OnCompletedAlgorithm();
        }
    }
}
