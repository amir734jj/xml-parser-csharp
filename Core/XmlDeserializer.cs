using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Tokens;
using FParsec;
using FParsec.CSharp;

namespace Core
{
    public class XmlDeserializer : IXmlDeserializer
    {
        private XmlDeserializer()
        {
        }

        private static object FromToken(IToken token, Type type)
        {
            switch (token)
            {
                case XmlTextToken xmlTextToken:
                    return xmlTextToken.Text;
                case XmlNode xmlNode:
                    var instance = Activator.CreateInstance(type);
                    foreach (var (propertyInfo, jProperty) in type.GetProperties().Join(xmlNode.Attributes,
                        info => info.Name, property => property.Key,
                        (propertyInfo, property) => (propertyInfo, property)))
                    {
                        propertyInfo.SetValue(instance, jProperty.Value);
                    }

                    return instance;
                default:
                    return null;
            }
        }

        public T FromXml<T>(string source)
        {
            var (status, result, _) = XmlParser.New().Parser.ParseString(source);

            return status == ReplyStatus.Ok ? (T) FromToken(result, typeof(T)) : default;
        }
    }
}