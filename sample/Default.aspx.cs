﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Newtonsoft.Json;
using System.Linq;

namespace sample
{
    public partial class _Default : System.Web.UI.Page
    {
        // Stock symbols seperated by space or comma.
        protected string m_symbol = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // The page is being loaded and accessed for the first time.
                // Извлекать user input from the form.
                if (Request.QueryString["s"] == null)
                    // Компания по умолчанию
                    m_symbol = "MSFT";
                else
                    // Get the user's input.
                    m_symbol = Request.QueryString["s"].ToString().ToUpper();
                // Update the textbox value.
                txtSymbol.Value = m_symbol;
                // This DIV that contains text and DIVs that displays stock quotes and chart from Yahoo.
                // Set the innerHTML property to replaces the existing content of the DIV.
                divService.InnerHtml = "<br />";
                if (m_symbol.Trim() != "")
                {
                    try
                    {
                        // Return the stock quote data in XML format.
                        String arg = GetQuote(m_symbol.Trim());                        
                        if (arg == null)
                            return;

                        // Read XML.
                        // Declare an XmlDocument object to represents an XML document.
                        XmlDocument xd = new XmlDocument();
                        // Loads the XML data from a stream.
                        xd.LoadXml(arg);
                        

                        // Read XSLT
                        // Declare an XslCompiledTransform object to transform XML data using an XSLT style sheet.
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        // Use the Load method to load the Xsl transform object.
                        xslt.Load(Server.MapPath("stock.xsl"));
                                                
                        // Transform the XML document into HTML.
                        StringWriter fs = new StringWriter();
                        xslt.Transform(xd.CreateNavigator(), null, fs);
                        string result = fs.ToString();

                        // Replace the characters "&gt;" and "&lt;" back to "<" and ">".
                        divService.InnerHtml = "<br />" + result.Replace("&lt;", "<").Replace("&gt;", ">") + "<br />";

                        // Display stock charts.
                        String[] symbols = m_symbol.Replace(",", " ").Split(' ');
                        // Loop through each stock
                        for (int i = 0; i < symbols.Length; ++i)
                        {
                            if (symbols[i].Trim() == "")
                                continue;
                            int index = divService.InnerHtml.ToLower().IndexOf(symbols[i].Trim().ToLower() + " is invalid.");
                            // If index = -1, the stock symbol is valid.
                            if (index == -1)
                            {                                
                                // Use a random number to defeat cache.
                                Random random = new Random();
                                divService.InnerHtml += "<img id='imgChart_" + i.ToString() + "' src='http://ichart.finance.yahoo.com/b?s=" + symbols[i].Trim().ToUpper() + "& " + random.Next() + "' border=0><br />";
                                // 1 days
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(0," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div1d_" + i.ToString() + "'><b>1d</b></span></a>&nbsp;&nbsp;";
                                // 5 days
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(1," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div5d_" + i.ToString() + "'>5d</span></a>&nbsp;&nbsp;";
                                // 3 months
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(2," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div3m_" + i.ToString() + "'>3m</span></a>&nbsp;&nbsp;";
                                // 6 months
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(3," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div6m_" + i.ToString() + "'>6m</span></a>&nbsp;&nbsp;";
                                // 1 yeas
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(4," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div1y_" + i.ToString() + "'>1y</span></a>&nbsp;&nbsp;";
                                // 2 years
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(5," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div2y_" + i.ToString() + "'>2y</span></a>&nbsp;&nbsp;";
                                // 5 years
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(6," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='div5y_" + i.ToString() + "'>5y</span></a>&nbsp;&nbsp;";
                                // Max
                                divService.InnerHtml += "<a style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: Blue;' href='javascript:changeChart(7," + i.ToString() + ", \"" + symbols[i].ToLower() + "\");'><span id='divMax_" + i.ToString() + "'>Max</span></a><br><br /><br />&nbsp;&nbsp;";
                            }
                        }
                    }
                    catch
                    {
                        // Handle exceptions
                    }
                }
            }
        }

        /// <summary>
        /// This function handles and parses multiple stock symbols as input parameters 
        /// and builds a valid XML return document.
        /// </summary>
        /// <param name="symbol">A bunch of stock symbols seperated by space or comma</param>
        /// <returns>Return stock quote data in XML format</returns>
        public string GetQuote(string symbol)
        {
            // Set the return string to null.
            string result = null;            
            try
            {
                // Use Yahoo finance service to download stock data from Yahoo
                // да, здесь получаем акцию за сейчас и ее инфу
                string yahooURL = @"http://download.finance.yahoo.com/d/quotes.csv?s=" + symbol + "&f=sl1d1t1c1hgvbap2";

                // Походу здесь только одно имя компании лежит и все
                string[] symbols = symbol.Replace(",", " ").Split(' ');

                // Initialize a new WebRequest.
                HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(yahooURL);
                // Get the response from the Internet resource.
                HttpWebResponse webresp = (HttpWebResponse)webreq.GetResponse();
                // Read the body of the response from the server.
                StreamReader strm = new StreamReader(webresp.GetResponseStream(), Encoding.ASCII);


                // *******************************************************************************
                
                 

                List<HistoricalStock> data = HistoricalStockDownloader.DownloadData(symbol, 1962);

             

                var buff = from p in data where p.Date.Year == 2016
                          select new
                           {
                               p.Date,
                               p.High,
                               p.Low
                           };

                foreach(var item in buff)
                    File.AppendAllText("G:/Info.txt", item + Environment.NewLine);

                DateTime dateOfHighestPrice = buff.Last().Date;
                File.AppendAllText("G:/Info.txt", "Та самая дата: " + dateOfHighestPrice + Environment.NewLine);
                NewsModel._fromDate = dateOfHighestPrice.AddDays(-1);
                File.AppendAllText("G:/Info.txt", "Предыдущий день: " + NewsModel._fromDate + Environment.NewLine);
                NewsModel._toDate = dateOfHighestPrice.AddDays(+1);
                File.AppendAllText("G:/Info.txt", "Следующий день: " + NewsModel._toDate + Environment.NewLine);

                NewsModel._phrase = symbol;

                // ********************************************************************************





                // Construct a XML in string format.
                string tmp = "<StockQuotes>";
                string content = "";
                for (int i = 0; i < symbols.Length; i++)
                {
                    // Loop through each line from the stream, building the return XML Document string
                    if (symbols[i].Trim() == "")
                        continue;

                    content = strm.ReadLine().Replace("\"", "");
                    string[] contents = content.ToString().Split(',');
                    // If contents[2] = "N/A". the stock symbol is invalid.
                    if (contents[2] == "N/A")
                    {
                        // Construct XML via strings.
                        tmp += "<Stock>";
                        // "<" and ">" are illegal in XML elements. Replace the characters "<" and ">" to "&gt;" and "&lt;".
                        tmp += "<Symbol>&lt;span style='color:red'&gt;" + symbols[i].ToUpper() + " is invalid.&lt;/span&gt;</Symbol>";
                        tmp += "<Last></Last>";
                        tmp += "<Date></Date>";
                        tmp += "<Time></Time>";
                        tmp += "<Change></Change>";
                        tmp += "<High></High>";
                        tmp += "<Low></Low>";
                        tmp += "<Volume></Volume>";
                        tmp += "<Bid></Bid>";
                        tmp += "<Ask></Ask>";
                        tmp += "<Ask></Ask>";
                        tmp += "</Stock>";
                    }
                    else
                    {
                        //construct XML via strings.
                        tmp += "<Stock>";
                        tmp += "<Symbol>" + contents[0] + "</Symbol>";
                        try
                        {
                            tmp += "<Last>" + String.Format("{0:c}", Convert.ToDouble(contents[1])) + "</Last>";
                        }
                        catch
                        {
                            tmp += "<Last>" + contents[1] + "</Last>";
                        }
                        tmp += "<Date>" + contents[2] + "</Date>";
                        tmp += "<Time>" + contents[3] + "</Time>";
                        // "<" and ">" are illegal in XML elements. Replace the characters "<" and ">" to "&gt;" and "&lt;".
                        if (contents[4].Trim().Substring(0, 1) == "-")
                            tmp += "<Change>&lt;span style='color:red'&gt;" + contents[4] + "(" + contents[10] + ")" + "&lt;span&gt;</Change>";
                        else if (contents[4].Trim().Substring(0, 1) == "+")
                            tmp += "<Change>&lt;span style='color:green'&gt;" + contents[4] + "(" + contents[10] + ")" + "&lt;span&gt;</Change>";
                        else
                            tmp += "<Change>" + contents[4] + "(" + contents[10] + ")" + "</Change>";
                        tmp += "<High>" + contents[5] + "</High>";
                        tmp += "<Low>" + contents[6] + "</Low>";
                        try
                        {
                            tmp += "<Volume>" + String.Format("{0:0,0}", Convert.ToInt64(contents[7])) + "</Volume>";
                        }
                        catch
                        {
                            tmp += "<Volume>" + contents[7] + "</Volume>";
                        }
                        tmp += "<Bid>" + contents[8] + "</Bid>";
                        tmp += "<Ask>" + contents[9] + "</Ask>";
                        tmp += "</Stock>";
                    }
                    // Set the return string
                    result += tmp;
                    tmp = "";
                }
                // Set the return string
                result += "</StockQuotes>";
                // Close the StreamReader object.
                strm.Close();
            }
            catch
            {
                // Handle exceptions.
            }
            // Return the stock quote data in XML format.
            return result;
        }





        // ******************************
        // New methos might be situeted here

        [WebMethod]
        public static ItemNews[] GetNewsContent(string SearchPhrase)
        {
            List<ItemNews> Details = new List<ItemNews>();

            // httpWebRequest with API url 
            //string guardianURL = @"http://content.guardianapis.com/search?q=" + SearchPhrase + "&from-date=" + NewsModel._fromDate.ToString("yyyy-MM-dd") + "&to-date=" + NewsModel._toDate.ToString("yyyy-MM-dd") + "&order-by=oldest&page-size=30" + "&api-key=6392a258-3c53-4e76-87ec-e9092356fa74";
            string guardianURL = @"http://content.guardianapis.com/search?q=" + SearchPhrase + "&section=technology&from-date=" + NewsModel._fromDate.ToString("yyyy-MM-dd") + "&to-date=" + NewsModel._toDate.ToString("yyyy-MM-dd") + "&order-by=oldest&page-size=30" + "&api-key=6392a258-3c53-4e76-87ec-e9092356fa74";

            //"&tag=" + NewsModel._phrase +

            // ********
            File.AppendAllText("G:/Error.txt", guardianURL + Environment.NewLine);
            // *********

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(guardianURL);

            //Method GET
            request.Method = "GET";

            //HttpWebResponse for result
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Mapping of status code
            if (response.StatusCode == HttpStatusCode.OK)
            {

                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == "")
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                //Get news data in json string

                string data = readStream.ReadToEnd();

                //Declare DataSet for put data in it.
                DataSet ds = new DataSet();
                StringReader reader = new StringReader(data);

                // uncomment
                //ds.ReadXml(reader);
                //DataTable dtGetNews = new DataTable();

                // parse my json here
                try
                {
                    //var model = JsonConvert.DeserializeObject<List<NewsModel.RootObject>>(data);
                    var model = JsonConvert.DeserializeObject<NewsModel.RootObject>(data);
                    foreach(NewsModel.Result res in model.response.results)
                    {
                        //File.AppendAllText("G:/News.txt", res + Environment.NewLine);
                        ItemNews news = new ItemNews();
                        news.title = res.webTitle;
                        news.link = res.webUrl;
                        news.item_id = res.id;
                        news.PubDate = res.webPublicationDate;
                        //news.Description = null;

                        Details.Add(news);
                        File.AppendAllText("G:/News.txt", news + Environment.NewLine);
                    }
                        
                } catch(Exception exc)
                {
                    File.AppendAllText("G:/Error.txt", "Something bad with json parsing. Message: " + exc.Message);
                }
                

                /*if (ds.Tables.Count > 3)
                {
                    dtGetNews = ds.Tables["item"];

                    foreach (DataRow dtRow in dtGetNews.Rows)
                    {
                        ItemNews DataObj = new ItemNews();
                        DataObj.title = dtRow["title"].ToString();
                        //DataObj.link = dtRow["link"].ToString();
                        //DataObj.Description = dtRow["description"].ToString();
                        //DataObj.PubDate = dtRow["pubDate"].ToString();
                        //DataObj.item_id = dtRow["item_id"].ToString();

                        //File.AppendAllText("G:/Info.txt", DataObj.title + Environment.NewLine);
                        //File.AppendAllText("G:/Info.txt", DataObj.link + Environment.NewLine);
                        //File.AppendAllText("G:/Info.txt", DataObj.Description + Environment.NewLine);
                        //File.AppendAllText("G:/Info.txt", DataObj.PubDate + Environment.NewLine + Environment.NewLine);

                        Details.Add(DataObj);
                    }

                }*/
            }


            //Return News array 
            return Details.ToArray();
        }

        //Define Class to return news data
        public class ItemNews
        {
            public string title { get; set; }
            public string link { get; set; }
            public string item_id { get; set; }
            public string PubDate { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return string.Format("Title: {0}, Link: {1}, Pubdate: {2}.", title, link, PubDate);
            }
        }
    }
}
