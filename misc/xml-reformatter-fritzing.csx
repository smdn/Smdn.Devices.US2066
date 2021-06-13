#!/usr/bin/env dotnet-script
//
// requirements:
//   .NET Core or .NET Runtime
//
// usage:
//   1. install dotnet-script
//     dotnet tool install -g dotnet-script
//   2. run this script
//     ./xml-reformatter-fritzing.csx a.xml b.xml
//     dotnet script ./xml-reformatter-fritzing.csx a.xml b.xml

using System;
using System.Xml;
using System.Xml.Linq;
using System.Text;

var settings = new XmlWriterSettings() {
  Encoding = new UTF8Encoding(false),
  Indent = true,
  IndentChars = " ",
  NewLineChars = "\n",
};

foreach (var file in Args) {
  Console.Write($"reformatting {file} ... ");

  var doc = XDocument.Load(file);

  // set background color
  doc.Root.Add(
    new XAttribute("style", "background-color: white;")
  );

  using (var writer = XmlWriter.Create(file, settings)) {
    doc.Save(writer);
  }

  Console.WriteLine("done");
}