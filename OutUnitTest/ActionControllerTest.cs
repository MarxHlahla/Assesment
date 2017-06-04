using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web;
using System.Linq;
using System.Configuration;
using System.Web.Script.Serialization;

namespace test.Out.OutUnitTest
{
	[TestClass]
	public class ActionControllerTest
	{
		public test.Out.OutEntity.ActionController actController
		{
			get { return new test.Out.OutEntity.ActionController(); }
		}  
	
		//	private test.Out.OutEntity.ActionController actController { get { return new OutEntity.ActionController(); } }
		[TestMethod]
		public void ShouldReturnWriteToFileLocation()
		{
			//Arrange
			actController.LoadData("Test/dataTest.csv");
			//Act
			object result = actController.WriteToFile("Scenario1", "/");
			//Assert
			string filename = "/Scenario1" + "_" + ".txt";
			object compVal = new { Success = true, Message = string.Format("Your file is saved in this location: {0}", filename) };
			var js = new JavaScriptSerializer();
			Assert.AreEqual(js.Serialize(compVal), js.Serialize(result));
		}
		[TestMethod]
		public void ShouldReturnDictionryListWithListOfTwoNamesAndTimCountTwo()
		{
			//Arrange 
			actController.LoadData("Test/dataTest.csv");
			Dictionary<string, int> kpv = new Dictionary<string, int>();
			kpv.Add("Tom", 2);
			kpv.Add("James", 1);
			//Act 
			Dictionary<string, int> freq = actController.FirstNameFreq();
			//Assert
			CollectionAssert.AreEquivalent(kpv, freq);

		}
		[TestMethod]
		public void ShouldReturnThreeAddresseStartingWithRisik()
		{
			//Arrange 
			actController.LoadData("Test/dataTest.csv");
			List<test.Out.Objects.AddressEntries> addresses = new List<Objects.AddressEntries>();
			test.Out.Objects.AddressEntries address1 = new Objects.AddressEntries() {
				StreetNumber = "102",
				StreetName = "Risik str"
			};
			
			test.Out.Objects.AddressEntries address2 = new Objects.AddressEntries()
			{
				StreetNumber = "65",
				StreetName = "Roma str"
			};
			test.Out.Objects.AddressEntries address3 = new Objects.AddressEntries()
			{
				StreetNumber = "82",
				StreetName = "Troy str"
			};
			addresses.Add(address1);
			addresses.Add(address2);
			addresses.Add(address3);
			//Act
			string result = actController.GetAddresses().FirstOrDefault().StreetName;
			//Assert
			Assert.AreEqual(addresses.FirstOrDefault().StreetName, result);
		}
	}
}
