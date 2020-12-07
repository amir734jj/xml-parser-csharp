namespace Core.Interfaces
{
    public interface IXmlDeserializer
    {
        T FromXml<T>(string source);
    }
}