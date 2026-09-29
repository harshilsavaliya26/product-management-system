POJ3 RUN FIX

1. Open poj3.sln if present, otherwise open poj3.csproj in Visual Studio 2026 / a Visual Studio version with .NET 10 SDK support.
2. Set poj3 as Startup Project.
3. Build > Clean Solution.
4. Build > Rebuild Solution. The file bin\Debug\net10.0\poj3.exe must be generated.
5. Select the `poj3` launch profile, NOT a stale `https` profile.
6. Press Ctrl+F5 or F5.

If Visual Studio still says it cannot find bin\Debug\net10.0\poj3.exe, the build has failed. Open View > Output, select `Build`, and fix the FIRST C# compile error shown there. Do not manually create the exe.

The project explicitly uses OutputType=Exe and AssemblyName=poj3, and the launch profile is named `poj3` to match the project.
