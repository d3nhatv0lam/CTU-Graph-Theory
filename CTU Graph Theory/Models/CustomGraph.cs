using AvaloniaGraphControl;
using CTU_Graph_Theory.Models;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            SimpleGraph,
            MultiGraph,
            PseudoGraph,
        };

        public enum GraphDirectType
        {
            UnDirected,
            Directed
        };

        public GraphType TypeOfGraph { get; private set; }
        public GraphDirectType DirectTypeOfGraph { get; private set; }

        public ObservableCollection<Vertex> Vertices { get; private set; }
        public int VetexCount { get => Vertices.Count; }
        public int EdgeCount
        {
            get => this.Edges.Count;
        }

        public CustomGraph() 
        {
            TypeOfGraph = GraphType.SimpleGraph;
            DirectTypeOfGraph = GraphDirectType.UnDirected;
            Vertices = new ObservableCollection<Vertex>();
        }

        public bool IsDirectedGraph() => DirectTypeOfGraph == GraphDirectType.Directed;
        public bool IsUnDirectedGraph() => DirectTypeOfGraph == GraphDirectType.UnDirected;
        public bool IsSimpleGraph() => TypeOfGraph == GraphType.SimpleGraph;
        public bool IsMultiGraph() => TypeOfGraph == GraphType.MultiGraph;
        public bool IsPseudoGraph() => TypeOfGraph == GraphType.PseudoGraph;
       

        public Vertex? GetVertex(string vertexTitle)
        {
            Vertex? u = null ;
            foreach (ShowableEdge edge in this.Edges)
            {
                u = edge.Tail as Vertex;
                if (u?.Title == vertexTitle) return u;
                u = edge.Head as Vertex;
                if (u?.Title == vertexTitle) return u;
            }
            return u;
        }

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

        public bool Adjacent(Vertex u, Vertex v)
        {
            if (GetEdge(u, v) == null) return false;
            return true;
        }

        public List<Vertex> NeighboursOfVertex(Vertex x)
        {
            List<Vertex> neighboursVertex = new List<Vertex>();
            if (x == Vertex.EmptyVertex) return neighboursVertex;

            foreach (ShowableEdge edge in this.Edges)
            {
                Vertex u = (Vertex)edge.Tail,
                        v = (Vertex)edge.Head;
                if (v == Vertex.EmptyVertex) continue;

                if (x == u) neighboursVertex.Add(v);
                else
                if (DirectTypeOfGraph == GraphDirectType.UnDirected && x == v) neighboursVertex.Add(u);
            }
            neighboursVertex.Sort((x, y) => x.Title.CompareTo(y.Title));
            return neighboursVertex;
        }

        public ShowableEdge? GetEdge(Vertex u, Vertex v)
        {   
            
            foreach (ShowableEdge edge in this.Edges)
            {
                Vertex? v1 = edge.Tail as Vertex;
                Vertex? v2 = edge.Head as Vertex;
                if (v1 == null || v2 == null) continue;

                // both 2 Direct type accept u->v
                if (Vertex.IsVertexEqual(u, v1) && Vertex.IsVertexEqual(v, v2)) return edge;

                // don't has u->v
                // is Undirected and has v->u
                if (IsUnDirectedGraph() && Vertex.IsVertexEqual(u, v2) && Vertex.IsVertexEqual(v, v1)) return edge;
                //    if (DirectTypeOfGraph == GraphDirectType.Directed)
                //{
                //     return edge;
                //    //if (u.Title == v1?.Title && v.Title == v2?.Title) return edge;
                //}
                //else
                //{
                //    if (Vertex.IsVertexEqual(u, v1) && Vertex.IsVertexEqual(v, v2))
                //    if ((u.Title == v1?.Title && v.Title == v2?.Title)
                //    || (u.Title == v2?.Title && v.Title == v1?.Title)) return edge;
                //}

            }
            // don't find
            return null;
        }

       
        public void UnVisitAndClearParentAll()
        {
            Vertex u;
            foreach(ShowableEdge edge in this.Edges)
            {
                // clean Edge
                edge.IsVisited = false;
                // clean Vertex
                if (edge.Tail != null)
                {
                    u = (Vertex)edge.Tail;
                    u.IsVisited = false;
                    u.IsPending = false;
                    u.ParentVertex = null;
                }
                if (edge.Head != null)
                {
                    u = (Vertex)edge.Head;
                    if (u == Vertex.EmptyVertex) continue;
                    u.IsVisited = false;
                    u.IsPending = false;
                    u.ParentVertex = null;
                }

            }
        }

        public static CustomGraph CreateNewGraphFromChangeGraphType(CustomGraph graph, GraphDirectType newType)
        {
            if (graph.DirectTypeOfGraph == newType) return graph;
            // create new graph
            CustomGraph newGraph = new CustomGraph();
            newGraph.DirectTypeOfGraph = newType;
            // add edge for graph
            foreach (ShowableEdge edge in graph.Edges)
            {
                ShowableEdge newEdge;
                if (newType == GraphDirectType.Directed)
                    newEdge = new ShowableEdge(edge.Tail, edge.Head, edge.VisibleState, edge.Label);
                else
                    newEdge = new ShowableEdge(edge.Tail, edge.Head, edge.VisibleState, edge.Label, Edge.Symbol.None, Edge.Symbol.None);
                newGraph.Edges.Add(newEdge);
            }
            return newGraph;
        }

        public static CustomGraph CreateNewGraphFromStringLineData(List<string> graphData,GraphDirectType type)
        {
            // create new graph
            CustomGraph newGraph = new CustomGraph();
            newGraph.DirectTypeOfGraph = type;

            // get symbol
            Edge.Symbol DirectGraphSymbol = newGraph.IsUnDirectedGraph() ? DirectGraphSymbol = Edge.Symbol.None: DirectGraphSymbol = Edge.Symbol.Arrow;
            HashSet<Vertex> set = new HashSet<Vertex>();

            foreach (var data in graphData)
            {
                string[] nodeData = data.Split(' ').Select((x) => x.Trim()).ToArray();
                
                if (nodeData.Length == 0 || nodeData.Length > 3) continue;
                // node1 - node2 - weight
                Vertex? u = null, v = null; long weight;
                ShowableEdge? newEdge = null;

                switch (nodeData.Length)
                {
                    case 1:
                        {
                            u = newGraph.GetVertex(nodeData[0]);
                            if (u != null) continue;

                            u = Vertex.CreateNewVertex(nodeData[0]);
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
            newGraph.Vertices = new ObservableCollection<Vertex>(set);
            return newGraph;
        }
    }
}
