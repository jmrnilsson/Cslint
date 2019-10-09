using System.IO;

namespace Cslint
{
	public interface IChecker
	{
		event ApplicationInformation Error;
		event ApplicationInformation Information;
		event ApplicationInformation Verbose;

		void Check(FileInfo path);
	}
}
