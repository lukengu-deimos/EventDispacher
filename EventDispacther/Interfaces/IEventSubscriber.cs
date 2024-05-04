using System;
namespace EventDispacther.Interfaces
{
	public interface IEventSubscriber<TEvent> where TEvent : IEvent
	{
		public void HandleEvent(TEvent @event);
	}
}

