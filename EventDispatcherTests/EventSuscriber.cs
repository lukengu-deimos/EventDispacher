using System;
using BPL.Event.EventDispatcher.Interfaces;

namespace EventDispatcherTests;

public class EventSubscriber : IEventSubscriber<NotificationEvent>
{
    public void HandleEvent(NotificationEvent @event)
    {
        Console.WriteLine(@event.userId.ToString());
        Console.WriteLine("expecting 28");
    }
}

