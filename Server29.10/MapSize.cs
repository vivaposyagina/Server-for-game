using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("map_size")]
    public class MapSize : BaseCommand
    {
        private int width;
        private int height;

        [XmlElement("width")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        [XmlElement("height")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public MapSize(int width, int height)
        {
            id = 7;
            this.width = width;
            this.height = height;
        }
        public MapSize()
        {
            id = 7;
            this.width = 0;
            this.height = 0;
        }

    }
}
