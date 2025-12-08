package com.company;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Main {

    static public Element[] elementArray;
    static public Node[] nodeArray;

    public static void creatingNodesElements() {
        //tworzenie siatki mes, zapelnienie jej liczbami
        InputData inputData = new InputData();
        elementArray = new Element[inputData.nE];
        nodeArray = new Node[inputData.nN];

        double dx = inputData.W / (inputData.nW - 1);
        double dy = inputData.H / (inputData.nH - 1);
        //System.out.println("Dx: " + dx + ", Dy: " + dy );
        int nodeCounter = 0;

        try {
            for (int j=0; j < inputData.nW; j++){
                for (int i=0; i < inputData.nH; i++){
                    double x = dx * j;
                    double y = dy * i;
                    nodeArray[nodeCounter] = new Node(x,y);
                    nodeArray[nodeCounter].t0 = inputData.t0;
                    if (x == 0 || y == 0 || x == inputData.W || y == inputData.H) {
                        nodeArray[nodeCounter].bcFlag = true;
                    }
                    nodeCounter++;
                }
            }
        } catch (Exception e){
            e.printStackTrace();
        }



        int i = 1;
        for (int k=0; k<inputData.nE; k++){
            if (i%(inputData.nH)==0){ //if we finish elements at top, we need to add 1 cause the node stays at top
                i++;
            }
            int id1, id2, id3, id4;
            id1 = i;
            id2 = id1 + inputData.nH;
            id3 = id2 + 1;
            id4 = id1 + 1;
            elementArray[k] = new Element(id1, id2, id3, id4);

            i++;
        }
    }


    static void calculating_H_and_C_Matrix(){

        creatingNodesElements();

        InputData inputData = new InputData();
        inputData.printAllInputData();

        SoE soe = new SoE();

        HCBCMatrix hcbcMatrix = new HCBCMatrix();

        double it = inputData.tn/inputData.dt;

        int iterationNumber = (int)(it);

        for (int n = 0; n <  iterationNumber; n++) {
            for (int elem = 0; elem < inputData.nE; elem++) {
                double[] x = new double[4];
                double[] y = new double[4];
                double[][] HL = new double[4][4];
                double[][] CL = new double[4][4];
                double[][] HBCL = new double[4][4];
                double[] PL = new double[4];
                double[][] H = new double [4][4];
                double[][] C = new double [4][4];


                for (int iP = 0; iP < inputData.numberOfIP * inputData.numberOfIP; iP++) {
                    for (int a=0; a<4; a++){
                        x[a] = nodeArray[elementArray[elem].elementID[a] - 1].x;
                        y[a] = nodeArray[elementArray[elem].elementID[a] - 1].y;
                    }
                    HL = hcbcMatrix.calcHlp(inputData.numberOfIP, iP, x, y);
                    CL = hcbcMatrix.calcClp(inputData.numberOfIP, iP, x, y, inputData.ro, inputData.c);


                    for (int k = 0; k < 4; k++) {
                        for (int l = 0; l < 4; l++) {
                            H[k][l] += HL[k][l];
                            C[k][l] += CL[k][l];
                        }
                    }

                    elementArray[elem].HL = H;
                    elementArray[elem].CL = C;
                }

                for (int iP = 0; iP < inputData.numberOfIP; iP++) {
                    HBCL = hcbcMatrix.calcHBC(inputData.numberOfIP, iP, elementArray, nodeArray, inputData.alfa, elem);
                    PL = hcbcMatrix.calcPL(inputData.numberOfIP, iP, elementArray, nodeArray, inputData.ta, inputData.alfa, elem);
                    for(int k = 0; k<4; k++){
                        for(int l = 0; l<4; l++){
                            elementArray[elem].HBC[k][l] += HBCL[k][l];
                        }
                    }
                    for (int i = 0; i < 4; i++) {
                        elementArray[elem].PL[i] += PL[i];
                    }
                }

                for(int k = 0; k<4;k++){
                    for(int l = 0; l<4; l++){
                        elementArray[elem].HL[k][l] += elementArray[elem].HBC[k][l];
                    }
                }

                for (int i = 0; i < 4; i++){
                    for (int j = 0; j < 4; j++){
                        soe.HG[elementArray[elem].elementID[i]-1][elementArray[elem].elementID[j]-1] += elementArray[elem].HL[i][j];
                        soe.CG[elementArray[elem].elementID[i]-1][elementArray[elem].elementID[j]-1] += elementArray[elem].CL[i][j];
                    }
                }

            }

            for(int elem = 0; elem < inputData.nE; elem++){
                for(int i = 0; i < 4; i++) {
                    soe.PG[elementArray[elem].elementID[i] - 1] += elementArray[elem].PL[i];
                }
            }
            

            for (int i = 0; i<inputData.nN; i++){
                for (int j = 0; j < inputData.nN; j++) {
                    soe.HG[i][j] += (soe.CG[i][j] / inputData.dt);
                    soe.PG[i] = soe.PG[i] - ((soe.CG[i][j]/inputData.dt) * nodeArray[j].t0) ;
                }
            }

            for (int i = 0; i<inputData.nN; i++){
                soe.PG[i] = (-1) * soe.PG[i];
            }

            double[] t1 = soe.gaussEquation(soe.HG, soe.PG);
            List<Double> t1List = new ArrayList<>();

            for (double v : t1) {
                t1List.add(v);
            }

            for(int i=0; i< inputData.nN; i++){
                nodeArray[i].t0 = t1List.get(i);
            }

            double min = Collections.min(t1List), max = Collections.max(t1List);

            System.out.println("Time: " + ((n+1)*inputData.dt) + " Minimum: " + min + " Maximum: " + max );

            for (int i =0; i < inputData.nE; i++){
                for (int j = 0; j < 4; j++){
                    for (int k = 0; k < 4; k++){
                        elementArray[i].HBC[j][k] = 0.0;
                        elementArray[i].HL[j][k] = 0.0;
                        elementArray[i].CL[j][k] = 0.0;
                        elementArray[i].PL[j] = 0.0;
                    }
                }
            }
            for (int j = 0; j < inputData.nN; j++){
                for (int k = 0; k < inputData.nN; k++){
                    soe.HG[j][k] = 0.0;
                    soe.CG[j][k] = 0.0;
                    soe.PG[j] = 0.0;
                }
            }
        }
    }


    public static void main(String[] args) {
        calculating_H_and_C_Matrix();
    }
}
