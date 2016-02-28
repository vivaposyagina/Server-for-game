using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("message")]
    public class Chat : BaseCommand
    {
        private string text;
        private string sender;

        [XmlElement("text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        [XmlElement("sender")]
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        public Chat()
        {
            id = 4;
        }
        public Chat(string answer)
        {
            text = answer;
            id = 4;
        }
    }
}
