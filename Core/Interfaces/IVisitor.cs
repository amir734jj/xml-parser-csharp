using Core.Tokens;

namespace Core.Interfaces
{
    public interface IVisitor<out T>
    {
        public abstract T Visit(XmlNode xmlNode);

        public abstract T Visit(XmlTextToken xmlTextToken);
    }
}