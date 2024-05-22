using UnityEngine;

public class GlobalCamera : MonoBehaviour
{
    private Camera globalMapCamera;
    private bool isActive = false;
    private Vector3 lastMousePosition;

    public float cameraSpeed = 10f; // �������� ����������� ������

    void Start()
    {
        globalMapCamera = GetComponent<Camera>();
        globalMapCamera.enabled = false; // ������ ���������� ���������     
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isActive = !isActive;
            globalMapCamera.enabled = isActive; // �������� ��� ��������� ������ ��� ������� �� ������� M

            // ������� ������ �� ���� "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // ������������� ��������� ��������� ������ � ��������� ������
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

            // �������������� � ������������ �����������
            Vector3 move = new Vector3(delta.x, delta.y, 0) * Time.deltaTime * cameraSpeed;

            // ����������� ������ ������ �� X � Y, �������� Z ����������
            transform.Translate(-move.x, -move.y, 0, Space.Self);
        }
    }
}