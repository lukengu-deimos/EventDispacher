using System.Reflection;

namespace BPL.Event.Dispatcher.Interfaces;

public interface IEventDispatcher
{
    public void RegisterEventSubscribers(Assembly assembly);
    public void RegisterEventSubscriber(object eventSubscription);
    public void AddListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent;
    public void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent;
    public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;
}