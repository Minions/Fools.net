using System;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public struct PossiblySpecified<T> : IEquatable<PossiblySpecified<T>> where T : struct
	{
		private readonly T _value;
		private readonly bool _isSpecified;

		private PossiblySpecified(T value)
		{
			_value = value;
			_isSpecified = true;
		}

		public static PossiblySpecified<T> Unspecifed { get; } = new PossiblySpecified<T>();
		public T Value
		{
			get
			{
				if (!_isSpecified) { throw new NullReferenceException("Attempted to get the value from a PossiblySpecified<T>.Unspecified value."); }
				return _value;
			}
		}

		public static PossiblySpecified<T> WithValue(T value)
		{
			return new PossiblySpecified<T>(value);
		}

		public bool Equals(PossiblySpecified<T> other)
		{
			return !_isSpecified || !other._isSpecified || _value.Equals(other._value);
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj)) { return false; }
			return obj is PossiblySpecified<T> && Equals((PossiblySpecified<T>) obj);
		}

		public override int GetHashCode()
		{
			unchecked { return (_value.GetHashCode()*397) ^ _isSpecified.GetHashCode(); }
		}

		public static bool operator ==(PossiblySpecified<T> left, PossiblySpecified<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(PossiblySpecified<T> left, PossiblySpecified<T> right)
		{
			return !left.Equals(right);
		}
	}
}
