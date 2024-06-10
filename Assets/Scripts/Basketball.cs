using UnityEngine;
using System.Collections;

public class Basketball : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition; // Define startPosition as a class member variable
    private AudioManager audioManager;
    private GameManager gameManager;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isThrown = false;
    private bool hasHitNet = false;
    public float distanceFromCamera = 1.5f;  // Adjust this to set how far the ball appears from the camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
        rb.isKinematic = true;  // Start as kinematic to keep the ball stationary

        // Set the initial position
        ResetBallPosition();
    }

    void Update()
    {
        HandleInput();
        if (isThrown)
        {
            CheckOutOfBounds();
        }
    }

    private void HandleInput()
    {
        if (isThrown)
            return;

        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                audioManager.PlayFingerTap();
                touchStartPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                Vector2 swipe = touchEndPos - touchStartPos;
                Vector3 direction = new Vector3(swipe.x, swipe.y, swipe.magnitude);
                direction = Camera.main.transform.TransformDirection(direction);
                float force = swipe.magnitude * 0.01f; // Adjust the force multiplier as needed

                rb.isKinematic = false; // Switch to non-kinematic mode to follow physics
                rb.AddForce(new Vector3(direction.x, direction.y * 1.5f, direction.z).normalized * force, ForceMode.Impulse);
                isThrown = true;
                hasHitNet = false;
                StartCoroutine(ResetAfterDelay(60f)); // Start the coroutine to reset the ball after 60 seconds
            }
        }

        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            audioManager.PlayFingerTap();
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchEndPos = Input.mousePosition;
            Vector2 swipe = touchEndPos - touchStartPos;
            Vector3 direction = new Vector3(swipe.x, swipe.y, swipe.magnitude);
            direction = Camera.main.transform.TransformDirection(direction);
            float force = swipe.magnitude * 0.01f; // Adjust the force multiplier as needed

            rb.isKinematic = false; // Switch to non-kinematic mode to follow physics
            rb.AddForce(new Vector3(direction.x, direction.y * 1.5f, direction.z).normalized * force, ForceMode.Impulse);
            isThrown = true;
            hasHitNet = false;
            StartCoroutine(ResetAfterDelay(60f)); // Start the coroutine to reset the ball after 60 seconds
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.name); // Debug log

        if (other.CompareTag("NetCollider"))
        {
            audioManager.PlayWin();
            gameManager.IncrementScore();
            hasHitNet = true;
            ResetBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.collider.name); // Debug log

        audioManager.PlayBasketballBounce();
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y < -1f) // Adjust this threshold as needed
        {
            Debug.Log("Ball is out of bounds. Resetting ball."); // Debug log
            ResetBall();
        }
    }

    private void ResetBall()
    {
        Debug.Log("Resetting ball to start position."); // Debug log
        ResetBallPosition();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // Return to kinematic mode to stay stationary
        isThrown = false;
        gameManager.StartNewThrow();
    }

    private void ResetBallPosition()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;  // Keep the ball at the same height as the camera
        cameraForward.Normalize();
        startPosition = Camera.main.transform.position + cameraForward * distanceFromCamera;

        // Adjust the startPosition to be more centered on the screen
        startPosition.y = Camera.main.transform.position.y - 0.5f; // Move the ball down a bit from the camera's position

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!hasHitNet)
        {
            Debug.Log("Ball did not hit the net. Resetting after delay."); // Debug log
            ResetBall();
        }
    }
}
