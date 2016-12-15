using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstationLSE.Topology
{
    class TopologyProcessor: Dictionary<string, Tree>
    {
        Dictionary<string, string> dic_vertex_tree = new Dictionary<string, string>();
        Dictionary<string, string> dic_edge_tree = new Dictionary<string, string>();
        string tree_count = "1"; 

        public void Append(KeyValuePair<string,string[]> edge_vertex)
        {
            string edge = edge_vertex.Key;
            string vertex1 = edge_vertex.Value[0];
            string vertex2 = edge_vertex.Value[1];
            if (dic_vertex_tree.ContainsKey(vertex1) && dic_vertex_tree.ContainsKey(vertex2))
            {
                string tree1 = dic_vertex_tree[vertex1];
                string tree2 = dic_vertex_tree[vertex2];
                if (tree1 == tree2)
                {
                    this[tree1].m_Edge_Vertex.Add(edge_vertex.Key, edge_vertex.Value);
                    this[tree1][vertex1].Add(edge);
                    this[tree1][vertex2].Add(edge);
                    dic_edge_tree.Add(edge, tree1);
                }
                else
                {
                    Merge(tree1, tree2, vertex1, vertex2, edge);
                }
            }
            else if (dic_vertex_tree.ContainsKey(vertex1))
            {
                string tree = dic_vertex_tree[vertex1];
                this[tree][vertex1].Add(edge);
                Edge edge_new = new Edge();
                edge_new.Add(edge);
                this[tree].Add(vertex2, edge_new);
                if (string.Compare(vertex2,this[tree].m_First_Vertex)<0)
                {
                    this[tree].m_First_Vertex = vertex2; 
                }
                dic_vertex_tree.Add(vertex2, tree);
                dic_edge_tree.Add(edge, tree);
                this[tree].m_Edge_Vertex.Add(edge, edge_vertex.Value);
            }
            else if (dic_vertex_tree.ContainsKey(vertex2))
            {
                string tree = dic_vertex_tree[vertex2];
                this[tree][vertex2].Add(edge);
                Edge edge_new = new Edge();
                edge_new.Add(edge);
                this[tree].Add(vertex1, edge_new);
                if (string.Compare(vertex1, this[tree].m_First_Vertex) < 0)
                {
                    this[tree].m_First_Vertex = vertex1;
                }
                dic_vertex_tree.Add(vertex1, tree);
                dic_edge_tree.Add(edge, tree);
                this[tree].m_Edge_Vertex.Add(edge, edge_vertex.Value);
            }
            else
            {
                string tree_id = tree_count;
                Tree tree = new Tree();
                tree.m_Tree_ID = tree_id;
                if (string.Compare(vertex1,vertex2)>0)
                {
                    tree.m_First_Vertex = vertex2;
                }
                else
                {
                    tree.m_First_Vertex = vertex1;
                }
                Edge edge1_new = new Edge();
                Edge edge2_new = new Edge();
                edge1_new.Add(edge);
                edge2_new.Add(edge);
                tree.Add(vertex1, edge1_new);
                tree.Add(vertex2, edge2_new);
                tree.m_Edge_Vertex.Add(edge_vertex.Key, edge_vertex.Value);
                dic_vertex_tree.Add(vertex1, tree_id);
                dic_vertex_tree.Add(vertex2, tree_id);
                dic_edge_tree.Add(edge, tree_id);
                Add(tree_id, tree);
                tree_count = Convert.ToString(Convert.ToInt32(tree_count) + 1);
            }
        }

        void Merge(string tree1, string tree2, string vertex1, string vertex2, string edge)
        {
            foreach(string vertex in this[tree2].Keys)
            {
                dic_vertex_tree[vertex] = tree1;
                if (vertex == vertex2)
                {
                    this[tree2][vertex].Add(edge);
                }
                this[tree1].Add(vertex, this[tree2][vertex]);
            }
            foreach(KeyValuePair<string,string[]> edge_node in this[tree2].m_Edge_Vertex)
            {
                dic_edge_tree[edge_node.Key] = tree1;
                this[tree1].m_Edge_Vertex.Add(edge_node.Key, edge_node.Value);
            }
            this[tree1][vertex1].Add(edge);
            string[] vertexes = { vertex1, vertex2 };
            this[tree1].m_Edge_Vertex.Add(edge, vertexes);
            if (string.Compare(this[tree1].m_First_Vertex,this[tree2].m_First_Vertex)>0)
            {
                this[tree1].m_First_Vertex = this[tree2].m_First_Vertex;
            }
            Remove(tree2);
        }
    }
}
