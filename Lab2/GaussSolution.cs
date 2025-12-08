using System;

namespace Lab2
{
    public static class ShapeFunctions {

        public static double N1(double x) { return (1.0 - x)/2.0; }
        public static double N2(double x) { return (x + 1.0)/2.0; }
    }
    public class GaussSolutionOneDimention
    {
        private static double[] doubleMethodFactors = { 1.0, 1.0 };
        private static double[] doubleMethodPositionModifiers = { -1.0/Math.Sqrt(3.0) , 1.0/Math.Sqrt(3.0) };

        private static double[] tripleMethodFactors = { 5.0/9.0, 8.0/9.0, 5.0/9.0 };
        private static double[] tripleMethodPositionModifiers = { -Math.Sqrt(3.0/5.0), 0.0, Math.Sqrt(3.0/5.0) };

        private Func<double,double> function;

        public GaussSolutionOneDimention(Func<double,double> f)
        {
            this.function = f;
        }

        public double CalcWithDoubleMethod()
        {
            double result = 0.0;
            for(int i=0;i<2;i++)
                result += function(doubleMethodPositionModifiers[i]) * doubleMethodFactors[i];
            return result;
        }
        public double CalcWithDoubleMethod(double rangeBegin,double rangeEnd)
        {
            Func<double,double> calcMethodPositions = (x) => ShapeFunctions.N1(x) * rangeBegin + ShapeFunctions.N2(x) * rangeEnd;
            double[] DoubleMethodPositions = new double[2];
            double result = 0.0;
            for(int i=0;i<2;i++)
            {
                DoubleMethodPositions[i] = calcMethodPositions(doubleMethodPositionModifiers[i]);
                result += function(DoubleMethodPositions[i]) * doubleMethodFactors[i];
            }
            return result;
        }
        public double CalcWithTripleMethod()
        {
            double result = 0.0;
            for(int i=0;i<3;i++)
                result += function(tripleMethodPositionModifiers[i]) * tripleMethodFactors[i];
            return result;
        }
        public double CalcWithTripleMethod(double rangeBegin,double rangeEnd)
        {
            Func<double,double> calcMethodPositions = (x) => ShapeFunctions.N1(x) * rangeBegin + ShapeFunctions.N2(x) * rangeEnd;
            double[] TripleMethodPositions = new double[3];
            double result = 0.0;
            for(int i=0;i<3;i++)
            {
                TripleMethodPositions[i] = calcMethodPositions(tripleMethodPositionModifiers[i]);
                result += function(TripleMethodPositions[i]) * tripleMethodFactors[i];
            }
            return result;
        }
    }
    public class GaussSolutionTwoDimention
    {
        private static double[] doubleMethodFactors = { 1.0, 1.0 };
        private static double[] doubleMethodPositionModifiers = { -1.0/Math.Sqrt(3.0) , 1.0/Math.Sqrt(3.0) };

        private static double[] tripleMethodFactors = { 5.0/9.0, 8.0/9.0, 5.0/9.0 };
        private static double[] tripleMethodPositionModifiers = { -Math.Sqrt(3.0/5.0), 0.0, Math.Sqrt(3.0/5.0) };

        private Func<double,double,double> function;

        public GaussSolutionTwoDimention(Func<double,double,double> f)
        {
            this.function = f;
        }

        public double CalcWithDoubleMethod()
        {
            (double,double)[] DoubleMethodPositions = new (double,double)[4];
            double result = 0.0;
            for(int i=0;i<2;i++)
            {
                for(int j=0;j<2;j++)
                {
                    DoubleMethodPositions[j + i*2] = (doubleMethodPositionModifiers[i],doubleMethodPositionModifiers[j]);
                    result += function(DoubleMethodPositions[j + i*2].Item1,DoubleMethodPositions[j + i*2].Item2) * doubleMethodFactors[i] * doubleMethodFactors[j];
                }
            }
            return result;
        }
        public double CalcWithDoubleMethod(double rangeBegin,double rangeEnd)
        {
            Func<double,double> calcMethodPositions = (x) => ShapeFunctions.N1(x) * rangeBegin + ShapeFunctions.N2(x) * rangeEnd;
            (double,double)[] DoubleMethodPositions = new (double,double)[4];
            double result = 0.0;
            for(int i=0;i<2;i++)
            {
                for(int j=0;j<2;j++)
                {
                    DoubleMethodPositions[j + i*2] = (calcMethodPositions(doubleMethodPositionModifiers[i]),calcMethodPositions(doubleMethodPositionModifiers[j]));
                    result += function(DoubleMethodPositions[j + i*2].Item1,DoubleMethodPositions[j + i*2].Item2) * doubleMethodFactors[i] * doubleMethodFactors[j];
                }
            }
            return result;
        }
        public double CalcWithTripleMethod()
        {
            (double,double)[] TripleMethodPositions = new (double,double)[9];
            double result = 0.0;
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    TripleMethodPositions[j + i*3] = (tripleMethodPositionModifiers[i],tripleMethodPositionModifiers[j]);
                    result += function(TripleMethodPositions[j + i*3].Item1,TripleMethodPositions[j + i*3].Item2) * tripleMethodFactors[i] * tripleMethodFactors[j];
                }
            }
            return result;
        }
        public double CalcWithTripleMethod(double rangeBegin,double rangeEnd)
        {
            Func<double,double> calcMethodPositions = (x) => ShapeFunctions.N1(x) * rangeBegin + ShapeFunctions.N2(x) * rangeEnd;
            (double,double)[] TripleMethodPositions = new (double,double)[9];
            double result = 0.0;
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    TripleMethodPositions[j + i*3] = (calcMethodPositions(tripleMethodPositionModifiers[i]),calcMethodPositions(tripleMethodPositionModifiers[j]));
                    result += function(TripleMethodPositions[j + i*3].Item1,TripleMethodPositions[j + i*3].Item2) * tripleMethodFactors[i] * tripleMethodFactors[j];
                }
            }
            return result;
        }
    }
}