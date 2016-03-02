using System.Xml.Schema;
using System.Xml.Linq;

namespace UtilityToolkit.FileTools
{
    public static class XMLTools
    {
        /// <summary>
        /// Wraps a string so that it begins with "<![CDATA[" and ends with "]]>" allowing literal text to be passed to XML.
        /// </summary>
        /// <param name="input">The string to wrap in CDATA tags.</param>
        /// <returns></returns>
        public static string ToXmlCDataString(this string input)
        {
            return "<![CDATA[" + input + "]]>";
        }

        /// <summary>
        /// Escapes all special XML characters in a string to their entity equivelent.
        /// </summary>
        /// <param name="input">A string to be included as data in an XML element</param>
        /// <returns>The input paramter string with all XML special character replaced with their entities.</returns>
        public static string XMLEncode(string input)
        {
            return input.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("&", "&amp;");
        }

        /// <summary>
        /// A simple yes/no validation check for XML files against a schema
        /// </summary>
        /// <param name="xmlFilePath">Full path to the XML file to be checked</param>
        /// <param name="xsdFilePath">Full path to the schema file to validate against</param>
        /// <returns>True if valid against schema, false otherwise</returns>
        public static bool ValidateXmlFileAgainstSchemaFile(string xmlFilePath, string xsdFilePath)
        {
            XDocument xdoc = XDocument.Load(xmlFilePath);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xsdFilePath);
            
            try
            {
                xdoc.Validate(schemaSet, null);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateXmlAgainstSchema(XDocument document, XmlSchema schema)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);

            try
            {
                document.Validate(schemaSet, null);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
