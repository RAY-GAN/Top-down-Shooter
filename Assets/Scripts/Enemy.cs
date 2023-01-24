using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float MoveSpeed;
    [SerializeField]private float VelocityX;
    [SerializeField] private float VelocityY;
    private float sign1;
    private float sign2;

    [SerializeField] private bool bisMoving;
    [SerializeField] private bool bisHit;

    [SerializeField] private float shootbackForce;

    [SerializeField] private float repelTime;
    [SerializeField] private float repelTimeCounter = 0;

    [SerializeField] private Vector2 PushBackDirection;

    [SerializeField] private float Damage;


    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);
        StartMoving();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bisHit)
        {
            CancelInvoke("ChangeDirection");
            repelTimeCounter += Time.deltaTime;
        }
        if (repelTimeCounter >= repelTime)
        {
            StartMoving();
            repelTimeCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (bisHit)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(PushBackDirection.normalized * shootbackForce);
        }

        if (bisMoving)
        {

            rb.velocity = new Vector2(VelocityX, VelocityY);

            if (transform.position.x <= -8 )
            {
                VelocityX = -VelocityX;
                rb.velocity = new Vector2(VelocityX, VelocityY);
                transform.Translate(new Vector3(0.2f, 0, 0));
            }

            else if (transform.position.x >= 8)
            {
                VelocityX = -VelocityX;
                rb.velocity = new Vector2(VelocityX, VelocityY);
                transform.Translate(new Vector3(-0.2f, 0, 0));
            }

            else if (transform.position.y <= -4 )
            {
                VelocityY = -VelocityY;
                rb.velocity = new Vector2(VelocityX, VelocityY);
                transform.Translate(new Vector3(0, 0.2f, 0));
            }

            else if (transform.position.y >= 4)
            {
                VelocityY = -VelocityY;
                rb.velocity = new Vector2(VelocityX, VelocityY);
                transform.Translate(new Vector3(0, -0.2f, 0));
            }

        }
    }

    private void ChangeDirection()
    {
        sign1 = Random.value < 0.5f ? -1f : 1f;
        sign2 = Random.value < 0.5f ? -1f : 1f;

        if (transform.position.x > -8 && transform.position.x < 8 && transform.position.y < 4 && transform.position.y > -4)
        {
            VelocityX = sign1 * Random.Range(3,MoveSpeed);
            VelocityY = sign2 * (5 - VelocityX/sign1);
        }

       
    }

    public void StartMoving()
    {
        bisMoving = true;
        bisHit = false;
        InvokeRepeating("ChangeDirection", 0.5f, 1.5f);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
           
            bisHit = true;
            bisMoving = false;

        }

        if (collision.gameObject.layer == 8)
        {
            GameManager.Instance.DeleteEnemy(gameObject);
            GameManager.Instance.Enemykilled++;
            Destroy(gameObject);
           

        }

        if (collision.gameObject.layer == 3)
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            player.TakeDamage(Damage);

            ChangeDirection();

        }

        if (collision.gameObject.layer == 7)
        {
            if (GameManager.Instance.Enemies.Count <12)
            {
                GameManager.Instance.SpawnEnemy();
            }
           
            VelocityX = -VelocityX;
            VelocityY = -VelocityY;
        }

    }

    public void SetPushBackDirection(Vector2 direction)
    {
        PushBackDirection = direction;
    }
}
