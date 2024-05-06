using System;
using BPL.Event.Dispatcher.Interfaces;

namespace BPL.Event.DispatcherTests
{
    public class NotificationSubscriber : IEventSubscriber<NotificationEvent>
    {
		

        void IEventSubscriber<NotificationEvent>.HandleEvent(NotificationEvent @event)
        {
            Console.Write(@event.UserId.ToString());
        }
    }
}

