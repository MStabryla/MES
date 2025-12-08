package com.example;

public class SOE {
    GlobalData globalData = new GlobalData();
    public double [][] HG = new double[globalData.nN][globalData.nN];
    public double [][] CG = new double[globalData.nN][globalData.nN];
    public double [] PG = new double[globalData.nN];

    private static final double EPSILON = 1e-10;
    public double[] GaussElimination(double[][] H, double[] P) {
        int n = P.length;

        for (int p = 0; p < n; p++) {

            // find pivot row and swap
            int max = p;
            for (int i = p + 1; i < n; i++) {
                if (Math.abs(H[i][p]) > Math.abs(H[max][p])) {
                    max = i;
                }
            }
            double[] temp = H[p]; H[p] = H[max]; H[max] = temp;
            double   t    = P[p]; P[p] = P[max]; P[max] = t;

            // singular or nearly singular
            if (Math.abs(H[p][p]) <= EPSILON) {
                throw new ArithmeticException("Matrix is singular or nearly singular");
            }

            // pivot within A and b
            for (int i = p + 1; i < n; i++) {
                double alpha = H[i][p] / H[p][p];
                P[i] -= alpha * P[p];
                for (int j = p; j < n; j++) {
                    H[i][j] -= alpha * H[p][j];
                }
            }
        }

        // back substitution
        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--) {
            double sum = 0.0;
            for (int j = i + 1; j < n; j++) {
                sum += H[i][j] * x[j];
            }
            x[i] = (P[i] - sum) / H[i][i];
        }
        return x;
    }


}
