using System;
using MES;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileDouble(args[0]);
            int integrationDegree = args.Length > 1 ?  Convert.ToInt16(args[1]) : 4;
            double W = data.numbers[0],H = data.numbers[1]; 
            int mW = Convert.ToInt16(data.numbers[2]),mH = Convert.ToInt16(data.numbers[3]);
            double T0 = Convert.ToDouble(data.numbers[4]),
                t = Convert.ToDouble(data.numbers[5]),
                deltat = Convert.ToDouble(data.numbers[6]), 
                talfa = Convert.ToDouble(data.numbers[7]),
                alfa = Convert.ToDouble(data.numbers[8]),
                c = Convert.ToDouble(data.numbers[9]), 
                k = Convert.ToDouble(data.numbers[10]),
                ro = Convert.ToDouble(data.numbers[11]);
            int iterations = (int)t / (int)deltat;
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
                nodes[i] = new Node((i/mW)*dw,(i % mH)*dh,T0,bc);
            }
            for(int x=0;x<mW-1;x++)
            {
                for(int y=0;y<mH-1;y++)
                {
                    var bc = new List<(int,int)>();
                    if(x == 0) bc.Add((3,0));
                    if(x == mW-2) bc.Add((1,2));
                    if(y == 0) bc.Add((0,1));
                    if(y == mH-2) bc.Add((2,3));
                    //if(x == 0 || y == 0 || x == W || y == H)

                    elements[x*(mH-1) + y] = new Element(y+x*mH , y+x*mH+mH , y+x*mH+mH+1 , y+x*mH+1,bc.ToArray(),alfa,talfa);
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
            EquasionSolve solve;
            double[,] Hglobal;
            double[,] Cglobal; 
            double[,] Hbcglobal;
            double[] Pglobal; 
            
            for(int ti=0;ti<iterations;ti++)
            {
                //DEBUG
                double[,] HglobalDebug = new double[nodes.Length,nodes.Length];


                Hglobal = new double[nodes.Length,nodes.Length];
                Cglobal = new double[nodes.Length,nodes.Length]; 
                Hbcglobal = new double[nodes.Length,nodes.Length];
                Pglobal = new double[nodes.Length];
                string content = ""; 
                
                for(int i=0;i<elements.Length;i++)
                {
                    var elem = elements[i];
                    Node[] tempNodes = new Node[elem.NodeId.Length];
                    for(int j=0;j<elem.NodeId.Length;j++)
                    {
                        tempNodes[j] = nodes[elem.NodeId[j]];
                    }
                    solution = new ShapeFunctionsSolution(tempNodes,elem.Borders,deriverateEpsilon,deriverateTsi,integrationDegree);
                    solution.calcAll(k);
                    solution.AddShapeFunctions(shapeFunc);
                    solution.calcC(new double[] {c,ro});
                    solution.calcHbc(elem.alfa,elem.talfa);
                    elements[i].Hmatrix = (double[,])solution.Hresult.Clone();
                    elements[i].Cmatrix = (double[,])solution.Cresult.Clone();
                    elements[i].Hbcmatrix = solution.HbcLocal;
                    elements[i].Hmatrix = (DenseMatrix.OfArray(elements[i].Hmatrix) + DenseMatrix.OfArray((double[,])solution.HbcLocal.Clone())).ToArray();

                    elements[i].Plocal = (double[])solution.Plocal.Clone();
                }
                
                foreach(Element elem in elements)
                {
                    for(int i=0;i<elem.NodeId.Length;i++)
                    {
                        for(int j=0;j<elem.NodeId.Length;j++)
                        {
                            Hglobal[elem.NodeId[i],elem.NodeId[j]] += elem.Hmatrix[i,j];
                            HglobalDebug[elem.NodeId[i],elem.NodeId[j]] += elem.Hmatrix[i,j] - elem.Hbcmatrix[i,j];
                            Hbcglobal[elem.NodeId[i],elem.NodeId[j]] += elem.Hbcmatrix[i,j];
                            Cglobal[elem.NodeId[i],elem.NodeId[j]] += elem.Cmatrix[i,j];
                        }
                    }
                }
                for(int j=0;j<elements.Length;j++)
                {
                    var elem = elements[j];
                    for(int i=0;i<elem.NodeId.Length;i++)
                    {
                        Pglobal[elem.NodeId[i]] += elem.Plocal[i];
                    }
                }
                solve = new EquasionSolve(Hglobal,Cglobal,Pglobal,deltat);
                //double[] nextT0 = solve.Compute(nodes);
                solve.GenerateT0(nodes);
                solve.GenerateTempH();
                solve.GenerateTempP();
                double[] nextT0 = solve.SolveEquasion();
                double nextT0min = double.MaxValue,nextT0max = double.MinValue;
                for(int i=0;i<nodes.Length;i++)
                {
                    if(nextT0min > nextT0[i]) nextT0min = nextT0[i];
                    if(nextT0max < nextT0[i]) nextT0max = nextT0[i];
                    nodes[i].t0 = nextT0[i];
                }
                Console.WriteLine("Iteracja: " + ti + "\tmin:\t" + nextT0min + "\tmax:\t" + nextT0max); 
                
                content += "Hglobal" + "\n";
                content += MES.StringMethods.MatrixToString(HglobalDebug,nodes.Length,nodes.Length);
                content += "Hbcglobal" + "\n";
                content += MES.StringMethods.MatrixToString(Hbcglobal,nodes.Length,nodes.Length);
                content += "Hglobal [H + C/dT]" + "\n";
                content += MES.StringMethods.MatrixToString(solve.TempH,nodes.Length,nodes.Length);
                content += "Cglobal" + "\n";
                content += MES.StringMethods.MatrixToString(Cglobal,nodes.Length,nodes.Length);
                content += "Pglobal" + "\n";
                content += MES.StringMethods.VectorToString(Pglobal,nodes.Length);
                content += "Pglobal [P + C/dT] * T0" + "\n";
                content += MES.StringMethods.VectorToString(solve.TempP,nodes.Length);
                ReadingFile.DataToFile("..\\resdata\\it" + ti + ".txt",content);
                //Console.Write(MES.StringMethods.VectorToString(nextT0,nodes.Length));
            }
            
            /*Console.WriteLine("Hglobal");
            Console.Write(MES.StringMethods.MatrixToString(Hglobal,nodes.Length,nodes.Length));
            Console.WriteLine("Cglobal");
            Console.Write(MES.StringMethods.MatrixToString(Cglobal,nodes.Length,nodes.Length));
            Console.WriteLine("Pglobal");
            Console.Write(MES.StringMethods.VectorToString(Pglobal,nodes.Length));*/
        }
    }
}
