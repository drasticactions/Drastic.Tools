// <copyright file="ListTagHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.Text;
using Drastic.HtmlLabel.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Org.Xml.Sax;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    /// <summary>
    /// Tag handler to support HTML lists.
    /// </summary>
    internal class ListTagHandler : Java.Lang.Object, Html.ITagHandler
    {
        public const string TagUl = "ULC";
        public const string TagOl = "OLC";
        public const string TagLi = "LIC";

        private ListBuilder listBuilder; // KWI-FIX: removed new, set in constructor

        public ListTagHandler(int listIndent) // KWI-FIX: added constructor with listIndent property
        {
            this.listBuilder = new ListBuilder(listIndent);
        }

        public void HandleTag(bool isOpening, string tag, IEditable output, IXMLReader xmlReader)
        {
            tag = tag.ToUpperInvariant();
            var isItem = tag == TagLi;

            // Is list item
            if (isItem)
            {
                this.listBuilder.AddListItem(isOpening, output);
            }

            // Is list
            else
            {
                if (isOpening)
                {
                    var isOrdered = tag == TagOl;
                    this.listBuilder = this.listBuilder.StartList(isOrdered, output);
                }
                else
                {
                    this.listBuilder = this.listBuilder.CloseList(output);
                }
            }
        }
    }
}
