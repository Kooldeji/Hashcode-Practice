using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode___Practice {
    class Program {


        static void Main(string[] args) {

            Console.Write("Enter the File name: ");
            string filePath = "C:\\Users\\opsij\\Documents\\Visual Studio 2017\\Projects\\Hashcode - Practice\\Hashcode - Practice\\IO files";
            string fn = Console.ReadLine();
            Console.Write("Name your solution: ");
            string outputTitle = Console.ReadLine();
            
            string[] inFiles;
            if (fn.Equals("all")){
                inFiles = Directory.GetFiles(filePath, "*.in", SearchOption.TopDirectoryOnly);
            } else {
                inFiles = new string[1];
                inFiles[0] = filePath + "\\"+fn;
            }

            foreach (string path in inFiles) {
                Console.WriteLine("Starting " + Path.GetFileName(path));
                IO_out(IO_in(path), path, outputTitle);
                Console.WriteLine("Finished " + Path.GetFileName(path));
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        

        static void IO_out(List<List<int>> assignedRides, string filePath, string fileName) {
            string outStr = "";
            foreach(List<int> carRides in assignedRides) {
                outStr += carRides.Count + " ";
                foreach(int ride in carRides) {
                    outStr += ride + " ";
                }
                outStr += "\n";
            }
            try {
                File.WriteAllText(filePath + "-results\\" + DateTime.Now.ToString("M-dd--HH-mm-ss") + "-" + fileName + ".out", outStr);
            }catch(ArgumentException e) {
                File.WriteAllText(filePath + "-results\\" + DateTime.Now.ToString("M-dd--HH-mm-ss") +".out", outStr);
            }
        }

        static List<List<int>> IO_in(string filePath) {
            // below IF no line split
            //string text = System.IO.File.ReadAllText(filePath);


            string[] lines = File.ReadAllLines(filePath);

            //Iterate over lines and split
            int lineNo = 1;
            int[] fl; //First line
            List<List<int>> rides = new List<List<int>>();
            foreach (string line in lines) {
                if (lineNo == 1) {
                    fl = Array.ConvertAll(line.Split(), st => int.Parse(st));
                    lineNo++;
                    continue;
                }
                rides.Add(Array.ConvertAll(line.Split(), st => int.Parse(st)).ToList());
            }
            return SelfDriving.Solve(fl[0], fl[1], fl[2], fl[3], fl[4], fl[5], rides);
        }

        static int[] StringToIntArr(string[] stringArr) {
            return Array.ConvertAll(stringArr, st => int.Parse(st));
        }

      
    }

   
}
