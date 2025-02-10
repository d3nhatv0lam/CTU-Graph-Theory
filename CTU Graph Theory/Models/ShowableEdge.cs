using AvaloniaGraphControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class ShowableEdge : Edge
    {
        public double Weight { get; set; }
        public bool IsShowEdge { get; private set; }
        public Visible VisibleState { get; }
        public enum Visible
        {
            NotShow,
            Show
        }

        public ShowableEdge(object tail, object head,Visible isShowEdge = Visible.Show, object? lable = null, Edge.Symbol tailSymbol = Symbol.None, Edge.Symbol headSymbol = Symbol.Arrow)
            : base(tail, head, lable, tailSymbol, headSymbol)
        {
            IsShowEdge = GetEdgeState(isShowEdge);
            VisibleState = isShowEdge;
        }

        private bool GetEdgeState(Visible isShowEdge)
        {
            bool isVisible = true;
            switch (isShowEdge)
            {
                case Visible.Show:
                    isVisible = true;
                    break;
                case Visible.NotShow:
                    isVisible = false;
                    break;
            }
            return isVisible;
        }
    }
}
