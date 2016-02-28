using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("time_left")]
    public class TimeLeft : BaseCommand
    {
        [XmlIgnore()]
        private TimeSpan time;
        [XmlElement("time")]
        public double Time
        {
            get { return time.TotalMilliseconds; }
            set { time = TimeSpan.FromMilliseconds(value); }
        }

        public TimeLeft()
        {
            id = 5;
        }
        public TimeLeft(TimeSpan time)
        {
            id = 5;
            this.time = time;
        }
    }
}
