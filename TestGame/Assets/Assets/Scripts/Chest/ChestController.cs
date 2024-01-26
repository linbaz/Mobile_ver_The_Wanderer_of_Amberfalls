using System.Collections;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject[] itemsToSpawn; 
    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            SpawnItems(); 
            isOpen = true; 
            Destroy(gameObject); 
        }
    }

    private void SpawnItems()
    {
        if (itemsToSpawn.Length > 0)
        {
            int randomItemIndex = Random.Range(0, itemsToSpawn.Length);

            GameObject spawnedItem = Instantiate(itemsToSpawn[randomItemIndex], transform.position + Vector3.up, Quaternion.identity);            
        }
    }

}
