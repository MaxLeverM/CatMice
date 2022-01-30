using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Camera look parameters")]
    [SerializeField] private float lookSensativity = 100;
    [Range(-90f, -60f)]
    [SerializeField] private float bottomCameraAngle = -60;
    [Range(60f, 90f)]
    [SerializeField] private float upperCameraAngle = 60;

    private float xAxisRotation = 0;

    protected virtual void Start()
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

        transform.localRotation = Quaternion.Euler(xAxisRotation, 0, 0);
        player.Rotate(Vector3.up * xAxisMouseMovement);
    }
}
