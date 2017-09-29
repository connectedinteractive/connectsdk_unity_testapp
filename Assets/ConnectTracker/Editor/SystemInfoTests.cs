using Assets.ConnectSdk.Scripts;
using Assets.ConnectSdk.Scripts.Unity;
using NUnit.Framework;
using UnityEngine;

namespace Assets.ConnectSdk.Editor
{
    public class SystemInfoTests
    {
        private ConnectSystemInfo _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new ConnectSystemInfo();
        }

        [Test]
        public void SystemInfoFiresEventWhenAdvertiserIdRegistered()
        {
            var eventTriggered = false;
            _subject.AdvertisingIdCollectedEvent += (s, e) => { eventTriggered = true; };
            _subject.RegisterAdvertisingId("dummy_advertising_id", true, null);
            Assert.IsTrue(eventTriggered);
            Assert.IsTrue(_subject.AdvertisingIdRegistered);
        }
    }
}