package com.company;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class InputData {
    public double H;
    public double W;
    public int nH;
    public int nW;
    public int nE ;
    public int nN;
    public int numberOfIP;
    int t0;
    int tn;
    int dt;
    int ta;
    int ro;
    int c;
    int k;
    int alfa;
    public InputData() {
        try{
            File file = new File("dane2.txt");
            Scanner scanner = new Scanner(file);
            this.H =  Double.parseDouble(scanner.nextLine());
            this.W =  Double.parseDouble(scanner.nextLine());
            this.nH = Integer.parseInt(scanner.nextLine());
            this.nW = Integer.parseInt(scanner.nextLine());
            this.numberOfIP = Integer.parseInt(scanner.nextLine());
            this.t0 = Integer.parseInt(scanner.nextLine());
            this.tn = Integer.parseInt(scanner.nextLine());
            this.dt = Integer.parseInt(scanner.nextLine());
            this.ta = Integer.parseInt(scanner.nextLine());
            this.c = Integer.parseInt(scanner.nextLine());
            this.ro = Integer.parseInt(scanner.nextLine());
            this.k = Integer.parseInt(scanner.nextLine());
            this.alfa = Integer.parseInt(scanner.nextLine());
            scanner.close();
        } catch(FileNotFoundException e){
            e.printStackTrace();
        }
        this.nE = (nW - 1) * (nH - 1);
        this.nN = nW * nH;
    }

    public void printAllInputData(){
        System.out.println("Height H = " + H);
        System.out.println("Width W = " + W);
        System.out.println("Number of nodes (by height) nH = " + nH);
        System.out.println("Number of nodes (by width) nW = " + nW);
        System.out.println("Number of elements nE = " + nE);
        System.out.println("Number of nodes nN = " + nN);
        System.out.println("Number of integration points numberOfIP = " + numberOfIP);
        System.out.println("Initial temperature t0 = " + t0);
        System.out.println("Simulation time tn = " + tn);
        System.out.println("Simulation step time dt = " + dt);
        System.out.println("Ambient temperature ta = " + ta);
        System.out.println("Specific heat c = " + c);
        System.out.println("Density ro = " + ro);
        System.out.println("Conductivity k = " + k);
        System.out.println("Alfa = " + alfa);
    }
}
