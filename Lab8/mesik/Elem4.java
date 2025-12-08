package com.company;

public class Elem4 {
    int ip_number;
    public double[] xi;
    public double[] eta;
    public double[][] derNXi;
    public double[][] derNEta;
    public double[][] N;
    public double[][] N_edge1;
    public double[][] N_edge2;
    public double[][] N_edge3;
    public double[][] N_edge4;
    public double[] derXXi;
    public double[] derYXi;
    public double[] derYEta;
    public double[] derXEta;
    public double[] w1;
    public double[] w2;
    public double[] wBC;
    public Elem4(int ilosc_pkt_calk) {
        this.ip_number = ilosc_pkt_calk;
        derXXi = new double[ilosc_pkt_calk*ilosc_pkt_calk];
        derYXi = new double[ilosc_pkt_calk*ilosc_pkt_calk];
        derYEta = new double[ilosc_pkt_calk*ilosc_pkt_calk];
        derXEta = new double[ilosc_pkt_calk*ilosc_pkt_calk];
        fill();
    }

    void fill() {
        if (ip_number == 2) {
            w1 = new double[4];
            w2 = new double[4];
            wBC = new double[]{1.0,1.0};
            for (int i = 0; i < 4; i++) {
                derXXi[i] = 0; derYXi[i] = 0; derYEta[i] = 0; derXEta[i] = 0;
                w1[i] = 1; w2[i] = 1;
            }
            xi = new double[] {-1/(Math.sqrt(3)), 1/(Math.sqrt(3)), 1/(Math.sqrt(3)), -1/(Math.sqrt(3))};
            eta = new double[] {-1/(Math.sqrt(3)), -1/(Math.sqrt(3)), 1/(Math.sqrt(3)), 1/(Math.sqrt(3))};
            derNXi = new double[][] {{-0.25 * (1-eta[0]), (0.25 * (1-eta[0])), (0.25 * (1+eta[0])), (-0.25 * (1+eta[0]))},
                    {(-0.25 * (1-eta[1])), (0.25 * (1-eta[1])), (0.25 * (1+eta[1])), (-0.25 * (1+eta[1]))},
                    {(-0.25 * (1-eta[2])), (0.25 * (1-eta[2])), (0.25 * (1+eta[2])), (-0.25 * (1+eta[2]))},
                    {(-0.25 * (1-eta[3])), (0.25 * (1-eta[3])), (0.25 * (1+eta[3])), (-0.25 * (1+eta[3]))}};
            derNEta = new double[][] {{(-0.25 * (1-xi[0])), (-0.25 * (1+xi[0])), (0.25 * (1+xi[0])), (0.25 * (1-xi[0]))},
                    {(-0.25 * (1-xi[1])), (-0.25 * (1+xi[1])), (0.25 * (1+xi[1])), (0.25 * (1-xi[1]))},
                    {(-0.25 * (1-xi[2])), (-0.25 * (1+xi[2])), (0.25 * (1+xi[2])), (0.25 * (1-xi[2]))},
                    {(-0.25 * (1-xi[3])), (-0.25 * (1+xi[3])), (0.25 * (1+xi[3])), (0.25 * (1-xi[3]))}};

            N = new double[][] {{0.25 * (1-eta[0]) * (1-xi[0]), 0.25 * (1-eta[0]) * (1+xi[0]), 0.25 * (1+eta[0]) * (1+xi[0]), 0.25 * (1+eta[0]) * (1-xi[0])},
                    {0.25 * (1-eta[1]) * (1-xi[1]), 0.25 * (1-eta[1]) * (1+xi[1]), 0.25 * (1+eta[1]) * (1+xi[1]), 0.25 * (1+eta[1]) * (1-xi[1])},
                    {0.25 * (1-eta[2]) * (1-xi[2]), 0.25 * (1-eta[2]) * (1+xi[2]), 0.25 * (1+eta[2]) * (1+xi[2]), 0.25 * (1+eta[2]) * (1-xi[2])},
                    {0.25 * (1-eta[3]) * (1-xi[3]), 0.25 * (1-eta[3]) * (1+xi[3]), 0.25 * (1+eta[3]) * (1+xi[3]), 0.25 * (1+eta[3]) * (1-xi[3])}};

            double[] xi_edge1 = {-(1/Math.sqrt(3)), 1/Math.sqrt(3) };

            double[] eta_edge2 = {-(1/Math.sqrt(3)), 1/Math.sqrt(3) };

            double[] xi_edge3 = {-(1/Math.sqrt(3)), (1/Math.sqrt(3)) };

            double[] eta_edge4 = {(1/Math.sqrt(3)), -(1/Math.sqrt(3))};


            N_edge1 = new double[][]{{0.5 * (1-xi_edge1[0]), 0.5 * (1+xi_edge1[0]), 0.0, 0.0},
                                    {0.5 * (1-xi_edge1[1]), 0.5 * (1+xi_edge1[1]), 0.0, 0.0}};

            N_edge2 = new double[][]{{0.0, 0.5 * (1-eta_edge2[0]), 0.5 * (1+eta_edge2[0]), 0.0},
                                    {0.0, 0.5 * (1-eta_edge2[1]), 0.5 * (1+eta_edge2[1]), 0.0}};

            N_edge3 = new double[][]{{0.0, 0.0, 0.5 * (1-xi_edge3[0]), 0.5 * (1+xi_edge3[0])},
                                    {0.0, 0.0, 0.5 * (1-xi_edge3[1]), 0.5 * (1+xi_edge3[1])}};

            N_edge4 = new double[][]{{ 0.5 * (1-eta_edge4[0]), 0.0, 0.0, 0.5 * (1+eta_edge4[0]), 0.0, 0.0},
                                    { 0.5 * (1-eta_edge4[1]), 0.0, 0.0, 0.5 * (1+eta_edge4[1]), 0.0, 0.0}};

        }
        else if (ip_number == 3) {
            w1 = new double[9];
            w2 = new double[9];
            double a1 = 5.0/9.0;
            double a2 = 8.0/9.0;
            wBC = new double[]{a1, a2, a1};
            for (int i = 0; i < 9; i++) {
                derXXi[i] = 0; derYXi[i] = 0; derYEta[i] = 0; derXEta[i] = 0;
            }
            w1 = new double[]{5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0};
            w2 = new double[]{5.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 8.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 5.0/9.0};
            xi = new double[]{-1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0)};
            eta = new double[]{-1*Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0,0,0, Math.sqrt(3.0/5.0), Math.sqrt(3.0/5.0), Math.sqrt(3.0/5.0)};

            derNXi = new double[][] {{-0.25 * (1-eta[0]), (0.25 * (1-eta[0])), (0.25 * (1+eta[0])), (-0.25 * (1+eta[0]))},
                    {(-0.25 * (1-eta[1])), (0.25 * (1-eta[1])), (0.25 * (1+eta[1])), (-0.25 * (1+eta[1]))},
                    {(-0.25 * (1-eta[2])), (0.25 * (1-eta[2])), (0.25 * (1+eta[2])), (-0.25 * (1+eta[2]))},
                    {(-0.25 * (1-eta[3])), (0.25 * (1-eta[3])), (0.25 * (1+eta[3])), (-0.25 * (1+eta[3]))},
                    {(-0.25 * (1-eta[4])), (0.25 * (1-eta[4])), (0.25 * (1+eta[4])), (-0.25 * (1+eta[4]))},
                    {(-0.25 * (1-eta[5])), (0.25 * (1-eta[5])), (0.25 * (1+eta[5])), (-0.25 * (1+eta[5]))},
                    {(-0.25 * (1-eta[6])), (0.25 * (1-eta[6])), (0.25 * (1+eta[6])), (-0.25 * (1+eta[6]))},
                    {(-0.25 * (1-eta[7])), (0.25 * (1-eta[7])), (0.25 * (1+eta[7])), (-0.25 * (1+eta[7]))},
                    {(-0.25 * (1-eta[8])), (0.25 * (1-eta[8])), (0.25 * (1+eta[8])), (-0.25 * (1+eta[8]))}};
            derNEta = new double[][] {{(-0.25 * (1-xi[0])), (-0.25 * (1+xi[0])), (0.25 * (1+xi[0])), (0.25 * (1-xi[0]))},
                    {(-0.25 * (1-xi[1])), (-0.25 * (1+xi[1])), (0.25 * (1+xi[1])), (0.25 * (1-xi[1]))},
                    {(-0.25 * (1-xi[2])), (-0.25 * (1+xi[2])), (0.25 * (1+xi[2])), (0.25 * (1-xi[2]))},
                    {(-0.25 * (1-xi[3])), (-0.25 * (1+xi[3])), (0.25 * (1+xi[3])), (0.25 * (1-xi[3]))},
                    {(-0.25 * (1-xi[4])), (-0.25 * (1+xi[4])), (0.25 * (1+xi[4])), (0.25 * (1-xi[4]))},
                    {(-0.25 * (1-xi[5])), (-0.25 * (1+xi[5])), (0.25 * (1+xi[5])), (0.25 * (1-xi[5]))},
                    {(-0.25 * (1-xi[6])), (-0.25 * (1+xi[6])), (0.25 * (1+xi[6])), (0.25 * (1-xi[6]))},
                    {(-0.25 * (1-xi[7])), (-0.25 * (1+xi[7])), (0.25 * (1+xi[7])), (0.25 * (1-xi[7]))},
                    {(-0.25 * (1-xi[8])), (-0.25 * (1+xi[8])), (0.25 * (1+xi[8])), (0.25 * (1-xi[8]))}};


            N = new double[][] {{0.25 * (1-eta[0]) * (1-xi[0]), 0.25 * (1-eta[0]) * (1+xi[0]), 0.25 * (1+eta[0]) * (1+xi[0]), 0.25 * (1+eta[0]) * (1-xi[0])},
                    {0.25 * (1-eta[1]) * (1-xi[1]), 0.25 * (1-eta[1]) * (1+xi[1]), 0.25 * (1+eta[1]) * (1+xi[1]), 0.25 * (1+eta[1]) * (1-xi[1])},
                    {0.25 * (1-eta[2]) * (1-xi[2]), 0.25 * (1-eta[2]) * (1+xi[2]), 0.25 * (1+eta[2]) * (1+xi[2]), 0.25 * (1+eta[2]) * (1-xi[2])},
                    {0.25 * (1-eta[3]) * (1-xi[3]), 0.25 * (1-eta[3]) * (1+xi[3]), 0.25 * (1+eta[3]) * (1+xi[3]), 0.25 * (1+eta[3]) * (1-xi[3])},
                    {0.25 * (1-eta[4]) * (1-xi[4]), 0.25 * (1-eta[4]) * (1+xi[4]), 0.25 * (1+eta[4]) * (1+xi[4]), 0.25 * (1+eta[4]) * (1-xi[4])},
                    {0.25 * (1-eta[5]) * (1-xi[5]), 0.25 * (1-eta[5]) * (1+xi[5]), 0.25 * (1+eta[5]) * (1+xi[5]), 0.25 * (1+eta[5]) * (1-xi[5])},
                    {0.25 * (1-eta[6]) * (1-xi[6]), 0.25 * (1-eta[6]) * (1+xi[6]), 0.25 * (1+eta[6]) * (1+xi[6]), 0.25 * (1+eta[6]) * (1-xi[6])},
                    {0.25 * (1-eta[7]) * (1-xi[7]), 0.25 * (1-eta[7]) * (1+xi[7]), 0.25 * (1+eta[7]) * (1+xi[7]), 0.25 * (1+eta[7]) * (1-xi[7])},
                    {0.25 * (1-eta[8]) * (1-xi[8]), 0.25 * (1-eta[8]) * (1+xi[8]), 0.25 * (1+eta[8]) * (1+xi[8]), 0.25 * (1+eta[8]) * (1-xi[8])}};

            double[] xi_edge1 = {-Math.sqrt(3.0/5.0), 0,  Math.sqrt(3.0/5.0)};

            double[] eta_edge2 = new double[]{-Math.sqrt(3.0/5.0), 0,Math.sqrt(3.0/5.0)};

            double[] xi_edge3 = new double[]{Math.sqrt(3.0/5.0), 0 , -Math.sqrt(3.0/5.0)};

            double[] eta_edge4 = new double[]{Math.sqrt(3.0/5.0),0, -Math.sqrt(3.0/5.0)};


           N_edge1 = new double[][]{
                   {0.5 * (1-xi_edge1[0]), 0.5 * (1+xi_edge1[0]), 0.0, 0.0},
                    {0.5 * (1-xi_edge1[1]), 0.5 * (1+xi_edge1[1]), 0.0, 0.0},
                    {0.5 * (1-xi_edge1[2]), 0.5 * (1+xi_edge1[2]), 0.0, 0.0}
                    };

            N_edge2 = new double[][]{{0.0, 0.5 * (1-eta_edge2[0]), 0.5 * (1+eta_edge2[0]), 0.0},
                    {0.0, 0.5 * (1-eta_edge2[1]), 0.5 * (1+eta_edge2[1]), 0.0},
                    {0.0, 0.5 * (1-eta_edge2[2]), 0.5 * (1+eta_edge2[2]), 0.0}};

            N_edge3 = new double[][]{{0.0, 0.0, 0.5 * (1-xi_edge3[0]), 0.5 * (1+xi_edge3[0])},
                    {0.0, 0.0, 0.5 * (1-xi_edge3[1]), 0.5 * (1+xi_edge3[1])},
                    {0.0, 0.0, 0.5 * (1-xi_edge3[2]), 0.5 * (1+xi_edge3[2])}};

            N_edge4 = new double[][]{{ 0.5 * (1-eta_edge4[0]), 0.0, 0.0, 0.5 * (1+eta_edge4[0]),},
                    {0.5 * (1-eta_edge4[1]), 0.0, 0.0, 0.5 * (1+eta_edge4[1])},
                    {0.5 * (1-eta_edge4[2]), 0.0, 0.0, 0.5 * (1+eta_edge4[2])}};
        }
    }
}
