﻿using LoveRead.Model;

namespace LoveRead.Views.Tabs.BookDetails
{
    public partial class BookDetailsViewModel
    {
        private string _saveAsPath;
        public string SaveAsPath
        {
            get => _saveAsPath;
            set => Set(() => SaveAsPath, ref _saveAsPath, value);
        }

        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
        }

        private string _bookName;
        public string BookName
        {
            get => _bookName;
            set => Set(() => BookName, ref _bookName, value);
        }

        private string _bookAuthor;
        public string BookAuthor
        {
            get => _bookAuthor;
            set => Set(() => BookAuthor, ref _bookAuthor, value);
        }

        private string _bookLogo;
        public string BookLogo
        {
            get => _bookLogo;
            set => Set(() => BookLogo, ref _bookLogo, value);
        }

        private int _bookPagesCount;
        public int BookPagesCount
        {
            get => _bookPagesCount;
            set => Set(() => BookPagesCount, ref _bookPagesCount, value);
        }

        private bool _isGenerateButtonEnabled;
        public bool IsGenerateButtonEnabled
        {
            get => _isGenerateButtonEnabled;
            set => Set(() => IsGenerateButtonEnabled, ref _isGenerateButtonEnabled, value);
        }
    }
}