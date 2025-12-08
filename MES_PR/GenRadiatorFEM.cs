using MES;
using System;
using System.Collections.Generic;

namespace MES_PR
{
    public class GenRadiatorFEM
    {
        public Node[] nodes;
        public Element[] elements;
        public double  H;
        public double W;
        public int WNodesLength;
        public int HNodesLength;
        //Wysokość jednej płytki wystającej z radiatora
        // hElementsLength < HElementsLength
        public int h;
        public int columnLength,noColumnLength;
        public double dw,dh;
        public GenRadiatorFEM(double _W,double _H,int _mW,int _mH, int _h)
        {
            
            W = _W;H = _H;
            WNodesLength = _mW;
            HNodesLength = _mH;
            h = _h;
            if(h >= (HNodesLength-1))
                throw new Exception("h should be less that HElements");
            nodes = new Node[WNodesLength * HNodesLength];
            columnLength = (WNodesLength-1) % 2 == 1 ? WNodesLength/2 : (WNodesLength-1)/2;
            noColumnLength = (WNodesLength-1) % 2 == 1 ? (WNodesLength-2)/2 : (WNodesLength-1)/2;
            int elemLength = (HNodesLength-1)*columnLength + (HNodesLength -1 -h)*noColumnLength;
            elements = new Element[elemLength];
            dw = W / (WNodesLength-1.0);
            dh = H / (HNodesLength-1.0);
        }
        //Wysokość płytki
        public int hp {
            get { return (HNodesLength-1) - h; }
        }

        public void Generate(double alfaProc,double alfaAir,double tProc,double tAir,double T0)
        {
            for(int i=0;i<nodes.Length;i++)
            {
                //int id = i + 1;
                nodes[i] = new Node((i/HNodesLength)*dw,(i % HNodesLength)*dh,T0);
            }
            int elemCounter = 0;
            for(int x=0;x<WNodesLength-1;x++)
            {
                int tempH = x % 2 == 0 ? HNodesLength - 1 : (HNodesLength - 1) - h;
                for(int y=0;y<tempH;y++)
                {
                    bool whichalfa = false;
                    var bc = new List<(int,int)>();
                    if(y == 0) {
                        bc.Add((0,1));
                        whichalfa = true;
                    }
                    if(y >= (HNodesLength - 1) - h)
                    {
                        bc.Add((1,2));
                        bc.Add((3,0));
                        if(y == (HNodesLength - 1) - 1)
                            bc.Add((2,3));
                    }
                    else
                    {
                        if(x == 0)
                        {
                            bc.Add((3,0));
                        }
                        else if(x == WNodesLength-2)
                        {
                            bc.Add((1,2));
                        }
                    }
                    if(tempH == (HNodesLength - 1) - h && y == (HNodesLength - 1) - h -1)
                    {
                        bc.Add((2,3));
                    }
                    elements[elemCounter++] = new Element(y+x*HNodesLength , y+x*HNodesLength+HNodesLength , y+x*HNodesLength+HNodesLength+1 , y+x*HNodesLength+1,bc.ToArray(),whichalfa ? alfaProc : alfaAir,whichalfa ? tProc : tAir);
                }
            }
        }
    }
}