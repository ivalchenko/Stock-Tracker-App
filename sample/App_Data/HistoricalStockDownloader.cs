using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

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
    }

    public class HistoricalStockDownloader
    {
        public static List<HistoricalStock> DownloadData(string ticker, int yearToStartFrom)
        {
            List<HistoricalStock> retval = new List<HistoricalStock>();

            using (WebClient web = new WebClient())
            {
                //string data = web.DownloadString(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&c={1}", ticker, yearToStartFrom));

                try
                {
                    File.WriteAllText("G:/TestOfTests.csv", web.DownloadString(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&c={1}", ticker, yearToStartFrom)));
                }
                catch (FileNotFoundException exc)
                {
                    Console.WriteLine(exc.Message);
                }

                StreamReader sr = new StreamReader("G:/TestOfTests.csv");
                
                string currentLine;
                List<string> stoksList = new List<string>();

                while ((currentLine = sr.ReadLine()) != null)
                    stoksList.Add(currentLine);

                stoksList.RemoveAt(0);

                foreach (string str in stoksList)
                {
                    //File.AppendAllText("G:/Info.txt", str + Environment.NewLine);

                    string[] parsedString = str.Split(',');

                    /*foreach(string s in parsedStrings)
                        File.AppendAllText("G:/Info.txt", s + Environment.NewLine);*/

                    HistoricalStock hs = new HistoricalStock();

                    hs.Date = Convert.ToDateTime(parsedString[0]);
                    hs.Open = Convert.ToDouble(parsedString[1]);
                    hs.High = Convert.ToDouble(parsedString[2]);
                    hs.Low = Convert.ToDouble(parsedString[3]);
                    hs.Close = Convert.ToDouble(parsedString[4]);
                    hs.Volume = Convert.ToDouble(parsedString[5]);
                    hs.AdjClose = Convert.ToDouble(parsedString[6]);

                    retval.Add(hs);
                }

                return retval;
            }
        }
    }
}