using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Game.Text.Model
{
    [Serializable()]
    [XmlRoot("font")]
    public class Font
    {
        [XmlElement("info")]
        public Info Info { get; set; }

        [XmlElement("common")]
        public Common Common { get; set; }

        [XmlArray("pages")]
        [XmlArrayItem("page", typeof(Page))]
        public Page[] Pages { get; set; }

        [XmlArray("chars")]
        [XmlArrayItem("char", typeof(Character))]
        public Character[] Characters { get; set; }

        [XmlArray("kernings")]
        [XmlArrayItem("kerning", typeof(Kerning))]
        public Kerning[] Kernings { get; set; }
    }

    [Serializable()]
    public class Info
    {
        [XmlAttribute("face")]
        public string Face { get; set; }
        [XmlAttribute("size")]
        public int Size { get; set; }
        [XmlAttribute("bold")]
        public int Bold { get; set; }
        [XmlAttribute("italic")]
        public int Italic { get; set; }
        [XmlAttribute("charset")]
        public string Charset { get; set; }
        [XmlAttribute("unicode")]
        public int Unicode { get; set; }
        [XmlAttribute("stretchH")]
        public int StretchH { get; set; }
        [XmlAttribute("smooth")]
        public int Smooth { get; set; }
        [XmlAttribute("aa")]
        public int Aa { get; set; }
        [XmlAttribute("padding")]
        public string Padding { get; set; }
        [XmlAttribute("spacing")]
        public string Spacing { get; set; }
        [XmlAttribute("outline")]
        public int Outline { get; set; }
    }

    [Serializable()]
    public class Common
    {
        [XmlAttribute("lineHeight")]
        public int LineHeight { get; set; }
        [XmlAttribute("base")]
        public int Base { get; set; }
        [XmlAttribute("scaleW")]
        public int ScaleW { get; set; }
        [XmlAttribute("scaleH")]
        public int ScaleH { get; set; }
        [XmlAttribute("pages")]
        public int Pages { get; set; }
        [XmlAttribute("packed")]
        public int Packed { get; set; }
        [XmlAttribute("alphaChnl")]
        public int AlphaChnl { get; set; }
        [XmlAttribute("redChnl")]
        public int RedChnl { get; set; }
        [XmlAttribute("greenChnl")]
        public int GreenChnl { get; set; }
        [XmlAttribute("blueChnl")]
        public int BlueChnl { get; set; }
    }

    [Serializable()]
    public class Page
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("file")]
        public string File { get; set; }
    }

    [Serializable()]
    public class Character
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("x")]
        public int X { get; set; }
        [XmlAttribute("y")]
        public int Y { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlAttribute("xoffset")]
        public int XOffset { get; set; }
        [XmlAttribute("yoffset")]
        public int YOffset { get; set; }
        [XmlAttribute("xadvance")]
        public int XAdvance { get; set; }
        [XmlAttribute("page")]
        public int Page { get; set; }
        [XmlAttribute("chnl")]
        public int Chnl { get; set; }
    }

    [Serializable()]
    public class Kerning
    {
        [XmlAttribute("first")]
        public int First { get; set; }
        [XmlAttribute("second")]
        public int Second { get; set; }
        [XmlAttribute("amount")]
        public int Amount { get; set; }
    }
}
