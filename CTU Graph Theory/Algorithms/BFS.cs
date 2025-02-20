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
    public class BFS : IAlgorithms
    {
        public string AlgorithmName { get; }
        private CancellationTokenSource? cts = new();

        private readonly Queue<Vertex> queue = new();
        private Vertex? StartVertex { get; set; } = null;



        public BFS()
        {
            AlgorithmName = "Duyệt theo chiều rộng";
        }

        public void StopAlgorithms()
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
        }

        public async void RunAlgorithms(CustomGraph graph,Vertex s)
        {
           // Đổi lại cấu trúc, thì khi này thuật toán sẽ chạy tiếp tục kể cả khi pause/run
           // nếu không có thì sẽ bị reset toàn bộ
            if ( StartVertex == null  ||
                ( StartVertex != null && StartVertex != s))
            {
                graph.UnVisitAndClearParentAll();
                queue.Clear();
                queue.Enqueue(s);
                StartVertex = s;
            }
            // copy ra để stop không bị bug
            var token = cts.Token;
           
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
                        await Task.Delay(1000);
                    }
                }
                await Task.Delay(1000);
            }

            if (queue.Count == 0) StartVertex = null;
        }


    }
}
