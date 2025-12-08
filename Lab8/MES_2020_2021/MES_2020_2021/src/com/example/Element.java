package com.example;

public class Element {
    public int ID[];
    public double [][] Hl = new double[4][4];
    public double [][] Cl = new double[4][4];
    public double [][] HBC = new double[4][4];
    public double [] Pl = new double[4];

    public Element(){
        this.ID = new int[4];
    }
}
