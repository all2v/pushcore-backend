SolutionName = Astrum
ModuleName = Project

add: create attach 

create:
	dotnet new classlib -o .\src\Modules\${ModuleName}\Domain -n ${SolutionName}.${ModuleName}.Domain	
	dotnet new classlib -o .\src\Modules\${ModuleName}\DomainServices -n ${SolutionName}.${ModuleName}.DomainServices	
	dotnet new classlib -o .\src\Modules\${ModuleName}\Application -n ${SolutionName}.${ModuleName}.Application	
	dotnet new classlib -o .\src\Modules\${ModuleName}\Infrastructure -n ${SolutionName}.${ModuleName}.Infrastructure	
	dotnet new classlib -o .\src\Modules\${ModuleName}\Backoffice -n ${SolutionName}.${ModuleName}.Backoffice	
	dotnet new classlib -o .\src\Modules\${ModuleName}\Persistence -n ${SolutionName}.${ModuleName}.Persistence	
	dotnet new classlib -o .\src\Modules\${ModuleName}\Startup -n ${SolutionName}.${ModuleName}.Startup	

attach:
	dotnet sln add .\src\Modules\${ModuleName}\Domain\${SolutionName}.${ModuleName}.Domain.csproj
	dotnet sln add .\src\Modules\${ModuleName}\DomainServices\${SolutionName}.${ModuleName}.DomainServices.csproj
	dotnet sln add .\src\Modules\${ModuleName}\Application\${SolutionName}.${ModuleName}.Application.csproj
	dotnet sln add .\src\Modules\${ModuleName}\Infrastructure\${SolutionName}.${ModuleName}.Infrastructure.csproj
	dotnet sln add .\src\Modules\${ModuleName}\Backoffice\${SolutionName}.${ModuleName}.Backoffice.csproj
	dotnet sln add .\src\Modules\${ModuleName}\Persistence\${SolutionName}.${ModuleName}.Persistence.csproj
	dotnet sln add .\src\Modules\${ModuleName}\Startup\${SolutionName}.${ModuleName}.Startup.csproj

delete:
	rm -f .\src\Modules\${ModuleName}\Domain\Class1.cs
	rm -f .\src\Modules\${ModuleName}\DomainServices\Class1.cs
	rm -f .\src\Modules\${ModuleName}\Application\Class1.cs
	rm -f .\src\Modules\${ModuleName}\Infrastructure\Class1.cs
	rm -f .\src\Modules\${ModuleName}\Backoffice\Class1.cs
	rm -f .\src\Modules\${ModuleName}\Persistence\Class1.cs
	rm -f .\src\Modules\${ModuleName}\Startup\Class1.cs