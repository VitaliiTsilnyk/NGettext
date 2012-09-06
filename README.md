NGettext
========

Copyright (C) 2012 Neris Ereptoris <http://neris.ws/>.


Just another one GNU/Gettext implementation for C#/.NET.
Requires Microsoft .NET Framework 2.0 or higher or Mono.

This implementation loads translations directly from gettext *.mo files (no need to compile a satellite assembly) and can handle multiple translation domains and multiple locales in one application instance.

This implementation currently not supports *.mo file headers (stored in file plural formulas, encoding, etc.).
It uses precompiled plural formulas and supports custom plural formulas passed through API.
It only supports Little-Endian UTF-8 MO files.



Installation and usage
----------------------

Add reference to `NGettext.dll` to your project.


Now you can use NGettext in your code:
```csharp
using NGettext;
```

```csharp

// This will load translations from "./locale/<CurrentUICulture>/LC_MESSAGES/Example.mo"
ICatalog catalog = new Catalog("Example", "./locale");

```

or

```csharp

// This will load translations from "./locale/ru_RU/LC_MESSAGES/Example.mo"
ICatalog catalog = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("ru-RU"));

```



```csharp

Console.WriteLine(catalog.GetString("test")); // will translate "test" using loaded translations
Console.WriteLine(catalog._("test")); // shorter version
Console.WriteLine(catalog._("Hello, {0}!", "World")); // String.Format support

```



### Plural forms

```csharp


catalog.GetPluralString("You have {0} apple.", "You have {0} apples.", count);
// Returns (for en_US locale):
//     "You have {0} apple." for count = 1
//     "You have {0} apples." otherwise


catalog.GetPluralString("You have {0} apple.", "You have {0} apples.", 5, 5);
// Returns translated plural massage: "You have 5 apples." (for en_US locale)


```



### Contexts

```csharp
catalog.GetParticularString("Menu|File|", "Open"); // will translate message "Open" using context "Menu|File|"
catalog.GetParticularString("Menu|Project|", "Open"); // will translate message "Open" using context "Menu|Project|"
```



### Multiple locales and domains in one application instance

```csharp

// "./locale/en_US/LC_MESSAGES/Example.mo"
ICatalog example_en = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("en-US"));


// "./locale/fr/LC_MESSAGES/Example.mo"
ICatalog example_fr = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("fr"));


// "./locale/<CurrentUICulture>/LC_MESSAGES/AnotherDomainName.mo"
ICatalog anotherDomain = new Catalog("AnotherDomainName", "./locale");

```



### Direct MO file loading

```csharp

Stream moFileStream = File.OpenRead("path/to/domain.mo");
ICatalog catalog = new Catalog(moFileStream);

```



### Custom plural formulas

```csharp

catalog.PluralForm.SetCustomFormula(cultureInfo, n => ( n == 1 ? 0 : 1 ));

```