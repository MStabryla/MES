using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MES
{
    public static class ReadingFile
    {
        public static DataFromFile<double> ReadFileDouble(string filepath)
        {
            FileStream fstream = new FileStream(filepath,FileMode.Open,FileAccess.Read);
            int fileLength = (int)fstream.Length;
            byte[] buffer = new byte[fileLength];
            fstream.Read(buffer,0,fileLength);
            string encodedText = UTF8Encoding.UTF8.GetString(buffer,0,fileLength);
            fstream.Close();
            string[] textInRow = encodedText.Split('\n');
            DataFromFile<double> data = new DataFromFile<double>();
            byte typeOfVariable = 0;
            List<double[]> tempMatrix = new List<double[]>();
            for(int i=0;i<textInRow.Length;i++)
            {
                string actLine = textInRow[i].Replace("\r","");
                if(actLine == "")
                {
                    if(typeOfVariable < 2) { typeOfVariable++; }
                    else {
                        int x = tempMatrix.Count,y = tempMatrix.Max((row) => row.Length);
                        double [,] matrix = new double[x,y];
                        for(int a=0;a<x;a++)
                        {
                            for(int b=0;b<y;b++)
                            {
                                matrix[a,b] = tempMatrix[a].Length-1 >= b ? tempMatrix[a][b] : 0.0;
                            }
                        }
                        data.matrixes.Add(matrix);
                        tempMatrix = new List<double[]>();
                    }
                    continue;
                }
                switch(typeOfVariable){
                    case 0:
                        data.numbers.Add(Convert.ToDouble(actLine));
                        break;
                    case 1:
                        string[] splittedLine = actLine.Split(' ');
                        double[] vertex = new double[splittedLine.Length];
                        for(int j=0;j<splittedLine.Length;j++)
                        {
                            vertex[j] = Convert.ToDouble(splittedLine[j]);
                        }
                        data.vertexes.Add(vertex);
                        break;
                    default:
                        string[] splittedLineToMatrix = actLine.Split(' ');
                        double[] row = new double[splittedLineToMatrix.Length];
                        for(int j=0;j<splittedLineToMatrix.Length;j++)
                        {
                            row[j] = Convert.ToDouble(splittedLineToMatrix[j]);
                        }
                        tempMatrix.Add(row);
                        break;
                }
            }
            return data;
        }
        public static DataFromFile<int> ReadFileInteger(string filepath)
        {
            FileStream fstream = new FileStream(filepath,FileMode.Open,FileAccess.Read);
            int fileLength = (int)fstream.Length;
            byte[] buffer = new byte[fileLength];
            fstream.Read(buffer,0,fileLength);
            string encodedText = UTF8Encoding.UTF8.GetString(buffer,0,fileLength);
            fstream.Close();
            string[] textInRow = encodedText.Split('\n');
            DataFromFile<int> data = new DataFromFile<int>();
            byte typeOfVariable = 0;
            List<int[]> tempMatrix = new List<int[]>();
            for(int i=0;i<textInRow.Length;i++)
            {
                string actLine = textInRow[i];
                if(actLine == "")
                {
                    if(typeOfVariable < 2) { typeOfVariable++; }
                    else {
                        int x = tempMatrix.Count,y = tempMatrix.Max((row) => row.Length);
                        int [,] matrix = new int[x,y];
                        for(int a=0;a<x;a++)
                        {
                            for(int b=0;b<y;b++)
                            {
                                matrix[a,b] = tempMatrix[a].Length-1 <= b ? tempMatrix[a][b] : 0;
                            }
                        }
                        data.matrixes.Add(matrix);
                        tempMatrix = new List<int[]>();
                    }
                    continue;
                }
                switch(typeOfVariable){
                    case 0:
                        data.numbers.Add(Convert.ToInt32(actLine));
                        break;
                    case 1:
                        string[] splittedLine = actLine.Split(' ');
                        int[] vertex = new int[splittedLine.Length];
                        for(int j=0;j<splittedLine.Length;j++)
                        {
                            vertex[j] = Convert.ToInt32(splittedLine[j]);
                        }
                        data.vertexes.Add(vertex);
                        break;
                    default:
                        string[] splittedLineToMatrix = actLine.Split(' ');
                        int[] row = new int[splittedLineToMatrix.Length];
                        for(int j=0;j<splittedLineToMatrix.Length;j++)
                        {
                            row[j] = Convert.ToInt32(splittedLineToMatrix[j]);
                        }
                        tempMatrix.Add(row);
                        break;
                }
            }
            return data;
        }
        public static void DataToFile(string filepath,string content)
        {
            using(FileStream stream = new FileStream(filepath,FileMode.Create,FileAccess.Write)){
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(content);
                stream.Write(buffer,0,buffer.Length);
                stream.Close();
            }
        }
    }
}