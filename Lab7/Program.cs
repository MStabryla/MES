using System;
using MES;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileDouble(args[0]);
            int integrationDegree = args.Length > 1 ?  Convert.ToInt16(args[1]) : 4;
            double W = data.numbers[0],H = data.numbers[1]; 
            int mW = Convert.ToInt16(data.numbers[2]),mH = Convert.ToInt16(data.numbers[3]);
            double c = Convert.ToDouble(data.numbers[4]), ro = Convert.ToDouble(data.numbers[5]), t0 = Convert.ToDouble(data.numbers[6]), k = Convert.ToDouble(data.numbers[7]), alfa = Convert.ToDouble(data.numbers[8]),talfa = Convert.ToDouble(data.numbers[9]);
            double dw = W / (mW-1);
            double dh = H / (mH-1);
            Node[] nodes = new Node[mW * mH];
            Element[] elements = new Element[(mW-1) * (mH-1)];
            
            for(int i=0;i<nodes.Length;i++)
            {
                //int id = i + 1;
                bool bc = false;
                if((i/mW)*dw == 0 || (i/mW)*dw == W || (i % mH)*dh == 0 || (i % mH)*dh == H)
                    bc = true;
                nodes[i] = new Node((i/mW)*dw,(i % mH)*dh,t0,bc);
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
                Node[] tempNodes = new Node[elem.NodeId.Length];
                for(int j=0;j<elem.NodeId.Length;j++)
                {
                    tempNodes[j] = nodes[elem.NodeId[j]];
                }
                solution = new ShapeFunctionsSolution(tempNodes,deriverateEpsilon,deriverateTsi,integrationDegree);
                solution.calcAll(k);
                solution.AddShapeFunctions(shapeFunc);
                solution.calcC(new double[] {c,ro});
                solution.calcHbc(alfa,talfa);
                //solution.calcAllTriple();
                elements[i].Hmatrix = (double[,])solution.Hresult.Clone();
                elements[i].Cmatrix = (double[,])solution.Cresult.Clone();
                elements[i].Hmatrix = (DenseMatrix.OfArray(elements[i].Hmatrix) + DenseMatrix.OfArray((double[,])solution.HbcLocal.Clone())).ToArray();
                elements[i].Plocal = (double[])solution.Plocal.Clone();
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
            double[] Pglobal = new double[nodes.Length]; 
            for(int j=0;j<elements.Length;j++)
            {
                var elem = elements[j];
                for(int i=0;i<elem.NodeId.Length;i++)
                {
                    Pglobal[elem.NodeId[i]] += elem.Plocal[i];
                }
            }
            Console.WriteLine("Hglobal");
            Console.Write(MES.StringMethods.MatrixToString(Hglobal,nodes.Length,nodes.Length));
            Console.WriteLine("Cglobal");
            Console.Write(MES.StringMethods.MatrixToString(Cglobal,nodes.Length,nodes.Length));
            Console.WriteLine("Pglobal");
            Console.Write(MES.StringMethods.VectorToString(Pglobal,nodes.Length));
        }
    }
}
