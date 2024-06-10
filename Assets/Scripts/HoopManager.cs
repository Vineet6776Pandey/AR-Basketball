using UnityEngine;

public class HoopManager : MonoBehaviour
{
    public GameObject hoopGameObject; // The hoop game object
    public AudioSource disappearSound; // Sound to play when hoop disappears
    public AudioSource reappearSound; // Sound to play when hoop reappears
    public GameObject[] newPositions; // Array of new positions for the hoop
    public float disappearDuration = 1.0f; // Duration for which the hoop disappears
    public float changeInterval = 20.0f;   // Time interval between position changes

    private bool isDisappearing;
    private bool isChangingPosition;
    private int currentIndex = -1;  // Start with -1 to move to the first position at the beginning

    void Start()
    {
        // Ensure all references are set
        if (hoopGameObject == null || disappearSound == null || reappearSound == null || newPositions == null || newPositions.Length == 0)
        {
            Debug.LogError("HoopManager: Missing references. Please ensure all references are set in the inspector.");
            return;
        }

        isDisappearing = false;
        isChangingPosition = false;
        InvokeRepeating("DisappearAndReappear", changeInterval, changeInterval); // Schedule the first position change
    }

    public void DisappearAndReappear()
    {
        if (!isDisappearing && !isChangingPosition)
        {
            isDisappearing = true;
            disappearSound.Play();
            hoopGameObject.SetActive(false);
            Invoke("CompleteDisappearing", disappearDuration);
        }
    }

    void CompleteDisappearing()
    {
        isDisappearing = false;
        ChangeHoopPosition();
    }

    void ChangeHoopPosition()
    {
        isChangingPosition = true;
        currentIndex = (currentIndex + 1) % newPositions.Length; // Move to the next position in the array
        hoopGameObject.transform.position = newPositions[currentIndex].transform.position;
        hoopGameObject.transform.rotation = newPositions[currentIndex].transform.rotation;
        hoopGameObject.SetActive(true);
        reappearSound.Play();
        Invoke("ResetChangePositionFlag", 1f); // Wait for 1 second before allowing another position change
    }

    void ResetChangePositionFlag()
    {
        isChangingPosition = false;
    }
}
