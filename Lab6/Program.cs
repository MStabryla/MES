using System;
using MES;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileDouble(args[0]);
            int integrationDegree = args.Length > 1 ?  Convert.ToInt16(args[1]) : 4;
            double W = data.numbers[0],H = data.numbers[1]; 
            int mW = Convert.ToInt16(data.numbers[2]),mH = Convert.ToInt16(data.numbers[3]);
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
            
            double c = Convert.ToDouble(data.numbers[4]), ro = Convert.ToDouble(data.numbers[5]), t0 = Convert.ToDouble(data.numbers[6]), k = Convert.ToDouble(data.numbers[7]);

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
            Func<(double,double),double>[] shapeFunc = new Func<(double, double), double>[]{
                (x) => 0.25 * (1.0 - x.Item1) * (1.0 - x.Item2),
                (x) => 0.25 * (1.0 + x.Item1) * (1.0 - x.Item2),
                (x) => 0.25 * (1.0 + x.Item1) * (1.0 + x.Item2),
                (x) => 0.25 * (1.0 - x.Item1) * (1.0 + x.Item2)
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
                solution = new ShapeFunctionsSolution(tempNodes,deriverateEpsilon,deriverateTsi,integrationDegree);
                solution.calcAll(25);
                solution.AddShapeFunctions(shapeFunc);
                solution.calcC(new double[] {c,ro});
                //solution.calcAllTriple();
                elements[i].Hmatrix = (double[,])solution.Hresult.Clone();
                elements[i].Cmatrix = (double[,])solution.Cresult.Clone();
            }
            double[,] Hglobal = new double[nodes.Length,nodes.Length]; 
            foreach(Element elem in elements)
            {
                for(int i=0;i<elem.NodeId.Length;i++)
                {
                    for(int j=0;j<elem.NodeId.Length;j++)
                    {
                        Hglobal[elem.NodeId[i],elem.NodeId[j]] += elem.Hmatrix[i,j];
                    }
                }
            }
            double[,] Cglobal = new double[nodes.Length,nodes.Length]; 
            foreach(Element elem in elements)
            {
                for(int i=0;i<elem.NodeId.Length;i++)
                {
                    for(int j=0;j<elem.NodeId.Length;j++)
                    {
                        Cglobal[elem.NodeId[i],elem.NodeId[j]] += elem.Cmatrix[i,j];
                    }
                }
            }
            /*Console.Write("\n");
            for(int i=0;i<nodes.Length;i++)
            {
                for(int j=0;j<nodes.Length;j++)
                {
                    Console.Write(Hglobal[i,j].ToString("F2") + " ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");*/
            //Console.Write(MES.StringMethods.MatrixToString(Hglobal,nodes.Length,nodes.Length));
            Console.Write(MES.StringMethods.MatrixToString(Cglobal,nodes.Length,nodes.Length));
        }
    }
}
