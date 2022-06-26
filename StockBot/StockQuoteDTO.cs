﻿using CsvHelper.Configuration.Attributes;
using System;

namespace ChatApplication.StockBot
{
    public class StockQuoteDTO
    {
        [Name("Close")]
        public decimal Quote { get; set; }
        [Name("Symbol")]
        public string Code { get; set; }
        [Name("Date")]
        public DateTime Date { get; set; }
        [Name("Time")]
        public TimeSpan Time { get; set; }
    }
}