﻿
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
        public bool _isVisited;
        public bool _isPending;

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

        public Vertex(string title, Visible isShowVertex = Visible.Show)
        {
            Title = title;
            IsShowVertex = GetVisibleState(isShowVertex);
            ParentVertex = null;
            IsVisited = IsPending = false;
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
