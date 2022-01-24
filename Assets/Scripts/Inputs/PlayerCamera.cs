using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraPoint;

    [Header("Camera look parameters")]
    [Range(0f, 100f)]
    [SerializeField] private float lookSensativity = 100;
    [Range(-90f, -60f)]
    [SerializeField] private float bottomCameraAngle = -60;
    [Range(60f, 90f)]
    [SerializeField] private float upperCameraAngle = 60;

    private float xAxisRotation = 0;
    private float yAxisRotation = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        transform.position = cameraPoint.position;
        PlayerLook();
    }

    private void PlayerLook()
    {
        float xAxisMouseMovement = Input.GetAxis("Mouse X") * lookSensativity * Time.deltaTime;
        float yAxisMouseMovement = Input.GetAxis("Mouse Y") * lookSensativity * Time.deltaTime;

        xAxisRotation -= yAxisMouseMovement;
        xAxisRotation = Mathf.Clamp(xAxisRotation, bottomCameraAngle, upperCameraAngle);

        yAxisRotation += xAxisMouseMovement;

        transform.rotation = Quaternion.Euler(xAxisRotation, yAxisRotation, 0);
        player.Rotate(Vector3.up * xAxisMouseMovement);
    }
}
