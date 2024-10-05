using MyValidationLibrary;

namespace StartUp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var t = new Test
			{
				Name = "Bhmed",
				Size = 55,
				Password = "null"
			};

			var validator = new Validator<Test>(t);
			var result = validator.RulesFor(t => t.Name).MustMatch(@"^[A,Z]").Validate();
			Console.WriteLine(result.IsValid);
			foreach (var failure in result.Errors)
			{
				Console.WriteLine(failure.Message);
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
