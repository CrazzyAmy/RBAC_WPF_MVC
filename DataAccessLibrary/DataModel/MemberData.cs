using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataModel
{
    public class MemberData
    {
        public string Account {  get; set; }
        public string Pword { get; set; }
        public int AuthorityLevel { get; set; }
        public DateTime StartDate { get; set; }
        
    }
}
