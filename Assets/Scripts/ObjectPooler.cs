using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //Pool describe
    [System.Serializable]
    public class Pool
    {
        public string name;
        public List<GameObject> prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //Pool parent
    public GameObject poolObject;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Event describe
        EventManager.Instance.onCreateCubes += CreatePassiveCube;

        //Create passive cubes with settings in the editor
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                for (int a = 0; a < pool.prefab.Count; a++)
                {
                    GameObject obj = Instantiate(pool.prefab[a],poolObject.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);

                }
            }
            poolDictionary.Add(pool.name, objectPool);
        }

    }

    //Create cube fonc for event
    private void CreatePassiveCube()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                for (int a = 0; a < pool.prefab.Count; a++)
                {
                    GameObject obj = Instantiate(pool.prefab[a], poolObject.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);

                }
            }
            
            poolDictionary.Add(pool.name, objectPool);
        }
    }
    //Can make active cubes where are you want
    public GameObject SpawnForGameObject(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.parent = parent;
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[name].Enqueue(objectToSpawn);
        return objectToSpawn;
    }


}