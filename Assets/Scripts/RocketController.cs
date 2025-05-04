using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float thrustForce = 10f;
    public float rotationSpeed = 3f;
    public float fuel = 100f;
    public float fuelBurnRate = 5f;
    public float health = 100f;

    private Rigidbody rb;
    private bool isMobile;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

#if UNITY_ANDROID && !UNITY_EDITOR
        isMobile = true;
        Input.gyro.enabled = true;
#else
        isMobile = false;
#endif
    }

    void Update()
    {
        if (isMobile)
        {
            HandleGyroRotation();
            HandleTouchThrust();
        }
        else
        {
            HandleKeyboardRotation();
            HandleKeyboardThrust();
        }

        CheckFuel();
    }

    void HandleGyroRotation()
    {
        Quaternion gyro = Input.gyro.attitude;
        Vector3 euler = new Vector3(-gyro.eulerAngles.x, -gyro.eulerAngles.y, gyro.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), Time.deltaTime * rotationSpeed);
    }

    void HandleTouchThrust()
    {
        if (Input.touchCount > 0 && fuel > 0)
        {
            rb.AddForce(transform.forward * thrustForce);
            fuel -= fuelBurnRate * Time.deltaTime;
        }
    }

    void HandleKeyboardRotation()
    {
        float pitch = -Input.GetAxis("Vertical");   // Up/Down Arrow
        float yaw = Input.GetAxis("Horizontal");    // Left/Right Arrow
        float roll = 0;

        if (Input.GetKey(KeyCode.Q)) roll = 1;
        if (Input.GetKey(KeyCode.E)) roll = -1;

        Vector3 rotationInput = new Vector3(pitch, yaw, roll) * rotationSpeed;
        transform.Rotate(rotationInput * Time.deltaTime, Space.Self);
    }

    void HandleKeyboardThrust()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && fuel > 0)
        {
            rb.AddForce(transform.forward * thrustForce);
            fuel -= fuelBurnRate * Time.deltaTime;
        }
    }

    void CheckFuel()
    {
        if (fuel <= 0)
        {
            Debug.Log("Out of fuel!");
        }
    }

    public void AddFuel(float amount)
    {
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0, 100);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Rocket destroyed!");
            // Add explosion or game over logic
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            AddFuel(20f);
            Destroy(other.gameObject);
        }
    }
}
