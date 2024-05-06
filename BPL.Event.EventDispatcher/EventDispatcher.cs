using System.Reflection;
using BPL.Event.Dispatcher.Interfaces;

namespace BPL.Event.Dispatcher;

public class EventDispatcher: IEventDispatcher
{ 
    private readonly Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

    public void AddListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (!_eventListeners.TryGetValue(eventType, out var value))
        {
            value = new List<Delegate>();
            _eventListeners[eventType] = value;
        }

        value.Add(listener);
    }

    public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (!_eventListeners.TryGetValue(eventType, out var eventListener)) return;
        foreach (var listener in eventListener.ToList())
        {
            ((Action<TEvent>)listener)(@event);
        }
    }

    public void RegisterEventSubscriber(object eventSubscriber)
    {

        if (!ImplementsEventSubscriberInterface(eventSubscriber.GetType()))
        {
            return;
        }
        var eventTypes = GetEventTypes(eventSubscriber.GetType());

        foreach (var eventType in eventTypes)
        {
            if (!_eventListeners.TryGetValue(eventType, out var value))
            {
                value = new List<Delegate>();
                _eventListeners[eventType] = value;
            }

            value.Add(CreateEventHandlerDelegate(eventSubscriber, eventType));
        }
    }

    public void RegisterEventSubscribers(Assembly assembly)
    {

        var types = assembly.GetTypes();
        var eventSubscriberTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && ImplementsEventSubscriberInterface(type));

        foreach (var eventSubscriberType in eventSubscriberTypes)
        {
            var eventSubscriber = Activator.CreateInstance(eventSubscriberType);
            RegisterEventSubscriber(eventSubscriber!);
        }
    }

    public void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (_eventListeners.TryGetValue(eventType, out var eventListener))
        {
            eventListener.Remove(listener);
        }
    }


    private static bool ImplementsEventSubscriberInterface(Type type)
    {
        return Array.Exists(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventSubscriber<>));
    }

    private static IEnumerable<Type> GetEventTypes(Type eventSubscriberType)
    {
        var eventSubscriberInterface = typeof(IEventSubscriber<>);
        return eventSubscriberType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventSubscriberInterface)
            .Select(i => i.GetGenericArguments()[0]);
    }

    private static Delegate CreateEventHandlerDelegate(object eventSubscriber, Type eventType)
    {
        var handleEventMethod = typeof(IEventSubscriber<>).MakeGenericType(eventType)
                 .GetMethod("HandleEvent");

        return Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), eventSubscriber, handleEventMethod!);
    }
    
}