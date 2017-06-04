using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace test.Out.OutEntity
{
	public class ActionController
	{
		public static List<test.Out.Objects.UserEntries> UserData { get; set; }
		public void LoadData(string csvfileLocation)
		{
			UserData = new List<Objects.UserEntries>();
			using(StreamReader reader = new StreamReader(csvfileLocation))
			{
				string i;

				while((i = reader.ReadLine()) != null)
				{
					try
					{
						Regex regx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
						//Separating columns to array
						string[] unserInfo = regx.Split(i);
						if(!unserInfo[0].Equals("FirstName", StringComparison.OrdinalIgnoreCase))
						{
							string[] address = unserInfo[2].ToString().Split(' ');
							test.Out.Objects.UserEntries user = new Objects.UserEntries()
							{
								Firstname = unserInfo[0].ToString(),
								Lastname = unserInfo[1].ToString(),
								Address = new test.Out.Objects.AddressEntries()
								{
									StreetName = string.Format("{0} {1}", address[1], address[2]),
									StreetNumber = address[0]
								},
								PhoneNumber = unserInfo[3].ToString()
							};
							UserData.Add(user);
						}
					}
					catch(Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
		}

		public object WriteToFile(string scenario,string folderPath)
		{
			string filename ="";
			try
			{
				if(Directory.Exists(folderPath))
				{
					 filename = Path.Combine(folderPath, scenario+"_"+ ".txt");
					if(File.Exists(filename))
						File.Delete(filename);
					using(FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite))
					using(StreamWriter sw = new StreamWriter(fs))
					{
						if(scenario == "Scenario1")
						{
							sw.WriteLine("The second should show the addresses sorted alphabetically by street name\n\n");

							string colAddress= "Address".PadRight(20);
							sw.WriteLine("{0}",colAddress);
							foreach(Objects.AddressEntries address in GetAddresses())
							{
								sw.WriteLine("{0} {1}", address.StreetNumber ,address.StreetName);
							}
						}
						else if(scenario == "Scenario2")
						{
							sw.WriteLine("frequency of the first and last names ordered\n");
							sw.WriteLine("\tfrequency of the first ordered by frequency\n");
							string colName = "Name".PadRight(20);
							string colCount = "Count".PadRight(20);
							sw.WriteLine("{0}{1}",colName,colCount);
							foreach(KeyValuePair<string, int> kpv in FirstNameFreq())
							{
								sw.WriteLine("{0}{1}",kpv.Key.ToString().PadRight(20), kpv.Value.ToString().PadRight(20));
							}

							sw.WriteLine("\n\n\n");
							sw.WriteLine("\tfrequency of the first ordered by frequency and Name and lastname\n");
							string colName2 = "Name".PadRight(20);
							string colCount2 = "Count".PadRight(20);
							sw.WriteLine("{0}{1}", colName2, colCount2);
							foreach(KeyValuePair<string, int> kpv in FirstNameSurFreq())
							{
								sw.WriteLine("{0}{1}", kpv.Key.ToString().PadRight(20), kpv.Value.ToString().PadRight(20));
							}
						}
					}
				}
				else
				{
					DirectoryInfo dInfo = new DirectoryInfo(folderPath);
					DirectorySecurity dSecurity = dInfo.GetAccessControl();
					dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
					dInfo.SetAccessControl(dSecurity);

					//	System.Security.AccessControl.DirectorySecurity directorySecurity = new System.Security.AccessControl.DirectorySecurity(folderPath, System.Security.AccessControl.AccessControlSections.All);
					Directory.CreateDirectory(folderPath+ "/Export", dSecurity);
					WriteToFile(scenario, folderPath);
				}
				return new {Success=true,Message= string.Format("Your file is saved in this location: {0}", filename) };
			}
			catch(Exception ex)
			{
				return new
				{
					Success = false,
					Error = ex.Message
				};
			}
		}
		//Scenario 	
		private Dictionary<string, int> GetNameFrequency(string[] names)
		{
			Dictionary<string, int> nameFreq = new Dictionary<string, int>();
			//loop and get name with duplicates and adds duplicate counts
			foreach(string name in names)
			{
				if(nameFreq.ContainsKey(name))
					nameFreq[name]++;
				else
					nameFreq.Add(name, 1);
			}
			return nameFreq;
		}
		//Frequency of firstaname
		public Dictionary<string, int> FirstNameFreq() {
			List<string> names = new List<string>();
			UserData.ForEach(a =>
			{
				names.Add(string.Format("{0}", a.Firstname));
			});
			Dictionary<string, int> FirstNameFreq = GetNameFrequency(names.ToArray()).OrderByDescending(a => a.Value).ToDictionary(k=>k.Key,v=>v.Value);
			return FirstNameFreq;
		}
		//Frequency of firstaname+ lastName
		public Dictionary<string, int> FirstNameSurFreq()
		{

			List<string> names = new List<string>();
			UserData.ForEach(a =>
			{
				names.Add(string.Format("{0} {1}", a.Firstname, a.Lastname));
			});
			Dictionary<string, int> FirstNameFreq = (Dictionary<string, int>)GetNameFrequency(names.ToArray()).OrderByDescending(a => a.Value).OrderBy(a => a.Key).ToDictionary(k => k.Key, v => v.Value);
			return FirstNameFreq;
		}
		//Scenario 1.2
		public List<Objects.AddressEntries> GetAddresses()
		{

			List<Objects.AddressEntries> Addresses = (from a in UserData.Select(a => a.Address)
													  orderby a.StreetName
													  select a).ToList();
			return Addresses;

		}
	}
}
