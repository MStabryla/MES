package com.example;


import java.util.ArrayList;
import java.util.List;

public class Main {

    public static void main(String[] args) {
	GlobalData global = new GlobalData();
	SOE soe = new SOE();

	double deltaX;
	double deltaY;
	deltaX = (global.W)/(global.nW-1);
	deltaY = (global.H)/(global.nH-1);
	List<Node> wezly = new ArrayList<>();
	List<Element> elementy = new ArrayList<>();


	//uzupełnienie węzłów
    for(int i=0; i < global.nW; i++)
    {
       for(int j = 0; j < global.nH; j++)
       {
           double x = i * deltaX;
           double y = j * deltaY;
            wezly.add(new Node(x, y));
       }
    }

    //uzupełnienie elementów
    for(int i =1; i < global.nE+(global.nW-1); i++)
    {

       if(i%global.nH==0) {
       continue;
       }
       Element pomoc = new Element();
       pomoc.ID[0] = i;
       pomoc.ID[1] = pomoc.ID[0] + global.nH ;
       pomoc.ID[2] = pomoc.ID[1] + 1;
       pomoc.ID[3] = pomoc.ID[0] + 1;
       elementy.add(pomoc);
    }





//    //wypisanie
//	for(int i = 0; i< gl.nE; i++)
//	{
//        System.out.println("Element: " + (i+1));
//        System.out.println("Wezly");
//		for(int j = 0; j<4;j++)
//		{
//            System.out.print(elementy.get(i).ID[j] + ": ");
//            System.out.println("x: " + wezly.get(elementy.get(i).ID[j] - 1).x + ", y: " + wezly.get(elementy.get(i).ID[j] - 1).y);
//		}
//	}
        double [][] pomocWynik;
        double [][] pomocWynik2;
        double [] pomocWynik3;
        Elem4 elem4 = new Elem4(global.pc);

        int kroki = global.simulationtime/ global.deltatau;

        for(int p = 0; p<kroki; p++) {
            for(int i = 0; i< global.nN; i++){
                for(int j = 0; j< global.nN; j++){
                    soe.HG[i][j] = 0;
                    soe.CG[i][j] = 0;
                    soe.PG[i] = 0;
                }
            }
            for (int e = 0; e < global.nE; e++) {
                    for (int k = 0; k < 4; k++) {
                        for (int l = 0; l < 4; l++) {
                            elementy.get(e).HBC[k][l] = 0;
                            elementy.get(e).Hl[k][l] = 0;
                            elementy.get(e).Cl[k][l] = 0;
                            elementy.get(e).Pl[k] = 0;
                        }
                    }
            }

            for (int e = 0; e < global.nE; e++) {
//            System.out.println("Element: " + (e+1) );
                for (int i = 0; i < global.pc; i++) {
                    pomocWynik = elem4.calcHBC(e, i, wezly, elementy, global.alfa, global.pc);

                    for (int k = 0; k < 4; k++) {
                        for (int l = 0; l < 4; l++) {
                            elementy.get(e).HBC[k][l] += pomocWynik[k][l];
                        }
                    }

                }
//            System.out.println();
//            System.out.println();
//            System.out.println("Macierz HBC: ");
//            for(int i = 0; i<4; i++){
//                for(int j = 0; j < 4; j++){
//                    System.out.print(elementy.get(e).HBC[i][j] + " ");
//                }
//                System.out.println();
//            }

            }
            System.out.println();
            for (int e = 0; e < global.nE; e++) {
//            System.out.println("Element: " + (e+1) );
                for (int i = 0; i < global.pc; i++) {
                    pomocWynik3 = elem4.calcP(e, i, wezly, elementy, global.alfa, global.talfa, global.pc);

                    for (int k = 0; k < 4; k++) {
                        elementy.get(e).Pl[k] += pomocWynik3[k];
                    }

                }
//            System.out.println();
//            System.out.println();
//            System.out.println("Wektor P: ");
//            for(int i = 0; i<4; i++){
//                    System.out.print(elementy.get(e).Pl[i] + " ");
//
//            }
//            System.out.println();

            }

            for (int e = 0; e < global.nE; e++) {
                for (int i = 0; i < 4; i++) {
                    soe.PG[elementy.get(e).ID[i] - 1] += elementy.get(e).Pl[i];
                }
            }

//        for (int i = 0; i<global.nN; i++){
//                System.out.print(soe.PG[i]+ " ");
//        }
//
//        System.out.println();


            for (int e = 0; e < global.nE; e++) {
//            System.out.println("Element: " + (e+1) );
                for (int i = 0; i < global.pc * global.pc; i++) {
                    pomocWynik = elem4.calc(e, i, wezly, elementy, global.pc);

                    for (int k = 0; k < 4; k++) {
                        for (int l = 0; l < 4; l++) {
                            elementy.get(e).Hl[k][l] += pomocWynik[k][l];
                        }
                    }

                }
//            System.out.println();
//            System.out.println();
                for (int k = 0; k < 4; k++) {
                    for (int l = 0; l < 4; l++) {
                        elementy.get(e).Hl[k][l] += elementy.get(e).HBC[k][l];
                    }
                }

//            System.out.println("Macierz Hl: ");
//            for(int i = 0; i<4; i++){
//                for(int j = 0; j < 4; j++){
//                    System.out.print(elementy.get(e).Hl[i][j] + " ");
//                }
//                System.out.println();
//            }

            }
//        System.out.println();
//        System.out.println();


            for (int e = 0; e < global.nE; e++) {
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        soe.HG[elementy.get(e).ID[i] - 1][elementy.get(e).ID[j] - 1] += elementy.get(e).Hl[i][j];
                    }
                }
            }

//        for (int i = 0; i<global.nN; i++){
//            for (int j = 0; j < global.nN; j++){
//                System.out.print(soe.HG[i][j]+ " ");
//            }
//            System.out.println();
//        }

