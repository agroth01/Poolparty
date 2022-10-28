# Poolparty
This is my simple solution for object pooling in Unity. It was created from the realization that I keep making the same systems over and over again.

# Usage
You will have to create a GameObject in your scene where you add the `PoolManager.cs` class.

Create a new pool
```csharp
PoolManager.Instance.CreatePool(prefab, poolsize);
```

Spawn an item from the pool
```csharp
PoolManager.Instance.SpawnObject(prefab);
// Or
PoolManager.Instance.SpawnObject(prefab, position, rotation);
```

Remove an item from the pool
```csharp
PoolManager.Instance.RemoveObject(gameObject);
```
