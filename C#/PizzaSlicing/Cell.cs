using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaSlicing {
    class Cell {
        public static Pizza pizza;

        public int R { get; }
        public int C { get; }
        public bool IsShroom { get; }

        public bool IsCut { get { return _isCut; } }
        bool _isCut;

        public Cell(int r, int c, bool isShroom) {
            R = r;
            C = c;
            IsShroom = isShroom;
            _isCut = false;
        }

        public IEnumerable<Slice> DrawSlices() {
            for (int i = 0; i < pizza.MaxSlice && i + R < pizza.Rows; i++) {
                for (int j = 0; (j + 1) * (i + 1) <= pizza.MaxSlice && j + C < pizza.Columns; j++) {
                    Slice slice = pizza.GetSlice(R, C, R + i, C + j);
                    if (slice.IsValid && slice.IsFull()) {
                        yield return slice;
                    }
                }
            }
        }


        public void Cut() {
            _isCut = true;
        }

        public void Restore() {
            _isCut = false;
        }

        public override string ToString() {
            return R+" "+C+" "+(IsShroom?"M":"T");
        }
    }
}
