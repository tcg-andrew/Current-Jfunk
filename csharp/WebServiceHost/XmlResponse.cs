#region Usings

using System.Xml;

#endregion

namespace WebServiceHost
{
    public class XmlResponse
    {
        #region Values

        private XmlNode exception;
        private XmlDataDocument doc;
        private XmlNode data;

        #endregion

        #region Properties

        public string Exception
        {
            set
            {
                XmlNode exceptionMessage = doc.CreateNode(XmlNodeType.Text, "", "");
                exceptionMessage.Value = value;
                exception.AppendChild(exceptionMessage);
            }
        }

        public XmlNode Data
        {
            get
            {
                return data;
            }
        }

        public XmlDataDocument Output
        {
            get
            {
                XmlDataDocument output = doc;
                return doc;
            }
        }

        #endregion Properties

        #region Constructors

        public XmlResponse()
        {
            doc = new XmlDataDocument();

            XmlNode root = doc.CreateNode(XmlNodeType.Element, "result", "");
            doc.AppendChild(root);

            exception = doc.CreateNode(XmlNodeType.Element, "exception", "");
            root.AppendChild(exception);

            data = doc.CreateNode(XmlNodeType.Element, "epicor", "");
            root.AppendChild(data);

        }

        #endregion

        #region Public Methods

        public XmlNode CreateNode(string name, object value)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, "");
            XmlNode nodeText = doc.CreateNode(XmlNodeType.Text, "", "");
            nodeText.Value = value.ToString();
            node.AppendChild(nodeText);
            return node;
        }

        public XmlNode CreateElement(string name)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, "");
            return node;
        }

        #endregion
    }
}