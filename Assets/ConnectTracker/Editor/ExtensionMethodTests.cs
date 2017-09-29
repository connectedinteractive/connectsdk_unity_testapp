using System.Collections.Generic;
using Assets.ConnectSdk.Scripts;
using Assets.ConnectSdk.Scripts.Unity;
using NUnit.Framework;
using UnityEngine;

namespace Assets.ConnectSdk.Editor
{
    public class ExtensionMethodTests {

        [Test]
        public void StringSubstitutionWorks()
        {
            var subject = "%REPLACE_THIS% touch this";
            var dictionary = new Dictionary<string, string>();
            dictionary["REPLACE_THIS"] = "can't";

            Assert.AreEqual(subject.Substitute(dictionary), "can't touch this");
        }
    }
}
