using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_app_api.Dto
{
    public class ItemDto
    {
        public class Add
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
        public class Update
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
        }
        public class List : Update
        {

        }
    }
}
