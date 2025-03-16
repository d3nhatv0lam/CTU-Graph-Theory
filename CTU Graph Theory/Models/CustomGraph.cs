using AvaloniaGraphControl;
using CTU_Graph_Theory.Models;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

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
        public int EdgeCount { get; private set; }
        public bool IsWeightGraph { get; private set; }
        public bool IsHasNegativeWeight { get; private set; }

        public CustomGraph() 
        {
            TypeOfGraph = GraphType.SimpleGraph;
            DirectTypeOfGraph = GraphDirectType.UnDirected;
            EdgeCount = 0;
            IsWeightGraph = false;
            Vertices = new ObservableCollection<Vertex>();
        }

        public bool IsDirectedGraph() => DirectTypeOfGraph == GraphDirectType.Directed;
        public bool IsUnDirectedGraph() => DirectTypeOfGraph == GraphDirectType.UnDirected;
        public bool IsSimpleGraph() => TypeOfGraph == GraphType.SimpleGraph;
        public bool IsMultiGraph() => TypeOfGraph == GraphType.MultiGraph;
        public bool IsPseudoGraph() => TypeOfGraph == GraphType.PseudoGraph;
       

        public Vertex? GetVertex(string vertexTitle)
        {
            Vertex? u = null;
            foreach (ShowableEdge edge in this.Edges)
            {
                u = edge.Tail as Vertex;
                if (u?.Title == vertexTitle) return u;
                u = edge.Head as Vertex;
                if (u?.Title == vertexTitle) return u;
            }
            return null;
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

                if (Vertex.IsVertexEqual(x,u)) neighboursVertex.Add(v);
                else
                if (DirectTypeOfGraph == GraphDirectType.UnDirected && Vertex.IsVertexEqual(x, v)) neighboursVertex.Add(u);
            }
            neighboursVertex.Sort((x, y) => x.CompareTo(y));
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
            }
            // don't find
            return null;
        }

        public ShowableEdge? GetEdgeWithMinWeight(Vertex? u, Vertex? v)
        {
            if (u == null || v == null) return null;
            if (!IsWeightGraph) return null;

            List < ShowableEdge > edgeList = GetAllEdges(u, v);
            ShowableEdge? edge = edgeList.MinBy(x => (Int64)x.Label);
            return edge;
        }

        public List<ShowableEdge> GetAllEdges(Vertex? u,Vertex? v)
        {
            List<ShowableEdge> edges = new List<ShowableEdge>();
            if (u == null || v == null) return edges;

            foreach (ShowableEdge edge in this.Edges)
            {
                Vertex? v1 = edge.Tail as Vertex;
                Vertex? v2 = edge.Head as Vertex;
                if (v1 == null || v2 == null) continue;

                // both 2 Direct type accept u->v
                if (Vertex.IsVertexEqual(u, v1) && Vertex.IsVertexEqual(v, v2)) edges.Add(edge);

                // don't has u->v
                // is Undirected and has v->u
                if (IsUnDirectedGraph() && Vertex.IsVertexEqual(u, v2) && Vertex.IsVertexEqual(v, v1)) edges.Add(edge);
            }
            return edges;
        }

        public void ColoredAllEdgeOfVertices(List<Vertex> vertices)
        {
            HashSet<Vertex> vertexSet = new(vertices);

            foreach (ShowableEdge edge in this.Edges)
            {
                if (vertexSet.Contains(edge.Head) && vertexSet.Contains(edge.Tail))
                {
                    edge.IsVisited = true;
                }
            }
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
                    u.UnSetAll();
                }
                if (edge.Head != null)
                {
                    u = (Vertex)edge.Head;
                    if (u == Vertex.EmptyVertex) continue;
                    u.UnSetAll();
                }

            }
        }

        public static CustomGraph CreateNewGraphFromChangeGraphType(CustomGraph graph, GraphDirectType newType)
        {
            if (graph.DirectTypeOfGraph == newType) return graph;
            // create new graph
            CustomGraph newGraph = new CustomGraph();
            newGraph.DirectTypeOfGraph = newType;
            newGraph.TypeOfGraph = graph.TypeOfGraph;
            newGraph.Vertices = new ObservableCollection<Vertex>(graph.Vertices.Select(vertex => vertex));
            newGraph.EdgeCount = graph.EdgeCount;
            newGraph.IsWeightGraph = graph.IsWeightGraph;
            newGraph.IsHasNegativeWeight = graph.IsHasNegativeWeight;
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
            newGraph.TypeOfGraph = GraphType.SimpleGraph;
            newGraph.DirectTypeOfGraph = type;

            // get symbol
            Edge.Symbol DirectGraphSymbol = newGraph.IsUnDirectedGraph() ? DirectGraphSymbol = Edge.Symbol.None: DirectGraphSymbol = Edge.Symbol.Arrow;
            HashSet<Vertex> set = new HashSet<Vertex>();
            int edgeCount = 0;
            bool isHasNegativeWeight = false;

            foreach (var data in graphData)
            {
                string[] nodeData = data.Split(' ').Select((x) => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                
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
                            newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.NotShow,"empty");
                            set.Add(u);
                        }
                        break;
                    case 2:
                        {
                            u = newGraph.GetOrCreateVertex(nodeData[0]);
                            v = newGraph.GetOrCreateVertex(nodeData[1]);
                            var emptyVertexEdge = newGraph.GetEdge(u, Vertex.EmptyVertex);
                            if (emptyVertexEdge != null) newGraph.Edges.Remove(emptyVertexEdge);

                            var hasEdge = newGraph.GetEdge(u, v);
                            // chỉ kiểm tra đa hướng khi là simplegraph
                            if (hasEdge != null && newGraph.IsSimpleGraph()) newGraph.TypeOfGraph = GraphType.MultiGraph;

                            if (u.Title == v.Title)
                            {
                                newEdge = new ShowableEdge(u, u, ShowableEdge.Visible.Show, null, Edge.Symbol.None, DirectGraphSymbol);
                                set.Add(u);
                                if (!newGraph.IsPseudoGraph()) newGraph.TypeOfGraph = GraphType.PseudoGraph;
                            }
                            else
                            {
                                newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.Show, null, Edge.Symbol.None, DirectGraphSymbol);
                                set.Add(u);
                                set.Add(v);
                            }
                            edgeCount++;
                        }
                        break;
                    case 3:
                        {
                            if (Int64.TryParse(nodeData[2], out weight))
                            {
                                u = newGraph.GetOrCreateVertex(nodeData[0]);
                                v = newGraph.GetOrCreateVertex(nodeData[1]);
                                var emptyVertexEdge = newGraph.GetEdge(u, Vertex.EmptyVertex);
                                if (emptyVertexEdge != null) newGraph.Edges.Remove(emptyVertexEdge);

                                var hasEdge = newGraph.GetEdge(u, v);
                                // chỉ kiểm tra đa hướng khi là simplegraph
                                if (hasEdge != null && newGraph.IsSimpleGraph()) newGraph.TypeOfGraph = GraphType.MultiGraph;
                                isHasNegativeWeight = isHasNegativeWeight  || (weight < 0);
                                if (u.Title == v.Title)
                                {
                                    newEdge = new ShowableEdge(u, u, ShowableEdge.Visible.Show, weight, Edge.Symbol.None, DirectGraphSymbol);
                                    set.Add(u);
                                    if (!newGraph.IsPseudoGraph()) newGraph.TypeOfGraph = GraphType.PseudoGraph;
                                }
                                else
                                {
                                    newEdge = new ShowableEdge(u, v, ShowableEdge.Visible.Show, weight, Edge.Symbol.None, DirectGraphSymbol);
                                    set.Add(u);
                                    set.Add(v);
                                }
                                edgeCount++;
                            }   
                        }   
                        break;
                }
                if (newEdge != null) newGraph.Edges.Add(newEdge);
            }
            //sort Vertex List
            var vertexList = set.ToList();
            vertexList.Sort((x,y) => x.CompareTo(y));

            newGraph.Vertices = new ObservableCollection<Vertex>(vertexList);
            newGraph.EdgeCount = edgeCount;
            newGraph.IsWeightGraph = newGraph.Edges.All(x => (x.Label as string) != string.Empty);
            newGraph.IsHasNegativeWeight = isHasNegativeWeight;
            return newGraph;
        }
    }
}
