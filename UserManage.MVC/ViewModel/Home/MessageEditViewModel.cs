using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class MessageEditViewModel:MessageCreateViewModel
    {
        public int Id { get; set; }
        public List<string> ExistingPhotoPath { get; set; }
    }
}
