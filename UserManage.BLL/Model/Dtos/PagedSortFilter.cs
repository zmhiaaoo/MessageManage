using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManage.BLL
{
   public class PagedSortFilter
    {
        public PagedSortFilter()
        {
            CurrentPage = 1;
            MaxResultCount = 10;
        }
        //当前页面
        public int CurrentPage { get; set; }
        //每页条数
        public int MaxResultCount { get; set; }
        //排序字段ID
        public string Sorting { get; set; }
        //查询字符串
        public string SearchString { get; set; }
        //过滤
        public GenderEnum? Gender { get; set; }
    }
}
