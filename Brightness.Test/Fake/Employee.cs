using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Test.Fake
{
    /// <summary>
    /// This is just a fake model class.
    /// </summary>
    class Employee : IEquatable<Employee>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Equals(Employee other)
        {
            return other.FirstName == this.FirstName && other.LastName == this.LastName;
        }
    }
}
