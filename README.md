# Lock Manager
[![Build status](https://ci.appveyor.com/api/projects/status/fqy7hvpjy9jl1g83?svg=true)](https://ci.appveyor.com/project/AhmadRajabi/lockmanager)

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
