using Avalonia.Media;
using AvaloniaGraphControl;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class ShowableEdge : Edge , IReactiveObject
    {
        public double Weight { get; set; }
        public bool IsShowEdge { get; private set; }
        public Visible VisibleState { get; }
        public enum Visible
        {
            NotShow,
            Show
        }

        public bool _isVisited = false;

        // kiểm tra đã duyệt trên UI
        public bool IsVisited
        {
            get => _isVisited;
            set 
            {
                
                this.RaisePropertyChanging(new PropertyChangingEventArgs(nameof(IsVisited)));
                _isVisited = value;
                this.RaisePropertyChanged(new PropertyChangedEventArgs(nameof(IsVisited)));
                //Debug.WriteLine(((Vertex)Tail).ToString() + ((Vertex)Head).ToString() + IsVisited + " " + _isVisited);
            }
        }



        public ShowableEdge(object tail, object head,Visible isShowEdge = Visible.Show, object? lable = null, Edge.Symbol tailSymbol = Symbol.None, Edge.Symbol headSymbol = Symbol.Arrow)
            : base(tail, head, lable, tailSymbol, headSymbol)
        {
            IsShowEdge = GetEdgeState(isShowEdge);
            VisibleState = isShowEdge;
        }

      

        public Vertex? GetRemainingVertexOfEdge(Vertex u)
        {
            if (this.Tail == u) return (Vertex)this.Head;
            if (this.Head == u) return (Vertex)this.Tail;
            return null;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
