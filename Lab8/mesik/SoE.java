package com.company;

public class SoE {
    double[][] HG;
    double[][] CG;
    double[] PG;
    private static final double EPSILON = 1e-11;

    public SoE() {
        InputData inputData = new InputData();
        HG = new double[inputData.nN][inputData.nN];
        CG = new double[inputData.nN][inputData.nN];
        PG = new double[inputData.nN];
    }

    public double[] gaussEquation(double[][] H, double[] P) {

        int dim = P.length;

        for (int i = 0; i < dim; i++) {

            int max = i;
            for (int j = i + 1; j < dim; j++) {
                if (Math.abs(H[j][i]) > Math.abs(H[max][i])) {
                    max = j;
                }
            }
            double[] temp = H[i];
            double t = P[i];
            H[i] = H[max];
            H[max] = temp;
            P[i] = P[max];
            P[max] = t;

            if (Math.abs(H[i][i]) <= EPSILON) {
                throw new ArithmeticException("Matrix is singular or nearly singular");
            }

            for (int j = i + 1; j < dim; j++) {
                double alpha = H[j][i] / H[i][i];
                P[j] -= alpha * P[i];
                for (int k = i; k < dim; k++) {
                    H[j][k] -= alpha * H[i][k];
                }
            }
        }

        double[] x = new double[dim];
        for (int i = dim - 1; i >= 0; i--) {
            double sum = 0.0;
            for (int j = i + 1; j < dim; j++) {
                sum += H[i][j] * x[j];
            }
            x[i] = (P[i] - sum) / H[i][i];
        }
        return x;
    }

}
