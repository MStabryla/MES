using System;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
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
            (double,double)[] baseNodes = new (double, double)[]{
                (0.0,0.0), (4.0,0.0),(4.0,6.0),(0.0,6.0)
            };
            var solution = new ShapeFunctionsSolution(baseNodes,deriverateEpsilon,deriverateTsi);
            solution.calcDeriverateMatrixes();
            solution.calcJacobian();
            solution.calcFinalShapeDeriverate();
            solution.calcHMatrix();
            Console.WriteLine(solution);
            Console.WriteLine("\nTriple:\n");
            solution.calcDeriverateMatrixesTriple();
            solution.calcJacobianTriple();
            solution.calcFinalShapeDeriverateTriple();
            solution.calcHMatrixTriple();
            Console.WriteLine(solution.ToStringTriple());
        }
    }
}
