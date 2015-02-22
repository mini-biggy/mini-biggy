# mini-biggy
##*Biggy got too big, so I created Mini-Biggy.*

[Biggy](https://github.com/robconery/biggy) is a Very Fast Document/Relational Query Tool with Full LINQ Compliance. 

The original project became too big, many refactorings were happening all the time and I could never find the nuget packages to work with. So I created Mini-Biggy.

## Nuget
Install-Package mini-biggy

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
Every time you create a list of type Tweet, it will load all saved tweets:
```
	var list = PersistentList.Create<Tweet>();
    var count = list.Count(); //equals 1
```

###Turning on AutoSave
```
	var t = new Tweet();
    var list = PersistentList.Create<Tweet>();
    list.AutoSave = true;
    list.Add(t);
    //it's already persisted, no need to call Save()
```
Just note that it can take some time to save, specially on loops (use AddRange when on loops).

###Is mini-biggy for you?
Mini-biggy is an excellent choice for storing your persistence data if:

 - you want a freaking fast and simple way to store your objects.
 - you have less than a couple hundred thousand objects (beware for mobile devices)
 - you want full Linq support 
 - your objects are json serialized and you will take care of them when changing your model

###Which platforms does it support?
 - .net 4.5 using filesystem
 - Universal Apps (Windows Phone and Windows Store Apps) using the Storage.



All the credits for the original idea go to [Rob Conery](https://github.com/robconery).
