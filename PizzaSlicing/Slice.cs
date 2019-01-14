using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaSlicing {
    class Slice {
        public static Pizza pizza;
        public int R1 { get; }
        public int C1 { get; }
        public int R2 { get; }
        public int C2 { get; }
        public bool IsValid { get; }

        public Slice(int r1, int c1, int r2, int c2) {
            R1 = r1;
            R2 = r2;
            C1 = c1;
            C2 = c2;
            int shrooms = 0;
            int tomats = 0;
            foreach (Cell cell in ContainedCells()) {
                shrooms += (cell.IsShroom) ? 1 : 0;
                tomats += (cell.IsShroom) ? 0 : 1;
            }
            int Area = (Math.Abs(R2 - R1) + 1) * (Math.Abs(C2 - C1) + 1);
            IsValid = (Area > 1) && (Area <= pizza.MaxSlice) && (shrooms >= pizza.MinIngredient) && (tomats >= pizza.MinIngredient);   
        }

        public IEnumerable<Cell> ContainedCells() {
            for(int i = R1; i <= R2; i++) {
                for(int j = C1; j<= C2; j++) {
                    yield return pizza.Cell[i][j];
                }
            }
        }
        

        public bool IsFull() {
            foreach(Cell cell in ContainedCells()) {
                if (cell.IsCut) {
                    return false; ;
                }
            }
            return true; ;
        }

        public override string ToString() {
            return R1+" "+ C1 + " " + R2 + " " + C2;
        }

    }
}
