# Configuration Transformation Classes
## ConfigurationExtensions
Is a set of classes allowing to map configuration secitons or values over ConfigurationManager.

### Usage 
Add using directive and go ahead!
```
using Zagidziran.ConfigurationExtensions;

var confgiurationManger = new ConfigurationManager();
confgiurationManger.AddYamlFile(@".\sample.yaml");

confgiurationManger.Transform();
```
When sample.yaml may look like this:
```
someData:
  someValue: 1

mappedData: $(someData)
mappedValue: $(someData.value1)
constructedValue: Text$(someData.value1)
```


Additionally you can try *WithTimeout* extension method to specify wait timeout or *cancellationToken* to pass cancellation token.

### NuGet

Nuget package to use is Zagidziran.ConfigurationExtensions. Awailable on nuget.org
[Zagidziran.ConfigurationExtensions](https://www.nuget.org/packages/Zagidziran.ConfigurationExtensions) ![Zagidziran.ConfigurationExtensions](https://img.shields.io/nuget/v/zagidziran.ConfigurationExtensions.svg)

### A Few Explanation Words

When you call the Transform() method the code traverses through configuration and replaces entries in the round brackets with dollar sign
like this $(someReference). The text inside the brackets should point to existing configuration key or you get ReferencedKeyNotFoundException.
The only values are transformed, not keys.