# Basics of Native AOT in dotnet

Simple example is used to show native aot in .net. 
The business logic will try to count primes based on provided arguments and show stats.

More information about the [dotnet native tool chain](https://devblogs.microsoft.com/dotnet/the-net-native-tool-chain/)

```
> dotnet run 750000
Found 11381621 in 4642ms
Memory used: 435688 bytes
Press any key to exit
```

```
> dotnet run 750000 -c Release
Found 11381621 in 4597ms
Memory used: 435608 bytes
Press any key to exit
```

Publish for standard CLR and JIT package. This requires dotnet installed on distributed system.
```
> dotnet publish -c Release   
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\AotCountPrimes.dll
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\publish\
```

Publish the self contained application with all dependencies in folder (folder contains CLR).
```
> dotnet publish -r win-x64 --self-contained -c Release
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  Restored C:\GIT\dotnet\AotCountPrimes\AotCountPrimes.csproj (in 19.78 sec).
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\AotCountPrimes.dll
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\publish\
```

Publish the self contained application into single file.
```
> dotnet publish -r win-x64 --self-contained -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  Restored C:\GIT\dotnet\AotCountPrimes\AotCountPrimes.csproj (in 8.19 sec).
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\AotCountPrimes.dll
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\publish\
```

Publish the application into native code. This will have smaller footprint in memory and on disk.
```
> dotnet publish -r win-x64 -c Release -p:PublishAot=true
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  Restored C:\GIT\dotnet\AotCountPrimes\AotCountPrimes.csproj (in 28.14 sec).
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\AotCountPrimes.dll
  Generating native code
     Creating library bin\Release\net7.0\win-x64\native\AotCountPrimes.lib and object bin\Release\net7.0\win-x64\native\AotCountPrimes.exp
  AotCountPrimes -> C:\GIT\dotnet\AotCountPrimes\bin\Release\net7.0\win-x64\publish\
```
Or you can specify in your ```*.csproj``` file:
```
<PropertyGroup>
    <PublishAot>true</PublishAot>
</PropertyGroup>
```
If you have the property you can just run:
```
> dotnet run 750000 -c Release
Found 11381621 in 4534ms
Memory used: 71448 bytes
Press any key to exit
```
This uses significantly less memory. Note this will not work with: ```dotnet run 750000 -c Release -p:PublishAot=true```

## Limitations of Native AOT deployment

Native AOT applications come with a few [current limitations](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) and compatibility issues. The key limitations include:

- No dynamic loading (for example, Assembly.LoadFile)
- No runtime code generation (for example, System.Reflection.Emit)
- No C++/CLI
- No built-in COM (only applies to Windows)
- Requires trimming, which has limitations
- Implies compilation into a single file, which has known incompatibilities
- Apps include required runtime libraries (just like self-contained apps, increasing their size, as compared to framework-dependent apps)

## Native library in dotnet

Here is an example of [native library in dotnet](https://github.com/dotnet/samples/tree/main/core/nativeaot/NativeLibrary)