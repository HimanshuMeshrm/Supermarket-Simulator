using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Offset = new Vector3(0, 10, -10);
    [SerializeField] private float RotationSpeed = 5f;
    [SerializeField] private float ZoomSpeed = 2f;
    [SerializeField] private float MinZoom = 5f;
    [SerializeField] private float MaxZoom = 20f;
    [SerializeField] private float SmoothTime = 0.2f;
    [SerializeField] private float Pitch = 55f; // Increased for top-down effect

    private Vector3 _currentVelocity;
    private float _currentZoom;
    private float _yaw;
    private Vector3 _desiredPosition;

    private void Start()
    {
        _currentZoom = Offset.magnitude;
        _yaw = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        HandleInput();
        UpdatePosition();
        UpdateRotation();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(1))
            _yaw += Input.GetAxis("Mouse X") * RotationSpeed;

        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, MinZoom, MaxZoom);
    }

    private void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(Pitch, _yaw, 0f); // Use pitch
        Offset = rotation * new Vector3(0, 0, -_currentZoom);
        _desiredPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _currentVelocity, SmoothTime);
    }

    private void UpdateRotation()
    {
        transform.LookAt(Target.position);
    }
}
