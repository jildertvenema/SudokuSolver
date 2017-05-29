using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver_Form
{
    class NietMogelijk
    {
        private List<int> numbers = new List<int>();
        public List<int> Numbers { get { return numbers; }set { numbers = value; } }

        public void NietsMogelijk()
        {
            numbers = new List<int>() {1,2,3,4,5,6,7,8,9};
        }
    }
}
