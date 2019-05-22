# Locks Manager
[![Build status](https://ci.appveyor.com/api/projects/status/47cyyra364lklsyk/branch/master?svg=true)](https://ci.appveyor.com/project/AhmadRajabi/locksmanager/branch/master)

```c#
static LockManager lockManager = new LockManager();
```
Sync:
```c#
using (lockManager.Lock("your key"))
{

}
```

Async:
```c#
using (await lockManager.LockAsync("your key"))
{

}
```
Set lock timeout:
```c#
using (await lockManager.LockAsync("your key", TimeSpan.FromSeconds(2)))
{

}
```
Get lock handler:
```c#
using (var handler = await lockManager.LockAsync("your key", TimeSpan.FromSeconds(2)))
{
    if (handler != null)
    {
      //lock is taken
    }
}
```
