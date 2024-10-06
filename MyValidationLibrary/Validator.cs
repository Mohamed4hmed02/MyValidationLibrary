using MyValidationLibrary.Results;
using System.Linq.Expressions;

namespace MyValidationLibrary
{
	/// <summary>
	/// Used To Validate Your Custom Validations
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="instance"></param>
	public class Validator<T>(T instance) where T : class
	{
		private readonly List<List<FailureResult>> _failures = [];

		public RulesBuilder RulesFor<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			var type = typeof(T);
			string pName = GetPropertyName(expression);

			var results = new List<FailureResult>();
			var rule = new RulesBuilder(type.GetProperty(pName), instance, results);
			_failures.Add(results);
			return rule;
		}

		public ValidationResult Validate()
		{
			var failureList = new List<FailureResult>();
			foreach (var failure in _failures)
			{
				failureList.AddRange(failure);
			}
			return new ValidationResult(failureList);
		}

		private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			if (expression.Body is MemberExpression memberExpression)
			{
				return memberExpression.Member.Name; // Property name
			}

			throw new ArgumentException("Expression is not a valid property expression.");
		}
	}
}
