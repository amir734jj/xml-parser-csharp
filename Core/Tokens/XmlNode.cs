using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Interfaces;

namespace Core.Tokens
{
    public class XmlNode : IToken
    {
        public string Name { get; }
        
        public ImmutableDictionary<string, string> Attributes { get; }
        
        public ImmutableList<IToken> Children { get; }

        public XmlNode(string name, ImmutableDictionary<string, string> attributes, ImmutableList<IToken> children)
        {
            Name = name;
            Attributes = attributes;
            Children = children;
        }
    }
}