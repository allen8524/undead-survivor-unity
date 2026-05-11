using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        if (prefabs == null)
        {
            pools = new List<GameObject>[0];
            return;
        }

        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index)
    {
        if (prefabs == null || index < 0 || index >= prefabs.Length || prefabs[index] == null)
            return null;

        if (pools == null || index >= pools.Length)
            return null;

        if (pools[index] == null)
            pools[index] = new List<GameObject>();

        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (item != null && !item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
