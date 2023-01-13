# Section 3.3 Special Instructions

In section 3.3 we explore caching using Redis in ASP.NET, this section is completely optional as ASP.NET is now (somewhat) legacy having been largely supplanted by ASP.NET Core. None of the materials here will be included in the exam required to pass this course so if this is of no interest to you, feel free to skip. That said it should still be instructive for those of you still interested in ASP.NET to learn how to use Redis in this context, so please bear in mind that there are some additional requirements for this section.

## Additional Prerequisites

* Because this uses ASP.NET and the .NET Framework, this needs to run on a Windows (preferably Windows 10+) machine or VM.
* You must have the [.NET Framework 4.8.1 SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) installed
* This example makes use of MS SQL Server Express's [Local DB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16) it should ship along side IIS Express.
* It's highly recommended that you use either Visual Studio or Rider for this section.