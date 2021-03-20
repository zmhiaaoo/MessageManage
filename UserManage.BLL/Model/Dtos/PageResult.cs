using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManage.BLL.Model.Dtos
{
   public class PageResult:PagedSortFilter
    {
        //数据总数
       public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount,MaxResultCount));
        public List<Message> Data { get; set; }
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
