using System.Linq.Expressions;

namespace MyValidationLibrary
{
	/// <summary>
	/// Used To Build Your Custom Validations
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="instance"></param>
	public class Validator<T>(T instance) where T : class
	{
		public RulesBuilder RulesFor<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			var type = typeof(T);
			string pName = GetPropertyName(expression);
			return new RulesBuilder(type.GetProperty(pName), instance);
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
