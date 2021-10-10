# angle-bracket-parser-csharp

Simple XML parser using `FParsec`


```csharp
var xmlString = @"
  <config logdir=""/var/log/foo/"" debugfile=""/tmp/foo.debug"">
    <server name=""sahara"" osname=""solaris"" osversion=""2.6"">
      <address>10.0.0.101</address>
      <address>10.0.1.101</address>
    </server>
    <server name=""gobi"" osname=""irix"" osversion=""6.5"">
      <address>10.0.0.102</address>
    </server>
    <server name=""kalahari"" osname=""linux"" osversion=""2.0.34"">
      <address>10.0.0.103</address>
      <address>10.0.1.103</address>
    </server>
  </config>
";

var p = XmlParser.New().Parser.ParseString(xmlString);

Console.Write(new XmlPrettyFormatter().Visit(p.Result));
```
