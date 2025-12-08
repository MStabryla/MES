package com.company;

public class HCBCMatrix {

    public void printJacobi(int i, double[][] JacobianMatrix, double[][] invertedJacobianMatrix, double det) {
        System.out.println("\n\t" + (i+1) + " INTEGRATION POINT:\n\tJacobian matrix");
        System.out.println("\t" + JacobianMatrix[0][0] + "   " + JacobianMatrix[0][1] + "\n\t" + JacobianMatrix[1][0] + "   " + JacobianMatrix[1][1]);
        System.out.println("\tDeterminant = " + det);
        System.out.println("\tInverted Jacobian matrix:");
        System.out.println("\t" + invertedJacobianMatrix[0][0] + "   " + invertedJacobianMatrix[0][1] + "\n\t" + invertedJacobianMatrix[1][0] + "   " + invertedJacobianMatrix[1][1]);
    }

    public double[][] calcHlp(int ipNumber, int ip, double[] x, double[] y) {
        InputData inputData = new InputData();
        Elem4 elem4 = new Elem4(ipNumber);

        double det = 0;
        double[][] JacobianMatrix;
        double[][] invertedJacobianMatrix;

        double[][] dNdx_dNxT = new double[4][4];
        double[][] dNdy_dNyT = new double[4][4];
        double[][] HLpc = new double[4][4];

        for (int i=0; i<4; i++) {
            elem4.derXXi[ip] += x[i] * elem4.derNXi[ip][i];
            elem4.derYXi[ip] += y[i] * elem4.derNXi[ip][i];
            elem4.derYEta[ip] += y[i] * elem4.derNEta[ip][i];
            elem4.derXEta[ip] += x[i] * elem4.derNEta[ip][i];
        }

        JacobianMatrix = new double[][]{{elem4.derXXi[ip], elem4.derYXi[ip]}, {elem4.derXEta[ip], elem4.derYEta[ip]}};
        det = JacobianMatrix[0][0] * JacobianMatrix[1][1] - JacobianMatrix[1][0] * JacobianMatrix[0][1];
        invertedJacobianMatrix = new double[][]{{1 / det * JacobianMatrix[1][1], (-1) / det * JacobianMatrix[0][1]}, {(-1) / det * JacobianMatrix[1][0], 1 / det * JacobianMatrix[0][0]}};

        //tablica elementow dN/dx i dN/dy
        double[] derNX = new double[4];
        double[] derNY = new double[4];

        for (int j = 0; j < 4; j++) {
            derNX[j] = invertedJacobianMatrix[0][0] * elem4.derNXi[ip][j] + invertedJacobianMatrix[0][1] * elem4.derNEta[ip][j];
            derNY[j] = invertedJacobianMatrix[1][0] * elem4.derNXi[ip][j] + invertedJacobianMatrix[1][1] * elem4.derNEta[ip][j];

        }

        for (int k = 0; k < 4; k++) {
            for (int l = 0; l < 4; l++) {
                dNdx_dNxT[k][l] = derNX[k] * derNX[l];
                dNdy_dNyT[k][l] = derNY[k] * derNY[l];
            }
        }
        for (int k = 0; k < 4; k++) {
            for (int l = 0; l < 4; l++) {
                HLpc[k][l] = inputData.k * (dNdx_dNxT[k][l] + dNdy_dNyT[k][l]) * det * elem4.w1[ip] * elem4.w2[ip];
            }
        }
        return HLpc;
    }

    public double[][] calcClp(int ipNumber, int ip, double[] x, double[] y, double ro, double c) {

        Elem4 elem4 = new Elem4(ipNumber);
        for (int i=0; i<4; i++) {
            elem4.derXXi[ip] += x[i] * elem4.derNXi[ip][i];
            elem4.derYXi[ip] += y[i] * elem4.derNXi[ip][i];
            elem4.derYEta[ip] += y[i] * elem4.derNEta[ip][i];
            elem4.derXEta[ip] += x[i] * elem4.derNEta[ip][i];
        }

        double[][] JacobianMatrix;
        double[][] CLpc = new double[4][4];
        double det = 0;

        JacobianMatrix = new double[][]{{elem4.derXXi[ip], elem4.derYXi[ip]}, {elem4.derXEta[ip], elem4.derYEta[ip]}};
        det = JacobianMatrix[0][0] * JacobianMatrix[1][1] - JacobianMatrix[1][0] * JacobianMatrix[0][1];

        double[][] dNdNT = new double[4][4];

        for (int k = 0; k < 4; k++) {
            for (int l = 0; l < 4; l++) {
                dNdNT[k][l] = elem4.N[ip][k] * elem4.N[ip][l];
            }
        }

        for (int k = 0; k < 4; k++) {
            for (int l = 0; l < 4; l++) {
                CLpc[k][l] = dNdNT[k][l] * c * ro * det * elem4.w1[ip] * elem4.w2[ip];
            }
        }

        return CLpc;
    }

