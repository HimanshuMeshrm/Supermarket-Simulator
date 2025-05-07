using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;
    public UnityEvent OnTriggerStayEvent;



    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke();
    }
    public void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke();
    }
    public void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke();
    }
}
