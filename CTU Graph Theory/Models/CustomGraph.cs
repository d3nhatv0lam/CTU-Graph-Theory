using AvaloniaGraphControl;
using CTU_Graph_Theory.Models;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CTU_Graph_Theory.Models
{
    public class CustomGraph : Graph
    {
        public enum GraphType
        {
            UnDirected,
            Directed
        };
        public GraphType _GraphType { get; private set; } = GraphType.UnDirected;

        public int VetexCount { get; private set; } = 0;
        public CustomGraph() { }


        public Vertex GetOrCreateVertex(string vertexTitle)
        {
            Vertex? u;
            // find vertex in graph
            foreach (ShowableEdge edge in this.Edges)
            {
                u = edge.Tail as Vertex;
                if (u?.Title == vertexTitle) return u;
                u = edge.Head as Vertex;
                if (u?.Title == vertexTitle) return u;
            }
            // dont see vetex => create new one
            u = new Vertex(vertexTitle);
            return u;
        }

        public ShowableEdge GetEdge(Vertex u, Vertex v)
        {
            
            foreach (ShowableEdge edge in this.Edges)
            {
                Vertex? v1 = edge.Head as Vertex;
                Vertex? v2 = edge.Tail as Vertex;

                if (_GraphType == GraphType.Directed)
                    if (u.Title == v1?.Title && v.Title == v2?.Title) return edge;
                    else
                    if ((u.Title == v1?.Title && v.Title == v2?.Title) || (u.Title == v2?.Title && v.Title == v1?.Title)) return edge;
            }
            // don't find
            return null;
        }

        public static CustomGraph CreateNewGraphFromChangeGraphType(CustomGraph graph, GraphType newType)
        {
            if (graph._GraphType == newType) return graph;

            CustomGraph newGraph = new CustomGraph();
            newGraph._GraphType = newType;
            foreach (ShowableEdge edge in graph.Edges)
            {
                ShowableEdge newEdge;
                if (newType == GraphType.Directed)
                    newEdge = new ShowableEdge(edge.Tail, edge.Head, edge.VisibleState, edge.Label);
                else
                    newEdge = new ShowableEdge(edge.Tail, edge.Head, edge.VisibleState, edge.Label, Edge.Symbol.None, Edge.Symbol.None);
                newGraph.Edges.Add(newEdge);
            }
            return newGraph;
        }

        public static CustomGraph CreateNewGraphFromStringLineData(List<string> graphData,GraphType type)
        {
            CustomGraph newGraph = new CustomGraph();
            newGraph._GraphType = type;

            // get symbol
            Edge.Symbol DirectGraphSymbol;
            if (type == GraphType.UnDirected)
                DirectGraphSymbol = Edge.Symbol.None;
            else
                DirectGraphSymbol = Edge.Symbol.Arrow;

            HashSet<Vertex> set = new HashSet<Vertex>();

            foreach (var data in graphData)
            {
                string[] nodeData = data.Split(' ').Select((x) => x.Trim()).ToArray();
                
                if (nodeData.Length == 0 || nodeData.Length > 3) continue;
                // node1 - node2 - weight
                Vertex u = null, v = null; long weight;
                ShowableEdge newEdge = null;

                switch (nodeData.Length)
                {
                    case 1:
                        {
                            u = newGraph.GetOrCreateVertex(nodeData[0]);
                            v = Vertex.EmptyVertex;
                            newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.NotShow);
                            set.Add(u);
                        }
                        break;
                    case 2:
                        {
                            u = newGraph.GetOrCreateVertex(nodeData[0]);
                            v = newGraph.GetOrCreateVertex(nodeData[1]);
                            if (u.Title == v.Title)
                            {
                                newEdge = new ShowableEdge(u, u, ShowableEdge.Visible.Show, null, Edge.Symbol.None, DirectGraphSymbol);
                                set.Add(u);
                            }
                            else
                            {
                                newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.Show, null, Edge.Symbol.None, DirectGraphSymbol);
                                set.Add(u);
                                set.Add(v);
                            }
                        }
                        break;
                    case 3:
                        {
                            u = newGraph.GetOrCreateVertex(nodeData[0]);
                            v = newGraph.GetOrCreateVertex(nodeData[1]);
                            if (Int64.TryParse(nodeData[2], out weight))
                                if (u.Title == v.Title)
                                {
                                    newEdge = new ShowableEdge(u, u, ShowableEdge.Visible.Show, weight, Edge.Symbol.None, DirectGraphSymbol);
                                    set.Add(u);
                                }
                                else
                                {
                                    newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.Show, weight, Edge.Symbol.None, DirectGraphSymbol);
                                    set.Add(u);
                                    set.Add(v);
                                }
                        }   
                        break;
                }
                if (newEdge != null) newGraph.Edges.Add(newEdge);
            }
            newGraph.VetexCount = set.Count;
            return newGraph;
        }
    }
}
