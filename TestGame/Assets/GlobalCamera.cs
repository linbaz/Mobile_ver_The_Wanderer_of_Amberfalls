using UnityEngine;
using UnityEngine.UI;

public class GlobalCamera : MonoBehaviour
{
    private Camera globalMapCamera;
    private bool isActive = false;
    private Vector3 lastMousePosition;

    public float cameraSpeed = 10f; // �������� ����������� ������
    public Button globalMapButton; // ������ �� ������ Global Map Button
    public Button exitGlobalMapButton; // ������ �� ������ Exit Global Map Button

    void Start()
    {
        globalMapCamera = GetComponent<Camera>();
        globalMapCamera.enabled = false; // ������ ���������� ���������

        // ������� ������ � ��������� ������, ������� ����� ���������� ��� ����� �� ���
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

            // �������������� � ������������ �����������
            Vector3 move = new Vector3(delta.x, delta.y, 0) * Time.deltaTime * cameraSpeed;

            // ����������� ������ ������ �� X � Y, �������� Z ����������
            transform.Translate(-move.x, -move.y, 0, Space.Self);
        }
    }

    // ����� ��� ������������ ��������� ������ �� ������ Global Map Button
    void ToggleGlobalMapCamera()
    {
        isActive = !isActive;
        globalMapCamera.enabled = isActive; // �������� ��� ��������� ������

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

    // ����� ��� ������ �� ���������� ����� �� ������ Exit Global Map Button
    void ExitGlobalMap()
    {
        isActive = false;
        globalMapCamera.enabled = false; // ��������� ������

        // ���������� ������ ������� � ��������� ���������, ����� �������� ������� � �������� ������
        transform.position = Vector3.zero;
    }
}
