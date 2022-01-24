using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Camera look parameters")]
    [Range(0f, 100f)]
    [SerializeField] private float lookSensativity = 100f;
    [Range(-90f, -60f)]
    [SerializeField] private float bottomCameraAngle = -60f;
    [Range(60f, 90f)]
    [SerializeField] private float upperCameraAngle = 60f;

    private float xAxisRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        PlayerLook();
    }

    private void PlayerLook()
    {
        float xAxisMouseMovement = Input.GetAxis("Mouse X") * lookSensativity * Time.deltaTime;
        float yAxisMouseMovement = Input.GetAxis("Mouse Y") * lookSensativity * Time.deltaTime;

        xAxisRotation -= yAxisMouseMovement;
        xAxisRotation = Mathf.Clamp(xAxisRotation, bottomCameraAngle, upperCameraAngle);

        transform.localRotation = Quaternion.Euler(xAxisRotation, 0f, 0f);
        player.Rotate(Vector3.up * xAxisMouseMovement);
    }
}
