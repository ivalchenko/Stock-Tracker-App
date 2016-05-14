using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sample
{
    public class NewsModel
    {
        public static DateTime _fromDate;
        public static DateTime _toDate;
        public static string _phrase;

        public class Response
        {
            public string status { get; set; }
            public string userTier { get; set; }
            public int total { get; set; }
            public int startIndex { get; set; }
            public int pageSize { get; set; }
            public int currentPage { get; set; }
            public int pages { get; set; }
            public string orderBy { get; set; }
            public List<Result> results { get; set; }
        }

        public class Result
        {
            public string type { get; set; }
            public string sectionId { get; set; }
            public string webTitle { get; set; }
            public string webPublicationDate { get; set; }
            public string id { get; set; }
            public string webUrl { get; set; }
            public string apiUrl { get; set; }
            public string sectionName { get; set; }

            public override string ToString()
            {
                return string.Format("Title: {0}, PubDate: {1}", webTitle, webPublicationDate);
            }
        }

        public class RootObject
        {
            public Response response { get; set; }
        }
    }
}