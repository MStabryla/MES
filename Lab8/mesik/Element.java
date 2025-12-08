package com.company;

public class Element {
    public int[] elementID = new int[4];
    public double[][] HL = new double[4][4];
    public double[][] CL = new double[4][4];
    public double[][] HBC = new double[4][4];
    public double[] PL = new double[4];

    public Element(int id1, int id2, int id3, int id4){
        elementID[0] = id1;
        elementID[1] = id2;
        elementID[2] = id3;
        elementID[3] = id4;
    }

    public void displayElement(int number){
        String text = "Element " + number + ": {" + elementID[0] + ", " + elementID[1] + ", " + elementID[2] + ", " + elementID[3] + "}";
        System.out.println(text);
    }
}
