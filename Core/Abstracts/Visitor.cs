using Core.Interfaces;
using Core.Tokens;

namespace Core.Abstracts
{
    public abstract class Visitor<T> : IVisitor<T>
    {
        protected T Visit(IToken token)
        {
            return token switch
            {
                XmlNode xmlNode => Visit(xmlNode),
                XmlTextToken xmlTextToken => Visit(xmlTextToken),
                _ => default
            };
        }

        public abstract T Visit(XmlNode xmlNode);

        public abstract T Visit(XmlTextToken xmlTextToken);
    }
}