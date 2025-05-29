using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float ShootForce;
    [SerializeField] private float LifeTime;
    [SerializeField] private float Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        //transform.forward eje Z azul
        //transform.up eje Y verde
        //transform.right eje X rojo
        rb.AddForce(transform.right * ShootForce, ForceMode2D.Impulse);
        Destroy(this.gameObject,LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            Debug.Log("NOO LA POLIZIAAA");
        }
        else if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            Debug.Log("PIU PIU PIU");
            LifeSystem PLayerSystem = collision.gameObject.GetComponent<LifeSystem>();
            PLayerSystem.Damage(Damage);
            Destroy(this.gameObject);
        }
    }
}
