using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SearchEngine;

namespace TestLucene
{
   [TestFixture]
    public class TestSet
    {
        [TestFixtureSetUp]
        public void Init()
        {
            int y = 0;           
        }

       [Test]
       public void MyTest()
        {
            var myEngine = new SearchEngineLucene(true);
            myEngine.CreateNewDocument();
            myEngine.AddField("Name", "Diego Aldo Burlando", "YES", "ANALYZED");
            myEngine.AddField("Email", "Diego@dmm888.com", "YES", "ANALYZED");
            myEngine.AddDocument();
            var results=  myEngine.Search("Name","Diego");
            var results1 = myEngine.Search("Name", "Diego", "Email"); 
            Assert.AreEqual(1, 1);
        }


    }
}
