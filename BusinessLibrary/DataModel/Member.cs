using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.DataModel
{
    public class Member
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public int AuthorityLevel { get; set; }
        public DateTime StartDate { get; set; }
    }
}
