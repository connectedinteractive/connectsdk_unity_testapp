using System.Threading;
using Assets.ConnectSdk.Scripts;
using Assets.ConnectSdk.Scripts.Unity;
using NSubstitute;
using NUnit.Framework;

namespace Assets.ConnectSdk.Editor
{
    public class MessageQueueTests
    {
        //private MessageQueue _subject;

        //[SetUp]
        //public void Setup()
        //{
        //    _subject = new MessageQueue(1, false);
        //}

        //[Test]
        //public void ManagerProcessesQueueAfterEnabled()
        //{
        //    var wait = new EventWaitHandle(false, EventResetMode.ManualReset);
        //    var stubMessage = Substitute.For<IMessage>();
        //    var response = new HttpLogEntry {ResponseCode = 200};
        //    stubMessage.Process().Returns(response).AndDoes(x => wait.Set());
        //    _subject.Enqueue(stubMessage);
        //    _subject.Enabled = true;
        //    Assert.IsTrue(wait.WaitOne(25));
        //    stubMessage.Received().Process();
        //}

        //[Test]
        //public void MessageRequeuedAfterError()
        //{
        //    var stubMessage = Substitute.For<IMessage>();
        //    var response = new HttpLogEntry {ResponseCode = 500};
        //    stubMessage.Process().Returns(response);
        //    _subject.Enqueue(stubMessage);
        //    _subject.ProcessQueue();
        //    Assert.IsTrue(_subject.Contains(stubMessage));
        //}
    }
}