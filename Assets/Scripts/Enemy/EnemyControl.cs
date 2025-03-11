using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3;
    [SerializeField] private float currentHealth;
    [SerializeField] private float reflectPower;


    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem frictionParticle;
    [SerializeField] private GameObject expBall;
    [SerializeField] private HealthSlider healthSlider;
    private Collider enemyCollider;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider>();
    }
    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            PlayerManager.Instance.score += 10;
            GameManager.Instance.returnGameObjectInPool("Enemy", gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            Instantiate(expBall, transform.position, explosionParticle.transform.rotation);
        }
    }

    void LateUpdate()
    {
        if(healthSlider != null)
        {
            healthSlider.UpdateHealthSlider(currentHealth, maxHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            currentHealth--;
            GameManager.Instance.returnGameObjectInPool("Bullet", other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigid = collision.gameObject.GetComponent<Rigidbody>();
            PlayerManager.Instance.currentHealth--;
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPosition = contact.point;
            Vector3 hitNormal = contact.normal * -1;
            playerRigid.AddForce(hitNormal * reflectPower, ForceMode.Impulse);
            Debug.DrawRay(hitPosition, hitNormal * 5, Color.green, 2.0f);
            Instantiate(frictionParticle, hitPosition, Quaternion.LookRotation(hitNormal));
        }
    }

}
