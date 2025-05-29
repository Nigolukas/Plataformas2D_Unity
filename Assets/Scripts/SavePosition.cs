using UnityEngine;

public class SavePosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox")) 
        {
            PlayerPrefs.SetFloat("PosX",collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("PosY", collision.gameObject.transform.position.y);
        }
    }
}
