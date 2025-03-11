using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerController playerController;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform enemyGroup;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform bulletGroup;
    [SerializeField] private List<GameObject> bulletPrefabs;
    [SerializeField] private Collider enemySpawnGround;
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private Dictionary<List<GameObject>, Transform> poolGroupDictionary;
    private Bounds spawnBounds;
    private List<GameObject> enemyPool;
    private List<GameObject> bulletPool;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        spawnBounds = enemySpawnGround.GetComponent<Collider>().bounds;
        poolGroupDictionary = new Dictionary<List<GameObject>, Transform>();
        InvokeRepeating("SpawnEnemy", 1f, enemySpawnRate);

        enemyPool = initPool(enemyPrefabs[0], enemyGroup);
        bulletPool = initPool(bulletPrefabs[0], bulletGroup);
    }

    // Update is called once per frame
    void Update()
    {
        speedText.SetText("Speed : " + playerController.getSpeed() + "km/h");
        scoreText.SetText("Score : " + PlayerManager.Instance.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Count);
        Vector3 spawnLocation = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x), spawnBounds.max.y + enemyPrefabs[enemyIndex].GetComponent<BoxCollider>().size.y / 2 + 0.01f, Random.Range(spawnBounds.min.z, spawnBounds.max.z));
        //Instantiate(enemyPrefabs[enemyIndex], spawnLocation, enemyPrefabs[enemyIndex].transform.rotation);
        GameObject enemy = getGameObjectInPool("Enemy");
        enemy.transform.position = spawnLocation;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);
    }

    List<GameObject> initPool(GameObject prefab, Transform parent)
    {
        List<GameObject> pool = new List<GameObject>(20);
        poolGroupDictionary.Add(pool, parent);
        for (int i = 0; i < pool.Capacity; i++)
        {
            GameObject gameObject = Instantiate(prefab, parent);
            gameObject.SetActive(false);
            pool.Add(gameObject);
        }
        return pool;
    }

    private GameObject getAvailableObjectInPool(List<GameObject> pool)
    {
        foreach (GameObject item in pool)
        {
            if (item.gameObject.activeSelf == false)
            {
                //item.gameObject.SetActive(true);
                return item;
            }
        }

        GameObject gameObject = Instantiate(pool[0], poolGroupDictionary[pool]);
        gameObject.SetActive(false);
        pool.Add(gameObject);
        return gameObject;
    }

    //private GameObject getAvailableObjectInPool(List<GameObject> pool, Vector3 position, Quaternion rotation)
    //{
    //    foreach (GameObject item in pool)
    //    {
    //        if (item.gameObject.activeInHierarchy == false)
    //        {
    //            item.transform.position = position;
    //            item.transform.rotation = rotation;
    //            item.gameObject.SetActive(true);
    //            return item;
    //        }
    //    }
    //    return null;
    //}

    //public GameObject getGameObjectInPool(string prefabName, Vector3 position, Quaternion rotation)
    //{
    //    if (prefabName.Equals("Enemy"))
    //    {
    //        return getAvailableObjectInPool(enemyPool, position, rotation);
    //    }
    //    else if (prefabName.Equals("Bullet"))
    //    {
    //        return getAvailableObjectInPool(bulletPool, position, rotation);
    //    }
    //    return null;
    //}

    public GameObject getGameObjectInPool(string prefabName)
    {
        if(prefabName.Equals("Enemy"))
        {
            return getAvailableObjectInPool(enemyPool);
        }
        else if(prefabName.Equals("Bullet"))
        {
            return getAvailableObjectInPool(bulletPool);
        }
        return null;
    }

    private void setAvailableObjectInPool(List<GameObject> pool, GameObject prefab)
    {
        prefab.SetActive(false);
    }

    public void returnGameObjectInPool(string prefabName, GameObject prefab)
    {
        if (prefabName.Equals("Enemy"))
        {
            setAvailableObjectInPool(enemyPool, prefab);
        }
        else if (prefabName.Equals("Bullet"))
        {
            setAvailableObjectInPool(bulletPool, prefab);
        }
    }

    public void gameOverSequence()
    {
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}