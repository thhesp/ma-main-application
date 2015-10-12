using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    /// <summary>
    /// Representation of the html element
    /// </summary>
    public class DOMElementModel
    {

        /// <summary>
        /// X coordinate which were calculated for the html
        /// </summary>
        private double _htmlX;

        /// <summary>
        /// Y coordinate which were calculated for the html
        /// </summary>
        private double _htmlY;

        /// <summary>
        /// Tag of the element
        /// </summary>
        private String _tag;

        /// <summary>
        /// Path from Base HTML to the element
        /// </summary>
        private String _path;

        /// <summary>
        /// Unique selector of the element
        /// </summary>
        private String _selector;

        /// <summary>
        /// ID of the element
        /// </summary>
        private String _id;

        /// <summary>
        /// title of the element
        /// </summary>
        private String _title;

        /// <summary>
        /// left coordinate of the element
        /// </summary>
        private double _left;

        /// <summary>
        /// top coordinate of the element
        /// </summary>
        private double _top;

        /// <summary>
        /// width of the element
        /// </summary>
        private double _width;

        /// <summary>
        /// height of the element
        /// </summary>
        private double _height;

        /// <summary>
        /// outer width of the element
        /// </summary>
        private double _outerWidth;

        /// <summary>
        /// outer height of the element
        /// </summary>
        private double _outerHeight;

        /// <summary>
        /// Classes of the element
        /// </summary>
        private List<String> _classes = new List<String>();

        /// <summary>
        /// All attributes of the element
        /// </summary>
        private List<AttributeModel> _attributes = new List<AttributeModel>();

        /// <summary>
        /// Constructor for loading from the xml
        /// </summary>
        public DOMElementModel()
        {

        }

        /// <summary>
        /// Getter/ Setter for the html x
        /// </summary>
        public Double HTMLX
        {
            get { return _htmlX; }
            set { _htmlX = value; }
        }

        /// <summary>
        /// Getter/ Setter for the html y
        /// </summary>
        public Double HTMLY
        {
            get { return _htmlY; }
            set { _htmlY = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element tag
        /// </summary>
        public String Tag
        {
            get { return _tag; }
            set { _tag = value.ToLower(); }
        }

        /// <summary>
        /// Getter / Setter for the element id
        /// </summary>
        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element title
        /// </summary>
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element path
        /// </summary>
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element selector
        /// </summary>
        public String Selector
        {
            get { return _selector; }
            set { _selector = value; }
        }

        /// <summary>
        /// Getter / Setter for the element left
        /// </summary>
        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Getter / Setter for the element top
        /// </summary>
        public double Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element width
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element height
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element outer width
        /// </summary>
        public double OuterWidth
        {
            get { return _outerWidth; }
            set { _outerWidth = value; }
        }

        /// <summary>
        /// Getter/ Setter for the element outer height
        /// </summary>
        public double OuterHeight
        {
            get { return _outerHeight; }
            set { _outerHeight = value; }
        }

        /// <summary>
        /// Method for adding a class
        /// </summary>
        /// <param name="className">class to add</param>
        public void AddClass(String className)
        {
            _classes.Add(className);
        }

        /// <summary>
        /// Adds classes
        /// </summary>
        /// <param name="classes">String representation of classes to add</param>
        public void AddClasses(String classes)
        {
            String[] classesArray = classes.Split(null);

            foreach (String className in classesArray)
            {
                AddClass(className);
            }
        }

        /// <summary>
        /// Returns the List of classes
        /// </summary>
        /// <returns>List of classes</returns>
        public List<String> GetClasses()
        {
            return _classes;
        }

        /// <summary>
        /// adds an attribute
        /// </summary>
        /// <param name="name">attribute name</param>
        /// <param name="value">attribute value</param>
        public void AddAttribute(String name, String value)
        {
            AttributeModel attrModel = new AttributeModel(name, value);

            _attributes.Add(attrModel);
        }

        /// <summary>
        /// Returns the list of attributes
        /// </summary>
        /// <returns>List of attributes</returns>
        public List<AttributeModel> GetAttributes()
        {
            return _attributes;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode elementNode = xmlDoc.CreateElement("element");

            //tag

            XmlAttribute tag = xmlDoc.CreateAttribute("tag");

            tag.Value = this.Tag;

            elementNode.Attributes.Append(tag);

            //path

            XmlAttribute path = xmlDoc.CreateAttribute("path");

            path.Value = this.Path;

            elementNode.Attributes.Append(path);


            XmlAttribute selector = xmlDoc.CreateAttribute("selector");

            selector.Value = this.Selector;

            elementNode.Attributes.Append(selector);

            //id

            XmlAttribute id = xmlDoc.CreateAttribute("id");

            id.Value = this.ID;

            elementNode.Attributes.Append(id);

            //title

            XmlAttribute title = xmlDoc.CreateAttribute("title");

            title.Value = this.Title;

            elementNode.Attributes.Append(title);

            //htmlX

            XmlAttribute htmlX = xmlDoc.CreateAttribute("html-x");

            htmlX.Value = this.HTMLX.ToString();

            elementNode.Attributes.Append(htmlX);

            //htmlY

            XmlAttribute htmlY = xmlDoc.CreateAttribute("html-y");

            htmlY.Value = this.HTMLY.ToString();

            elementNode.Attributes.Append(htmlY);

            //left

            XmlAttribute left = xmlDoc.CreateAttribute("left");

            left.Value = this.Left.ToString();

            elementNode.Attributes.Append(left);


            //top

            XmlAttribute top = xmlDoc.CreateAttribute("top");

            top.Value = this.Top.ToString();

            elementNode.Attributes.Append(top);

            //width

            XmlAttribute width = xmlDoc.CreateAttribute("width");

            width.Value = this.Width.ToString();

            elementNode.Attributes.Append(width);

            //height

            XmlAttribute height = xmlDoc.CreateAttribute("height");

            height.Value = this.Height.ToString();

            elementNode.Attributes.Append(height);


            //outerHeight

            XmlAttribute outerHeight = xmlDoc.CreateAttribute("outerHeight");

            outerHeight.Value = this.OuterHeight.ToString();

            elementNode.Attributes.Append(outerHeight);

            //left

            XmlAttribute outerWidth = xmlDoc.CreateAttribute("outerWidth");

            outerWidth.Value = this.OuterWidth.ToString();

            elementNode.Attributes.Append(outerWidth);

            //classes

            XmlNode classesNode = xmlDoc.CreateElement("classes");

            foreach (String className in _classes)
            {
                XmlNode classNode = xmlDoc.CreateElement("class");
                classNode.InnerText = className;
                classesNode.AppendChild(classNode);

            }

            elementNode.AppendChild(classesNode);

            //attributes

            XmlNode attributesNode = xmlDoc.CreateElement("attributes");

            foreach (AttributeModel attribute in _attributes)
            {
                attributesNode.AppendChild(attribute.ToXML(xmlDoc));
            }

            elementNode.AppendChild(attributesNode);

            return elementNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="elementNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static DOMElementModel LoadFromXML(XmlNode elementNode)
        {
            if (elementNode.Name == "element")
            {
                DOMElementModel elementModel = new DOMElementModel();

                foreach (XmlAttribute attr in elementNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "tag":
                            elementModel.Tag = attr.Value;
                            break;
                        case "id":
                            elementModel.ID = attr.Value;
                            break;
                        case "path":
                            elementModel.Path = attr.Value;
                            break;
                        case "selector":
                            elementModel.Selector = attr.Value;
                            break;
                        case "title":
                            elementModel.Title = attr.Value;
                            break;
                        case "html-x":
                            elementModel.HTMLX = Double.Parse(attr.Value);
                            break;
                        case "html-y":
                            elementModel.HTMLY = Double.Parse(attr.Value);
                            break;
                        case "left":
                            elementModel.Left = Double.Parse(attr.Value);
                            break;
                        case "top":
                            elementModel.Top = Double.Parse(attr.Value);
                            break;
                        case "width":
                            elementModel.Width = Double.Parse(attr.Value);
                            break;
                        case "height":
                            elementModel.Height = Double.Parse(attr.Value);
                            break;
                        case "outerHeight":
                            elementModel.OuterHeight = Double.Parse(attr.Value);
                            break;
                        case "outerWidth":
                            elementModel.OuterWidth = Double.Parse(attr.Value);
                            break;
                    }
                }

                foreach (XmlNode child in elementNode.ChildNodes)
                {
                    if (child.Name == "classes")
                    {
                        foreach (XmlNode classesChild in child.ChildNodes)
                        {
                            elementModel.AddClass(classesChild.InnerText);
                        }
                    }
                    else if (child.Name == "attributes")
                    {
                        foreach (XmlNode attrChild in child.ChildNodes)
                        {
                            AttributeModel attribute = AttributeModel.LoadFromXML(attrChild);

                            if (attribute != null)
                            {
                                elementModel._attributes.Add(attribute);
                            }
                        }
                    }
                }

                return elementModel;

            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
