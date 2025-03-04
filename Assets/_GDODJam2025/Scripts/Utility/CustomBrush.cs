using System.Collections.Generic;

using UnityEngine;

public class CustomBrush : MonoBehaviour
{
    [Header("Brush Settings")]
    public bool enableDrawing = false; // Toggle for drawing
    public bool enableSurfaceSnapping = true; // Align models to surface
    public bool enableLookAt = false; // Enable or disable look-at behavior

    [Header("Placement Settings")]
    public float placementCooldown = 0.5f; // Cooldown between model placements (in seconds)
    public Transform parentTransform; // Parent for placed models
    public Vector3 randomOffsetRange = Vector3.zero; // Random offset range
    public Transform lookAtTarget; // The target models should look at
    public Vector3 specifiedScale = Vector3.one;  // The scale to apply to all models when placed

    [Header("Models")]
    public List<GameObject> modelsToPlace = new List<GameObject>(); // List of models to choose from
}
