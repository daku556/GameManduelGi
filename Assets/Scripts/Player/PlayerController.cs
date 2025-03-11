using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float depth;
    private float verticalInput;
    private float horizontalInput;
    private float jumpInput;
   
    private bool isOnGround;
    private bool isOnFire;
    
    private float currentSpeed;
    private float playerKnockBack = 10f; // enemy로 이동권장

    private Rigidbody playerRigidbody;
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = PlayerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerManager.isAlive)
        {
            gameObject.SetActive(false);
        }

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
        Vector3 lookDirection = (worldPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection);// * Quaternion.Euler(90, 0, 0);

        currentSpeed = playerRigidbody.velocity.magnitude * 3.6f;
    }

    private void FixedUpdate()
    {
        if (isOnGround && jumpInput == 1)
        {
            playerRigidbody.AddForce(0, jumpInput * playerManager.baseJumpPower, 0, ForceMode.Impulse);
            jumpInput = 0;
            isOnGround = false;
        }

        if (currentSpeed <= playerManager.maxSpeed && isOnGround)
        {
            playerRigidbody.AddForce(horizontalInput * playerManager.baseSpeed, 0, verticalInput * playerManager.baseSpeed);
        }

        playerRigidbody.velocity *= 0.98f;
        playerRigidbody.angularVelocity *= 0.98f;
    }

    void OnMovement(InputValue value)
    {
        horizontalInput = value.Get<Vector3>().x;
        verticalInput = value.Get<Vector3>().z;
    }

    void OnJump(InputValue value)
    {
        jumpInput = 1f;
    }

    void OnFire(InputValue value)
    {
        isOnFire = value.isPressed;
        Gun playerGun = playerManager.playerGun;
        if (isOnFire && playerGun.isFireing() == false)
        {
            playerGun.startFire();
        }
        else if (!isOnFire && playerGun.isFireing() == true)
        {
            playerGun.stopFire();
        }
    }

    public int getSpeed()
    {
        return Mathf.FloorToInt(currentSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item != null)
        {
            Item addingItem = new Item(item.itemObject);
            if(PlayerManager.Instance.AddItemToInventory(addingItem, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerManager.currentHealth--;
            playerRigidbody.AddForce((transform.position - collision.gameObject.transform.position).normalized * playerKnockBack, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
    public bool isPlayerOnGround()
    {
        return isOnGround;
    }

    public void playerDisabled()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        // 모든 자식의 스크립트 비활성화
        foreach (MonoBehaviour script in GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }
}
