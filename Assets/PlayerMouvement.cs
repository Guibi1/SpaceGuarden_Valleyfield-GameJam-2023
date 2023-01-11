using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public float speedMultiplier = 50;
    public bool controlsInverted = false;
    public float maxHealth = 100;
    public float currentHealth = 100;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        if (controlsInverted)
        {
            horizontalAxis = -horizontalAxis;
            verticalAxis = -verticalAxis;
        }

        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        rb.AddForce(targetPos * speedMultiplier * Time.deltaTime * 144);

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
