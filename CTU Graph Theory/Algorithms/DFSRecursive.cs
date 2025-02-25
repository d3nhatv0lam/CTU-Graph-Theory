using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Models;

namespace CTU_Graph_Theory.Algorithms
{
    public class DFSRecursive : AbstractAlgorithm
    {
        public override string AlgorithmName { get; }
        public DFSRecursive() : base()
        {
            AlgorithmName = "DFS - Duyệt theo chiều sâu (dệ quy)";
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

        public override void ContinueAlgorithm(CustomGraph graph)
        {
            throw new NotImplementedException();
        }

        public override void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            throw new NotImplementedException();
        }

        public override void RunAlgorithm(CustomGraph graph)
        {
            base.BaseRunAlgorithm(graph);
        }

        public override void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            throw new NotImplementedException();
        }


    }
}
