using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.TimelineAsset;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created}
    [SerializeField] private GameObject Panel;
    [SerializeField] private bool ShowStart;
    private CanvasGroup canvasGroup;
    void Start()
    {
        if (ShowStart)
        {
            canvasGroup = Panel.AddComponent<CanvasGroup>();
            StartCoroutine(ShowAtStart());  
        }
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Openlevel(string levelName)
    {
        if (levelName != null) 
        {
            SceneManager.LoadScene(levelName);
        }
    }


    /*public void Openlevel2()
    {
         SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
    }*/

    public void restartVariables()
    {
        PlayerPrefs.SetInt("coins", 0);
        //PlayerPrefs.SetString("Level", "Gameplay 1");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    IEnumerator ShowAtStart()
    {
        Panel.SetActive(true);
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(3f); // tiempo visible completamente

        float count = 0f;

        while (count < 5)
        {
            count += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, count / 5);
            yield return null;
        }

        Panel.SetActive(false);
    }
}