    public double[][] calcHBC(int ipNumber, int ip, Element[] elementArray, Node[] nodeArray, double alfa, int elemNum) {

        Elem4 elem4 = new Elem4(ipNumber);
        Node[] edge1Nodes = {nodeArray[elementArray[elemNum].elementID[0] - 1], nodeArray[elementArray[elemNum].elementID[1] - 1]};
        Node[] edge2Nodes = {nodeArray[elementArray[elemNum].elementID[1] - 1], nodeArray[elementArray[elemNum].elementID[2] - 1]};
        Node[] edge3Nodes = {nodeArray[elementArray[elemNum].elementID[2] - 1], nodeArray[elementArray[elemNum].elementID[3] - 1]};
        Node[] edge4Nodes = {nodeArray[elementArray[elemNum].elementID[3] - 1], nodeArray[elementArray[elemNum].elementID[0] - 1]};

        double[][] AEdge1 = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};
        double[][] AEdge2 = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};
        double[][] AEdge3 = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};
        double[][] AEdge4 = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};
        double[][] A1 = new double[4][4];
        double[][] A2 = new double[4][4];
        double[][] A3 = new double[4][4];
        double[][] A4 = new double[4][4];

        if (edge1Nodes[0].bcFlag && edge1Nodes[1].bcFlag) {
            AEdge1[0][0] += elem4.N_edge1[ip][0]*elem4.N_edge1[ip][0]*alfa;
            AEdge1[0][1] += elem4.N_edge1[ip][0]*elem4.N_edge1[ip][1]*alfa;
            AEdge1[1][0] += elem4.N_edge1[ip][1]*elem4.N_edge1[ip][0]*alfa;
            AEdge1[1][1] += elem4.N_edge1[ip][1]*elem4.N_edge1[ip][1]*alfa;
            double det =  Math.sqrt(Math.pow(edge1Nodes[1].x - edge1Nodes[0].x,2) + Math.pow(edge1Nodes[1].y - edge1Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                for(int j = 0; j<4; j++){
                    A1[i][j] = AEdge1[i][j] * det * elem4.wBC[ip];
                }
            }
        }
        if (edge2Nodes[0].bcFlag && edge2Nodes[1].bcFlag) {
            AEdge2[1][1] += elem4.N_edge2[ip][1]*elem4.N_edge2[ip][1]*alfa;
            AEdge2[1][2] += elem4.N_edge2[ip][1]*elem4.N_edge2[ip][2]*alfa;
            AEdge2[2][1] += elem4.N_edge2[ip][2]*elem4.N_edge2[ip][1]*alfa;
            AEdge2[2][2] += elem4.N_edge2[ip][2]*elem4.N_edge2[ip][2]*alfa;
            double det =  Math.sqrt(Math.pow(edge2Nodes[1].x - edge2Nodes[0].x,2) + Math.pow(edge2Nodes[1].y - edge2Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                for(int j = 0; j<4; j++){
                    A2[i][j] = AEdge2[i][j]*det*elem4.wBC[ip];
                }
            }
        }
        if (edge3Nodes[0].bcFlag && edge3Nodes[1].bcFlag) {
            AEdge3[2][2] += elem4.N_edge3[ip][2]*elem4.N_edge3[ip][2]*alfa;
            AEdge3[2][3] += elem4.N_edge3[ip][2]*elem4.N_edge3[ip][3]*alfa;
            AEdge3[3][2] += elem4.N_edge3[ip][3]*elem4.N_edge3[ip][2]*alfa;
            AEdge3[3][3] += elem4.N_edge3[ip][3]*elem4.N_edge3[ip][3]*alfa;
            double det =  Math.sqrt(Math.pow(edge3Nodes[1].x - edge3Nodes[0].x,2) + Math.pow(edge3Nodes[1].y - edge3Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                for(int j = 0; j<4; j++){
                    A3[i][j] = AEdge3[i][j]*det*elem4.wBC[ip];
                }
            }
        }
        if (edge4Nodes[0].bcFlag && edge4Nodes[1].bcFlag) {
            AEdge4[0][0] += elem4.N_edge4[ip][0]*elem4.N_edge4[ip][0]*alfa;
            AEdge4[0][3] += elem4.N_edge4[ip][0]*elem4.N_edge4[ip][3]*alfa;
            AEdge4[3][0] += elem4.N_edge4[ip][3]*elem4.N_edge4[ip][0]*alfa;
            AEdge4[3][3] += elem4.N_edge4[ip][3]*elem4.N_edge4[ip][3]*alfa;
            double det =  Math.sqrt(Math.pow(edge4Nodes[1].x - edge4Nodes[0].x,2) + Math.pow(edge4Nodes[1].y - edge4Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                for(int j = 0; j<4; j++){
                    A4[i][j] = AEdge4[i][j]*det*elem4.wBC[ip];
                }
            }
        }

        double[][] HBCL = {{0,0,0,0}, {0,0,0,0}, {0,0,0,0}, {0,0,0,0}};

        for(int i = 0; i<4; i++){
            for(int j = 0; j<4; j++){
                HBCL[i][j] = A1[i][j] + A2[i][j] + A3[i][j] + A4[i][j];
            }
        }

        return HBCL;
    }
    public double[] calcPL(int ipNumber, int ip, Element[] elementArray, Node[] nodeArray, double ta, double alfa, int elemNum) {
        double[] PL = {0,0,0,0};

        Elem4 elem4 = new Elem4(ipNumber);
        Node[] edge1Nodes = {nodeArray[elementArray[elemNum].elementID[0] - 1], nodeArray[elementArray[elemNum].elementID[1] - 1]};
        Node[] edge2Nodes = {nodeArray[elementArray[elemNum].elementID[1] - 1], nodeArray[elementArray[elemNum].elementID[2] - 1]};
        Node[] edge3Nodes = {nodeArray[elementArray[elemNum].elementID[2] - 1], nodeArray[elementArray[elemNum].elementID[3] - 1]};
        Node[] edge4Nodes = {nodeArray[elementArray[elemNum].elementID[3] - 1], nodeArray[elementArray[elemNum].elementID[0] - 1]};


        if(edge1Nodes[0].bcFlag && edge1Nodes[1].bcFlag){
            double det = Math.sqrt(Math.pow(edge1Nodes[1].x - edge1Nodes[0].x,2) + Math.pow(edge1Nodes[1].y - edge1Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                PL[i] += (-alfa)*elem4.N_edge1[ip][i]*elem4.wBC[ip]*ta*det;
            }
        }

        if(edge2Nodes[0].bcFlag && edge2Nodes[1].bcFlag){
            double det = Math.sqrt(Math.pow(edge2Nodes[1].x - edge2Nodes[0].x,2) + Math.pow(edge2Nodes[1].y - edge2Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                PL[i] += (-alfa)*elem4.N_edge2[ip][i]*elem4.wBC[ip]*ta*det;
            }
        }

        if(edge3Nodes[0].bcFlag && edge3Nodes[1].bcFlag){
            double det = Math.sqrt(Math.pow(edge3Nodes[1].x - edge3Nodes[0].x,2) + Math.pow(edge3Nodes[1].y - edge3Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                PL[i] += (-alfa)*elem4.N_edge3[ip][i]*elem4.wBC[ip]*ta*det;
            }
        }

        if(edge4Nodes[0].bcFlag && edge4Nodes[1].bcFlag){
            double det = Math.sqrt(Math.pow(edge4Nodes[1].x - edge4Nodes[0].x,2) + Math.pow(edge4Nodes[1].y - edge4Nodes[0].y,2))/2;
            for(int i = 0; i<4; i++){
                PL[i] += (-alfa)*elem4.N_edge4[ip][i]*elem4.wBC[ip]*ta*det;
            }
        }


        return PL;
    }
}

