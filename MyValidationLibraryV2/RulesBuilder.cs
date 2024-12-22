using MyValidationLibraryV2.Structures;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MyValidationLibraryV2
{
	/// <summary>
	/// Contains The Rules Method
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RulesBuilder<T>
	{
		private readonly IList<CustomeStructure> _compiledDelegates;
		private readonly string _propName;
		private readonly Type _propType;

		internal RulesBuilder(IList<CustomeStructure> delegates, string propName, Type propType)
		{
			_compiledDelegates = delegates;
			_propName = propName;
			_propType = propType;
		}

		public RulesBuilder<T> HasMaxLength(int maxLength)
		{
			ParameterExpression left = Expression.Parameter(typeof(object));
			ConstantExpression constant = Expression.Constant(maxLength);

			Expression body;

			if (_propType == typeof(string))
			{
				var lenProp = Expression.Property(Expression.Convert(left, typeof(string)), "Length");
				body = Expression.LessThanOrEqual(lenProp, constant);
			}
			else
			{
				body = Expression.LessThanOrEqual(Expression.Convert(left, typeof(int)), constant);
			}

			Expression<Func<object, bool>> lambda =
			Expression.Lambda<Func<object, bool>>(body, left);

			_compiledDelegates.Add(new CustomeStructure(lambda.Compile(), _propName, RulesName.HasMaxLength, maxLength));

			return this;
		}

		public RulesBuilder<T> HasMinLength(int minLength)
		{
			ParameterExpression left = Expression.Parameter(typeof(object));
			ConstantExpression constant = Expression.Constant(minLength);

			Expression body;

			if (_propType == typeof(string))
			{
				var lenProp = Expression.Property(Expression.Convert(left, typeof(string)), "Length");
				body = Expression.GreaterThanOrEqual(lenProp, constant);
			}
			else
			{
				body = Expression.GreaterThanOrEqual(Expression.Convert(left, typeof(int)), constant);
			}

			Expression<Func<object, bool>> lambda =
			Expression.Lambda<Func<object, bool>>(body, left);

			_compiledDelegates.Add(new CustomeStructure(lambda.Compile(), _propName, RulesName.HasMinLength, minLength));

			return this;
		}

		public RulesBuilder<T> MustMatch(Regex regex)
		{
			if (_propType != typeof(string))
				return this;
			var input = Expression.Parameter(typeof(string));
			var callExpression = Expression.Call(
				Expression.Constant(regex),
				typeof(Regex).GetMethod("IsMatch", [typeof(string)]),
				input
			);
			var lambdaExpression = Expression.Lambda<Func<string, bool>>(callExpression, input);
			_compiledDelegates.Add(new CustomeStructure(lambdaExpression.Compile(), _propName, RulesName.MustMatch, regex.ToString()));
			return this;
		}
	}
}
