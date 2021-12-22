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
        public Location GetLocation ()
        {
            return this.location; 
        }
    }
}
