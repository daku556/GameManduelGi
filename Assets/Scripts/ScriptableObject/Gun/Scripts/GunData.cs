using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun System/Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;
    public float fireRate;


    public GameObject bulletPrefab;
    public int bulletDamage;
    public float bulletSpeed;
    public float bulletLifeTime;
}
