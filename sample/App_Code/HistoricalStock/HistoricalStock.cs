using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sample
{
    public class HistoricalStock
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public double AdjClose { get; set; }

        public override string ToString()
        {
            return string.Format("Date: {0}, Open: {1}, High: {2}, Low: {3}, Close: {4}, Volume: {5}, Adj Close: {6}", Date, Open, High, Low, Close, Volume, AdjClose);
        }
    }
}