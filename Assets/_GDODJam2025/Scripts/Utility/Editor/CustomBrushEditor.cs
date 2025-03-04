using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomBrush))]
public class CustomBrushEditor : Editor
{
    private float lastPlacementTime = 0f; // Time of the last model placement
    private bool isDrawing;

    private void OnSceneGUI()
    {
        CustomBrush brush = (CustomBrush)target;

        if (!brush.enableDrawing) return; // If drawing is not enabled, don't process

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Visualize the placement position with preview
            Handles.color = Color.green;

            // Calculate the preview position based on the hit and the random offset range
            Vector3 previewPosition = hit.point;

            if (brush.randomOffsetRange != Vector3.zero)
            {
                previewPosition += new Vector3(
                    Random.Range(-brush.randomOffsetRange.x, brush.randomOffsetRange.x),
                    Random.Range(-brush.randomOffsetRange.y, brush.randomOffsetRange.y),
                    Random.Range(-brush.randomOffsetRange.z, brush.randomOffsetRange.z)
                );
            }

            // Adjust the preview position based on the parent's scale if a parent exists
            if (brush.parentTransform != null)
            {
                previewPosition = brush.parentTransform.TransformPoint(previewPosition);
            }

            // Visualize placement with wireframe or a placeholder preview model
            Handles.DrawWireDisc(previewPosition, hit.normal, 0.5f);

            // Optional: Create a temporary preview of the model (scaled)
            if (brush.modelsToPlace.Count > 0)
            {
                GameObject previewModel = brush.modelsToPlace[Random.Range(0, brush.modelsToPlace.Count)];

                // Create a temporary preview instance
                GameObject tempPreviewInstance = PrefabUtility.InstantiatePrefab(previewModel) as GameObject;
                tempPreviewInstance.transform.position = previewPosition;
                tempPreviewInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                // Apply the parent's scale to the preview model
                if (brush.parentTransform != null)
                {
                    tempPreviewInstance.transform.localScale = Vector3.Scale(tempPreviewInstance.transform.localScale, brush.parentTransform.localScale);
                }

                // Hide the preview object once the operation is done
                DestroyImmediate(tempPreviewInstance);
            }
        }

        // Handle mouse click or drag to place the model
        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log($"collided obj: {hit.collider.gameObject.name}, hit point: {hit.point}");
                PlaceModel(brush, hit); // Place the model at the hit location
                e.Use();
            }
        }
    }

    private void PlaceModel(CustomBrush brush, RaycastHit hit)
    {
        if (brush.modelsToPlace.Count == 0) return;
        
        // Check if enough time has passed since the last placement
        if (Time.time - lastPlacementTime < brush.placementCooldown)
            return; // Skip placement if we're still within cooldown

        // Select a random model from the list
        GameObject selectedModel = brush.modelsToPlace[Random.Range(0, brush.modelsToPlace.Count)];

        // Calculate the base position based on the hit point
        Vector3 position = hit.point;

        // Apply a random offset (added to the hit position, not replacing it)
        if (brush.randomOffsetRange != Vector3.zero)
        {
            position += new Vector3(
                Random.Range(-brush.randomOffsetRange.x, brush.randomOffsetRange.x),
                Random.Range(-brush.randomOffsetRange.y, brush.randomOffsetRange.y),
                Random.Range(-brush.randomOffsetRange.z, brush.randomOffsetRange.z)
            );
        }

        // If the parent exists, transform the position from world space to local space of the parent
        // if (brush.parentTransform != null)
        // {
        //     // Convert world space position to parent's local space
        //     position = brush.parentTransform.InverseTransformPoint(position);
        // }

        // Calculate rotation based on surface normal
        Quaternion rotation = Quaternion.identity;
        if (brush.enableSurfaceSnapping)
        {
            rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        // Look-at behavior ignoring Y-axis
        if (brush.enableLookAt && brush.lookAtTarget != null)
        {
            Vector3 targetPosition = brush.lookAtTarget.position;
            targetPosition.y = position.y; // Match the Y level of the model
            Vector3 directionToTarget = (targetPosition - position).normalized;
            Quaternion lookAtRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Combine surface alignment and look-at rotation
            rotation = Quaternion.RotateTowards(rotation, lookAtRotation, 360f);
        }

        // Instantiate the model
        GameObject instance = PrefabUtility.InstantiatePrefab(selectedModel) as GameObject;
        instance.transform.position = position;
        instance.transform.rotation = rotation;

        // Set the parent
        if (brush.parentTransform != null)
        {
            instance.transform.SetParent(brush.parentTransform);
        }

        // Apply the specified scale to the model after parenting
        instance.transform.localScale = brush.specifiedScale;

        // Register undo for editor
        Undo.RegisterCreatedObjectUndo(instance, "Place Model");
        
        // Update the last placement time
        lastPlacementTime = Time.time;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default inspector

        // Drawing state toggle
        CustomBrush brush = (CustomBrush)target;
        if (GUILayout.Button(brush.enableDrawing ? "Stop Drawing" : "Start Drawing"))
        {
            brush.enableDrawing = !brush.enableDrawing;
        }
    }
}
