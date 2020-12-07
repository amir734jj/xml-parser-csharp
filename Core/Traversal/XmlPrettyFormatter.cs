using System.Text;
using Core.Abstracts;
using Core.Tokens;

namespace Core.Traversal
{
    public class XmlPrettyFormatter : Visitor<string>
    {
        private int _indent;

        private string Indent()
        {
            return string.Empty.PadRight(_indent, ' ');
        }

        public override string Visit(XmlNode xmlNode)
        {
            var sb = new StringBuilder();
            sb.Append($"{Indent()}<{xmlNode.Name}");
            
            foreach (var (name, value) in xmlNode.Attributes)
            {
                sb.Append(@$" {name}=""{value}""");
            }

            sb.AppendLine(">");

            if (xmlNode.Children.IsEmpty)
            {
                sb.Append(" />");
            }
            else
            {
                _indent++;
                foreach (var xmlNodeChild in xmlNode.Children)
                {
                    sb.AppendLine(Visit(xmlNodeChild));
                }
                
                _indent--;

                sb.Append($"{Indent()}</{xmlNode.Name}>");
            }

            return sb.ToString();
        }

        public override string Visit(XmlTextToken xmlTextToken)
        {
            return Indent() + xmlTextToken.Text;
        }
    }
}