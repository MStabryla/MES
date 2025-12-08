using System.Linq;
using System;

namespace MES
{
    public static class StringMethods
    {
        public static string MatrixToString(double[,] response,int x, int y){
            string strResponse = "";
            int[] columnWidth = new int[y];
            for(int i=0;i<x;i++)
            {
                for(int j=0;j<y;j++)
                {
                    if(columnWidth[i] < response[i,j].ToString("F2").Length)
                        columnWidth[i] = response[i,j].ToString("F2").Length;
                }
            }
            string strColumns = new string('-',columnWidth.Sum() + y*3 + 1); strColumns += '\n';
            strResponse += strColumns;
            for(int i=0;i<x;i++)
            {
                for(int j=0;j < y;j++)
                {
                    string valueStr = response[i,j].ToString("F2");
                    strResponse += "| " + valueStr + new string(' ',columnWidth[j] - valueStr.Length + 1);
                }
                strResponse += "|\n";
            }
            strResponse += new string('-',columnWidth.Sum() + y*3 + 1) + "\n";
            return strResponse;
        }
        public static string VectorToString(double[] response,int x){
            string strResponse = "";
            int[] columnWidth = new int[x];
            for(int i=0;i<x;i++)
            {
                if(columnWidth[i] < response[i].ToString("F2").Length)
                    columnWidth[i] = response[i].ToString("F2").Length;
            }
            string strColumns = new string('-',columnWidth.Sum() + x*3 + 1); strColumns += '\n';
            strResponse += strColumns;
            for(int i=0;i<x;i++)
            {
                string valueStr = response[i].ToString("F2");
                    strResponse += "| " + valueStr + new string(' ',columnWidth[i] - valueStr.Length + 1);
            }
            strResponse += "\n" + new string('-',columnWidth.Sum() + x*3 + 1) + "\n";
            return strResponse;
        }
        public static void RadiatorWrite(Node[] nodes,int h,int w,double tmax,double tmin,int ph)
        {
            Console.Write("\n");
            var nodessorted = nodes.OrderBy(x => x.t0).ToList();
            for(int j = 0; j < h; j++)
            {
                for(int i=0;i<w;i++)
                {
                    var actnode = nodes[i*h + (h-j -1)];
                    double colorFactor = /*(actnode.t0 - tmin)/(tmax-tmin);*/ (double)nodessorted.IndexOf(nodessorted.First(x => x.t0 == actnode.t0)) / (double)nodessorted.Count;
                    Console.ForegroundColor = SetColor(colorFactor);
                    if(i == w - 1)
                        Console.Write("+");
                    else if(i % 2 == 0 || j > ph)
                        Console.Write("+++");
                    else
                        Console.Write("+  ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }
        private static ConsoleColor SetColor(double w)
        {
            ConsoleColor[] colors = {
                ConsoleColor.Red,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.DarkGreen,
                ConsoleColor.Blue,
                ConsoleColor.DarkBlue
            };
            double step = 1.0 / colors.Length;
            for(int i = 0;i<colors.Length;i++)
            {
                if(w >= 1.0 - (i+1)*step)
                    return colors[i];
            }
            return ConsoleColor.DarkBlue;
        }
    }
}