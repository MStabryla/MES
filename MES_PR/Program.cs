using System;
using MES;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

namespace MES_PR
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath =  args.Length > 0 ? args[0] : "./data.txt";
            var data = ReadingFile.ReadFileDouble(filepath);
            int integrationDegree = args.Length > 1 ?  Convert.ToInt16(args[1]) : 4;
            double W = data.numbers[0],H = data.numbers[1]; 
            int mW = Convert.ToInt16(data.numbers[2]),mH = Convert.ToInt16(data.numbers[3]);
            double T0 = Convert.ToDouble(data.numbers[4]),
                t = Convert.ToDouble(data.numbers[5]),
                deltat = Convert.ToDouble(data.numbers[6]), 
                talfa = Convert.ToDouble(data.numbers[7]),
                talfaAir = Convert.ToDouble(data.numbers[8]),
                alfa = Convert.ToDouble(data.numbers[9]),
                vAir = Convert.ToDouble(data.numbers[10]),
                c = Convert.ToDouble(data.numbers[11]), 
                k = Convert.ToDouble(data.numbers[12]),
                ro = Convert.ToDouble(data.numbers[13]);
            int iterations = (int)t / (int)deltat;
            
            
            //roAir - gęstość powietrza, visAir - współczynnik lepkości powietrza,cAir - ciepło właściwe powietrza
            double roAir,visAir,cAir,C,a,b,d,Re,Pr,Nu,realAlfaAir = 0.0;
            if(vAir != 0.0)
            {
                roAir = 101325 /(287.05 * (273.5 + talfaAir));
                visAir = 17.080 * ((273.0 + 112.0)/(talfaAir + 112.0)) * Math.Pow(talfaAir/273.0,3.0/2.0);
                cAir = 1005.0;
                d = (mH - 3.0)*H;
                C = 0.332;a = 0.6;b = 0.33;
                Re = (vAir * d * roAir)/visAir;
                Pr = (cAir * visAir)/k;
                Nu = C * Math.Pow(Re,a) * Math.Pow(Pr,b);
                realAlfaAir = (Nu * k)/d;
            }
            else
            {
                roAir = 101325 /(287.05 * (273.5 + talfaAir));
                visAir = 17.080 * ((273.0 + 112.0)/(talfaAir + 112.0)) * Math.Pow(talfaAir/273.0,3.0/2.0);
                cAir = 1005.0;
                d = (mH - 3.0)*H;
                double g = 9.81,dt = talfaAir - T0;
                double beta = 1.0 / talfaAir;
                double Gr = g * Math.Pow(d,3) * Math.Pow(roAir,2)/(Math.Pow(visAir,2)) * beta * dt;
                Pr = (cAir * visAir)/k;
                double n = 1.0;
                if((Gr*Pr) < 50.0)
                    { C = 1.18; n = 1.0/8.0; }
                else if((Gr*Pr) < 2.0 * Math.Pow(10,7))
                    { C = 0.54; n = 1.0/4.0; }
                else
                    { C = 0.135; n = 1.0/3.0; }
                Nu = C * Math.Pow(Gr*Pr,n);
                realAlfaAir = (Nu * k)/d;
            }

            GenRadiatorFEM model = new GenRadiatorFEM(W,H,mW,mH,mH - 3);
            model.Generate(alfa,realAlfaAir,talfa,talfaAir,T0);

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
            
            string content = ""; 
            double nextT0min = double.MaxValue,nextT0max = double.MinValue;
            for(int ti=0;ti<iterations;ti++)
            {
                //DEBUG
                double[,] HglobalDebug = new double[model.nodes.Length,model.nodes.Length];


                Hglobal = new double[model.nodes.Length,model.nodes.Length];
                Cglobal = new double[model.nodes.Length,model.nodes.Length]; 
                Hbcglobal = new double[model.nodes.Length,model.nodes.Length];
                Pglobal = new double[model.nodes.Length];
                
                
                for(int i=0;i<model.elements.Length;i++)
                {
                    var elem = model.elements[i];
                    Node[] tempNodes = new Node[elem.NodeId.Length];
                    for(int j=0;j<elem.NodeId.Length;j++)
                    {
                        tempNodes[j] = model.nodes[elem.NodeId[j]];
                    }
                    solution = new ShapeFunctionsSolution(tempNodes,elem.Borders,deriverateEpsilon,deriverateTsi,integrationDegree);
                    solution.calcAll(k);
                    solution.AddShapeFunctions(shapeFunc);
                    solution.calcC(new double[] {c,ro});
                    solution.calcHbc(elem.alfa,elem.talfa);
                    model.elements[i].Hmatrix = (double[,])solution.Hresult.Clone();
                    model.elements[i].Cmatrix = (double[,])solution.Cresult.Clone();
                    model.elements[i].Hbcmatrix = solution.HbcLocal;
                    model.elements[i].Hmatrix = (DenseMatrix.OfArray(model.elements[i].Hmatrix) + DenseMatrix.OfArray((double[,])solution.HbcLocal.Clone())).ToArray();

                    model.elements[i].Plocal = (double[])solution.Plocal.Clone();
                }
                
                foreach(Element elem in model.elements)
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
                for(int j=0;j<model.elements.Length;j++)
                {
                    var elem = model.elements[j];
                    for(int i=0;i<elem.NodeId.Length;i++)
                    {
                        Pglobal[elem.NodeId[i]] += elem.Plocal[i];
                    }
                }
                solve = new EquasionSolve(Hglobal,Cglobal,Pglobal,deltat);
                //double[] nextT0 = solve.Compute(nodes);
                solve.GenerateT0(model.nodes);
                solve.GenerateTempH();
                solve.GenerateTempP();
                double[] nextT0 = solve.SolveEquasion();
                double T0Sum = 0.0;
                int actNodes = 0;
                nextT0min = double.MaxValue;nextT0max = double.MinValue;
                for(int i=0;i<model.nodes.Length;i++)
                {
                    if(nextT0min > nextT0[i] && nextT0[i] >= T0) nextT0min = nextT0[i];
                    if(nextT0max < nextT0[i]) nextT0max = nextT0[i];
                    if(nextT0[i] >= T0){
                        T0Sum += nextT0[i]; actNodes++;
                    }
                        
                    model.nodes[i].t0 = nextT0[i];
                }
                double avg = (T0Sum/actNodes );
                if(ti == iterations - 1)
                    Console.WriteLine("Iteracja: " + ti + "\tmin:\t" + nextT0min.ToString("F3") + "\tmax:\t" + nextT0max.ToString("F3") + "\tavg:\t" + avg.ToString("F3")); 
                content += ti + "\t" + nextT0min.ToString("F3") + "\t" + nextT0max.ToString("F3") + "\t" + avg.ToString("F3") + "\n";
            }
            MES.StringMethods.RadiatorWrite(model.nodes,model.HNodesLength,model.WNodesLength,nextT0max,nextT0min,model.h);
            string resultFile = args.Length > 2 ? args[2] : "result.txt";
            ReadingFile.DataToFile(resultFile,content);
        }
    }
}
