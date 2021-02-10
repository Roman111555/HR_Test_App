using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary.Models;

namespace PersonLibrary
{
    public class EmployeeModel : PersonMainModel    
    {
        public int chef_id_salesman { get; set; }
        public int chef_id_manager { get; set; }

    }
}
