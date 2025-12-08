
using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;
using System.Collections.Generic;

namespace MES
{
    public class ShapeFunctionsSolution
    {
        private static (double,double)[] integrationDoubleMethodElements = {
            (-1.0/Math.Sqrt(3.0), -1.0/Math.Sqrt(3.0)),
            (1.0/Math.Sqrt(3.0), -1.0/Math.Sqrt(3.0)),
            (1.0/Math.Sqrt(3.0), 1.0/Math.Sqrt(3.0)),
            (-1.0/Math.Sqrt(3.0), 1.0/Math.Sqrt(3.0))
        };
        private static double[] integrationDoubleMethodElementsScale = {
            1,1
        };
        private static (double,double)[] integrationTripleMethodElements = {
            (-Math.Sqrt(3.0/5.0), -Math.Sqrt(3.0/5.0)),
            (-Math.Sqrt(3.0/5.0), 0.0),
            (-Math.Sqrt(3.0/5.0), Math.Sqrt(3.0/5.0)),
            (0.0, -Math.Sqrt(3.0/5.0)),
            (0.0, 0.0),
            (0.0, Math.Sqrt(3.0/5.0)),
            (Math.Sqrt(3.0/5.0), -Math.Sqrt(3.0/5.0)),
            (Math.Sqrt(3.0/5.0), 0.0),
            (Math.Sqrt(3.0/5.0), Math.Sqrt(3.0/5.0)),
        };
        private static double[] integrationTripleMethodElementsScale = {
            5.0/9.0,
            8.0/9.0,
            5.0/9.0
        };
        private static (double,double)[] integrationQuadraMethodElements = {
            (-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),

            (-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),

            (Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),

            (Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),-Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0))),
            (Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))),

        };
        private static double[] integrationQuadraMethodElementsScale = {
            (18.0 - Math.Sqrt(30.0))/36.0,
            (18.0 + Math.Sqrt(30.0))/36.0,
            (18.0 + Math.Sqrt(30.0))/36.0,
            (18.0 - Math.Sqrt(30.0))/36.0
        };

        private static double[] integrationDoubleMethodElements1D = {
            -1.0/Math.Sqrt(3.0), 1.0/Math.Sqrt(3.0)
        };
        private static double[] integrationTripleMethodElements1D = {
            -Math.Sqrt(3.0/5.0), 0.0, Math.Sqrt(3.0/5.0)
        };
        private static double[] integrationQuadraMethodElements1D = {
            -Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0)),
            -Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),
            Math.Sqrt(3.0/7.0 - (2.0/7.0) * Math.Sqrt(6.0/5.0)),
            Math.Sqrt(3.0/7.0 + (2.0/7.0) * Math.Sqrt(6.0/5.0))
        };
        Func<(double,double),double>[] deriverateFuncEpsilon;
        Func<(double,double),double>[] deriverateFuncTsi;
        Func<(double,double),double>[] shapeFunc;
        public ShapeFunctionsSolution((double,double)[] _baseNodes,Func<(double,double),double>[] _deriverateFuncEpsilon,Func<(double,double),double>[] _deriverateFuncTsi)
        {
            this.baseNodes = _baseNodes;
            deriverateFuncEpsilon = _deriverateFuncEpsilon;
            deriverateFuncTsi = _deriverateFuncTsi;
            epsilonDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            tsiDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            jacobianStructure = new (double[,],double,double[,])[ActIntegrationMethodElements.Length];
            shapeResultMatrix = new (double,double)[ActIntegrationMethodElements.Length,baseNodes.Length];
            Hresult = new double[baseNodes.Length,baseNodes.Length];
        }
        public ShapeFunctionsSolution((double,double)[] _baseNodes,Func<(double,double),double>[] _deriverateFuncEpsilon,Func<(double,double),double>[] _deriverateFuncTsi,int integrateDegree)
        {
            integrateGaussMethodDegree = integrateDegree;
            this.baseNodes = _baseNodes;
            deriverateFuncEpsilon = _deriverateFuncEpsilon;
            deriverateFuncTsi = _deriverateFuncTsi;
            epsilonDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            tsiDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            CshapeMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            jacobianStructure = new (double[,],double,double[,])[ActIntegrationMethodElements.Length];
            CJacobianMatrix = new (double[,],double,double[,])[ActIntegrationMethodElements.Length];
            shapeResultMatrix = new (double,double)[ActIntegrationMethodElements.Length,baseNodes.Length];
            CShapeResultMatrix = new (double,double)[ActIntegrationMethodElements.Length,baseNodes.Length];
            Hresult = new double[baseNodes.Length,baseNodes.Length];
            Cresult = new double[baseNodes.Length,baseNodes.Length];
            Borderbc = new Dictionary<(int,int),double[,]>();
            Plocal = new double[baseNodes.Length];
        }
        public ShapeFunctionsSolution(Node[] _baseNodes,(int,int)[] _Borders,Func<(double,double),double>[] _deriverateFuncEpsilon,Func<(double,double),double>[] _deriverateFuncTsi,int integrateDegree)
        {
            integrateGaussMethodDegree = integrateDegree;
            baseNodesS = _baseNodes;
            this.baseNodes = _baseNodes.Select(x => (x.x,x.y)).ToArray();
            deriverateFuncEpsilon = _deriverateFuncEpsilon;
            deriverateFuncTsi = _deriverateFuncTsi;
            epsilonDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            tsiDeriverateMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            CshapeMatrix = new double[ActIntegrationMethodElements.Length,baseNodes.Length];
            jacobianStructure = new (double[,],double,double[,])[ActIntegrationMethodElements.Length];
            CJacobianMatrix = new (double[,],double,double[,])[ActIntegrationMethodElements.Length];
            shapeResultMatrix = new (double,double)[ActIntegrationMethodElements.Length,baseNodes.Length];
            CShapeResultMatrix = new (double,double)[ActIntegrationMethodElements.Length,baseNodes.Length];
            Hresult = new double[baseNodes.Length,baseNodes.Length];
            Cresult = new double[baseNodes.Length,baseNodes.Length];
            Borders = _Borders;
            Borderbc = new Dictionary<(int,int),double[,]>();
            HbcLocal = new double[baseNodes.Length,baseNodes.Length];
            Plocal = new double[baseNodes.Length];
        }

        public int integrateGaussMethodDegree = 2;
        public (double,double)[] ActIntegrationMethodElements {
            get {
                switch(integrateGaussMethodDegree){
                    case 2:
                        return integrationDoubleMethodElements;
                    case 3:
                        return integrationTripleMethodElements;
                    case 4:
                        return integrationQuadraMethodElements;
                    default:
                        return integrationDoubleMethodElements;
                }
            }
        }

        public double[] ActIntegrationMethodElementsScale {
            get {
                switch(integrateGaussMethodDegree){
                    case 2:
                        return integrationDoubleMethodElementsScale;
                    case 3:
                        return integrationTripleMethodElementsScale;
                    case 4:
                        return integrationQuadraMethodElementsScale;
                    default:
                        return integrationDoubleMethodElementsScale;
                }
            }
        }

        public double[] ActIntegrationMethodElements1D {
            get{
                switch(integrateGaussMethodDegree){
                    case 2:
                        return integrationDoubleMethodElements1D;
                    case 3:
                        return integrationTripleMethodElements1D;
                    case 4:
                        return integrationQuadraMethodElements1D;
                    default:
                        return integrationDoubleMethodElements1D;
                }
            }
        }
        public (double,double)[] baseNodes;
        public Node[] baseNodesS;
        
        public double[,] epsilonDeriverateMatrix;
        public double[,] tsiDeriverateMatrix;
        public (double[,],double,double[,])[] jacobianStructure;
        public (double,double)[,] shapeResultMatrix;
        public double[,] Hresult;
        public double[,] CshapeMatrix;
        public (double[,],double,double[,])[] CJacobianMatrix;
        public (double,double)[,] CShapeResultMatrix;
        public double[,] Cresult;
        public (int,int)[] Borders;

        public Dictionary<(int,int),double[,]> Borderbc;
        public double[,] HbcLocal;
        public double[] Plocal;

        public void calcDeriverateMatrixes()
        {
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    epsilonDeriverateMatrix[i,j] = deriverateFuncEpsilon[j](ActIntegrationMethodElements[i]);
                    tsiDeriverateMatrix[i,j] = deriverateFuncTsi[j](ActIntegrationMethodElements[i]);
                }
            }
        }
        public void calcJacobian()
        {
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                double[,] jacobian = new double[2,2];
                for(int j=0;j<baseNodes.Length;j++)
                {
                    jacobian[0,0] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[0,1] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item2;
                    jacobian[1,0] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[1,1] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item2;
                }
                jacobianStructure[i].Item1 = jacobian;
                double detJacobian = jacobian[0,0] * jacobian[1,1] - jacobian[0,1] * jacobian[1,0];
                jacobianStructure[i].Item2 = detJacobian;
                double[,] reverseJacobian = new double[2,2]{
                    { jacobian[1,1]/detJacobian, -jacobian[1,0]/detJacobian },
                    { -jacobian[0,1]/detJacobian, jacobian[0,0]/detJacobian },
                };
                jacobianStructure[i].Item3 = reverseJacobian;
            }
        }
        public void calcFinalShapeDeriverate()
        {
            for(int x = 0;x<ActIntegrationMethodElements.Length;x++)
            {
                var elem = jacobianStructure[x];
                for(int i=0;i<baseNodes.Length;i++)
                {
                    shapeResultMatrix[x,i].Item1 = (elem.Item3[0,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[0,1] * tsiDeriverateMatrix[x,i]);
                    shapeResultMatrix[x,i].Item2 = (elem.Item3[1,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[1,1] * tsiDeriverateMatrix[x,i]);
                }
            }
        }
        public void calcHMatrix(double k)
        {
            Matrix<double> Hmatrix = DenseMatrix.OfArray(Hresult);
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                double[] sRMRowX = new double[baseNodes.Length];
                double[] sRMRowY = new double[baseNodes.Length];
                for(int j=0;j<baseNodes.Length;j++)
                {
                    sRMRowX[j] = shapeResultMatrix[i,j].Item1;
                    sRMRowY[j] = shapeResultMatrix[i,j].Item2;
                }
                var NMatrixX = DenseVector.OfArray(sRMRowX).ToRowMatrix();
                var NMatrixY = DenseVector.OfArray(sRMRowY).ToRowMatrix();
                var XRes = NMatrixX.Transpose() * NMatrixX;
                var YRes = NMatrixY.Transpose() * NMatrixY;
                var result = (XRes + YRes);
                Hmatrix += result.Multiply(k * jacobianStructure[i].Item2 * (ActIntegrationMethodElementsScale[i % integrateGaussMethodDegree] * ActIntegrationMethodElementsScale[i / integrateGaussMethodDegree]));
            }
            Hresult = Hmatrix.ToArray();
        }

        public void AddShapeFunctions(Func<(double,double),double>[] shapeF)
        {
            shapeFunc = shapeF;
        }
        public void calcAll(double k)
        {
            calcDeriverateMatrixes();
            calcJacobian();
            calcFinalShapeDeriverate();
            calcHMatrix(k);
        }
        public void calcC(double[] parameters){
            calcCShapeMatrix();
            //calcCJacobian();
            //calcCShapeFinal();
            calcCMatrix(parameters[0],parameters[1]);
        }
        public void calcCShapeMatrix(){
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<shapeFunc.Length;j++)
                {
                    CshapeMatrix[i,j] = shapeFunc[j](ActIntegrationMethodElements[i]);
                }
            }
        }
        public void calcCMatrix(double c,double ro){
            Matrix<double> Cmatrix = DenseMatrix.OfArray(Cresult);
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                double[] sRMRow = new double[baseNodes.Length];
                for(int j=0;j<baseNodes.Length;j++)
                {
                    sRMRow[j] = CshapeMatrix[i,j];
                }
                var NMatrix = DenseVector.OfArray(sRMRow).ToRowMatrix();
                var result = NMatrix.Transpose() * NMatrix;
                result = result.Multiply(c * ro * jacobianStructure[i].Item2 * (ActIntegrationMethodElementsScale[i % integrateGaussMethodDegree] * ActIntegrationMethodElementsScale[i / integrateGaussMethodDegree]));
                Cmatrix += result;
            }
            Cresult = Cmatrix.ToArray();
        }

        public void calcHbc(double alfa,double talfa){
            /*for(int i=0;i<baseNodesS.Length;i++)
            {
                int nextI = i < baseNodesS.Length - 1 ? i+1 : 0;
                if(baseNodesS[i].bc && baseNodesS[nextI].bc)
                    Borderbc.Add((i,nextI),new double[ActIntegrationMethodElements1D.Length,baseNodesS.Length]);
            }*/
            for(int i=0;i<Borders.Length;i++)
            {
                Borderbc.Add(Borders[i],new double[ActIntegrationMethodElements1D.Length,baseNodesS.Length]);
            }
            foreach(var hbc in Borderbc){
                for(int i=0;i<ActIntegrationMethodElements1D.Length;i++)
                {
                    double y = hbc.Key.Item1 == 0 && hbc.Key.Item2 == 1 ? -1 : (hbc.Key.Item1 == 2 && hbc.Key.Item2 == 3 ? 1 : ActIntegrationMethodElements1D[i]);
                    double x = hbc.Key.Item1 == 1 && hbc.Key.Item2 == 2 ? 1 : (hbc.Key.Item1 == 3 && hbc.Key.Item2 == 0 ? -1 : ActIntegrationMethodElements1D[i]);
                    (double,double) point = (x,y);
                    for(int j=0;j<shapeFunc.Length;j++)
                    {
                        hbc.Value[i,j] = shapeFunc[j](point);
                    }
                }
                double jacob = Math.Sqrt(Math.Pow((baseNodes[hbc.Key.Item2].Item1 - baseNodes[hbc.Key.Item1].Item1),2) + Math.Pow((baseNodes[hbc.Key.Item2].Item2 - baseNodes[hbc.Key.Item1].Item2),2)) / 2;

                for(int i=0;i<ActIntegrationMethodElements1D.Length;i++)
                {
                    double[] Narray = new double[baseNodes.Length];
                    for(int j=0;j<Narray.Length;j++)
                    {
                        Narray[j] = hbc.Value[i,j];
                    }
                    var N = DenseVector.OfArray(Narray).ToRowMatrix();
                    var NMatrix = N.Transpose() * N;
                    NMatrix *= alfa * jacob * ActIntegrationMethodElementsScale[i];
                    HbcLocal = (DenseMatrix.OfArray(HbcLocal) + NMatrix).ToArray();
                }

                
                for(int i=0;i<Plocal.Length;i++)
                {
                    double tempPlocal = 0.0;
                    for(int j=0;j<ActIntegrationMethodElements1D.Length;j++)
                    {
                        tempPlocal += hbc.Value[j,i] * ActIntegrationMethodElementsScale[j];
                    }
                    Plocal[i] += tempPlocal * -1 * alfa * talfa * jacob;
                }
            }
            
        }

        public override string ToString()
        {
            string result = "";
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += epsilonDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\n";
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += tsiDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }

            result += "\nResult matrix: \n";

            for(int x=0;x<ActIntegrationMethodElements.Length;x++)
            {
                result += "\n\n" + ActIntegrationMethodElements[x].ToString() + ":\n\nJacobian:\n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianStructure[x].Item1[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
                result += "\ndet Jacobian: " + jacobianStructure[x].Item2.ToString("F2") + "\n\nReverse Jacobian: \n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianStructure[x].Item3[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
            }
            result += "\nShapeResult:\n";
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<baseNodes.Length;j++)
                {
                    result += shapeResultMatrix[i,j].Item1.ToString("F2") + "," + shapeResultMatrix[i,j].Item2.ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\nHResult:\n";
            for(int i=0;i<ActIntegrationMethodElements.Length;i++)
            {
                for(int j=0;j<baseNodes.Length;j++)
                {
                    result += Hresult[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }
            return result;
        }
    }
}