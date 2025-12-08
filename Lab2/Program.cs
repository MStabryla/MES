using System;
using MES;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileDouble(args[0]);
            
            double rangeBegin = data.numbers[0];
            double rangeEnd = data.numbers[1];
            if(rangeEnd <= rangeBegin)
                throw new Exception("Incorrect range definition");
            var gaussSol = new GaussSolutionOneDimention((x) => x*x );
            double gausslSolDouble = gaussSol.CalcWithDoubleMethod(rangeBegin,rangeEnd);
            double gausslSolTriple = gaussSol.CalcWithTripleMethod(rangeBegin,rangeEnd);
            Console.Write("Results:\nDouble method = {0}\nTriple method = {1}\n",gausslSolDouble,gausslSolTriple);

            var gaussSol2D = new GaussSolutionTwoDimention((x,y) => x*x + y*y );
            double gauss2DSimpleSolDouble = gaussSol2D.CalcWithDoubleMethod();
            double gauss2DSimpleSolTriple = gaussSol2D.CalcWithTripleMethod();
            Console.Write("Results 2D Simple:\nDouble method = {0}\nTriple method = {1}\n",gauss2DSimpleSolDouble,gauss2DSimpleSolTriple);

            double gauss2DSolDouble = gaussSol2D.CalcWithDoubleMethod(rangeBegin,rangeEnd);
            double gauss2DSolTriple = gaussSol2D.CalcWithTripleMethod(rangeBegin,rangeEnd);
            Console.Write("Results 2D:\nDouble method = {0}\nTriple method = {1}\n",gauss2DSolDouble,gauss2DSolTriple);
        }
    }
}
