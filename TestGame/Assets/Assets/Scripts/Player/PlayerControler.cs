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
    private Vector3 lastPosition; // Предыдущее положение персонажа

    public CoinManager cm;

    private void Start()
    {
        StartCoroutine(CheckFogOfWar(checkInterval));
        secondaryFogOfWar.localScale = new Vector2(sightDistance, sightDistance) * 2f;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Начальный поворот вправо
        spriteRenderer.flipX = false;

        // Сохраняем начальное положение персонажа
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                // Get movement input
                float inputX = Input.GetAxis("Horizontal");
                float inputY = Input.GetAxis("Vertical");

                // Calculate movement vector and apply velocity
                Vector2 movement = new Vector2(inputX, inputY);
                rb.velocity = movement * speed;

                // Сравниваем текущее положение с предыдущим для определения направления движения
                if (transform.position.x > lastPosition.x)
                {
                    spriteRenderer.flipX = false; // Поворот вправо
                }
                else if (transform.position.x < lastPosition.x)
                {
                    spriteRenderer.flipX = true; // Поворот влево
                }

                // Обновляем предыдущее положение
                lastPosition = transform.position;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }
}

