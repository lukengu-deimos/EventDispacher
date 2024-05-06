using System.Reflection;
using BPL.Event.Dispatcher;

namespace BPL.Event.DispatcherTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        var dispatcher = new EventDispatcher();
        var notificationEvent = new NotificationEvent(289);
        dispatcher.RegisterEventSubscribers(Assembly.GetExecutingAssembly());
        dispatcher.Dispatch(notificationEvent);
    }
}
