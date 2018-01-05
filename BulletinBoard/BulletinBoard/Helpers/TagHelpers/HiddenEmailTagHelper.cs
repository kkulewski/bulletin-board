using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulletinBoard.Helpers.TagHelpers
{
    [HtmlTargetElement("p", Attributes = ForAttributeName)]
    public class HiddenEmailTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-hidden-email";

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
            var formatted = "...";

            if (text.Length > 5)
            {
                var beginning = text.Substring(0, 2);
                var ending = text.Substring(text.Length - 3, 3);
                formatted = beginning + "..." + ending;
            }

            output.Content.SetContent(formatted);
        }
    }
}
