package com.example;

public class Node {
    public double x, y, t0;
    int BC;
    GlobalData sprawdzacz = new GlobalData();
    public Node(double x, double y){
        this.x = x;
        this.y = y;
        this.t0 = sprawdzacz.t0;
        if(this.x == 0 || this.y == 0 || this.x == sprawdzacz.W || this.y == sprawdzacz.H){
            this.BC = 1;
        }
        else{
            this.BC = 0;
        }
    }
}
