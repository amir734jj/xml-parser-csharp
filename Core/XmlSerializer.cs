using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Core.Interfaces;
using Core.Tokens;

namespace Core
{
    public class XmlSerializer : IXmlSerializer
    {
        private XmlSerializer()
        {
        }

        private static IToken ToToken<T>(T source)
        {
            switch (source)
            {
                case null:
                    return new XmlTextToken(string.Empty);
                case bool value:
                    return new XmlTextToken(value.ToString().ToLower());
                case double value:
                    return new XmlTextToken(value.ToString(CultureInfo.InvariantCulture));
                case string value:
                    return new XmlTextToken(value);
                case IList list:
                    return new XmlNode(list.GetType().Name, ImmutableDictionary<string, string>.Empty, (from object item in list select ToToken(item)).ToImmutableList());
                case { } value:
                    return new XmlNode(value.GetType().Name,
                        value.GetType().GetProperties()
                            .Select(x => new KeyValuePair<string, string>(x.Name, x.GetValue(value).ToString()))
                            .ToImmutableDictionary(),
                        ImmutableList<IToken>.Empty
                    );
            }
        }

        public string ToXml<T>(T source)
        {
            return ToToken(source).ToString();
        }
    }
}