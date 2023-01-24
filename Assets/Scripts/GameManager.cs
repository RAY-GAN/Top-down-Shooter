using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private float disbetweenPlayer;

    [SerializeField]
    private float disbetweenEnemy;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private float Spawnrate;

    public List<GameObject> Enemies = new List<GameObject>();

    private GameObject RemovingEnemy;

    public int Enemykilled;

    public Text Enemykilledtext;

    public Text Enemykilledscore;

    public static GameManager Instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 3, Spawnrate);

    }

    // Update is called once per frame
    void Update()
    {
        Enemykilledtext.text = Enemykilled.ToString();
    }

    public void SpawnEnemy()
    {
        

        bool bisSpawned = false;
        while (!bisSpawned)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-7, 7), Random.Range(-3, 3), 0f);

            if((Player.position - SpawnPos).magnitude < disbetweenPlayer)
            {
                continue;
            }
            else
            {
                bool bcanSpawn = true;

                if (Enemies.Count == 0)
                {
                    GameObject newEnemy = Instantiate(EnemyPrefab, SpawnPos, Quaternion.identity);
                    Enemies.Add(newEnemy);
                    bisSpawned = true;
                }

                else
                {
                    foreach (GameObject Enemy in Enemies)
                    {
                        if ((Enemy.transform.position - SpawnPos).magnitude < disbetweenEnemy)
                        {
                            bcanSpawn = false;
                            continue;

                        }
                        else
                        {
                            bcanSpawn = true && bcanSpawn;
                        }
                    }

                    if (bcanSpawn)
                    {
                        GameObject newEnemy = Instantiate(EnemyPrefab, SpawnPos, Quaternion.identity);
                        Enemies.Add(newEnemy);
                        bisSpawned = true;
                    }

                }
               
            }
        }
    }


    public void DeleteEnemy(GameObject deadEnemy)
    {
        
        foreach (GameObject Enemy in Enemies)
        {
            if (GameObject.ReferenceEquals(Enemy,deadEnemy))
            {
                RemovingEnemy = Enemy;
            }
        }

        Enemies.Remove(RemovingEnemy);
    }

    public void GameOver()
    {
        Enemykilledscore.gameObject.SetActive(true);
        Enemykilledscore.text = "Game Over, You have killed  " + Enemykilled;
        Time.timeScale = 0;
    }

}
