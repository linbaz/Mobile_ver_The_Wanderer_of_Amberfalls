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
    private Vector3 lastPosition; // ���������� ��������� ���������

    public CoinManager cm;

    private void Start()
    {
        StartCoroutine(CheckFogOfWar(checkInterval));
        secondaryFogOfWar.localScale = new Vector2(sightDistance, sightDistance) * 2f;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ��������� ������� ������
        spriteRenderer.flipX = false;

        // ��������� ��������� ��������� ���������
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

                // ���������� ������� ��������� � ���������� ��� ����������� ����������� ��������
                if (transform.position.x > lastPosition.x)
                {
                    spriteRenderer.flipX = false; // ������� ������
                }
                else if (transform.position.x < lastPosition.x)
                {
                    spriteRenderer.flipX = true; // ������� �����
                }

                // ��������� ���������� ���������
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

