A very minimal `.csproj` file looks like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
  </ItemGroup>
</Project>
```

- Two core tags
	- `<PropertyGroup>` - Property Group
		- This defines how to make msbuild build this project.
	 - `<ItemGroup>` - Item Group
		 - Defines "what materials to use".