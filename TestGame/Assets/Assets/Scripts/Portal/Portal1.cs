using UnityEngine;

public class Portal1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint2");

        if (spawnPoint != null)
        {
            player.position = spawnPoint.transform.position + new Vector3(-2f, 0.5f, 0f);
        }
        else
        {
            Debug.LogError("SpawnPoint not found!");
        }
    }
}
