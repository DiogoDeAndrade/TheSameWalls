using UnityEngine;

public class CameraIdleSway : MonoBehaviour
{
    [Header("Position Sway")]
    [SerializeField] private float positionAmplitude = 0.05f; // meters
    [SerializeField] private float positionFrequency = 0.2f;  // cycles per second

    [Header("Rotation Sway")]
    [SerializeField] private float rotationAmplitude = 0.5f;  // degrees
    [SerializeField] private float rotationFrequency = 0.1f;  // cycles per second

    [Header("Axes Control")]
    [SerializeField] private Vector3 positionAxes = new Vector3(1, 0.5f, 0);
    [SerializeField] private Vector3 rotationAxes = new Vector3(0.2f, 1, 0);

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        float t = Time.time;

        // Position sway (gentle figure-eight type)
        Vector3 posOffset = new Vector3(
            Mathf.Sin(t * positionFrequency * Mathf.PI * 2f) * positionAxes.x,
            Mathf.Sin(t * positionFrequency * Mathf.PI * 2f + 1f) * positionAxes.y,
            Mathf.Sin(t * positionFrequency * Mathf.PI * 2f + 2f) * positionAxes.z
        ) * positionAmplitude;

        transform.localPosition = initialPosition + posOffset;

        // Rotation sway (slower, subtler)
        Vector3 rotOffset = new Vector3(
            Mathf.Sin(t * rotationFrequency * Mathf.PI * 2f) * rotationAxes.x,
            Mathf.Sin(t * rotationFrequency * Mathf.PI * 2f + 1f) * rotationAxes.y,
            Mathf.Sin(t * rotationFrequency * Mathf.PI * 2f + 2f) * rotationAxes.z
        ) * rotationAmplitude;

        transform.localRotation = initialRotation * Quaternion.Euler(rotOffset);
    }

    private void OnDisable()
    {
        // Reset camera to base position/rotation
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
}
