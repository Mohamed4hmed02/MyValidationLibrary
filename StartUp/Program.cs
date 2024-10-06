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
			validator.RulesFor(t => t.Name).MustEqual("");
			validator.RulesFor(t => t.Size).MustEqual(55);

			var result = validator.Validate();
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
