package com.example;

import java.io.*;

import java.util.Scanner;

public class GlobalData {
    double W, H, ro, cp, t0, alfa, talfa;
    int nH, nW, nE, nN, pc, deltatau, simulationtime;

    public GlobalData(){
        try{
            File file = new File("dane.txt");
            Scanner Reader = new Scanner(file);
            this.W = Reader.nextDouble();
            this.H = Reader.nextDouble();
            this.nW = Reader.nextInt();
            this.nH = Reader.nextInt();
            this.ro = Reader.nextDouble();
            this.cp = Reader.nextDouble();
            this.t0 = Reader.nextDouble();
            this.alfa = Reader.nextDouble();
            this.talfa = Reader.nextDouble();
            this.deltatau = Reader.nextInt();
            this.simulationtime = Reader.nextInt();
            this.pc = Reader.nextInt();

            this.nE = (nW-1)*(nH-1);
            this.nN = nW*nH;

            Reader.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }
}
