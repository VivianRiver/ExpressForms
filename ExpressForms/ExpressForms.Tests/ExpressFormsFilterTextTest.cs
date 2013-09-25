using ExpressForms.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressForms.Tests
{
    
    
    /// <summary>
    ///This is a test class for ExpressFormsFilterTextTest and is intended
    ///to contain all ExpressFormsFilterTextTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExpressFormsFilterTextTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public class TestClass2
        {
            public string Text { get; set; }
        }

        /// <summary>
        ///A test for GenerateFilterIl
        ///</summary>
        [DeploymentItem(@"ExpressForms.Tests\FilterTestData.mdf"), DataSource("System.Data.SqlClient", @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\FilterTestData.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True", "Text_TestValues", DataAccessMethod.Sequential), TestMethod()]
        public void GenerateFilterIlTest()
        {
            // Arrange
            ExpressFormsFilterText target = new ExpressFormsFilterText();
            DynamicMethod method = new DynamicMethod("Wow", typeof(bool), new Type[] { typeof(TestClass2) });
            ILGenerator generator = method.GetILGenerator();
            PropertyInfo property = typeof(TestClass2).GetProperty("Text");
            Dictionary<string, string> thisFilter = new Dictionary<string, string>() { { "filterMode", Convert.ToString(TestContext.DataRow["FilterMode"]) }, { "filterText", Convert.ToString(TestContext.DataRow["FilterText"]) } };
            TestClass2 argument = new TestClass2() { Text = Convert.ToString(TestContext.DataRow["ValueToMatch"]) };            

            // Act
            target.GenerateFilterIl(generator, thisFilter, property);
            // The method would eventually return true if it didn't encounter a reason to return false.
            generator.EmitReturnTrue();
            object objResult = method.Invoke(null, new[] { argument });
            bool result = (bool)objResult;

            // Assert
            bool expected = Convert.ToBoolean(TestContext.DataRow["Result"]);
            Assert.AreEqual(expected, result);
        }
    }
}
