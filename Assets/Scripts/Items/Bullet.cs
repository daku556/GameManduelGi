using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float depth;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletLifeTime;

    private GameObject player;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        player = GameObject.Find("Player");
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    public void SetProperties(GunData gunData)
    {
        bulletSpeed = gunData.bulletSpeed;
        bulletDamage = gunData.bulletDamage;
        bulletLifeTime = gunData.bulletLifeTime;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 shootDirection;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
        shootDirection = (worldPosition - transform.position).normalized;
        shootDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(shootDirection) * Quaternion.Euler(90, 0, 0);
        bulletRigidbody.velocity = shootDirection * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletLifeTime >= 0)
        {
            bulletLifeTime -= Time.deltaTime;
        }
        else if (bulletLifeTime < 0)
        {
            GameManager.Instance.returnGameObjectInPool("Bullet", gameObject);
        }
    }
}
