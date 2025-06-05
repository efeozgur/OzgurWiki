using Markdig;
using OzgurWiki.Models;
using System.Text.RegularExpressions;

namespace OzgurWiki.Helpers
{
    public static class ContentHelper
    {
        public static string LinkifyWikiContent(string content, List<WikiPage> allPages)
        {
            if (string.IsNullOrWhiteSpace(content)) return "";

            // 1. Markdown'dan HTML'ye çevir
            content = Markdown.ToHtml(content);

            // 2. İç linklemeleri uygula
            return Regex.Replace(content, @"\[\[(.*?)\]\]", match =>
            {
                var title = match.Groups[1].Value;
                var page = allPages.FirstOrDefault(p =>
                    p.Title.ToLowerInvariant() == title.ToLowerInvariant());

                if (page != null)
                {
                    return $"<a href=\"/Wiki/Detail/{page.Id}\">{title}</a>";
                }
                else
                {
                    return $"<a href=\"/Wiki/Create?title={Uri.EscapeDataString(title)}\" class=\"text-muted\">{title}</a>";
                }
            });
        }



    }
}
