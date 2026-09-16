using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    public float speed = 3.0f; // Initial speed
    public float speedIncrease = 3f; // How much to increase per throw

    private Vector3 targetPosition;

    void Start()
    {
        SetNewTargetPosition();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
    }

    public void IncreaseSpeed()
    {
        speed += speedIncrease;
        Debug.Log("New Crosshair Speed: " + speed);
    }
}
