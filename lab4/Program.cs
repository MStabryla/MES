using System;
using MES;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileInteger(args[0]);
            int W = data.numbers[0],H = data.numbers[1], mW = data.numbers[2],mH = data.numbers[3];
            double dw = W / (mW-1);
            double dh = H / (mH-1);
            Node[] nodes = new Node[mW * mH];
            Element[] elements = new Element[(mW-1) * (mH-1)];
            for(int i=0;i<nodes.Length;i++)
            {
                //int id = i + 1;
                nodes[i] = new Node((i/mH)*dw,(i % mH)*dh);
            }
            for(int x=0;x<mW-1;x++)
            {
                for(int y=0;y<mH-1;y++)
                {
                    elements[x*(mH-1) + y] = new Element(y+x*mH , y+x*mH+mH , y+x*mH+mH+1 , y+x*mH+1);
                }
            }

            Func<(double,double),double>[] deriverateEpsilon = new Func<(double, double), double>[]{
                (x) => -0.25 * (1.0 - x.Item2),
                (x) => 0.25 * (1.0 - x.Item2),
                (x) => 0.25 * (1.0 + x.Item2),
                (x) => -0.25 * (1.0 + x.Item2)
            };
            Func<(double,double),double>[] deriverateTsi = new Func<(double, double), double>[]{
                (x) => -0.25 * (1.0 + x.Item1),
                (x) => -0.25 * (1.0 - x.Item1),
                (x) => 0.25 * (1.0 - x.Item1),
                (x) => 0.25 * (1.0 + x.Item1),
            };
            ShapeFunctionsSolution solution;
            for(int i=0;i<elements.Length;i++)
            {
                var elem = elements[i];
                (double,double)[] tempNodes = new (double, double)[4];
                for(int j=0;j<elem.NodeId.Length;j++)
                {
                    Node tempNode = nodes[elem.NodeId[j]];
                    tempNodes[j] = (tempNode.x,tempNode.y);
                }
                solution = new ShapeFunctionsSolution(tempNodes,deriverateEpsilon,deriverateTsi);
                solution.calcAll(25);
                elements[i].Hmatrix = (double[,])solution.Hresult.Clone();
            }
            foreach(var elem in elements)
            {
                Console.Write("\n");
                for(int i=0;i<4;i++)
                {
                    for(int j=0;j<4;j++)
                    {
                        Console.Write(elem.Hmatrix[i,j].ToString("F2") + " ");
                    }
                    Console.Write("\n");
                }
                Console.Write("\n");
            }
            Console.WriteLine("End");
        }
    }
}
