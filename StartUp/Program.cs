using MyValidationLibraryV2;
using System.Text.RegularExpressions;

namespace StartUp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var t1 = new Test
			{
				Name = "Ahmed",
				Size = 55,
				Password = "null"
			};

			var t2 = new Test
			{
				Name = "Ahmed",
				Size = 55,
				Password = "3Password"
			};

			var validator = new ValidatorV2<Test>();
			validator.GetPropertyRuleBuilder(t => t.Size).HasMaxLength(5);
			validator.GetPropertyRuleBuilder(t => t.Password).HasMinLength(1);
			validator.GetPropertyRuleBuilder(t => t.Password).MustMatch(new Regex("^3"));
			var res = validator.Validate(t1);
			foreach (var fail in res.Errors)
			{
				Console.WriteLine(fail.Message);
			}
		}
	}
	class Test
	{
		public string? Name { get; set; }
		public string? Password { get; set; }
		public int Size { get; set; }
	}
}
