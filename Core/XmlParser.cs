using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Interfaces;
using Core.Tokens;
using FParsec.CSharp;
using FParsec;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.CharParsersCS;
using static FParsec.CSharp.PrimitivesCS;

namespace Core
{
    public class XmlParser : IXmlParser
    {
        public FSharpFunc<CharStream<Unit>,Reply<XmlNode>> Parser { get; }

        public static IXmlParser New()
        {
            return new XmlParser();
        }
        
        private XmlParser()
        {
            var nameStart = Choice(Letter, CharP('_'));
            var nameChar = Choice(Letter, Digit, AnyOf("-_."));
            var name = Many1Chars(nameStart, nameChar).And(WS);

            var quotedString = Between('"', ManyChars(NoneOf("\"")), '"');
            var attribute = name.And(Skip('=')).And(WS).And(quotedString).And(WS)
                .Lbl_("attribute")
                .Map((attrName, attrVal) => new KeyValuePair<string, string>(attrName, attrVal));
            var attributes = Many(attribute);

            FSharpFunc<CharStream<Unit>,Reply<IToken>> element = null;

            var elementStart = Skip('<').AndTry(name.Lbl("tag name")).And(attributes);

            FSharpFunc<CharStream<Unit>, Reply<string>> closingTag(string tagName) => Between("</", StringP(tagName).And(WS), ">")
                .Lbl_($"closing tag '</{tagName}>'");

            FSharpFunc<CharStream<Unit>, Reply<IToken>> textContent(string leadingWS) => NotEmpty(ManyChars(NoneOf("<"))
                .Map(text => leadingWS + text)
                .Map(x => (IToken) new XmlTextToken(x))
                .Lbl_("text content"));

            var childElement = Rec(() => element).Lbl_("child element");

            IEnumerable<IToken> EmptyContentToEmptyString(FSharpList<IToken> xs) => xs.IsEmpty ? (IEnumerable<IToken>) ImmutableList.Create(new XmlTextToken("")) : xs;

            var elementContent = Many(WS.WithSkipped().AndTry(ws => 
                    Choice(textContent(ws), childElement)))
                .Map(EmptyContentToEmptyString);

            FSharpFunc<CharStream<Unit>,Reply<IToken>> elementEnd(string elName, FSharpList<KeyValuePair<string, string>> elAttrs) =>
                Choice(
                        Skip("/>").Return(Enumerable.Empty<IToken>()),
                        Skip(">").And(elementContent).And(WS).AndL(closingTag(elName)))
                    .Map(elContent => (IToken)new XmlNode(elName, elAttrs.ToImmutableDictionary(), elContent.ToImmutableList()));

            element = elementStart.And(elementEnd);

            Parser = WS.And(element).And(WS).And(EOF).Map(x => x switch
            {
                XmlNode token => token,
                _ => null
            });
        }
    }
}