            for (int e = 0; e < global.nE; e++) {
//            System.out.println("Element: " + (e+1) );
                for (int i = 0; i < global.pc * global.pc; i++) {
                    pomocWynik2 = elem4.calc2(e, i, wezly, elementy, global.pc, global.ro, global.cp);

                    for (int k = 0; k < 4; k++) {
                        for (int l = 0; l < 4; l++) {
                            elementy.get(e).Cl[k][l] += pomocWynik2[k][l];
                        }
                    }

                }
//            System.out.println();
//            System.out.println();
//            System.out.println("Macierz Cl: ");
//            for(int i = 0; i<4; i++){
//                for(int j = 0; j < 4; j++){
//                    System.out.print(elementy.get(e).Cl[i][j] + " ");
//                }
//                System.out.println();
//            }

            }

            for (int e = 0; e < global.nE; e++) {
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        soe.CG[elementy.get(e).ID[i] - 1][elementy.get(e).ID[j] - 1] += elementy.get(e).Cl[i][j];
                    }
                }
            }
//        System.out.println();
//        for (int i = 0; i<global.nN; i++){
//            for (int j = 0; j < global.nN; j++){
//                System.out.print(soe.CG[i][j]+ " ");
//            }
//            System.out.println();
//        }

            for (int i = 0; i < global.nN; i++) {
                for (int j = 0; j < global.nN; j++) {
                    soe.HG[i][j] += (soe.CG[i][j] / global.deltatau);
                }
            }


//            System.out.println();
//            for (int i = 0; i < global.nN; i++) {
//                for (int j = 0; j < global.nN; j++) {
//                    System.out.print(soe.HG[i][j] + " ");
//                }
//                System.out.println();
//            }

            for (int i = 0; i < global.nN; i++) {
                for (int j = 0; j < global.nN; j++) {
                    soe.PG[i] = soe.PG[i] - (soe.CG[i][j] / global.deltatau) * wezly.get(j).t0;
                }
                soe.PG[i] = -1 * soe.PG[i];
            }

//            System.out.println();
//            for (int i = 0; i < global.nN; i++) {
//                System.out.print(soe.PG[i] + " ");
//            }
//
//            System.out.println();

            double [] T = soe.GaussElimination(soe.HG, soe.PG);


            for(int i=0; i< global.nN; i++){
                wezly.get(i).t0 = T[i];
            }
            double min = T[0], max = T[0];
            for(int i = 0; i< global.nN; i++ ){
                if(min > T[i]){
                    min = T[i];
                }
            }
            for(int i = 0; i< global.nN; i++ ){
                if(max < T[i]){
                    max = T[i];
                }
            }


            System.out.println("T0 min " + min + " Max: " + max );


        }


//        calkaKwadraturyGaussa wynik = new calkaKwadraturyGaussa();
//        System.out.println("Wynik calkowania: " + wynik.dwaWezly());
//        System.out.println("Wynik calkowania: " + wynik.trzyWezly());


    }
}
