using Inventory.Model;
using Inventory.UI;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FogOfWar fogOfWar;
    public Transform secondaryFogOfWar;
    [Range(0, 100)]
    public float sightDistance;
    public float checkInterval;
    public UIInventoryPage inventory;

    public float speed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        StartCoroutine(CheckFogOfWar(checkInterval));
        secondaryFogOfWar.localScale = new Vector2(sightDistance, sightDistance) * 2f;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 
      
    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 playerPosition = transform.position;

                if (mousePosition.x < playerPosition.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }

                float inputX = Input.GetAxis("Horizontal");
                float inputY = Input.GetAxis("Vertical");

                Vector2 movement = new Vector2(inputX, inputY);

                rb.velocity = movement * speed;
            }
        }
        
            
    }

    private IEnumerator CheckFogOfWar(float checkInterval)
    {
        while (true)
        {
            fogOfWar.MakeHole(transform.position, sightDistance);
            yield return new WaitForSeconds(checkInterval);
        }
    }

}
