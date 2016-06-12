using System;
using System.Linq.Expressions;
using FluentAssertions;

namespace Ide.Tests
{
    static class EventMonitoringAssertionExtensions
    {
        public static EventMonitoringAssertionHelper<T> MonitoringEvents<T>(this T subject, Action<T> action)
        {
            return new EventMonitoringAssertionHelper<T>(subject, action);
        }

        public class EventMonitoringAssertionHelper<T>
        {
            readonly Action<T> _action;
            readonly T _subject;

            public EventMonitoringAssertionHelper(T subject, Action<T> action)
            {
                _subject = subject;
                _action = action;
            }

            public void ShouldRaisePropertyChangeFor(Expression<Func<T, object>> propertyExpression)
            {
                _subject.MonitorEvents();
                _action(_subject);
                _subject.ShouldRaisePropertyChangeFor(propertyExpression);
            }
        }
    }
}