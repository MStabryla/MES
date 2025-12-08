package com.company;

public class Gauss {
    final double node1_1 = 0.577350269;
    final double node2_1 = 0.774596669;
    final int node2_2 = 0;
    final double val2_1 = 0.555555555555556; // 5/9
    final double val2_2 = 0.888888888888889; // 8/9
    double[] w = { val2_1, val2_2, val2_1 };

    double functionResult(double x, double y) {
        //return -2 * x * x * y + 2 * x * y + 4;
        return -5 * x * x * y + 3 * x * y * y + 10;
    }

    double twoPointGauss() {
        double Fp1 = functionResult(-node1_1, -node1_1);
        double Fp2 = functionResult(node1_1, -node1_1);
        double Fp3 = functionResult(node1_1, node1_1);
        double Fp4 = functionResult(-node1_1, node1_1);
        return Fp1 + Fp2 + Fp3 + Fp4;
    }

    double threePointGauss() {
        double[] x = { -node2_1, node2_2, node2_1 };
        double[] y = { -node2_1, node2_2, node2_1 };
        double sum = 0;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                sum += functionResult(x[i], y[j]) * w[i] * w[j];
            }
        }
        return sum;
    }
}
