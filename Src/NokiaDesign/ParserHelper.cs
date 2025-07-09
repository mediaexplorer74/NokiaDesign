using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NokiaDesign
{
    public class ProjectInfo
    {
        public string NodeId { get; set; }
        public string Title { get; set; }
    }

    public static class ParserHelper
    {
        public static async Task<List<string>> ParseSiteAsync(string url, string type)
        {
            var results = new List<string>();
            try
            {
                HttpClient client = new HttpClient();
                string html = await client.GetStringAsync(new Uri(url));
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                switch (type)
                {
                    case "about":
                        var aboutNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'content')]");
                        if (aboutNode != null)
                            results.Add(aboutNode.InnerText.Trim());
                        break;
                    case "research":
                        var researchNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'news-item')] | //div[contains(@class,'content')]");
                        if (researchNodes != null)
                        {
                            foreach (var node in researchNodes)
                                results.Add(node.InnerText.Trim());
                        }
                        break;
                    case "network":
                        var networkNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'content')]");
                        if (networkNode != null)
                            results.Add(networkNode.InnerText.Trim());
                        break;
                    case "chronology":
                        var timelineNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'content')]");
                        if (timelineNode != null)
                            results.Add(timelineNode.InnerText.Trim());
                        break;
                }
            }
            catch (Exception ex)
            {
                results.Add($"Error: {ex.Message}");
            }
            return results;
        }

        // Получить список проектов (nodeId, title)
        public static async Task<List<ProjectInfo>> GetProjectListAsync(int maxCountPerPrefix = 3)
        {
            var projects = new List<ProjectInfo>();
            string[] prefixes = { "A", "B", "C" };
            foreach (var prefix in prefixes)
            {
                int total = await GetTotalProjectsAsync(prefix);
                if (total == 0) total = maxCountPerPrefix; // fallback
                int count = 0;
                for (int i = 1; i <= total && count < maxCountPerPrefix; i++)
                {
                    string id = $"{prefix}{i.ToString("D4")}";
                    var info = await GetProjectInfoAsync(id);
                    if (info != null && !string.IsNullOrWhiteSpace(info.Title))
                    {
                        projects.Add(new ProjectInfo { NodeId = id, Title = info.Title });
                        count++;
                    }
                }
            }
            return projects;
        }

        // Получить число проектов для каждой серии (например, A, B, C)
        public static async Task<int> GetTotalProjectsAsync(string prefix = "A")
        {
            try
            {
                string url = $"https://nokiadesignarchive.aalto.fi/?node={prefix}0001";
                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Mobile/14E5239e");
                    string html = await client.GetStringAsync(new Uri(url));
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var navTextNode = doc.DocumentNode.SelectSingleNode("//body//*[contains(text(),'/')]");
                    if (navTextNode != null)
                    {
                        var text = navTextNode.InnerText;
                        var slashIdx = text.IndexOf("/");
                        if (slashIdx > 0)
                        {
                            var part = text.Substring(slashIdx + 1).Trim();
                            int n;
                            if (int.TryParse(new string(part.TakeWhile(char.IsDigit).ToArray()), out n))
                                return n;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        // Получить число проектов из "1 / NNN"
        public static async Task<int> GetTotalProjectsAsync()
        {
            try
            {
                string url = "https://nokiadesignarchive.aalto.fi/?node=A0001";
                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Mobile/14E5239e");
                    string html = await client.GetStringAsync(new Uri(url));
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var navTextNode = doc.DocumentNode.SelectSingleNode("//body//*[contains(text(),'/')]");
                    if (navTextNode != null)
                    {
                        var text = navTextNode.InnerText;
                        var slashIdx = text.IndexOf("/");
                        if (slashIdx > 0)
                        {
                            var part = text.Substring(slashIdx + 1).Trim();
                            int n;
                            if (int.TryParse(new string(part.TakeWhile(char.IsDigit).ToArray()), out n))
                                return n;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        // Получить инфо о проекте и id следующего
        public static async Task<ProjectInfoWithNext> GetProjectInfoAsync(string nodeId)
        {
            try
            {
                string url = $"https://nokiadesignarchive.aalto.fi/?node={nodeId}";
                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Mobile/14E5239e");
                    string html = await client.GetStringAsync(new Uri(url));
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var contentNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'content')]");
                    string title = nodeId;
                    if (contentNode != null)
                    {
                        var h2 = contentNode.SelectSingleNode(".//h2");
                        var h1 = contentNode.SelectSingleNode(".//h1");
                        var h3 = contentNode.SelectSingleNode(".//h3");
                        if (h2 != null && !string.IsNullOrWhiteSpace(h2.InnerText))
                            title = h2.InnerText.Trim();
                        else if (h1 != null && !string.IsNullOrWhiteSpace(h1.InnerText))
                            title = h1.InnerText.Trim();
                        else if (h3 != null && !string.IsNullOrWhiteSpace(h3.InnerText))
                            title = h3.InnerText.Trim();
                        else
                        {
                            // fallback: первая не пустая строка текста
                            var text = contentNode.InnerText.Split('\n').Select(s => s.Trim()).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
                            if (!string.IsNullOrWhiteSpace(text))
                                title = text;
                        }
                    }
                    // Кнопка/ссылка на следующий проект (оставим для совместимости)
                    string nextId = null;
                    return new ProjectInfoWithNext { NodeId = nodeId, Title = title, NextNodeId = nextId };
                }
            }
            catch { }
            return null;
        }

        public class ProjectInfoWithNext : ProjectInfo
        {
            public string NextNodeId { get; set; }
        }

        // Получить детали по nodeId
        public static async Task<List<string>> GetProjectDetailsAsync(string nodeId)
        {
            var results = new List<string>();
            try
            {
                string url = $"https://nokiadesignarchive.aalto.fi/?node={nodeId}";
                HttpClient client = new HttpClient();
                string html = await client.GetStringAsync(new Uri(url));
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var contentNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'content')]");
                if (contentNode != null)
                {
                    results.Add(contentNode.InnerText.Trim());
                }
            }
            catch (Exception ex)
            {
                results.Add($"Error: {ex.Message}");
            }
            return results;
        }
    }
}
