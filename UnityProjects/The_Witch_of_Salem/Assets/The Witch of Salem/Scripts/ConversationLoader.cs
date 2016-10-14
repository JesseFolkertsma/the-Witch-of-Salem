using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ConversationLoader {
    
    public Conversation LoadText(string file)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Conversation));
        FileStream stream = new FileStream(Application.dataPath + "/The Witch of Salem/Conversations/" + file, FileMode.Open);
        Conversation conv = serializer.Deserialize(stream) as Conversation;
        stream.Close();
        
        return (conv);
    }


    public void SaveGame()
    {
        Conversation conv = new Conversation();
        conv.text.Add("Text nummer 1");
        conv.text.Add("Text n2221");
        conv.text.Add("Weerk verdomme");
        conv.text.Add("Text nummer 4");

        XmlSerializer serializer = new XmlSerializer(typeof(Conversation));
        FileStream stream = new FileStream(Application.dataPath + "/The Witch of Salem/Conversations/" + "TestNPC.xml", FileMode.Create);
        serializer.Serialize(stream, conv);
        stream.Close();
    }
}

public class Conversation
{
    [XmlArray("TextArray")]
    [XmlArrayItem("Text")]
    public List<string> text = new List<string>();
}
