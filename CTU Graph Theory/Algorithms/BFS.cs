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
            "Đưa đỉnh bất kỳ vào Hàng đợi",
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

            await ChooseStartVertex();

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
                await WhileState();

                if (token.IsCancellationRequested)
                    return;

                Vertex u = await ChooseVertexUState();

                //visit vertex => update into UI
                bool isMarked = await IsVertexMarkedState(u);
                if (isMarked) continue;

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
                    await ForLoopVState();

                    await IfVisitedState(v);
                    if (v.IsVisited == false)
                    {
                        await AddVertexIntoQueueState(v,u);  
                    }
                }
            }
            Pseudocodes[1].IsSelectionCode = false;
            OnCompletedAlgorithm();
        }

        private async Task ChooseStartVertex()
        {
            Pseudocodes[0].IsSelectionCode = true;
            Pseudocodes[0].FillVertextIntoCode(StartVertex);
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[0].IsSelectionCode = false;
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
            Vertex u = queue.Dequeue();
            Pseudocodes[2].FillVertextIntoCode(u);
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[2].IsSelectionCode = false;
            return u;
        }
      
        private async Task<bool> IsVertexMarkedState(Vertex u)
        {
            bool isContinue = false;
            Pseudocodes[3].IsSelectionCode = true;
            Pseudocodes[3].FillVertextIntoCode(u);
            await Task.Delay(this.TimeDelayOfLineCode);
            if (u.IsVisited == true)
            {
                Pseudocodes[3].IsSelectionCode = false;
                Pseudocodes[4].IsSelectionCode = true;
                await Task.Delay(this.TimeDelayOfLineCode);
                Pseudocodes[4].IsSelectionCode = false;
                isContinue = true;
            }
            Pseudocodes[3].IsSelectionCode = false;
            return isContinue;
        }

        private async Task MarkVertexState(Vertex u)
        {
            Pseudocodes[5].FillVertextIntoCode(u);
            Pseudocodes[6].FillVertextIntoCode(u);
            Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = true;
            u.IsPending = false;
            u.IsVisited = true;
            await Task.Delay(this.TimeDelayOfLineCode);
            Pseudocodes[5].IsSelectionCode = Pseudocodes[6].IsSelectionCode = false;
        }

        private async Task ForLoopVState()
        {
            Pseudocodes[7].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[7].IsSelectionCode = false;
        }

        private async Task IfVisitedState(Vertex v)
        {
            Pseudocodes[8].IsSelectionCode = true;
            Pseudocodes[8].FillVertextIntoCode(v);
            Pseudocodes[9].FillVertextIntoCode(v);
            await Task.Delay(TimeDelayOfLineCode);
            Pseudocodes[8].IsSelectionCode = false;
        }

        private async Task AddVertexIntoQueueState(Vertex v, Vertex u)
        {
            Pseudocodes[9].IsSelectionCode = true;
            await Task.Delay(TimeDelayOfLineCode);
            queue.Enqueue(v);
            if (v.ParentVertex == null) v.ParentVertex = u;
            Pseudocodes[9].IsSelectionCode = false;
            v.IsPending = true;
            await Task.Delay(TimeDelayOfLineCode);
        }
    }
}
