using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionPrompt = "Press E to interact";

    public string InteractionPrompt => interactionPrompt;
    public bool IsInteractable { get; protected set; } = true;
    protected IInteractor CurrentInteractor { get; private set; }

    protected virtual void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsInteractable) return;

        if (other.TryGetComponent<IInteractor>(out var interactor))
        {
            CurrentInteractor = interactor;
            interactor.SetInteractable(this);
            OnFocus();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!IsInteractable) return;

        if (other.TryGetComponent<IInteractor>(out var interactor) && interactor == CurrentInteractor)
        {
            interactor.SetInteractable(null);
            OnUnfocus();
            CurrentInteractor = null;
        }
    }

    public void TryInteract()
    {
        if (IsInteractable && CurrentInteractor != null)
            Interact((CurrentInteractor as Component).gameObject);
    }

    public virtual void OnFocus() { }
    public virtual void OnUnfocus() { }
    public abstract void Interact(GameObject interactor);
}

public interface IInteractor
{
    void SetInteractable(Interactable interactable);
    bool IsInteracting { get; set; }
    void Interact();
}

