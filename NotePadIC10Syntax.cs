using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Assets.Scripts;
using System;

namespace DataExtractor
{
    static class XMLSchemaExporter
    {
        public static void ExportSchema()
        {
            var L = StationpediaExporter.Logger;
            L.LogInfo("Exporting XML Schema...");
            var schemas = new XmlSchemas();
            var exporter = new XmlSchemaExporter(schemas);
            var importer = new XmlReflectionImporter();

            foreach (Type t in typeof(SpawnData).Assembly.GetTypes())
            {
                try
                {
                    if (
                        t.IsClass
                        && !t.IsGenericTypeDefinition
                        && Attribute.IsDefined(t, typeof(XmlRootAttribute))
                    )
                    {
                        var mapping = importer.ImportTypeMapping(t);
                        exporter.ExportTypeMapping(mapping);
                    }
                }
                catch (Exception ex)
                {
                    L.LogError($"Error processing type {t.FullName}: {ex}");
                }
            }

            using (
                var writer = XmlWriter.Create(
                    "data/schema.xsd",
                    new XmlWriterSettings { Indent = true }
                )
            )
            {
                foreach (XmlSchema schema in schemas)
                {
                    try
                    {
                        // Remove top-level <xs:element> definitions
                        var elementsToRemove = schema
                            .Items.OfType<XmlSchemaElement>()
                            .Where(e => e.Parent == schema) // only top-level
                            .ToList();

                        foreach (var element in elementsToRemove)
                        {
                            schema.Items.Remove(element);
                        }
                        schema.Write(writer);
                    }
                    catch (Exception ex)
                    {
                        L.LogError($"Error writing schema: {ex}");
                    }
                }

            }
            L.LogInfo("XML Schema export completed.");
        }
    }
}
