using System;
namespace SharedLibraries
{
	public interface IPostgreSQLService
	{
        void AddMessage(string text, DateTime createdOn);
	}
}

