using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParticleInfo
{
    public Vector2 pos;
    public Quaternion rot;
    public Vector2 dir;
    public float force;
    public string tag;

    //for 3d sounds
    public string sound_name;
    public bool loop;
}

public class ObjectPooler : MonoBehaviour
{
    // Class for a pool type
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // List of pools
    public List<Pool> pools;
   
    // Dictionary of pools
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool<Type>(ParticleInfo particleInfo) where Type : IPooledObject
    {
        if(!poolDictionary.ContainsKey(particleInfo.tag))
        {
            Debug.LogWarning("Pool with tag" + particleInfo.tag + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[particleInfo.tag].Dequeue();

        objectToSpawn.SetActive(true);

        //objectToSpawn.transform.position = particleInfo.pos;
        //objectToSpawn.transform.rotation = particleInfo.rot;

        Type script = objectToSpawn.GetComponent<Type>();

        if(script != null)
        {
            script.OnObjectSpawn(particleInfo);
        }

        poolDictionary[particleInfo.tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
