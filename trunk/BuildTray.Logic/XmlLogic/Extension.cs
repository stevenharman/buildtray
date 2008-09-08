using System.Linq;
using System.Text;
using System.Xml;

namespace BuildTray.Logic.XmlLogic
{
    public static class Extension
    {
        public static string GetTestOutput(this XmlNode node)
        {
            var debugTraceNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "StdOut");
            var errorInfoNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "ErrorInfo");

            var builder = new StringBuilder();

            if (debugTraceNode != null)
            {
                builder.AppendLine("Error Info:");
                builder.AppendLine(debugTraceNode.InnerText);
                builder.AppendLine();
            }
            if (errorInfoNode != null)
            {
                builder.AppendLine("Stack Trace:");
                builder.AppendLine(errorInfoNode.InnerText);
            }

            return builder.ToString();
        }

        public static string GetClassNameForTest(this XmlDocument document, string testId)
        {
            var nodes = document.GetElementsByTagName("UnitTest");
            var node = nodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Attributes.GetNamedItem("id").InnerText == testId);
            var methodNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "TestMethod");
            string className = methodNode.Attributes.GetNamedItem("className").InnerText;
            int length = className.IndexOf(",");
            return className.Substring(0, length);

        }
    }
}
