brightness
==========
This library compares two CSV files and finds the rows that have changed.

## Installation
You can use brightness in two ways:

### Via NuGet Package
```
Install-Package brightness
```

### Manually (latest version)
* Clone this repository
* Build Brightness.csproj
* Reference Brightness.dll into your project

## Usage
```csharp
using Brightness;
using Brightness.Diff;

DeltaEngine.Diff("file1.txt", "file2.txt", (YourModel model) => model.Id);
```

Please note that your files must have an identifier: brightness uses it to uniquely identify rows, so make sure you have one before continuing.

The identifier could also be composed:
```csharp
DeltaEngine.Diff("file1.txt", "file2.txt", (YourModel model) => new { model.A, model.B, model.C });
```

## Contribute
Please report any issue you find or submit a PR if you have any improvements.
