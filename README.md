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

Now you are ready to use changed rows as you want. `DeltaEngine.Diff(...)` will return an `IEnumerable<RowDiff<TIdentity, TModel>>``, so you could for example update your database using your ORM:
```csharp
var differences = DeltaEngine.Diff(...);

foreach (var difference in differences)
{
  if (difference.Status == RowStatus.Added)
    myContext.Add(difference.Row);
  else if (difference.Status == RowStatus.Deleted)
    myContext.Remove(difference.Id);
  else if (difference.Status == RowStatus.Updated)
    myContext.Update(difference.Id, difference.Row);
}
```

## Contribute
Please report any issue you find or submit a PR if you have any improvements.
