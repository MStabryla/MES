package com.example;

public class calkaKwadraturyGaussa {
    public double f(double x, double y)
    {
        return (-2*x*x*y) +2*x*y + 4;
    }

    public double dwaWezly(){
        double x[] = {-0.577,0.577};
        double y[] = {-0.577,0.577};
        double A[] = {1,1};
        double wynik = 0;

        for(int i = 0; i<2; i++){
            for(int j = 0; j<2; j++){
                wynik += f(x[j],y[i])*A[j]*A[i];
            }
        }
        return wynik;
    }
    public double trzyWezly(){
        double x[]={-0.77,0,0.77};
        double y[]={-0.77,0,0.77};
        double A[]={0.555555,0.8888888889,0.555555};

        double wynik = 0;

        for(int i = 0; i<3; i++){
            for(int j = 0; j<3; j++){
                wynik += f(x[j],y[i])*A[j]*A[i];
            }
        }
        return wynik;
    }

}
