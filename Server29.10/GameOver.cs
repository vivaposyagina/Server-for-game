using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("game_over")]
    public class GameOver : BaseCommand
    {
        private int result;
        [XmlElement("result")]
        public int Result
        {
            get { return result; }
            set { result = value; }
        }

        public GameOver()
        {
            id = 11;
        }
        public GameOver(int result)
        {
            id = 11;
            this.result = result;
        }
    }
}
