using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestinationTrigger : MonoBehaviour
{
    public bool HasReachedDestination { get; private set; }
    private string objectName;
    private Collider Collider;

    
    private void Awake()
    {
        if(Collider == null)
        {
            Collider = GetComponent<Collider>();
            Collider.isTrigger = true;
        }
    }
    public void SetDestination(Vector3 targetPosition, string objectName)
    {
        transform.SetParent(null);
        transform.position = targetPosition;
        this.objectName = objectName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == objectName)
        {
            HasReachedDestination = true;
        }
    }
}
