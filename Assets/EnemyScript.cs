using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 5f;
    public float frequencyX = 1f;
    public float frequencyY = 1.5f;

    private float timeCounterX = 1f;
    private float timeCounterY = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position
        initialPosition = transform.position;
    }

    void Update()
    {
        MoveInSineWave();
    }

    void MoveInSineWave()
    {
        // Increment the time counters
        timeCounterX += Time.deltaTime * frequencyX;
        timeCounterY += Time.deltaTime * frequencyY;

        // Calculate new positions based on sine waves for both X and Y directions
        float x = initialPosition.x + Mathf.Sin(timeCounterX) * speed;
        float y = initialPosition.y + Mathf.Sin(timeCounterY) * speed;

        // Set the new position
        transform.position = new Vector3(x, y, 0f);
    }
}
