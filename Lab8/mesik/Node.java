package com.company;

public class Node {
    public double x;
    public double y;
    public double t0;
    public boolean bcFlag;
    public Node(double a, double b){
        x=a; y= b;
        bcFlag = false;
    }
    public Node(double a, double b, double t){
        x=a; y= b; t0 = t;
    }
    public void displayNode(int number){
        String text = "Node " + number + ": (" + x +", " + y + ")";
        System.out.println(text);
    }
}
