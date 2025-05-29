using System.Collections;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    [SerializeField] private GameObject PanelTuto;
    //[SerializeField] private bool AutoStart;
    [SerializeField] private float durationFade = 2f;
    private CanvasGroup canvasGroup;
    private Coroutine tutorialCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = PanelTuto.AddComponent<CanvasGroup>();
        /*if (AutoStart)
        {
            StartCoroutine(TUTO());
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            if (tutorialCoroutine == null)
            {
                tutorialCoroutine = StartCoroutine(TUTO());
            }
        }
    }

    IEnumerator TUTO()
    {
        PanelTuto.SetActive(true);
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(3f); // tiempo visible completamente

        float count = 0f;

        while (count < durationFade)
        {
            count += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, count / durationFade);
            yield return null;
        }

        PanelTuto.SetActive(false);
        tutorialCoroutine = null;
    }
}
