using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;

    private Camera mainCam;
    private Vector3 mousePos;

    private float VelocityX;
    private float VelocityY;

    [SerializeField]
    private float MaxHealth;

    [SerializeField]
    private float Firerate;

    private float firecounter;

    private float CurrtHealth;

    public Slider HealthBar;
   
    public Transform firePos;

    public float MoveSpeed;
    public GameObject bulletPrefab;

    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CurrtHealth = MaxHealth;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = CurrtHealth;
        firecounter = 0;
    }

    // Update is called once per frame
    void Update()
    {

        VelocityX = Input.GetAxisRaw("Horizontal")* MoveSpeed * Time.deltaTime;
        VelocityY = Input.GetAxisRaw("Vertical") * MoveSpeed * Time.deltaTime;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        firecounter += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && firecounter > Firerate)
        {
            firecounter = 0;
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Shoot(transform.right);
        }

        HealthBar.value = CurrtHealth;

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(VelocityX, VelocityY);
    }

    public void TakeDamage(float Damage)
    {
        CurrtHealth -= Damage;
        
        if(CurrtHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
