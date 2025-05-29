using UnityEngine;

public class Coin : MonoBehaviour
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
        Debug.Log("choque");
        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            LifeSystem_player PLayerSystem = collision.gameObject.GetComponent<LifeSystem_player>();
            PLayerSystem.GetCoin();
            Destroy(this.gameObject);
        }
    }
}
