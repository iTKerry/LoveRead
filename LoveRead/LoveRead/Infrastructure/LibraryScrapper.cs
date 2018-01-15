﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LoveRead.Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace LoveRead.Infrastructure
{
    public class LibraryScrapper : ILibraryScrapper
    {
        private readonly IMessangerService _messanger;
        private readonly ScrapingBrowser _scrapingBrowser;

        private readonly Regex _idFromUrlRegex = new Regex(@"[^?]+(?:\?id=([^&]+).*)?");

        private const string BookPattern = "http://loveread.ec/read_book.php?id={0}&p={1}";

        public LibraryScrapper(ScrapingBrowser scrapingBrowser, IMessangerService messanger)
        {
            _scrapingBrowser = scrapingBrowser;
            _messanger = messanger;
        }

        public async Task<WebBook> ReadBook(string bookUrl)
        {
            Log("Reading started!");
            var book = await GetBookBaseInfo();
            Log($"Book has ID={book.Id} with {book.PagesCount} pages");
            await GetBookPages();
            Log("Done!");
            return book;

            async Task<WebBook> GetBookBaseInfo()
            {
                var firstPage = await NavigateBrowserToPage(bookUrl);
                var webBook = new WebBook
                {
                    Id = int.Parse(_idFromUrlRegex.Match(bookUrl).Groups[1].Value),
                    Url = bookUrl,
                    PagesCount = firstPage.Html.CssSelect("div.navigation > a").Select(n => n.InnerHtml)
                        .Where(t => int.TryParse(t, out int _)).Select(int.Parse).Max(),
                    Pages = new List<WebBookPage>()
                };
                return webBook;
            }

            async Task GetBookPages()
            {
                var currentPageNumber = 1;
                while (currentPageNumber <= book.PagesCount)
                {
                    var currentPageUrl = string.Format(BookPattern, book.Id, currentPageNumber++);
                    Log($"Navigating to: {currentPageUrl}");
                    var currentPage = await NavigateBrowserToPage(currentPageUrl);
                    book.Pages.Add(new WebBookPage { WebBookTexts = GetPageText(currentPage, currentPageNumber) });
                }
            }
        }

        private IEnumerable<IWebBookText> GetPageText(WebPage webPage, int pageNumber)
        {
            Log($"Reading page #{pageNumber} ...");
            var nodes = webPage.Html.CssSelect("div.MsoNormal").SingleOrDefault()?.ChildNodes.Nodes();
            if (nodes == null) yield break;

            foreach (var node in nodes)
            {
                if (node.Attributes.Any(attr => attr.Value == "take_h1"))
                    yield return new WebBookHeader {Text = node.InnerText};
                if (node.Attributes.Any(attr => attr.Value == "MsoNormal"))
                    yield return new WebBookParagraph {Text = node.InnerText};
            }
        }

        private async Task<WebPage> NavigateBrowserToPage(string pageUrl) 
            => await _scrapingBrowser.NavigateToPageAsync(new Uri(pageUrl));

        private void Log(string messange) 
            => _messanger.Log(new LogMessange {Text = messange});
    }

    public interface ILibraryScrapper
    {
        Task<WebBook> ReadBook(string bookUrl);
    }
}