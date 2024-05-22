using UnityEngine;

public class KeepZPosition : MonoBehaviour
{
    private float initialZPosition; // ���������� ��� �������� ��������� ������� �� ��� Z

    void Start()
    {
        // ��������� ��������� ������� �� ��� Z ��� ������� �����
        initialZPosition = transform.position.z;
    }

    void Update()
    {
        // �������� ������� ������� �������
        Vector3 currentPosition = transform.position;

        // ������������� ������� �� ��� Z ������ ��������� �������
        currentPosition.z = initialZPosition;

        // ������������� ����� ������� �������
        transform.position = currentPosition;
    }
}
