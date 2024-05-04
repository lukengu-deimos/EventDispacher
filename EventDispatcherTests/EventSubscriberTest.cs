using EventDispacther.Dispatcher;
using System.Reflection;
namespace EventDispatcherTests;

[TestClass]
public class EventSubscriberTest
{
    [TestMethod]
    public void RegisterEventSubscribersTest()
    {
        Console.WriteLine("Begining Test");
        EventDispatcher dispatcher = new EventDispatcher();
        dispatcher.RegisterEventSubscribers(Assembly.GetExecutingAssembly());
        dispatcher.Dispatch(new NotificationEvent(28));
    }
}
