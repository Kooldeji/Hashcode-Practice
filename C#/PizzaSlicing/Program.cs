using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaSlicing {
    class Program {
        static string path;
        static string outputTitle;

        static void Main(string[] args) {
            Console.Write("Enter the File name: ");
            string filePath = "C:\\Users\\opsij\\Documents\\Visual Studio 2017\\Projects\\Hashcode - Practice\\PizzaSlicing\\IO files";
            string fn = Console.ReadLine();
            Console.Write("Name your solution: ");
            string outputTitle = Console.ReadLine();

            string[] inFiles;
            if (fn.Equals("all")) {
                inFiles = Directory.GetFiles(filePath, "*.in", SearchOption.TopDirectoryOnly);
            } else {
                inFiles = new string[1];
                inFiles[0] = filePath + "\\" + fn;
            }


            
            foreach (string path in inFiles) {
                Console.WriteLine("Starting " + Path.GetFileName(path));
                ThreadStart threadDelegate = new ThreadStart(exeAll);
                Thread T = new Thread(threadDelegate, 50000000);
                Program.path = path;
                Program.outputTitle = outputTitle;
                T.Start();
                
            }

           
        }
        static void exeAll() {
            IO_out(IO_in(path), path, outputTitle);
            Console.WriteLine("Finished " + Path.GetFileName(path));
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static List<Slice> IO_in(string filePath) {


            string[] lines = File.ReadAllLines(filePath);

            //Iterate over lines and split
            int lineNo = 1;
            int[] fl = new int[0]; //First line
            List<List<Cell>> pizza = new List<List<Cell>>();
            int r = 0;
            int c = 0;
            foreach (string line in lines) {
                if (lineNo == 1) {
                    fl = Array.ConvertAll(line.Split(), st => int.Parse(st));
                    lineNo++;
                    continue;
                }
                List<Cell> thisRow = new List<Cell>();
                foreach(char i in line) {
                    thisRow.Add(new Cell(r, c++, i == 'M'));
                }
                pizza.Add(thisRow);
                r++;
                c = 0;
            }
            return PizzaSlicing.Solve(fl[0], fl[1], fl[2], fl[3], pizza);
        }

        static void IO_out(List<Slice> allSlices, string filePath, string fileName) {
            string outStr = allSlices.Count + "\n";
            foreach(Slice slice in allSlices) {
                outStr += slice.ToString()+"\n";
            }
            try {
                File.WriteAllText(filePath + "-results\\" + DateTime.Now.ToString("M-dd--HH-mm-ss") + "-" + fileName + ".out", outStr);
            } catch (ArgumentException e) {
                File.WriteAllText(filePath + "-results\\" + DateTime.Now.ToString("M-dd--HH-mm-ss") + ".out", outStr);
            }
        }
    }
}
