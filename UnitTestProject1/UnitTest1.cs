using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResXcore;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		private string _xmlWithThreeValues;
		private string _xmlEmpty;

		[TestInitialize]
		public void Init()
		{
			_xmlWithThreeValues = $@"{Directory.GetCurrentDirectory()}\Resource1.resx";
			_xmlEmpty = $@"{Directory.GetCurrentDirectory()}\Resource_empty.resx";
		}

		[TestMethod]
		public void GetTheThreeElements()
		{
			using (var resXReader = new ResXResourceReader(_xmlWithThreeValues))
			{
				int count = resXReader.Count();
				int expected = 3;

				Assert.AreEqual(expected, count);
			}
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ReadNullFileWithException()
		{
			using (var resXReader = new ResXResourceReader(null))
				resXReader.Count();
		}

		[TestMethod]
		public void ReadEmptyResx()
		{
			using (var resXReader = new ResXResourceReader(_xmlEmpty))
			{
				int count = resXReader.Count();
				int expected = 0;

				Assert.AreEqual(expected, count);
			}
		}

		[TestMethod]
		public void GetValueByKey()
		{
			using (var resXReader = new ResXResourceReader(_xmlWithThreeValues))
			{
				string key = "keyValue";

				string actualValue = (string)resXReader[key];
				string expected = "any value";

				Assert.AreEqual(expected, actualValue);
			}
		}

		[TestMethod]
		public void TestThreeIterationByResx()
		{
			using (var resXReader = new ResXResourceReader(_xmlWithThreeValues))
			{
				int actualValue = 0;
				int expected = 3;

				foreach (var res in resXReader)
					actualValue++;

				Assert.AreEqual(expected, actualValue);
			}
		}
	}
}