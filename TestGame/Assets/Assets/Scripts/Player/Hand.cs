using System.Collections;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float offset;
    private bool isCodeDisabled = false;
    private float currentDelay = 0.3f;

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (!isCodeDisabled)
            {
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float rotateWeapon = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotateWeapon + offset);

                Vector2 scale = transform.localScale;
                if (direction.x < 0)
                {
                    scale.y = -1;
                }
                else if (direction.x > 0)
                {
                    scale.y = 1;
                }

                transform.localScale = scale;

                if (Input.GetMouseButton(0))
                {
                    currentDelay = Mathf.Clamp(currentDelay - Time.deltaTime, 0.1f, 1.0f);
                    StartCoroutine(DisableCodeForDuration(currentDelay));
                }
            }
        }
        
    }

    private IEnumerator DisableCodeForDuration(float duration)
    {
        isCodeDisabled = true;
        yield return new WaitForSeconds(duration);
        isCodeDisabled = false;
    }
}
