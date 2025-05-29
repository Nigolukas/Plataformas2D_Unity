using System.Collections;
using UnityEngine;

public class Wizard_script : MonoBehaviour
{
    [SerializeField] private GameObject Fireball;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float TimeBeforeAttack;
    [SerializeField] private float DamageInside;
    private Animator animator;
    private bool IsClose;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Ataque());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Ataque()
    {
        while (true)
        {
            animator.SetTrigger("atacar");
            if (IsClose) {
                Flip(player.transform.position.x);
            }
            yield return new WaitForSeconds(TimeBeforeAttack);
            
        }
    }

    private void LanzarBola()
    {
        Instantiate(Fireball, SpawnPoint.position, transform.rotation);
    }


    private void Flip(float targetX)
    {
        if (targetX > transform.position.x)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            IsClose = true;
            player = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            LifeSystem PLayerSystem = collision.gameObject.GetComponent<LifeSystem>();
            PLayerSystem.Damage(DamageInside);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            IsClose = false;
        }
    }
}
