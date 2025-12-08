using System;
using System.Collections.Generic;

namespace MES
{
    public struct Node
    {
        public Node(double x, double y)
        {
            this.x = x; this.y = y; this.t0 = 0; this.bc = false;
        }
        public Node(double x, double y,double t0)
        {
            this.x = x; this.y = y; this.t0 = t0; this.bc = false;
        }
        public Node(double x, double y,double t0,bool bc)
        {
            this.x = x; this.y = y; this.t0 = t0; this.bc = bc;
        }
        public double x;
        public double y;

        //Lab6
        public double t0;
        public bool bc;
        public static implicit operator string(Node item)
        {
            return item.x + "," + item.y;
        }
    }
    public struct Element {

        public Element(int nodeId1,int nodeId2,int nodeId3,int nodeId4,(int,int)[] Borders,double alfa,double talfa)
        {
            NodeId = new int[] {nodeId1,nodeId2,nodeId3,nodeId4};
            Hmatrix = new double[4,4];
            Hbcmatrix = new double[4,4];
            Cmatrix = new double[4,4];
            Plocal = new double[4];
            this.Borders = Borders;
            this.alfa = alfa;
            this.talfa = talfa;
        }
        public int[] NodeId;
        public double[,] Hmatrix;
        public double[,] Hbcmatrix;
        //Lab6
        public double[,] Cmatrix;
        public double[] Plocal;
        public (int,int)[] Borders;
        public double alfa;
        public double talfa;

        public static implicit operator string(Element item)
        {
            string result = "";
            foreach(var elem in item.NodeId)
                result += elem.ToString() + " ";
            return result;
        }
    }
    public class DataFromFile<T>
    {
        public DataFromFile() { numbers = new List<T>(); vertexes = new List<T[]>(); matrixes = new List<T[,]>(); }
        public List<T> numbers;
        public List<T[]> vertexes;
        public List<T[,]> matrixes;
    }
}
