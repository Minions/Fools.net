using System;
using System.Linq.Expressions;
using FluentAssertions;

namespace Lair.Tests.ZzTestHelpers
{
	internal static class EventMonitoringAssertionExtensions
	{
		public static EventMonitoringAssertionHelper<T> MonitoringEvents<T>(this T subject, Action<T> action)
		{
			return new EventMonitoringAssertionHelper<T>(subject, action);
		}

		public class EventMonitoringAssertionHelper<T>
		{
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

			private readonly Action<T> _action;
			private readonly T _subject;
		}
	}
}
