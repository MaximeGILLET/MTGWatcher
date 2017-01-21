using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTGWatcher.Models
{
    public class PaginationModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDir { get; set; }
        public int ResultCount { get; set; }
    }
}