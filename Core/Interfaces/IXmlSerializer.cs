namespace Core.Interfaces
{
    public interface IXmlSerializer
    {
        string ToXml<T>(T source);
    }
}