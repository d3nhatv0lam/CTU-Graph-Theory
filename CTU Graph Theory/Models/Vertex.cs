
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class Vertex : ReactiveObject
    {
        public static Vertex EmptyVertex = new Vertex("empty", Visible.NotShow);

        public enum Visible
        {
            NotShow,
            Show
        }
        // cần để duyệt đồ thị và update UI
        private bool _isVisited;
        private bool _isPending;
        private bool _isPointedTo;


        public string Title {  get; }
        public bool IsShowVertex { get; }
        public Vertex? ParentVertex { get; set; }
        public bool IsVisited
        {
            get => _isVisited;
            set => this.RaiseAndSetIfChanged(ref _isVisited, value);
        }
        public bool IsPending
        {
            get => _isPending;
            set => this.RaiseAndSetIfChanged(ref _isPending, value);
        }
        public bool IsPointedTo
        {
            get => _isPointedTo;
            set => this.RaiseAndSetIfChanged(ref _isPointedTo, value);
        }

        public Vertex(string title, Visible isShowVertex = Visible.Show)
        {
            Title = title;
            IsShowVertex = GetVisibleState(isShowVertex);
            ParentVertex = null;
            IsVisited = IsPending = IsPointedTo = false;
        }

        private bool GetVisibleState(Visible isShowVertex)
        {
            bool isVisible = true;
            switch (isShowVertex) 
            {
                case Visible.Show: isVisible = true;
                    break;
                case Visible.NotShow: isVisible = false;
                    break;
            }
            return isVisible;
        }

        public void UnSetAll()
        {
            UnSetAllState();
            ParentVertex = null;
        }

        public void UnSetAllState()
        {
            IsVisited = false;
            IsPending = false;
            IsPointedTo = false;
        }
        public void SetVitsited()
        {
            IsPending = false;
            IsVisited = true;
        }
        public void UnSetVisited()
        {
            IsVisited = false;
        }

        public void SetPending()
        {
            IsPending = true;
        }
        public void UnSetPending()
        {
            IsPending = false;
        }
        public void SetPointTo()
        {
            IsPointedTo = true;
        }
        public void UnSetPointedTo()
        {
            IsPointedTo = false;
        }

        public int Compare(Vertex v)
        {
            
            var regex = new Regex("^(d+)");

            if (Int32.TryParse(Title,out var uTitle) && Int32.TryParse(v.Title,out var vTitle))
            {
                return uTitle - vTitle;
            }

            // run the regex on both strings
            var xRegexResult = regex.Match(Title);
            var yRegexResult = regex.Match(v.Title);

            // check if they are both numbers
            if (xRegexResult.Success && yRegexResult.Success)
            {
                return int.Parse(xRegexResult.Groups[1].Value).CompareTo(int.Parse(yRegexResult.Groups[1].Value));
            }

            // otherwise return as string comparison
            return Title.CompareTo(v.Title);
            
        }

        public static Vertex CreateNewVertex(string Title)
        {
            return new Vertex(Title);
        }

        public static bool IsVertexEqual(Vertex v1, Vertex v2)
        {
            return v1.Title == v2.Title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
