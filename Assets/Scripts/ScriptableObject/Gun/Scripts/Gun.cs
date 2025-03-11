using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;  // ScriptableObject를 연결할 변수
    private float fireRate;
    private float fireCooldown;
    private bool isOnFire = false;
    // Start is called before the first frame update
    private void Awake()
    {
        fireRate = gunData.fireRate;
        fireCooldown = 0;
    }

    private void Update()
    {
        if (isOnFire)
        {
            if (fireCooldown <= 0)
            {
                fireBullet();
                fireCooldown = fireRate;
            }
            else
            {
                fireCooldown -= Time.deltaTime;
            }
        }
    }

    public void fireBullet()
    {
        GameObject bullet = GameManager.Instance.getGameObjectInPool("Bullet");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetProperties(gunData);
        bullet.SetActive(true);
    }

    public void startFire()
    {
        isOnFire = true;
    }
    public void stopFire()
    {
        isOnFire = false;
    }

    public bool isFireing()
    {
        return isOnFire; 
    }

}
