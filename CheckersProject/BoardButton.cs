using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersProject
{
    class BoardButton : Button
    {
        Location location; 
        public BoardButton(Location loc)
        {
            location = loc; 
        }
    }
}
