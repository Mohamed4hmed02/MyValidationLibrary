namespace MyValidationLibrary.Results
{
	public class ValidationResult
	{
		public IEnumerable<FailureResult> Errors { get; }
		public bool IsValid { get; }
		public ValidationResult(IEnumerable<FailureResult> failures)
		{
			Errors = failures;
			IsValid = !Errors.Any();
		}
	}
}
