package com.example;

import java.util.List;
import java.util.regex.Matcher;

public class Elem4 {
//    double e[];
//    double n[];

//    double waga1[] = {5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0};
//    double waga2[] = {5.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 8.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 5.0/9.0};


    public double [][] tabE;
    public double [][] tabN;
    public double [][] N;
    public double [][] N_pow1;
    public double [][] N_pow2;
    public double [][] N_pow3;
    public double [][] N_pow4;

    double waga1[];
    double waga2[];
    double waga1_pow[];
    double waga_pow1[];
    double waga_pow2[];
    double waga_pow3[];
    double waga_pow4[];

    public double  dxksi[];
    public double  dyeta[];
    public double  dxeta[];
    public double  dyksi[];


    Elem4(int pc){
        if(pc == 2) {
            double e[]= {-(1/Math.sqrt(3)), 1/Math.sqrt(3), 1/Math.sqrt(3), -(1/Math.sqrt(3))};
            double n[] = {-(1/Math.sqrt(3)), -(1/Math.sqrt(3)), 1/Math.sqrt(3), 1/Math.sqrt(3)};

            double e_pow1[] = {-(1/Math.sqrt(3)), 1/Math.sqrt(3) };
            double e_pow2[] = {1, 1};
            double e_pow3[] = {-(1/Math.sqrt(3)), (1/Math.sqrt(3)) };
            double e_pow4[] = {-1, -1};
            double n_pow1[] = {-1, -1 };
            double n_pow2[] = {-(1/Math.sqrt(3)), 1/Math.sqrt(3) };
            double n_pow3[] = {1,1};
            double n_pow4[] = {(1/Math.sqrt(3)), -(1/Math.sqrt(3)) };



            tabE = new double[4][4];
            tabN = new double[4][4];
            N = new double[4][4];
            N_pow1 = new double[2][4];
            N_pow2 = new double[2][4];
            N_pow3 = new double[2][4];
            N_pow4 = new double[2][4];

            waga1 = new double[4];
            waga2 = new double[4];

            dxksi = new double[4];
            dyksi = new double[4];
            dxeta = new double[4];
            dyeta = new double[4];

            waga_pow1 = new double[]{1,1};


            for(int i = 0; i < 4; i++){
                waga1[i] = 1;
                waga2[i] = 1;
                dxksi[i] = 0;
                dxeta[i] = 0;
                dyksi[i] = 0;
                dyeta[i] = 0;

            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabE[i][j] = -0.25 * (1 - n[i]);
                    if (j == 1) tabE[i][j] = 0.25 * (1 - n[i]);
                    if (j == 2) tabE[i][j] = 0.25 * (1 + n[i]);
                    if (j == 3) tabE[i][j] = -0.25 * (1 + n[i]);
                }
            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabN[i][j] = -0.25 * (1 - e[i]);
                    if (j == 1) tabN[i][j] = -0.25 * (1 + e[i]);
                    if (j == 2) tabN[i][j] = 0.25 * (1 + e[i]);
                    if (j == 3) tabN[i][j] = 0.25 * (1 - e[i]);
                }
            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N[i][j] = 0.25 *(1-e[i]) * (1 - n[i]);
                    if (j == 1) N[i][j] = 0.25 *(1+e[i]) * (1 - n[i]);
                    if (j == 2) N[i][j] = 0.25 *(1+e[i]) * (1 + n[i]);
                    if (j == 3) N[i][j] = 0.25 *(1-e[i]) * (1 + n[i]);
                }
            }

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow1[i][j] = 0.5 * (1-e_pow1[i]);
                    if (j == 1) N_pow1[i][j] = 0.5 * (1+e_pow1[i]);
                    if (j == 2) N_pow1[i][j] = 0;
                    if (j == 3) N_pow1[i][j] = 0;
                }
            }

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow2[i][j] = 0;
                    if (j == 1) N_pow2[i][j] = 0.5 * (1-n_pow2[i]);
                    if (j == 2) N_pow2[i][j] = 0.5 * (1+n_pow2[i]);
                    if (j == 3) N_pow2[i][j] = 0;
                }
            }

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow3[i][j] = 0;
                    if (j == 1) N_pow3[i][j] = 0;
                    if (j == 2) N_pow3[i][j] = 0.5 * (1-e_pow3[i]);
                    if (j == 3) N_pow3[i][j] = 0.5 * (1+e_pow3[i]);
                }
            }

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow4[i][j] = 0.5*(1-n_pow4[i]);
                    if (j == 1) N_pow4[i][j] = 0;
                    if (j == 2) N_pow4[i][j] = 0;
                    if (j == 3) N_pow4[i][j] = 0.5 * (1+n_pow4[i]);
                }
            }


        }
        else if(pc == 3){
            double e[] = {-1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0, Math.sqrt(3.0/5.0)};
            double n[] = {-1*Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), -1*Math.sqrt(3.0/5.0), 0,0,0, Math.sqrt(3.0/5.0), Math.sqrt(3.0/5.0), Math.sqrt(3.0/5.0)};

            double e_pow1[] = {-Math.sqrt(3.0/5.0), 0,  Math.sqrt(3.0/5.0) };
            double e_pow2[] = {1, 1, 1};
            double e_pow3[] = {Math.sqrt(3.0/5.0), 0 , -Math.sqrt(3.0/5.0) };
            double e_pow4[] = {-1, -1, -1};
            double n_pow1[] = {-1, -1, -1 };
            double n_pow2[] = {-Math.sqrt(3.0/5.0), 0,Math.sqrt(3.0/5.0) };
            double n_pow3[] = {1,1, 1};
            double n_pow4[] = {Math.sqrt(3.0/5.0),0, -Math.sqrt(3.0/5.0) };

            tabE = new double[9][4];
            tabN = new double[9][4];
            N = new double[9][4];
            N_pow1 = new double[3][4];
            N_pow2 = new double[3][4];
            N_pow3 = new double[3][4];
            N_pow4 = new double[3][4];


            waga1 = new double[9];
            waga2 = new double[9];

            dxksi = new double[9];
            dyksi = new double[9];
            dxeta = new double[9];
            dyeta = new double[9];

            waga1 = new double[]{5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 5.0/9.0};
            waga2 = new double[]{5.0/9.0, 5.0/9.0, 5.0/9.0, 8.0/9.0, 8.0/9.0, 8.0/9.0, 5.0/9.0, 5.0/9.0, 5.0/9.0};

            waga_pow1 = new double[]{5.0/9.0, 8.0/9.0, 5.0/9.0};



            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabE[i][j] = -0.25 * (1 - n[i]);
                    if (j == 1) tabE[i][j] = 0.25 * (1 - n[i]);
                    if (j == 2) tabE[i][j] = 0.25 * (1 + n[i]);
                    if (j == 3) tabE[i][j] = -0.25 * (1 + n[i]);
                }
            }

            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabN[i][j] = -0.25 * (1 - e[i]);
                    if (j == 1) tabN[i][j] = -0.25 * (1 + e[i]);
                    if (j == 2) tabN[i][j] = 0.25 * (1 + e[i]);
                    if (j == 3) tabN[i][j] = 0.25 * (1 - e[i]);
                }
            }

            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N[i][j] = 0.25 *(1-e[i]) * (1 - n[i]);
                    if (j == 1) N[i][j] = 0.25 *(1+e[i]) * (1 - n[i]);
                    if (j == 2) N[i][j] = 0.25 *(1+e[i]) * (1 + n[i]);
                    if (j == 3) N[i][j] = 0.25 *(1-e[i]) * (1 + n[i]);
                }
            }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow1[i][j] = 0.5 * (1-e_pow1[i]);
                    if (j == 1) N_pow1[i][j] = 0.5 * (1+e_pow1[i]);
                    if (j == 2) N_pow1[i][j] = 0;
                    if (j == 3) N_pow1[i][j] = 0;
                }
            }

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow2[i][j] = 0;
                    if (j == 1) N_pow2[i][j] = 0.5 * (1-n_pow2[i]);
                    if (j == 2) N_pow2[i][j] = 0.5 * (1+n_pow2[i]);
                    if (j == 3) N_pow2[i][j] = 0;
                }
            }

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow3[i][j] = 0;
                    if (j == 1) N_pow3[i][j] = 0;
                    if (j == 2) N_pow3[i][j] = 0.5 * (1-e_pow3[i]);
                    if (j == 3) N_pow3[i][j] = 0.5 * (1+e_pow3[i]);
                }
            }

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow4[i][j] = 0.5*(1-n_pow4[i]);
                    if (j == 1) N_pow4[i][j] = 0;
                    if (j == 2) N_pow4[i][j] = 0;
                    if (j == 3) N_pow4[i][j] = 0.5 * (1+n_pow4[i]);
                }
            }


        }
        else if(pc == 4){
            double a = -1*Math.sqrt((3.0/7.0)+((2.0/7.0)*Math.sqrt(6.0/5.0)));
            double b = -1*Math.sqrt((3.0/7.0)-((2.0/7.0)*Math.sqrt(6.0/5.0)));
            double c = Math.sqrt((3.0/7.0)-((2.0/7.0)*Math.sqrt(6.0/5.0)));
            double d = Math.sqrt((3.0/7.0)+((2.0/7.0)*Math.sqrt(6.0/5.0)));
            double e[] = {a,b,c,d,a,b,c,d,a,b,c,d,a,b,c,d};
            double n[] = {a,a,a,a,b,b,b,b,c,c,c,c,d,d,d,d};

            double e_pow1[] = {a, b, c, d};
            double e_pow2[] = {1, 1, 1, 1};
            double e_pow3[] = {d, c, b, a };
            double e_pow4[] = {-1, -1, -1, -1};
            double n_pow1[] = {-1, -1, -1, -1 };
            double n_pow2[] = {a, b, c, d};
            double n_pow3[] = {1,1, 1, 1};
            double n_pow4[] = {d, c, b, a };

            double f = (18+ Math.sqrt(30))/36;
            double g = (18-Math.sqrt(30))/36;

            tabE = new double[16][4];
            tabN = new double[16][4];
            N = new double[16][4];
            N_pow1 = new double[4][4];
            N_pow2 = new double[4][4];
            N_pow3 = new double[4][4];
            N_pow4 = new double[4][4];

            waga1 = new double[16];
            waga2 = new double[16];

            dxksi = new double[16];
            dyksi = new double[16];
            dxeta = new double[16];
            dyeta = new double[16];

            waga_pow1 = new double[]{g, f, f, g};

            waga1 = new double[]{(18.0-Math.sqrt(30.0))/36.0,(18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0,  (18.0-Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0,
                    (18.0+Math.sqrt(30.0))/36.0,(18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0,(18.0-Math.sqrt(30.0))/36.0,
                    (18.0-Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0};
            waga2 = new double[]{(18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0,
                    (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0,
                    (18.0+Math.sqrt(30.0))/36.0, (18.0+Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0, (18.0-Math.sqrt(30.0))/36.0,
                    (18.0-Math.sqrt(30.0))/36.0};


            for(int i = 0; i<16;i++){
                dyeta[i] = 0;
                dxeta[i] = 0;
                dxksi[i] = 0;
                dyksi[i] = 0;
            }

            for (int i = 0; i < 16; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabE[i][j] = -0.25 * (1 - n[i]);
                    if (j == 1) tabE[i][j] = 0.25 * (1 - n[i]);
                    if (j == 2) tabE[i][j] = 0.25 * (1 + n[i]);
                    if (j == 3) tabE[i][j] = -0.25 * (1 + n[i]);
                }
            }

            for (int i = 0; i < 16; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) tabN[i][j] = -0.25 * (1 - e[i]);
                    if (j == 1) tabN[i][j] = -0.25 * (1 + e[i]);
                    if (j == 2) tabN[i][j] = 0.25 * (1 + e[i]);
                    if (j == 3) tabN[i][j] = 0.25 * (1 - e[i]);
                }
            }
            for (int i = 0; i < 16; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N[i][j] = 0.25 *(1-e[i]) * (1 - n[i]);
                    if (j == 1) N[i][j] = 0.25 *(1+e[i]) * (1 - n[i]);
                    if (j == 2) N[i][j] = 0.25 *(1+e[i]) * (1 + n[i]);
                    if (j == 3) N[i][j] = 0.25 *(1-e[i]) * (1 + n[i]);
                }
            }
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow1[i][j] = 0.5 * (1-e_pow1[i]);
                    if (j == 1) N_pow1[i][j] = 0.5 * (1+e_pow1[i]);
                    if (j == 2) N_pow1[i][j] = 0;
                    if (j == 3) N_pow1[i][j] = 0;
                }
            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow2[i][j] = 0;
                    if (j == 1) N_pow2[i][j] = 0.5 * (1-n_pow2[i]);
                    if (j == 2) N_pow2[i][j] = 0.5 * (1+n_pow2[i]);
                    if (j == 3) N_pow2[i][j] = 0;
                }
            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow3[i][j] = 0;
                    if (j == 1) N_pow3[i][j] = 0;
                    if (j == 2) N_pow3[i][j] = 0.5 * (1-e_pow3[i]);
                    if (j == 3) N_pow3[i][j] = 0.5 * (1+e_pow3[i]);
                }
            }

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (j == 0) N_pow4[i][j] = 0.5*(1-n_pow4[i]);
                    if (j == 1) N_pow4[i][j] = 0;
                    if (j == 2) N_pow4[i][j] = 0;
                    if (j == 3) N_pow4[i][j] = 0.5 * (1+n_pow4[i]);
                }
            }

        }


    }
    public double[][] Jacobi(int e, int i, List<Node> wezly, List<Element> elementy, int pc/*, double[] x, double[] y*/){
        Elem4 temp = new Elem4(pc);
        double [][] Jacobi = new double[2][2];
        for(int j=0;j<4;j++){
            temp.dxksi[i]=temp.dxksi[i]+temp.tabE[i][j]*wezly.get(elementy.get(e).ID[j] - 1).x;
            //temp.dxksi[i]=temp.dxksi[i]+temp.tabE[i][j]*x[j];
        }

        for(int j=0;j<4;j++){
            temp.dyeta[i]=temp.dyeta[i]+temp.tabN[i][j]*wezly.get(elementy.get(e).ID[j] - 1).y;
            //temp.dyeta[i]=temp.dyeta[i]+temp.tabN[i][j]*y[j];
        }

        for(int j=0;j<4;j++){
            temp.dxeta[i]=temp.dxeta[i]+temp.tabN[i][j]*wezly.get(elementy.get(e).ID[j] - 1).x;
            //temp.dxeta[i]=temp.dxeta[i]+temp.tabN[i][j]*x[j];
        }

        for(int j=0;j<4;j++){
            temp.dyksi[i]=temp.dyksi[i]+temp.tabE[i][j]*wezly.get(elementy.get(e).ID[j] - 1).y;
            //temp.dyksi[i]=temp.dyksi[i]+temp.tabE[i][j]*y[j];
        }

        for(int j = 0; j<2;j++){
            for(int k = 0; k<2;k++){
                if(j==0 && k==0) Jacobi[j][k] = temp.dxksi[i];
                if(j==0 && k==1) Jacobi[j][k] = temp.dyksi[i];
                if(j==1 && k==0) Jacobi[j][k] = temp.dxeta[i];
                if(j==1 && k==1) Jacobi[j][k] = temp.dyeta[i];
            }
        }
        return Jacobi;
    }

    public double[][] odwrotnyJacobi(int e, int i, List<Node> wezly, List<Element> elementy, int pc, double detJ){
        Elem4 temp = new Elem4(pc);
        double [][] Jacobi = new double[2][2];
        Jacobi = Jacobi(e, i, wezly, elementy, pc);
        double [][] odwrotnyJacobi = new double[2][2];
        for(int j = 0; j<2;j++){
            for(int k = 0; k<2;k++){
                if(j==0 && k==0) odwrotnyJacobi[j][k] = (Jacobi[1][1])/detJ;
                if(j==0 && k==1) odwrotnyJacobi[j][k] = (-Jacobi[j][k])/detJ;
                if(j==1 && k==0) odwrotnyJacobi[j][k] = (-Jacobi[j][k])/detJ;
                if(j==1 && k==1) odwrotnyJacobi[j][k] = (Jacobi[0][0])/detJ;
            }
        }
        return odwrotnyJacobi;
    }


    public double[][] calc(int e, int i, List<Node> wezly, List<Element> elementy, int pc){
        Elem4 temp = new Elem4(pc);
        //double [] x = {0,4,4,0};
        //double [] y = {0,0,4,4};

        double [] dNdX = new double[4];
        double [] dNdY = new double[4];
        double [][] HPC = new double [4][4];


        double [][] Jacobi = new double[2][2];
        double detJ=0;
        double [][] odwrotnyJacobi = new double[2][2];

//        for(int j=0;j<4;j++){
//            temp.dxksi[i]=temp.dxksi[i]+temp.tabE[i][j]*wezly.get(elementy.get(e).ID[j] - 1).x;
//        }
//
//        for(int j=0;j<4;j++){
//            temp.dyeta[i]=temp.dyeta[i]+temp.tabN[i][j]*wezly.get(elementy.get(e).ID[j] - 1).y;
//        }
//
//        for(int j=0;j<4;j++){
//            temp.dxeta[i]=temp.dxeta[i]+temp.tabN[i][j]*wezly.get(elementy.get(e).ID[j] - 1).x;
//        }
//
//        for(int j=0;j<4;j++){
//            temp.dyksi[i]=temp.dyksi[i]+temp.tabE[i][j]*wezly.get(elementy.get(e).ID[j] - 1).y;
//        }
//
//        for(int j = 0; j<2;j++){
//            for(int k = 0; k<2;k++){
//                if(j==0 && k==0) Jacobi[j][k] = temp.dxksi[i];
//                if(j==0 && k==1) Jacobi[j][k] = temp.dyksi[i];
//                if(j==1 && k==0) Jacobi[j][k] = temp.dxeta[i];
//                if(j==1 && k==1) Jacobi[j][k] = temp.dyeta[i];
//            }
//        }
        Jacobi = Jacobi(e, i, wezly, elementy,pc/*, x, y*/);
//        System.out.println("Macierz Jacobiego dla " + (i+1) + " punktu calkowania: ");
//        for(int j = 0; j<2;j++){
//            for(int k = 0; k<2;k++){
//                System.out.print(Jacobi[j][k] + " ");
//            }
//            System.out.println();
//        }
//        System.out.println();

//        System.out.println("Wyznacznik Jacobiego dla " + (i+1) + " punktu calkowania: ");
        detJ = Jacobi[0][0]*Jacobi[1][1]-Jacobi[0][1]*Jacobi[1][0];
//        System.out.println(detJ);
//        System.out.println();
//        System.out.println();
//        for(int j = 0; j<2;j++){
//            for(int k = 0; k<2;k++){
//                if(j==0 && k==0) odwrotnyJacobi[j][k] = (Jacobi[1][1])/detJ;
//                if(j==0 && k==1) odwrotnyJacobi[j][k] = (-Jacobi[j][k])/detJ;
//                if(j==1 && k==0) odwrotnyJacobi[j][k] = (-Jacobi[j][k])/detJ;
//                if(j==1 && k==1) odwrotnyJacobi[j][k] = (Jacobi[0][0])/detJ;
//            }
//        }
        odwrotnyJacobi = odwrotnyJacobi(e,i, wezly, elementy, pc, detJ);
//        System.out.println("Odwrocona macierz Jacobiego dla " + (i+1) + " punktu calkowania: ");
//        for(int j = 0; j<2;j++){
//            for(int k = 0; k<2;k++){
//                System.out.print(odwrotnyJacobi[j][k] + " ");
//            }
//            System.out.println();
//        }

        for(int j = 0; j<4; j++){
            dNdX[j] = odwrotnyJacobi[0][0] * temp.tabE[i][j] + odwrotnyJacobi[0][1] * temp.tabN[i][j];
            dNdY[j] = odwrotnyJacobi[1][0] * temp.tabE[i][j] + odwrotnyJacobi[1][1] * temp.tabN[i][j];
        }

//        System.out.println();
//        for(int j = 0; j<4; j++){
//            System.out.println("dN/dx: " + dNdX[j] + ", dN/dy: " + dNdY[j]);
//        }

        double [][] dNdXxdNdXT = new double[4][4];
        double [][] dNdYxdNdYT = new double[4][4];

        for(int k = 0; k<4;k++)
        {
            for(int j = 0; j<4;j++){
                dNdXxdNdXT[k][j] = dNdX[k]*dNdX[j];
                dNdYxdNdYT[k][j] = dNdY[k]*dNdY[j];
            }
        }

//        System.out.println();
//        for(int k = 0; k<4;k++)
//        {
//            for(int j = 0; j<4;j++){
//                System.out.print(dNdXxdNdXT[k][j] + " ");
//            }
//            System.out.println();
//        }


//        System.out.println();
//        for(int k = 0; k<4;k++)
//        {
//            for(int j = 0; j<4;j++){
//                System.out.print( dNdYxdNdYT[k][j] + " ");
//            }
//            System.out.println();
//        }

        for(int k = 0; k<4;k++){
            for(int j = 0; j<4; j++){
                HPC[k][j] = ((dNdXxdNdXT[k][j] + dNdYxdNdYT[k][j])*25*detJ)*temp.waga1[i]*temp.waga2[i];
            }
        }

        double [][] H = new double[4][4];

        for(int l = 0; l < 4; l++ ){
            for(int m = 0; m<4; m++){
                H[l][m] = 0;
            }
        }

        for(int k = 0; k<4;k++){
            for(int j = 0; j<4; j++){
                H[k][j] = ((dNdXxdNdXT[k][j] + dNdYxdNdYT[k][j])*25*detJ) * temp.waga1[i] * temp.waga2[i];
            }
        }


//        System.out.println();
//        for(int k = 0; k<4;k++){
//            for(int j = 0; j<4; j++){
//                System.out.print(HPC[k][j] + " ");
//            }
//            System.out.println();
//        }
//        System.out.println();




    return H;

    }

    public double[][] calc2(int e, int i, List<Node> wezly, List<Element> elementy, int pc,double ro, double cp){
        Elem4 temp = new Elem4(pc);
        double [][] Jacobi = new double[2][2];
        double detJ=0;

        Jacobi = Jacobi(e, i, wezly, elementy,pc);
        detJ = Jacobi[0][0]*Jacobi[1][1]-Jacobi[0][1]*Jacobi[1][0];

        double [][] dNxdNT = new double[4][4];
        for(int k = 0; k<4;k++)
        {
            for(int j = 0; j<4;j++){
                dNxdNT[k][j] = temp.N[i][k]*temp.N[i][j];
            }
        }
        double [][] C = new double[4][4];
        for(int l = 0; l < 4; l++ ){
            for(int m = 0; m<4; m++){
                C[l][m] = 0;
            }
        }

        for(int k = 0; k<4;k++){
            for(int j = 0; j<4; j++){
                C[k][j] = ((dNxdNT[k][j])*cp*ro*detJ) * temp.waga1[i] * temp.waga2[i];
            }
        }

        return C;
    }

    double detJHBC(Node node1, Node node2){
        return Math.sqrt(Math.pow(node2.x - node1.x,2) + Math.pow(node2.y - node1.y,2))/2;
    }

    public double[][] calcHBC(int e, int i, List<Node> wezly, List<Element> elementy, double alfa, int pc){
        Elem4 temp = new Elem4(pc);
        Node boki[][] = new Node[4][2];

        for(int j=0; j<4; j++){
                if(wezly.get(elementy.get(e).ID[j] - 1).BC==1 && wezly.get(elementy.get(e).ID[(j+1)%4] - 1).BC==1){
                    boki[j][0] = wezly.get(elementy.get(e).ID[j] - 1);
                    boki[j][1] = wezly.get(elementy.get(e).ID[(j+1)%4] - 1);
                }
        }

        double HBC[][] = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};
        //Powierzchnia 1
        if(boki[0][0] != null && boki[0][1] != null){
            double detJ = detJHBC(boki[0][0], boki[0][1]);

            for(int k = 0; k<4; k++){
                for(int l = 0; l<4; l++){
                    HBC[k][l] += ((temp.N_pow1[i][k]*temp.N_pow1[i][l])*alfa*detJ) * temp.waga_pow1[i];                }
            }

        }
        //Powierzchnia 2
        if(boki[1][0] != null && boki[1][1] != null){
            double detJ = detJHBC(boki[1][0], boki[1][1]);

            for(int k = 0; k<4; k++){
                for(int l = 0; l<4; l++){
                    HBC[k][l] += ((temp.N_pow2[i][k]*temp.N_pow2[i][l])*alfa*detJ) * temp.waga_pow1[i];
                }
            }

        }
        //Powierzchnia 3
        if(boki[2][0] != null && boki[2][1] != null){
            double detJ = detJHBC(boki[2][0], boki[2][1]);

            for(int k = 0; k<4; k++){
                for(int l = 0; l<4; l++){
                    HBC[k][l] += ((temp.N_pow3[i][k]*temp.N_pow3[i][l])*alfa*detJ) * temp.waga_pow1[i];
                }
            }

        }
        //Powierzchnia 4
        if(boki[3][0] != null && boki[3][1] != null){
            double detJ = detJHBC(boki[3][0], boki[3][1]);

            for(int k = 0; k<4; k++){
                for(int l = 0; l<4; l++){
                    HBC[k][l] += ((temp.N_pow4[i][k]*temp.N_pow4[i][l])*alfa*detJ) * temp.waga_pow1[i];
                }
            }

        }

        return HBC;
    }

    public double[] calcP(int e, int i, List<Node> wezly, List<Element> elementy, double alfa, double talfa, int pc){
        Elem4 temp = new Elem4(pc);
        Node boki[][] = new Node[4][2];

        for(int j=0; j<4; j++){
            if(wezly.get(elementy.get(e).ID[j] - 1).BC==1 && wezly.get(elementy.get(e).ID[(j+1)%4] - 1).BC==1){
                boki[j][0] = wezly.get(elementy.get(e).ID[j] - 1);
                boki[j][1] = wezly.get(elementy.get(e).ID[(j+1)%4] - 1);
            }
        }

        double [] P = {0,0,0,0};
        //Powierzchnia 1
        if(boki[0][0] != null && boki[0][1] != null){
            double detJ = detJHBC(boki[0][0], boki[0][1]);

                for(int l = 0; l<4; l++){
                    P[l] += (-alfa)*N_pow1[i][l]*waga_pow1[i]*talfa*detJ;
                }


        }
        //Powierzchnia 2
        if(boki[1][0] != null && boki[1][1] != null){
            double detJ = detJHBC(boki[1][0], boki[1][1]);

                for(int l = 0; l<4; l++){
                    P[l] += (-alfa)*N_pow2[i][l]*waga_pow1[i]*talfa*detJ;
                }


        }
        //Powierzchnia 3
        if(boki[2][0] != null && boki[2][1] != null){
            double detJ = detJHBC(boki[2][0], boki[2][1]);

                for(int l = 0; l<4; l++){
                    P[l] += (-alfa)*N_pow3[i][l]*waga_pow1[i]*talfa*detJ;
                }


        }
        //Powierzchnia 4
        if(boki[3][0] != null && boki[3][1] != null){
            double detJ = detJHBC(boki[3][0], boki[3][1]);

                for(int l = 0; l<4; l++){
                    P[l] += (-alfa)*N_pow4[i][l]*waga_pow1[i]*talfa*detJ;
                }

        }

        return P;
    }



}

