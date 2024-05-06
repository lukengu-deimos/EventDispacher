namespace BPL.Event.Dispatcher.Interfaces;

public interface IEventSubscriber<TEvent> where TEvent : IEvent
{
    public void HandleEvent(TEvent @event);   
}