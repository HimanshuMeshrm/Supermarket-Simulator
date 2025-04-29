using UnityEngine;

public class TransformGizmoDrawer : MonoBehaviour
{
    public enum GizmoType { WireCube, WireSphere, SolidCube, SolidSphere, Icon }

    [Header("Gizmo Settings")]
    public GizmoType gizmoType = GizmoType.WireCube;
    public Color gizmoColor = Color.yellow;
    public Vector3 gizmoSize = Vector3.one;

    [Header("Icon (only for GizmoType.Icon)")]
    public string iconName = "sv_label_0"; // Built-in icon name like: sv_label_0 to sv_label_6

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        switch (gizmoType)
        {
            case GizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position, gizmoSize);
                break;
            case GizmoType.SolidCube:
                Gizmos.DrawCube(transform.position, gizmoSize);
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position, gizmoSize.x * 0.5f);
                break;
            case GizmoType.SolidSphere:
                Gizmos.DrawSphere(transform.position, gizmoSize.x * 0.5f);
                break;
            case GizmoType.Icon:
#if UNITY_EDITOR
                UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, name);
                UnityEditor.Handles.BeginGUI();
                UnityEditor.Handles.EndGUI();
#endif
                break;
        }
    }
}
