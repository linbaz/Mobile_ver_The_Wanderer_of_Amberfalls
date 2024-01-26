using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 
      
    private void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            // Получаем положение курсора в мировых координатах
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Получаем положение игрока в мировых координатах
            Vector3 playerPosition = transform.position;

            // Определяем направление взгляда
            
            
                if (mousePosition.x < playerPosition.x)
                {
                    // Если курсор слева от игрока, отзеркаливаем спрайт по горизонтальной оси
                    spriteRenderer.flipX = true;
                }
                else
                {
                    // Если курсор справа от игрока, не отзеркаливаем спрайт
                    spriteRenderer.flipX = false;
                }
            
       

            // Обрабатываем движение
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(inputX, inputY);

            rb.velocity = movement * speed;
        }
            
    }
}
