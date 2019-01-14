using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaSlicing {
    class Pizza {
        public int Rows { get; }
        public int Columns { get; }
        public int MaxSlice { get; }
        public int MinIngredient { get; }
        public List<List<Cell>> Cell { get; }
        public int Score { get { return score; } }

        Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Slice>>>> slices;
        int score;
        HashSet<Cell> uncutCells;
        
        public Pizza(int R, int C, int L, int H, List<List<Cell>> CellGrid) {
            Rows = R;
            Columns = C;
            MinIngredient = L;
            MaxSlice = H;
            Cell = CellGrid;
            score = 0;
            uncutCells = new HashSet<Cell>();
            slices = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Slice>>>>();
            foreach (Cell cell in AllCells()) {
                uncutCells.Add(cell);
            }
        }

        public Slice GetSlice(int R1, int C1, int R2, int C2) {
            if (!slices.ContainsKey(R1)) {
                slices.Add(R1, new Dictionary<int, Dictionary<int, Dictionary<int, Slice>>>());
            }
            if (!slices[R1].ContainsKey(C1)) {
                slices[R1].Add(C1, new Dictionary<int, Dictionary<int, Slice>>());
            }
            if (!slices[R1][C1].ContainsKey(R2)) {
                slices[R1][C1].Add(R2, new Dictionary<int, Slice>());
            }
            if (!slices[R1][C1][R2].ContainsKey(C2)) {
                slices[R1][C1][R2].Add(C2, new Slice(R1,C1,R2,C2));
            }
            return slices[R1][C1][R2][C2];
        }

        public string GenerateHash() {
            // TODO optimize
            string hash = "";
            foreach (Cell cell in AllCells()) {
                hash += (cell.IsCut) ? "0" : "1";
            }
            return hash;
        }

        public IEnumerable<Cell> UncutCells() {
            // TODO oprtimize
            foreach(Cell cell in AllCells()) {
                if (!cell.IsCut) {
                    yield return cell;
                }
            }
        }

        public IEnumerable<Cell> AllCells() {
            foreach (List<Cell> row in Cell) {
                foreach (Cell cell in row) {
                    yield return cell;
                }
            }
        }

        public void CutSlice(Slice slice) {
            foreach(Cell cell in slice.ContainedCells()) {
                cell.Cut();
                uncutCells.Remove(cell);
                score++;
            }
        }

        public void RestoreSlice(Slice slice) {
            foreach (Cell cell in slice.ContainedCells()) {
                cell.Restore();
                uncutCells.Add(cell);
                score--;
            }
        }

        
    }
}
