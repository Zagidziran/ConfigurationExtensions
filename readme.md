# Configuration Transformation Classes
## ConfigurationExtensions
Is a set of classes allowing to map and transform configuration secitons or values over ConfigurationManager.

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
mappedValue: $(someData:value1)
constructedValue: Text$(someData:value1)
```

### Advanced Usage 

In advanced scenario you may use C# code to define your transformation. 

```
someData:
  someValue: 1

multioledVale: |-
  ${
    var someValue = int.Parse(Configuration["someData:someValue"]);
    return someValue * 17;
  }
```
### NuGet

Nuget package to use is Zagidziran.ConfigurationExtensions. Awailable on nuget.org
[Zagidziran.ConfigurationExtensions](https://www.nuget.org/packages/Zagidziran.ConfigurationExtensions) ![Zagidziran.ConfigurationExtensions](https://img.shields.io/nuget/v/zagidziran.ConfigurationExtensions.svg)

### A Few Explanation Words

When you call the Transform() method the code traverses through configuration and replaces entries in the brackets or parentheses with leading dollar sign.
Parentheses allows to map a configuration value to another one. When bracers allows to write a C# code returning a dictionary string or null.
The text inside the parentheses should point to existing configuration key or you get ReferencedKeyNotFoundException. The delimiter is semicolon.
The code may return doctionary. Thereat all dictionary content will be added to configuration with prefix pointing to the code.
The only values are transformed, not keys.