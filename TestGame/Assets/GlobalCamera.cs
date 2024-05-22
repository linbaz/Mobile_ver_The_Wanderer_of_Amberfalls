using UnityEngine;

public class GlobalCamera : MonoBehaviour
{
    private Camera globalMapCamera;
    private bool isActive = false;
    private Vector3 lastMousePosition;

    public float cameraSpeed = 10f; // Скорость перемещения камеры

    void Start()
    {
        globalMapCamera = GetComponent<Camera>();
        globalMapCamera.enabled = false; // Камера изначально неактивна     
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isActive = !isActive;
            globalMapCamera.enabled = isActive; // Включаем или выключаем камеру при нажатии на клавишу M

            // Находим игрока по тегу "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Устанавливаем начальное положение камеры в положение игрока
                transform.position = player.transform.position;
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
        }

        if (isActive && Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (isActive && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // Горизонтальное и вертикальное перемещение
            Vector3 move = new Vector3(delta.x, delta.y, 0) * Time.deltaTime * cameraSpeed;

            // Передвигаем камеру только по X и Y, оставляя Z неизменной
            transform.Translate(-move.x, -move.y, 0, Space.Self);
        }
    }
}