using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Toolkit.Parsers.Markdown;
using NL.IC.Generator.Core.Mediating;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            var intermediateContractDefinition =
                LoadIntermediateContractDefinition(@"{your repo path}\NLIC\NL.IC.Generator.Core\Mediating\IntermediateContractDefinition.config");

            var markdownDocument = new MarkdownDocument();
            markdownDocument.Parse(ReadDocument(@"{your repo path}\NLIC\NL.IC.Generator.Core\SemanticDocument.md"));

            if (IsValid(markdownDocument, "Natural Language Document")
                && IsValid(intermediateContractDefinition, "Intermediate Contract Definition"))
            {
                SemanticAnalyser semanticAnalyser = new SemanticAnalyser();
                var semanticGraph = semanticAnalyser.Analyse(markdownDocument, intermediateContractDefinition);
                Serialize(semanticGraph, @"{your repo path}\NLIC\NL.IC.Generator.Core\SemanticGraph.generated.xml");

                Mediator mediator = new Mediator();
                var intermediateContract = mediator.Mediate(semanticGraph);
                intermediateContract.Save(@"{your repo path}\NLIC\NL.IC.Generator.Core\IntermediateContract.generated.xml");
            }
        }

        private static bool IsValid(object Obj, string fileName)
        {
            if (Obj != null) return true;
            Console.WriteLine($"Please select {fileName} first.");
            return false;
        }

        private static IntermediateContractDefinition LoadIntermediateContractDefinition(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IntermediateContractDefinition));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (IntermediateContractDefinition)serializer.Deserialize(reader);
            }
        }

        private static string ReadDocument(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private static void Serialize<T>(T obj, string filePath)
        {
            XmlSerializer objSerializer = new XmlSerializer(typeof(T));
            using (var fileStream = File.Create(filePath))
            {
                objSerializer.Serialize(fileStream, obj);
            }
        }

        private static string Serialize<T>(T obj)
        {
            XmlSerializer objSerializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    objSerializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
