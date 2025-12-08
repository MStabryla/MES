
using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lab3
{
    public class ShapeFunctionsSolution
    {
        private static (double,double)[] integrationDoubleMethodElements = {
            (-1.0/Math.Sqrt(3.0), -1.0/Math.Sqrt(3.0)),
            (1.0/Math.Sqrt(3.0), -1.0/Math.Sqrt(3.0)),
            (1.0/Math.Sqrt(3.0), 1.0/Math.Sqrt(3.0)),
            (-1.0/Math.Sqrt(3.0), 1.0/Math.Sqrt(3.0))
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

        Func<(double,double),double>[] deriverateFuncEpsilon;
        Func<(double,double),double>[] deriverateFuncTsi;
        public ShapeFunctionsSolution((double,double)[] _baseNodes,Func<(double,double),double>[] _deriverateFuncEpsilon,Func<(double,double),double>[] _deriverateFuncTsi)
        {
            this.baseNodes = _baseNodes;
            deriverateFuncEpsilon = _deriverateFuncEpsilon;
            deriverateFuncTsi = _deriverateFuncTsi;
            epsilonDeriverateMatrix = new double[integrationDoubleMethodElements.Length,baseNodes.Length];
            tsiDeriverateMatrix = new double[integrationDoubleMethodElements.Length,baseNodes.Length];
            jacobianMatrix = new (double[,],double,double[,])[integrationDoubleMethodElements.Length];
            shapeResultMatrix = new (double,double)[integrationDoubleMethodElements.Length,baseNodes.Length];
            Hresult = new double[integrationDoubleMethodElements.Length,baseNodes.Length];
        }

        public (double,double)[] baseNodes;
        
        public double[,] epsilonDeriverateMatrix;
        public double[,] tsiDeriverateMatrix;
        public (double[,],double,double[,])[] jacobianMatrix;
        public (double,double)[,] shapeResultMatrix;
        public double[,] Hresult;

        public void calcDeriverateMatrixes()
        {
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    epsilonDeriverateMatrix[i,j] = deriverateFuncEpsilon[j](integrationDoubleMethodElements[i]);
                    tsiDeriverateMatrix[i,j] = deriverateFuncTsi[j](integrationDoubleMethodElements[i]);
                }
            }
        }
        public void calcJacobian()
        {
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                double[,] jacobian = new double[2,2];
                for(int j=0;j<baseNodes.Length;j++)
                {
                    jacobian[0,0] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[0,1] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item2;
                    jacobian[1,0] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[1,1] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item2;
                }
                jacobianMatrix[i].Item1 = jacobian;
                double detJacobian = jacobian[0,0] * jacobian[1,1] - jacobian[0,1] * jacobian[1,0];
                jacobianMatrix[i].Item2 = detJacobian;
                double[,] reverseJacobian = new double[2,2]{
                    { jacobian[1,1]/detJacobian, -jacobian[1,0]/detJacobian },
                    { -jacobian[0,1]/detJacobian, jacobian[0,0]/detJacobian },
                };
                jacobianMatrix[i].Item3 = reverseJacobian;
            }
        }
        public void calcFinalShapeDeriverate()
        {
            for(int x = 0;x<integrationDoubleMethodElements.Length;x++)
            {
                var elem = jacobianMatrix[x];
                for(int i=0;i<baseNodes.Length;i++)
                {
                    shapeResultMatrix[x,i].Item1 = (elem.Item3[0,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[0,1] * tsiDeriverateMatrix[x,i]);
                    shapeResultMatrix[x,i].Item2 = (elem.Item3[1,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[1,1] * tsiDeriverateMatrix[x,i]);
                }
            }
        }
        public void calcHMatrix()
        {
            Matrix<double> Hmatrix = DenseMatrix.OfArray(Hresult);
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
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
                Hmatrix += result.Multiply(30 * jacobianMatrix[i].Item2);
            }
            Hresult = Hmatrix.ToArray();
        }
        public override string ToString()
        {
            string result = "";
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += epsilonDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\n";
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += tsiDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }

            result += "\nResult matrix: \n";

            for(int x=0;x<integrationDoubleMethodElements.Length;x++)
            {
                result += "\n\n" + integrationDoubleMethodElements[x].ToString() + ":\n\nJacobian:\n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianMatrix[x].Item1[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
                result += "\ndet Jacobian: " + jacobianMatrix[x].Item2.ToString("F2") + "\n\nReverse Jacobian: \n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianMatrix[x].Item3[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
            }
            result += "\nShapeResult:\n";
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                for(int j=0;j<baseNodes.Length;j++)
                {
                    result += shapeResultMatrix[i,j].Item1.ToString("F2") + "," + shapeResultMatrix[i,j].Item2.ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\nHResult:\n";
            for(int i=0;i<integrationDoubleMethodElements.Length;i++)
            {
                for(int j=0;j<baseNodes.Length;j++)
                {
                    result += Hresult[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }
            return result;
        }
        public void calcAll()
        {
            calcDeriverateMatrixes();
            calcJacobian();
            calcFinalShapeDeriverate();
            calcHMatrix();
        }
        public void calcDeriverateMatrixesTriple()
        {
            epsilonDeriverateMatrix = new double[integrationTripleMethodElements.Length,baseNodes.Length];
            tsiDeriverateMatrix = new double[integrationTripleMethodElements.Length,baseNodes.Length];
            jacobianMatrix = new (double[,],double,double[,])[integrationTripleMethodElements.Length];
            shapeResultMatrix = new (double,double)[integrationTripleMethodElements.Length,baseNodes.Length];
            Hresult = new double[baseNodes.Length,baseNodes.Length];

            for(int i=0;i<integrationTripleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    epsilonDeriverateMatrix[i,j] = deriverateFuncEpsilon[j](integrationTripleMethodElements[i]);
                    tsiDeriverateMatrix[i,j] = deriverateFuncTsi[j](integrationTripleMethodElements[i]);
                }
            }
        }
        public void calcJacobianTriple()
        {
            for(int i=0;i<integrationTripleMethodElements.Length;i++)
            {
                double[,] jacobian = new double[2,2];
                for(int j=0;j<baseNodes.Length;j++)
                {
                    jacobian[0,0] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[0,1] += epsilonDeriverateMatrix[i,j] * baseNodes[j].Item2;
                    jacobian[1,0] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item1;
                    jacobian[1,1] += tsiDeriverateMatrix[i,j] * baseNodes[j].Item2;
                }
                jacobianMatrix[i].Item1 = jacobian;
                double detJacobian = jacobian[0,0] * jacobian[1,1] - jacobian[0,1] * jacobian[1,0];
                jacobianMatrix[i].Item2 = detJacobian;
                double[,] reverseJacobian = new double[2,2]{
                    { jacobian[1,1]/detJacobian, -jacobian[1,0]/detJacobian },
                    { -jacobian[0,1]/detJacobian, jacobian[0,0]/detJacobian },
                };
                jacobianMatrix[i].Item3 = reverseJacobian;
            }
        }
        public void calcFinalShapeDeriverateTriple()
        {
            for(int x = 0;x<integrationTripleMethodElements.Length;x++)
            {
                var elem = jacobianMatrix[x];
                for(int i=0;i<baseNodes.Length;i++)
                {
                    shapeResultMatrix[x,i].Item1 = (elem.Item3[0,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[0,1] * tsiDeriverateMatrix[x,i]);
                    shapeResultMatrix[x,i].Item2 = (elem.Item3[1,0] * epsilonDeriverateMatrix[x,i] - elem.Item3[1,1] * tsiDeriverateMatrix[x,i]);
                }
            }
        }
        public void calcHMatrixTriple()
        {
            Matrix<double> Hmatrix = DenseMatrix.OfArray(Hresult);
            for(int i=0;i<integrationTripleMethodElements.Length;i++)
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
                Hmatrix += result.Multiply(30 * jacobianMatrix[i].Item2);
            }
            Hresult = Hmatrix.ToArray();
        }
        public string ToStringTriple()
        {
            string result = "";
            for(int i=0;i<integrationTripleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += epsilonDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\n";
            for(int i=0;i<integrationTripleMethodElements.Length;i++)
            {
                for(int j=0;j<deriverateFuncEpsilon.Length;j++)
                {
                    result += tsiDeriverateMatrix[i,j].ToString("F2") + " ";
                }
                result += "\n";
            }

            result += "\nResult matrix: \n";

            for(int x=0;x<integrationTripleMethodElements.Length;x++)
            {
                result += "\n\n" + integrationTripleMethodElements[x].ToString() + ":\n\nJacobian:\n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianMatrix[x].Item1[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
                result += "\ndet Jacobian: " + jacobianMatrix[x].Item2.ToString("F2") + "\n\nReverse Jacobian: \n";
                for(int i=0;i<2;i++)
                {
                    for(int j=0;j<2;j++)
                    {
                        result += jacobianMatrix[x].Item3[i,j].ToString("F2") + " ";
                    }
                    result += "\n";
                }
            }
            result += "\nShapeResult:\n";
            for(int i=0;i<integrationTripleMethodElements.Length;i++)
            {
                for(int j=0;j<baseNodes.Length;j++)
                {
                    result += shapeResultMatrix[i,j].Item1.ToString("F2") + "," + shapeResultMatrix[i,j].Item2.ToString("F2") + " ";
                }
                result += "\n";
            }
            result += "\nHResult:\n";
            for(int i=0;i<baseNodes.Length;i++)
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