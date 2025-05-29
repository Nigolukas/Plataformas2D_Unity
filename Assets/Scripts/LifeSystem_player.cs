using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LifeSystem_player : LifeSystem
{
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private TextMeshProUGUI CoinsText;
    [SerializeField] private int coins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        coins = PlayerPrefs.GetInt("coins");
        CoinsText.text = coins.ToString();
    }

    public override void Damage(float damage)
    {
        Lifes -= damage;
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (Lifes <= 0)
        {
            Destroy(gameObject);
            panelGameOver.SetActive(true);
        }
        UpdateVida();
    }

    public void GetCoin()
    {
        coins++;
        Lifes += 3;
        CoinsText.text = coins.ToString();
        UpdateVida();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Stop();
        }
    }

    public void Stop()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            panelPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            panelPause.SetActive(false);
        }
    }



    public void Win()
    {
            Time.timeScale = 0.0f;
            panelWin.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Win"))
        {
            PlayerPrefs.SetInt("coins", coins);
            //PlayerPrefs.SetString("Level", "Gameplay 1");
            Win();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
