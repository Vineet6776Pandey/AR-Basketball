using UnityEngine;

public class NetCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basketball"))
        {
            if (IsBasketballInNet(other.transform.position))
            {
                // Handle the ball passing through the net
                // Example: Increment score, play sound, etc.
            }
        }
    }

    private bool IsBasketballInNet(Vector3 ballPosition)
    {
        // Get the net collider
        Collider netCollider = GetComponent<Collider>(); // Assuming the net's collider is attached to the same GameObject

        // Check if the basketball's position is within the net area
        if (netCollider.bounds.Contains(ballPosition))
        {
            // Additional checks to ensure the basketball is passing through the net
            // You may need to adjust these checks based on the shape and size of your net collider
            Vector3 netTop = netCollider.bounds.center + new Vector3(0, netCollider.bounds.extents.y, 0);
            Vector3 netBottom = netCollider.bounds.center - new Vector3(0, netCollider.bounds.extents.y, 0);
            
            if (ballPosition.y > netBottom.y && ballPosition.y < netTop.y)
            {
                return true; // Ball is within the net area
            }
        }
        
        return false; // Ball is outside the net area
    }
}
