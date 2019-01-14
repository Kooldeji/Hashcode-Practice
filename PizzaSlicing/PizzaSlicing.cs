using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaSlicing {
    class PizzaSlicing {
        static int OverallBest;
        static int OBCount;
        public static List<Slice> Solve(int noOfRows, int noOfCols, int minIng, int maxCells, List<List<Cell>> pizzaGrid) {
            Pizza pizza = new Pizza(noOfRows, noOfCols, minIng, maxCells, pizzaGrid);
            Cell.pizza = Slice.pizza = pizza;
            Dictionary<string, int> store = new Dictionary<string, int>();
            Dictionary<string, Slice> bestSliceFrom = new Dictionary<string, Slice>();
            OverallBest = 0;
            OBCount = 0;
            PS(pizza, store, bestSliceFrom,0);
            string pizzaState = pizza.GenerateHash();
            List<Slice> slices = new List<Slice>();

            while (bestSliceFrom.ContainsKey(pizzaState)) {
                slices.Add(bestSliceFrom[pizzaState]);
                pizza.CutSlice(bestSliceFrom[pizzaState]);
                pizzaState = pizza.GenerateHash();
            }
            return slices;
        }

        public static int PS(Pizza pizza, Dictionary<string, int> store, Dictionary<string,Slice> bestSliceFrom, int noOfSlices) {
            string pizzaState = pizza.GenerateHash();
            if (store.ContainsKey(pizzaState)) {
                return store[pizzaState];
            }

            int bestScore = 0;

            int score;
            bool foundLegalSlice = false;
            foreach(Cell cell in pizza.UncutCells()) {
                foreach(Slice slice in cell.DrawSlices()) {
                    pizza.CutSlice(slice);
                    score = PS(pizza, store, bestSliceFrom,noOfSlices+1);
                    if (score > bestScore) {
                        bestScore = score;
                        bestSliceFrom[pizzaState] = slice;
                    }
                    pizza.RestoreSlice(slice);

                    foundLegalSlice = true;
                }
            }
            if (!foundLegalSlice) {
                score = pizza.Score;
                if (score > bestScore) {
                    bestScore = score;
                    if (bestScore > OverallBest) {
                        OverallBest = bestScore;
                        OBCount++;
                        Console.WriteLine(OBCount + ". " + OverallBest);
                    }
                }
            }
            store[pizzaState] = bestScore;
            return store[pizzaState];
        }
    }
}
