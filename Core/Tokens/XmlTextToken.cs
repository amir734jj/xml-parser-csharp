using Core.Interfaces;

namespace Core.Tokens
{
    public class XmlTextToken : IToken
    {
        public string Text { get; }

        public XmlTextToken(string text)
        {
            Text = text;
        }
    }
}