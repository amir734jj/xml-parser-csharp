using Core.Tokens;
using FParsec;
using Microsoft.FSharp.Core;

namespace Core.Interfaces
{
    public interface IXmlParser
    {
        public FSharpFunc<CharStream<Unit>,Reply<XmlNode>> Parser { get; }
    }
}