using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CLF.Web.Framework.TagHelpers
{
    [HtmlTargetElement("app-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class AppLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string TextAttributeName = "text";
        private const  string RequiredAttributeName = "required"; //是否显示必填的*号
        protected IHtmlGenerator Generator { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(TextAttributeName)]
        public string Text { get; set; }

        [HtmlAttributeName(RequiredAttributeName)]
        public bool Required { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AppLabelTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var labelText = string.Empty;
            if (!string.IsNullOrEmpty(For.Metadata.DisplayName) && string.IsNullOrEmpty(Text))
            {
                labelText = For.Metadata.DisplayName;
            }
            else
            {
                labelText = Text;
            }

            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, labelText, new { @class = "control-label" });
            if (tagBuilder != null)
            {
                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;
                var classValue = output.Attributes.ContainsName("class")
                                    ? $"{output.Attributes["class"].Value} label-wrapper"
                                    : "label-wrapper";
                output.Attributes.SetAttribute("class", classValue);
            
                //add label
                output.Content.SetHtmlContent(tagBuilder);

                if(Required)
                {
                    output.Content.AppendHtml("<span style='color:red'>*</span>");
                }
            }
        }
    }
}
