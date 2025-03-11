using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float speed;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 forwardToPlayer = (player.transform.position - transform.position).normalized;
        forwardToPlayer.y = 0f;
        if (player.GetComponent<PlayerController>().isPlayerOnGround())
        {
            rb.velocity = forwardToPlayer * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
        //transform.Translate((player.transform.position - transform.position).normalized * speed);
    }
}
