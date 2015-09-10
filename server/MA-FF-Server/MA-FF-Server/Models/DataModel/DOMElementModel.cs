using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    public class DOMElementModel
    {

        private String _tag;

        private String _path;
        private String _selector;

        private String _id;
        private String _title;

        private double _left;
        private double _top;

        private double _width;
        private double _height;

        private double _outerWidth;
        private double _outerHeight;

        private List<String> _classes = new List<String>();
        private List<AttributeModel> _attributes = new List<AttributeModel>();


        public DOMElementModel()
        {

        }

        #region GetterSetterFunctions

        public String Tag
        {
            get { return _tag; }
            set { _tag = value.ToLower(); }
        }


        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public String Selector
        {
            get { return _selector; }
            set { _selector = value; }
        }

        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public double Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public double OuterWidth
        {
            get { return _outerWidth; }
            set { _outerWidth = value; }
        }

        public double OuterHeight
        {
            get { return _outerHeight; }
            set { _outerHeight = value; }
        }

        public void AddClass(String className)
        {
            _classes.Add(className);
        }

        public void AddClasses(String classes)
        {
            String[] classesArray = classes.Split(null);

            foreach (String className in classesArray)
            {
                AddClass(className);
            }
        }

        public List<String> GetClasses()
        {
            return _classes;
        }

        public void AddAttribute(String name, String value)
        {
            AttributeModel attrModel = new AttributeModel(name, value);

            _attributes.Add(attrModel);
        }

        public List<AttributeModel> GetAttributes()
        {
            return _attributes;
        }

        #endregion


        #region XMLFunctions

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

        #endregion
    }
}
