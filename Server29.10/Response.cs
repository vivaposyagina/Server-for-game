using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("response")]
    public class Response : BaseCommand
    {
        private string result;
        [XmlAttribute("result")]
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        private string text;
        [XmlElement("text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public Response()
        {
            id = 1;
        }
        public Response(string result,string answer)
        {
            this.result = result;
            text = answer;
            id = 1;
        }
    }
}
