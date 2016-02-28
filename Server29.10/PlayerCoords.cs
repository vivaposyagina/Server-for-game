using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("player_coords")]
    public class PlayerCoords : BaseCommand
    {
        private int row;
        private int col;

        [XmlElement("row")]
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        [XmlElement("col")]
        public int Col
        {
            get { return col; }
            set { col = value; }
        }

        public PlayerCoords(int row, int col)
        {
            id = 6;
            this.row = row;
            this.col = col;
        }
        public PlayerCoords()
        {
            id = 6;
            this.row = 0;
            this.col = 0;
        }
    }
}
