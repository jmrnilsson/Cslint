namespace Cslint
{
	public abstract class CheckerBase
	{
		public event ApplicationInformation Information;
		public event ApplicationInformation Error;
		public event ApplicationInformation Verbose;

		protected void OnInformation(string information)
		{
			Information?.Invoke(information);
		}

		protected void OnError(string information)
		{
			Error?.Invoke(information);
		}

		protected void OnVerbose(string information)
		{
			Verbose?.Invoke(information);
		}
	}
}

