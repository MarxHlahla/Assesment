using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Out.Objects
{
    public class UserEntries
    {
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public AddressEntries Address { get; set; }
		public string PhoneNumber { get; set; }

	}
	public class AddressEntries
	{
		public string StreetNumber { get; set; }
		public string StreetName { get; set; }
	}
}
