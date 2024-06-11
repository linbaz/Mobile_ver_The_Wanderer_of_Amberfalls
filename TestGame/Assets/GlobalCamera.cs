using UnityEngine;
using UnityEngine.UI;

public class GlobalCamera : MonoBehaviour
{
    private Camera globalMapCamera;
    private bool isActive = false;
    private Vector3 lastMousePosition;

    public float cameraSpeed = 10f; // Скорость перемещения камеры
    public Button globalMapButton; // Ссылка на кнопку Global Map Button
    public Button exitGlobalMapButton; // Ссылка на кнопку Exit Global Map Button

    void Start()
    {
        globalMapCamera = GetComponent<Camera>();
        globalMapCamera.enabled = false; // Камера изначально неактивна

        // Находим кнопки и добавляем методы, которые будут вызываться при клике на них
        globalMapButton.onClick.AddListener(ToggleGlobalMapCamera);
        exitGlobalMapButton.onClick.AddListener(ExitGlobalMap);
    }

    void Update()
    {
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

    // Метод для переключения активации камеры по кнопке Global Map Button
    void ToggleGlobalMapCamera()
    {
        isActive = !isActive;
        globalMapCamera.enabled = isActive; // Включаем или выключаем камеру

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

    // Метод для выхода из глобальной карты по кнопке Exit Global Map Button
    void ExitGlobalMap()
    {
        isActive = false;
        globalMapCamera.enabled = false; // Выключаем камеру

        // Перемещаем камеру обратно в начальное положение, чтобы избежать проблем с позицией камеры
        transform.position = Vector3.zero;
    }
}
