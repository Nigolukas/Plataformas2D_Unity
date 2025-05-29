using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] protected float Lifes;
    [SerializeField] protected Slider BarraVida;
    protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public virtual void Damage(float damage)
    {
        Lifes -= damage;
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (Lifes <= 0)
        {
            Destroy(gameObject);

        }
        UpdateVida();
    }


    public void UpdateVida()
    {
        if (BarraVida != null)
        {
            BarraVida.value = Lifes;
        }
    }

    protected IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(0.57f,0,0.05f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
}
