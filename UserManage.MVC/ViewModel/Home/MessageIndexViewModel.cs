using MessageManage.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class MessageIndexViewModel
    {
        public string SearchString { get; set; }
        public GenderEnum? Gender { get; set; }
        public string Sorting { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public List<Message> Messages { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageCount));
        //显示上一页
        public bool ShowPrevious => CurrentPage > 1;
        //显示下一页
        public bool ShowNext => CurrentPage < TotalPages;
        //是否为第一页
        public bool ShowFirst => CurrentPage != 1;
        //是否为最后一页
        public bool ShowLast => CurrentPage != TotalPages;
    }
}
