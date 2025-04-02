using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<PoolType, Queue<Cell>> poolDictionary = new Dictionary<PoolType, Queue<Cell>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePool(PoolType poolType, Cell prefab, int count)
    {
        if (!poolDictionary.ContainsKey(poolType))
        {
            poolDictionary[poolType] = new Queue<Cell>();
            for (int i = 0; i < count; i++)
            {
                Cell newCell = Instantiate(prefab);
                newCell.gameObject.SetActive(false);
                poolDictionary[poolType].Enqueue(newCell);
            }
        }
    }

    public Cell GetFromPool(PoolType poolType)
    {
        if (poolDictionary.ContainsKey(poolType) && poolDictionary[poolType].Count > 0)
        {
            Cell obj = poolDictionary[poolType].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnToPool(PoolType poolType, Cell obj)
    {
        obj.gameObject.SetActive(false);
        poolDictionary[poolType].Enqueue(obj);
    }
}
