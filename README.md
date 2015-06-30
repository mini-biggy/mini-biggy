<p align="center">
  <img src="https://github.com/mini-biggy/mini-biggy/blob/master/Assets/mini_biggy.png" width="350px" alt="mini-biggy" />
</p>

##*Biggy got too big, so I created Mini-Biggy.*

[Biggy](https://github.com/robconery/biggy) is a Very Fast Document/Relational Query Tool with Full LINQ Compliance. 

The original project became too big, many refactorings were happening all the time and I could never find the nuget packages to work with. So I created Mini-Biggy.

## Nuget
**Install-Package mini-biggy**

##Quick Start

###Saving your objects
Let's create a class called Tweet and save a tweet object.
This code will create a file called tweet.js with our tweet serialized. Call Save() or SaveAsync() to persist the list.
```
    var t = new Tweet();
    var list = PersistentList.Create<Tweet>();
    list.Add(t);
    await list.SaveAsync();
```

###Loading them later
Every time you create a list of some type, it will load all saved objects of that type:
```
    var list = PersistentList.Create<Tweet>();
    var count = list.Count(); //equals 1
```

### Linq Goodness

Remember, your list is entirely in memory, so any linq operator will work just fine:

```
 biggyList.Where(x => x.Username == "admin" && x.Message.ToLower().Contains("urgent"))
                .OrderBy(x => x.DateTime)
                .FirstOrDefault();
```

##Saving Modes

You can follow 3 strategies to save your list using biggy:

###Manual Save

The list will be saved only when Save() or SaveAsync() is called. This is the default behavior.

```
    var t = new Tweet();
    var list = PersistentList.Create<Tweet>();
    list.Add(t);
    list.Save();
```

###Automatic Save

The list will be saved on every change: adding, deleting or updating an item saves the list. You still can call Save and SaveAsync if you want.

```
    var t = new Tweet();
    var list = PersistentList.Create<Tweet>(new SaveOnEveryChange());
    list.Add(t); //list saved
```

Just note that it can take some time to save, specially on loops (use AddRange when on loops).

###Background Save

This is really useful if your list changes a lot, specilly by multithread applications (web). The list will be saved every X seconds, but only if it was modified. You still can call Save and SaveAsync if you want.

```
    var t = new Tweet();
    var list = PersistentList.Create<Tweet>(new BackgroundSave(TimeSpan.FromSeconds(3)));
    list.Add(t); //it will be saved on next loop, in a background thread
```

##Is mini-biggy for you?
Mini-biggy is an excellent choice for storing your persistence data if:

 - you want a freaking fast and simple way to store your objects.
 - you have less than a couple hundred thousand objects (beware for mobile devices)
 - you want full Linq support 
 - your objects are json serialized and you will take care of them when changing your model

##Which platforms does it support?
 - .net 4.5 using filesystem
 - Universal Apps (Windows Phone and Windows Store Apps) using the Storage.



All the credits for the original idea go to [Rob Conery](https://github.com/robconery).
