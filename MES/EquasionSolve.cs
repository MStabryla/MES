using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MES
{
    public class EquasionSolve
    {
        private double[,] H;
        private double[,] C;
        private double[] P;
        private int dimention;
        public EquasionSolve(double[,] _H,double[,] _C,double[] _P,double tau){
            H = _H;C = _C;P = _P;this.tau = tau;
            dimention = P.Length;
        }
        public double tau;
        public double[,] TempH;
        public double[] TempP;
        public double[] T1;


        public double[] Compute(Node[] nodes){
            GenerateT0(nodes);
            GenerateTempH();
            GenerateTempP();
            return SolveEquasion();
        }
        public void GenerateT0(Node[] nodes){
            T1 = new double[dimention];
            for(int i=0;i<nodes.Length;i++)
            {
                T1[i] = nodes[i].t0;
            }
        }
        public void GenerateTempH(){
            TempH = new double[dimention,dimention];
            for(int i=0;i<dimention;i++)
            {
                for(int j=0;j<dimention;j++)
                {
                    TempH[i,j] = H[i,j] + C[i,j]/tau;
                }
            }
        }
        public void GenerateTempP(){
            double[,] TempC = new double[dimention,dimention];
            for(int i=0;i<dimention;i++)
            {
                for(int j=0;j<dimention;j++)
                {
                    TempC[i,j] = C[i,j] / tau;
                }
            }
            Matrix<double> matrixTempC = DenseMatrix.OfArray(TempC);
            Vector<double> vecTempP = DenseVector.OfArray(P);
            Vector<double> vecT1 = DenseVector.OfArray(T1);

            var result = vecTempP - matrixTempC * vecT1;
            TempP = result.AsArray();
        }
        public double[] SolveEquasion(){
            Matrix<double> matrixTempH = DenseMatrix.OfArray(TempH);
            Vector<double> vecTempP = DenseVector.OfArray(TempP);
            vecTempP *= -1;
            var result = vecTempP * matrixTempH.Inverse();
            return result.AsArray();
        }
    }
}