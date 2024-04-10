using System;
namespace PublishingService
{
	public interface IPostgreSQLService
	{
		void AddMessage(string text, DateTime createdOn);
	}
}

