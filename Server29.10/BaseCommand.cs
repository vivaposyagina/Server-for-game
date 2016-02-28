using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    public class BaseCommand
    {
        protected int id;

        static Type[] CommandTypes;

        static BaseCommand()
        {
            CommandTypes = new Type[100];
            CommandTypes[0] = null;
            CommandTypes[1] = typeof(Response);
            CommandTypes[2] = typeof(Intro);
            CommandTypes[3] = typeof(PlayerList);
            CommandTypes[4] = typeof(Chat);
            CommandTypes[5] = typeof(TimeLeft);
            CommandTypes[6] = typeof(PlayerCoords);
            CommandTypes[7] = typeof(MapSize);
            CommandTypes[8] = typeof(VisibleObjects);
            CommandTypes[9] = typeof(VisiblePlayers);
            CommandTypes[10] = typeof(PlayerMove);
            CommandTypes[11] = typeof(GameOver);
            CommandTypes[12] = typeof(PlayerDisconnect);
       
        }
        

        public BaseCommand()
        {
            this.id = 0;            
        }        

        [XmlAttribute("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }   


        public string Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return Convert.ToString(writer);
        }

        public static BaseCommand Deserialize(int id, string xml)
        {
            if (id <= 0 && id > 4)
            {
                //Надо ли здесь выбрасывать исключение и сообщать пользователю об ошибке через консоль?
                return null;
            }
            else
            {
                //xml = xml.Substring(1);
                XmlSerializer serializer = new XmlSerializer(CommandTypes[id]);
                StringReader reader = new StringReader(xml);
                BaseCommand newCommand = (BaseCommand)serializer.Deserialize(reader);
                return newCommand;
            }
        }
    }
}
