using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector3 MoveDirection;
    

    public float ShootForce;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
        rb.velocity = MoveDirection.normalized * ShootForce * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {

            Enemy EnemyScript = collision.transform.GetComponent<Enemy>();
            EnemyScript.SetPushBackDirection(MoveDirection);
        }


        Destroy(this.gameObject);
    }

    public void Shoot(Vector3 Direction)
    {
        MoveDirection = Direction;
       
    }

    
   
}
