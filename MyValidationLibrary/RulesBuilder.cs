using MyValidationLibrary.Results;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MyValidationLibrary
{
	public class RulesBuilder
	{
		private readonly PropertyInfo? _property;
		private readonly object _instance;
		private readonly IList<FailureResult> _failures;
		private readonly bool _isNull;
		private readonly object? _value;

		public RulesBuilder(
			PropertyInfo? info,
			object instance,
			IList<FailureResult> failures)
		{
			_property = info;
			_failures = failures;
			_instance = instance;
			_isNull = _property?.GetValue(_instance) is null;
			_value = _property?.GetValue(_instance);
		}

		public RulesBuilder NotNull()
		{
			if (_isNull)
			{
				_failures.Add(new FailureResult
				{
					PropertyName = _property.Name,
					Message = $"{_property.Name}: Has A Null Value"
				});
			}
			return this;
		}

		public RulesBuilder MaxLength(int length)
		{
			if (_value is string)
			{
				var stringValue = _value as string;
				if (stringValue?.Length > length)
				{
					_failures.Add(new FailureResult
					{
						PropertyName = _property.Name,
						Message = $"{_property.Name}: Has Exceed The Maximum Lenght {length}"
					});
				}
			}
			else
				_failures.Add(new FailureResult
				{
					PropertyName = _property.Name,
					Message = $"{_property.Name}: Is Not An Array Type"
				});

			return this;
		}

		public RulesBuilder MustEqual(object value)
		{
			if (!_value.Equals(value))
			{
				_failures.Add(new FailureResult
				{
					PropertyName = _property.Name,
					Message = $"{_property.Name}:Is Not Equal The Given Value"
				});
			}
			return this;
		}

		public RulesBuilder MustMatch(string pattern)
		{
			var stringValue = _value as string;
			if (stringValue is null || !Regex.IsMatch(stringValue, pattern))
			{
				_failures.Add(new FailureResult
				{
					PropertyName = _property.Name,
					Message = $"{_property.Name}:Is Not Matching The Given Pattern"
				});
			}
			return this;
		}
	}
}
