using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaCorp
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BDate { get; set; }

        public Employee(int id, string firstName, string lastName, DateTime bDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BDate = bDate;
        }
    }
}
