using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 20f;

    void Update()
    {
        // Rotate around Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
