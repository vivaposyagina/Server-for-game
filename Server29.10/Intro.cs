using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("player")]
    public class Intro : BaseCommand
    {

        private string name;
        [XmlElement("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Intro()
        {
            id = 2;
        }
        public Intro(string name)
        {
            id = 2;
            this.name = name;
        }
    }
}
