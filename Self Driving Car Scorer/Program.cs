using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_Driving_Car_Scorer {
    class Program {
        static void Main(string[] args) {
            string IO_Path = "C:\\Users\\opsij\\Documents\\Visual Studio 2017\\Projects\\Hashcode - Practice\\Hashcode - Practice\\IO files";
            string[] inFiles = Directory.GetFiles(IO_Path, "*.in", SearchOption.TopDirectoryOnly);
            string compiledScores = "";
            Dictionary<string, Tuple<int, string>> scoreBoard = new Dictionary<string, Tuple<int, string>>();

            foreach(string inFilePath in inFiles) {
                string resultsDir = inFilePath + "-results";
                string[] outFiles = Directory.GetFiles(resultsDir, "*.out", SearchOption.TopDirectoryOnly);

                scoreBoard[inFilePath] = new Tuple<int, string>( -1,"");
                Dictionary<string, int> outputScores = new Dictionary<string, int>();

                foreach(string outFilePath in outFiles) {
                    int score = Score(inFilePath, outFilePath);
                    outputScores[outFilePath] = score;
                    if (score >= scoreBoard[inFilePath].Item1) {
                        scoreBoard[inFilePath] = new Tuple<int, string>(score, outFilePath);
                    }
                }
                compiledScores += Path.GetFileNameWithoutExtension(inFilePath)+"\n\n"+ OutputScores(resultsDir, outputScores)+"\n\n\n";
            }
            OutputScoreBoard(IO_Path, scoreBoard, compiledScores);
        }

        static int Score(string inputFilePath, string outputFilePath) {
            string[] lines = File.ReadAllLines(outputFilePath);
            
            List<List<int>> carRides = new List<List<int>>();
            foreach (string line in lines) {
                List<int> outRides = Array.ConvertAll(line.Trim().Split(), st => int.Parse(st)).ToList();
                if(outRides[0] != outRides.Count - 1) {
                    throw new DataMisalignedException("Result is bugged");
                }
                carRides.Add(outRides);
            }

            string[] inLines = File.ReadAllLines(inputFilePath);

            //Iterate over lines and split
            int lineNo = 1;
            int[] fl = new int[0]; //First line
            List<List<int>> rides = new List<List<int>>();
            foreach (string line in inLines) {
                if (lineNo == 1) {
                    fl = Array.ConvertAll(line.Trim().Split(), st => int.Parse(st));
                    lineNo++;
                    continue;
                }
                rides.Add(Array.ConvertAll(line.Trim().Split(), st => int.Parse(st)).ToList());
            }
            return Score(fl[0], fl[1], fl[2], fl[3], fl[4], fl[5], rides, carRides);
        }

        static int Score(int R, int C, int F, int N, int B, int T, List<List<int>> rides, List<List<int>> answer) {
            int totalScore = 0;
            foreach (List<int> car in answer) {
                int step = 0;
                int currX = 0;
                int currY = 0;
                bool firstVal = true;
                foreach(int ride in car) {
                    if (firstVal) {
                        firstVal = false;
                        continue;
                    }
                    List<int> currentRide = rides[ride];
                    // GO To start
                    step += Math.Abs(currX - currentRide[0]) + Math.Abs(currY - currentRide[1]);
                    // Await ride start
                    if(step < currentRide[4]) {
                        step = currentRide[4];
                    }
                    bool bonus = (step == currentRide[4]);
                    int score = Math.Abs(currentRide[0] - currentRide[2]) + Math.Abs(currentRide[1] - currentRide[3]);
                    step += score;
                    score += bonus ? B : 0;
                    currX = currentRide[2];
                    currY = currentRide[3];
                    if (step <= T) {
                        if (step <= currentRide[5]) {
                            totalScore += score;
                        }
                    } else {
                        break;
                    }
                }
            }
            return totalScore;
        }

        static string OutputScores(string Directory, Dictionary<string, int> scores) {
            string outStr = "Score\t\tFile\n\n";
            foreach(string path in scores.Keys) {
                
                outStr += scores[path].ToString("N0") + "\t";
                outStr += (scores[path] < 1000000) ? "\t" : "";
                outStr += Path.GetFileNameWithoutExtension(path)+"\n";
            }
            File.WriteAllText(Directory + "\\scores.txt", outStr);
            return outStr;
        }
        static void OutputScoreBoard(string Directory, Dictionary<string, Tuple<int, string>> scores, string allScores) {
            string outStr = "Input\tScore\t\tOutput\n\n";
            int totalScore = 0;
            foreach (string path in scores.Keys) {
                outStr += Path.GetFileNameWithoutExtension(path);
                outStr += "\t" + scores[path].Item1.ToString("N0") + "\t";
                outStr += (scores[path].Item1 < 1000000) ? "\t" : "";
                totalScore += scores[path].Item1;
                outStr += Path.GetFileNameWithoutExtension(scores[path].Item2) + "\n";
            }
            outStr += "\nTotal score: " + totalScore.ToString("N0");
            File.WriteAllText(Directory + "\\scores.txt", outStr);
            File.WriteAllText(Directory + "\\scoresBreakdown.txt", allScores);
        }
    }
}
