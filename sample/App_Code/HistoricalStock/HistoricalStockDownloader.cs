using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Globalization;

namespace sample
{
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

                //foreach (string str in stoksList)
                for(int i = 0; i < 10; i++)
                {
                    string str = stoksList.ElementAt(i);

                    string[] parsedString = str.Split(',');

                    HistoricalStock hs = new HistoricalStock();

                    try
                    {
                        hs.Date = DateTime.ParseExact(parsedString[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        hs.Open = double.Parse(parsedString[1], CultureInfo.InvariantCulture);
                        hs.High = double.Parse(parsedString[2], CultureInfo.InvariantCulture);
                        hs.Low = double.Parse(parsedString[3], CultureInfo.InvariantCulture);
                        hs.Close = double.Parse(parsedString[4], CultureInfo.InvariantCulture);
                        hs.Volume = double.Parse(parsedString[5], CultureInfo.InvariantCulture);
                        hs.AdjClose = double.Parse(parsedString[6], CultureInfo.InvariantCulture);
                    }
                    catch (Exception exc)
                    {
                        // Add log4net
                        File.AppendAllText("G:/Errors.txt", exc.Message + Environment.NewLine);
                    }
                    

                    //File.AppendAllText("G:/InfoNEW.txt", hs + Environment.NewLine);

                    retval.Add(hs);
                }

                return retval;
            }
        }
    }
}
