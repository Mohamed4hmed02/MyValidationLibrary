using MyValidationLibraryV2.Results;
using MyValidationLibraryV2.Structures;
using System.Linq.Expressions;

namespace MyValidationLibraryV2
{
	/// <summary>
	/// Get The Rules Builder And Validate An Object Of Type <typeparamref name="T"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ValidatorV2<T>
	{
		private readonly IList<CustomeStructure> structure = [];

		public RulesBuilderV2<Tprop> GetPropertyRuleBuilder<Tprop>(Expression<Func<T, Tprop>> propertyRuleBuilder)
		{
			var property = (propertyRuleBuilder.Body as MemberExpression);
			return new RulesBuilderV2<Tprop>(structure, property.Member.Name, property.Type);
		}

		/// <summary>
		/// Validate an instance of <typeparamref name="T"/> based on the rules that have been set 
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public ValidationResults Validate(T instance)
		{
			var type = typeof(T);
			var failes = new List<FailureResult>();
			foreach (var stra in structure)
			{
				var propValue = type.GetProperty(stra.PropName).GetValue(instance);
				if (!Convert.ToBoolean(stra.Delegate.DynamicInvoke(propValue)))
				{
					if (stra.RuleName == RulesName.HasMaxLength)
						failes.Add(new FailureResult { Message = $"Property ({stra.PropName}) With Value ({propValue}) Has Length Above The Limit Length Of ({stra.InputValue})" });
					else if (stra.RuleName == RulesName.HasMinLength)
						failes.Add(new FailureResult { Message = $"Property ({stra.PropName}) With Value ({propValue}) Has Length Below The Limit Length Of ({stra.InputValue})" });
					else if (stra.RuleName == RulesName.MustMatch)
						failes.Add(new FailureResult { Message = $"Property ({stra.PropName}) With Value ({propValue}) Not Matched The Given Expression ({stra.InputValue})" });

				}
			}
			return new ValidationResults(failes);
		}
	}
}
