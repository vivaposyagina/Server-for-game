using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("visible_players")]
    public class VisiblePlayers : BaseCommand
    {
        public class Player
        {
            private string name;
            private int row;
            private int col;            

            [XmlElement("name")]
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

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

            public Player(string n, int col, int row)
            {
                this.name = n;
                this.row = row;
                this.col = col;
            }
            public Player()
            {
                this.name = "";
                this.row = 0;
                this.col = 0;
            }
        }

        private List<Player> players;
        [XmlArray("players")]
        [XmlArrayItem("player")]
        public List<Player> Players
        {
            get {return players;}
        }

        public VisiblePlayers()
        {
            id = 9;
            players = new List<Player>();
        }
        public VisiblePlayers(List<Player> list)
        {
            id = 9;
            players = list;
        } 
    }
}
