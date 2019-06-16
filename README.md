# Locks Manager
[![Build status](https://ci.appveyor.com/api/projects/status/47cyyra364lklsyk/branch/master?svg=true)](https://ci.appveyor.com/project/AhmadRajabi/locksmanager/branch/master)
[![NuGet](https://img.shields.io/nuget/vpre/mediatr.svg)](https://www.nuget.org/packages/LocksManager)


```c#
static LocksManager locksManager = new LocksManager();
```
Sync:
```c#
using (locksManager.Lock("your key"))
{

}
```

Async:
```c#
using (await locksManager.LockAsync("your key"))
{

}
```
Set lock timeout:
```c#
using (await locksManager.LockAsync("your key", TimeSpan.FromSeconds(2)))
{

}
```
Get lock handler:
```c#
using (var handler = await locksManager.LockAsync("your key", TimeSpan.FromSeconds(2)))
{
    if (handler != null)
    {
      //lock is taken
    }
}
```
