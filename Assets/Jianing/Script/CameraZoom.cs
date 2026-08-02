//using Cinemachine;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Zoom")]
    public float minDistance = 10f;
    public float maxDistance = 22f;
    public float zoomMultiplier = 1.2f;

    [Header("Smooth")]
    public float zoomSpeed = 5f;

    private CinemachineThirdPersonFollow follow;

    void Start()
    {
        follow = GetComponent<CinemachineThirdPersonFollow>();
    }

    void Update()
    {
        if (follow == null)
            return;

        float distance = Vector3.Distance(player1.position, player2.position);

        float targetDistance =
            Mathf.Clamp(distance * zoomMultiplier,
                        minDistance,
                        maxDistance);

        follow.CameraDistance = Mathf.Lerp(
            follow.CameraDistance,
            targetDistance,
            zoomSpeed * Time.deltaTime);
    }
}