//******************************************************************************************************
//  Node.cs
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      October 1st 2015
//      October 14th 2015
//      October 16th 2015
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstationLSE.Topology
{
    class Tree: Dictionary<string, Edge>
    {
        #region [Private Members]
        List<Vertex> m_VertexCluster = new List<Vertex>();
        List<Edge> m_Edge_List = new List<Edge>();
        public Dictionary<string, string[]> m_Edge_Vertex = new Dictionary<string, string[]>();
        public string m_Tree_ID;
        public string m_First_Vertex;
        int m_Bus_Num; 
        #endregion

        #region [Properties]
        public List<Vertex> VertexCluster
        {
            get
            {
                return m_VertexCluster;
            }
            set
            {
                m_VertexCluster = value;
            }
        }
        public List<Edge> Edge_List
        {
            get
            {
                return m_Edge_List;
            }
            set
            {
                m_Edge_List = value;
            }
        }
        #endregion


    }
}
