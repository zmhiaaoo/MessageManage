using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManage.BLL
{
  public  class GetMessageIndex:PagedSortFilter
    {
        public GetMessageIndex()
        {
            Sorting = "Id";
        }
    }
}
