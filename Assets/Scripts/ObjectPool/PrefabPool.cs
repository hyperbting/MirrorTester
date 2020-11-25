using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PrefabPool
{
    public int startSize = 5;
    public int maxSize = 100;
    Func<object> InstantiateMethod;

    public Queue<object> pool;
    public int currentCount;

    public void InitializePool(Func<object> instantiateMethod)
    {
        InstantiateMethod = instantiateMethod;

        pool = new Queue<object>();
        for (int i = 0; i < startSize; i++)
        {
            object next = CreateNew();

            pool.Enqueue(next);
        }
    }

    object CreateNew()
    {
        if (currentCount > maxSize)
        {
            return null;
        }

        // use this object as parent so that objects dont crowd hierarchy
        object next = InstantiateMethod?.Invoke();
        currentCount++;
        return next;
    }

    /// <summary>
    /// Used to take Object from Pool.
    /// <para>Should be used on server to get the next Object</para>
    /// <para>Used on client by ClientScene to spawn objects</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public virtual object GetFromPool()
    {
        object next = pool.Count > 0
            ? pool.Dequeue() // take from pool
            : CreateNew(); // create new because pool is empty

        // CreateNew might return null if max size is reached
        if (next == null) { return null; }

        return next;
    }

    /// <summary>
    /// Used to put object back into pool so they can b
    /// <para>Should be used on server after unspawning an object</para>
    /// <para>Used on client by ClientScene to unspawn objects</para>
    /// </summary>
    /// <param name="spawned"></param>
    public virtual void PutBackInPool(object spawned)
    {
        // add back to pool
        pool.Enqueue(spawned);
    }
}
