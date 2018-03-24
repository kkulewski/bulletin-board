using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulletinBoard.Helpers.TagHelpers
{
    [HtmlTargetElement("p", Attributes = ForAttributeName)]
    public class DisplayForTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-display-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

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

            var text = For.ModelExplorer.GetSimpleDisplayText();
            output.Content.SetContent(text);
        }
    }
}
