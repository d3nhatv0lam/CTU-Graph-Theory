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
            queue.Clear();
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
            queue.Enqueue(StartVertex);
            IsStartVertexChanged = false;

            Pseudocodes[0].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;

            // clone token để xóa biết đường tự hủy
            var token = cts.Token;
            RunBFSLoop(graph,token);
        }

        public override void ContinueAlgorithm(CustomGraph graph)
        {
            base.ContinueAlgorithm(graph);
            var token = cts.Token;
            RunBFSLoop(graph,token);
        }

        private async void RunBFSLoop(CustomGraph graph,CancellationToken token)
        {
            
            while (queue.Count != 0)
            {
                Pseudocodes[1].IsSelectionCode = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[1].IsSelectionCode = false;

                if (token.IsCancellationRequested)
                    return;

                Pseudocodes[2].IsSelectionCode = true;
                Vertex u = queue.Dequeue();
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
                        queue.Enqueue(v);
                        if (v.ParentVertex == null) v.ParentVertex = u;
                        Pseudocodes[9].IsSelectionCode = false ;
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
