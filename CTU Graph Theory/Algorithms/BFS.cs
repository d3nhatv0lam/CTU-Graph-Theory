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
    public class BFS : AlgorithmBase
    {
        // Algorithms
        private readonly Queue<Vertex> queue;
        public BFS()
        {
            AlgorithmName = "BFS - Duyệt theo chiều rộng";
            Pseudocodes = new System.Collections.ObjectModel.ObservableCollection<StringPseudoCode>();
            queue = new();
            FillPseudoCode();
        }

        protected override void FillPseudoCode()
        {
             List<string> code_lines = [
            "Đưa 1 đỉnh bất kỳ vào Hàng đợi",
            "while Hàng đợi chưa rỗng {",
            "    u = lấy đỉnh ở đầu hàng đợi ra",
            "    if (u đã duyệt)",
            "        continue;",
            "    Duyệt u",
            "    Đánh dấu u đã được duyệt",
            "    for các đỉnh kề v của u",
            "        if v chưa được duyệt",
            "            Đưa v vào hàng đợi",
            "}" ];
           foreach (var line in code_lines)
                Pseudocodes.Add(new StringPseudoCode(line));
            
        }

        private void CleanBFS(CustomGraph graph)
        {
            graph.UnVisitAndClearParentAll();
            queue.Clear();
        }

        protected override void StartVetexChanged(CustomGraph graph)
        {
            CleanBFS(graph);
        }

        public override void PauseAlgorithm()
        {
            base.PauseAlgorithm();
        }

        public override void RunAlgorithm(CustomGraph graph)
        {
            if (StartVertex == null)
            {
                CleanBFS(graph);
                return;
            }
            if (queue.Count == 0 || IsStartVertexChanged == true)
            {
                CleanBFS(graph);
                queue.Enqueue(StartVertex);
                IsStartVertexChanged = false;
            }
            // clone token để xóa biết đường tự hủy
            var token = cts.Token;
            RunBFSLoop(graph,token);
        }

        private async void RunBFSLoop(CustomGraph graph,CancellationToken token)
        {
            while (queue.Count != 0)
            {
                if (token.IsCancellationRequested)
                    return;

                Vertex u = queue.Dequeue();
                //visit vertex => update into UI
                if (u.IsVisited == true) continue;

                u.IsPending = false;
                u.IsVisited = true;
                // draw adjacent => update into UI
                if (u.ParentVertex != null)
                {
                    ShowableEdge? AdjacentEdge = graph.GetEdge(u.ParentVertex, u);
                    if (AdjacentEdge != null)
                        AdjacentEdge.IsVisited = true;
                }

                foreach (var v in graph.NeighboursOfVertex(u))
                {
                    if (v.IsVisited == false)
                    {
                        queue.Enqueue(v);
                        if (v.ParentVertex == null) v.ParentVertex = u;
                        v.IsPending = true;
                        await Task.Delay(this.RunSpeed);
                    }
                }
                try
                {
                    await Task.Delay(this.RunSpeed, token);
                }
                catch { return; }
            }
            OnCompletedAlgorithm();
        }
    }
}
