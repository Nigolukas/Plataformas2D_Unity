using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bat_script : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float Speed;
    [SerializeField] private float Damage;
    private Coroutine currentCoroutine;
    private Animator animator;
    private Vector3 FinalDestiny;
    private int Index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FinalDestiny = wayPoints[Index].position;
        animator = GetComponent<Animator>();
        currentCoroutine = StartCoroutine(Patrol());
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

    IEnumerator Attack(GameObject player)
    {
        while (transform.position != player.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
            Flip(player.transform.position.x);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Patrol());
    }


    private void Flip(float targetX)
    {
        if (targetX > transform.position.x)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void NewDestiny()
    {
        Index++;
        if (Index >= wayPoints.Length)
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
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            animator.SetTrigger("detectar");
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Attack(collision.gameObject));
        }
        else if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            animator.SetTrigger("atacar");
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Patrol());
            LifeSystem PLayerSystem = collision.gameObject.GetComponent<LifeSystem>();
            PLayerSystem.Damage(Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Patrol());
        }
            
    }
}
