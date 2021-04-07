using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    //적 스폰 포인트
    public Vector2[] spawnPos;

    //적 
    public GameObject enemyPrefab;
    public GameObject enemyPoolManager;
    public List<GameObject> enemyPool;

    //날라가는 물체 
    public GameObject bulletPrefab;
    public GameObject bulletPoolManager;
    public GameObject skillPoolManager;
    public List<GameObject> bulletsPool;
    public Transform bulletPos;

    //떨어진 돈
    public GameObject coinPrefab;
    public GameObject coinPoolManager;
    public List<GameObject> coinPool;

    //버린 아이템
    public GameObject dropItemPrefab;
    public GameObject dropItemPoolManager;
    public List<GameObject> dropItemPool;

    public PlayerController playerController;
    public EnemyController enemy;
    public SkillManager skillManager;
    public float spawnTime;
    public string damageType; //enemyController에서 type별로 스킬데미지 정해짐
    //enemy pool size
    int poolSize = 3;
    int coinPoolSize = 10;
    //enemy죽었을때 스폰되는 아이템
    int[] itemName;

    private void Awake()
    {
        enemy = FindObjectOfType<EnemyController>();

        enemyPool = new List<GameObject>();
        bulletsPool = new List<GameObject>();
        coinPool = new List<GameObject>();
        dropItemPool = new List<GameObject>();
        itemName = new int[6] { 10001, 10002, 10003, 10004, 20001, 20002 };

        spawnPos = new Vector2[transform.childCount];
        for (int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.GetChild(i).position;
        }
    }
    private void FixedUpdate()
    {
        //던전에 플레이어가 입장하면
        if (playerController.currentMapName == "dungeon")
        {
            //적 생성
            GetEnemy();
            spawnTime += Time.deltaTime;

            if (spawnTime > 3f)
            {
                GetEnemy();
                CreateEnemy();
                spawnTime = 0;
            }
        }
        if (playerController.currentMapName == "dungeonRoad")
        {
            DeleteEnemy();
            spawnTime = 0;
        }


    }
        
    public GameObject GetEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeSelf)
            {
                return enemy;
            }
        }

        GameObject obj = Instantiate(enemyPrefab, enemyPoolManager.transform);
        obj.name = string.Format($"Enemy_{enemyPool.Count}");
        obj.SetActive(false);
        enemyPool.Add(obj);
        return obj;
    }
    public void CreateEnemy()
    {
        int randomIndex = Random.Range(0, spawnPos.Length);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemyObj = enemyPool[i];
            if (enemyObj.activeSelf == true)
            {
                continue;
            }
            enemyObj.transform.position = new Vector3(spawnPos[randomIndex].x, spawnPos[randomIndex].y, 0f);
            enemyObj.SetActive(true);
            break;
        }
    }
    public GameObject GetBullet()
    {
        foreach (var bullet in bulletsPool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        GameObject obj = Instantiate(bulletPrefab, bulletPoolManager.transform);
        obj.name = string.Format($"Bullet_{bulletsPool.Count}");
        obj.SetActive(false);
        bulletsPool.Add(obj);
        return obj;
    }
    public GameObject GetCoin()
    {
        foreach (var coin in coinPool)
        {
            if (!coin.activeSelf)
            {
                return coin;
            }
        }

        GameObject obj = Instantiate(coinPrefab, coinPoolManager.transform);
        obj.name = string.Format($"Coin_{coinPool.Count}");
        obj.SetActive(false);
        coinPool.Add(obj);
        return obj;
    }
    public GameObject GetItem()
    {
        foreach (var dropItem in dropItemPool)
        {
            if (!dropItem.activeSelf)
            {
                return dropItem;
            }
        }
        GameObject obj = Instantiate(dropItemPrefab, dropItemPoolManager.transform);
        int random = Random.Range(0, 5);
        obj.name = itemName[random].ToString();
        Debug.Log(obj.name);
        obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"ItemIcon/{obj.name}");
        obj.SetActive(false);
        dropItemPool.Add(obj);
        return obj;
    }
    public void CreateItem(Vector3Int enemyPos)
    {
        GetItem();
        for (int i = 0; i < coinPoolSize; i++)
        {
            GameObject itemObj = dropItemPool[i];
            if (itemObj.activeSelf == true)
            {
                continue;
            }
            itemObj.transform.position = enemyPos;
            itemObj.SetActive(true);
            break;
        }
    }
    public void CreateBullet()
    {
        GetBullet();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletObj = bulletsPool[i];
            if (bulletObj.activeSelf == true)
            {
                continue;
            }
            bulletObj.transform.position = bulletPos.position;

            if (playerController.bulletDir.x == -1f)
            {
                bulletObj.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 180));
            }
            if (playerController.bulletDir.x == 1f)
            {
                bulletObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            bulletObj.SetActive(true);
            break;
        }
    }
    public void CreateCoin(Vector3Int enemyPos)
    {
        GetCoin();
        for (int i = 0; i < coinPoolSize; i++)
        {
            GameObject coinObj = coinPool[i];
            if (coinObj.activeSelf == true)
            {
                continue;
            }
            coinObj.transform.position = enemyPos;
            coinObj.SetActive(true);
            break;
        }
    }

    public void SkillBullet()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, skillPoolManager.transform);
        bulletObj.name = string.Format($"WindBullet");
        bulletObj.transform.position = bulletPos.position;

        if (playerController.bulletDir.x == -1f)
        {
            bulletObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        if (playerController.bulletDir.x == 1f)
        {
            bulletObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        bulletObj.SetActive(true);
        DeleteBullet();
    }

    public void DeleteEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (enemy.activeSelf)
            {
                enemy.SetActive(false);
            }
        }

    }
    public void DeleteBullet()
    {
        if (skillPoolManager.transform.GetChild(0).gameObject.activeSelf == false)
        {
            Destroy(skillPoolManager.transform.GetChild(0).gameObject);
        }
    }


}
