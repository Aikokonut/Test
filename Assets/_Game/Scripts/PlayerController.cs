using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Disc Settings")]
    public Transform DiscSpawnPoint;
    public GameObject DiscPrefab;
    public float MaxThrowAngle = 45f;
    public float MinThrowAngle = -10f;
    public float MaxThrowPower = 30f;
    public float MinThrowPower = 5f;

    [Header("Audio")]
    public AudioSource ThrowSound;

    [Header("Gameplay")]
    public int ThrowCount = 0;

    public void ThrowDisc(float angle, float power)
    {
        // Clamp angle and power within the allowed range
        angle = Mathf.Clamp(angle, MinThrowAngle, MaxThrowAngle);
        power = Mathf.Clamp(power, MinThrowPower, MaxThrowPower);

        // Instantiate the disc at the spawn point
        GameObject disc = Instantiate(DiscPrefab, DiscSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = disc.GetComponent<Rigidbody>();

        // Calculate throw direction and apply force
        Vector3 throwDirection = Quaternion.Euler(angle, 0, 0) * transform.forward;
        rb.AddForce(throwDirection * power, ForceMode.Impulse);

        // Increment throw count
        ThrowCount++;

        // Play throw sound
        if (ThrowSound != null)
        {
            ThrowSound.Play();
        }

        Debug.Log($"Disc thrown! Angle: {angle}, Power: {power}");
    }
}
