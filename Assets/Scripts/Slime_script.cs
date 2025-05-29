using System.Collections;
using UnityEngine;

public class Slime_script : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float Speed;
    [SerializeField] private float Damage;
    private Animator animator;
    private Vector3 FinalDestiny;
    private int Index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FinalDestiny = wayPoints[Index].position;
        animator = GetComponent<Animator>();
        StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            while (transform.position != FinalDestiny)
            {
                transform.position = Vector3.MoveTowards(transform.position, FinalDestiny, Speed * Time.deltaTime);
                yield return null;
            }
            NewDestiny();
        }
    }
    private void NewDestiny()
    {
        Index++;
        if(Index >= wayPoints.Length)
        {
            Index = 0;
        }
        FinalDestiny = wayPoints[Index].position;
        if (FinalDestiny.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            animator.SetBool("atacando", true);
            Speed = Speed+4;

        }
        else if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            LifeSystem PLayerSystem = collision.gameObject.GetComponent<LifeSystem>();
            PLayerSystem.Damage(Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            animator.SetBool("atacando", false);
            Speed = Speed - 4;

        }
    }
}
