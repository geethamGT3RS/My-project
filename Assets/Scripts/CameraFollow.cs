using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // Rocket
    public Vector3 offset = new Vector3(0, 2, -6);
    public float followSpeed = 5f;
    public float shakeIntensity = 0.2f;
    public float shakeDuration = 0.3f;

    private float shakeTimer = 0f;
    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = offset;
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        transform.LookAt(target);

        if (shakeTimer > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position += shakeOffset;
            shakeTimer -= Time.deltaTime;
        }
    }

    public void Shake(float duration = 0.3f, float intensity = 0.2f)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        shakeTimer = shakeDuration;
    }
}
