namespace MyValidationLibraryV2.Results
{
	public class ValidationResults
	{
		public IEnumerable<FailureResult> Errors { get; }
		public bool IsValid { get; }
		public ValidationResults(IEnumerable<FailureResult> failures)
		{
			Errors = failures;
			IsValid = !Errors.Any();
		}
	}
}
