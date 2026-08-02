using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Camera Settings")]
    public float height = 8f;          // Lens height
    public float minDistance = 10f;    // nearest distance
    public float maxDistance = 25f;    // the farthest distance

    [Header("Zoom")]
    public float zoomMultiplier = 1.2f;

    [Header("Smooth")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        // Two player center points
        Vector3 center = (player1.position + player2.position) * 0.5f;

        // Player distance
        float distance = Vector3.Distance(player1.position, player2.position);

        // Calculate lens distance based on distance
        float cameraDistance = Mathf.Clamp(
            distance * zoomMultiplier,
            minDistance,
            maxDistance);

        // Target position of the lens
        Vector3 targetPosition =
            center
            - transform.forward * cameraDistance
            + Vector3.up * height;

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime);

        // Always look toward the center
        transform.LookAt(center);
    }
}
