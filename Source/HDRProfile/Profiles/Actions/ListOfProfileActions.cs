﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoHDR.Profiles.Actions
{

    public class ListOfProfileActions : ObservableCollection<IProfileAction>, IXmlSerializable
    {
        public ListOfProfileActions() : base() { }

        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {

            reader.Read();
            if (!reader.IsStartElement("IProfileAction"))
                return;
            while (reader.IsStartElement("IProfileAction"))
            {
                object value = reader.GetAttribute("AssemblyQualifiedName");
                if (string.IsNullOrEmpty((string)value))
                    return;

                Type type = Type.GetType((string)value);
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("IProfileAction");
                this.Add((IProfileAction)serial.Deserialize(reader));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();


        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (IProfileAction dispatcher in this)
            {
                writer.WriteStartElement("IProfileAction");
                writer.WriteAttributeString
                ("AssemblyQualifiedName", dispatcher.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(dispatcher.GetType());
                xmlSerializer.Serialize(writer, dispatcher);
                writer.WriteEndElement();
            }
        }
    }
}