using System;
using System.IO;
using System.Text;
using MES;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadingFile.ReadFileInteger(args[0]);
            int W = data.numbers[0],H = data.numbers[1], mW = data.numbers[2],mH = data.numbers[3];
            double dw = W / (mW-1);
            double dh = H / (mH-1);
            Node[] nodes = new Node[mW * mH];
            Element[] elements = new Element[(mW-1) * (mH-1)];
            for(int i=0;i<nodes.Length;i++)
            {
                //int id = i + 1;
                nodes[i] = new Node((i/mH)*dw,(i % mH)*dh);
            }
            for(int x=0;x<mW-1;x++)
            {
                for(int y=0;y<mH-1;y++)
                {
                    elements[x*(mH-1) + y] = new Element(y+x*mH , y+x*mH+mH , y+x*mH+mH+1 , y+x*mH+1);
                }
            }
            //Write Nodes
            /*string NodeListString = "";
            for(int i=0;i<mW;i++)
            {
                for(int j=0;j<mH;j++)
                {

                }
            }*/
            Console.ReadKey();
        }
    }
}
