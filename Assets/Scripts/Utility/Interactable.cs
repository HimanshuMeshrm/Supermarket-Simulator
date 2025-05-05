using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private Transform UIInteract;

    public string InteractionPrompt => interactionPrompt;
    public bool IsInteractable { get; protected set; } = true;
    protected IInteractor CurrentInteractor { get; private set; }

    protected virtual void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError($"{gameObject.name}: Missing Collider.");
            return;
        }

        col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsInteractable) return;

        if (other.TryGetComponent<IInteractor>(out var interactor))
        {
            if (CurrentInteractor != null)
            {
                Debug.LogWarning($"{gameObject.name}: Already interacting with {CurrentInteractor}");
            }


            CurrentInteractor = interactor;
            interactor.SetInteractable(this);
            if(interactor is Player player)
            {
                UIManager.Instance.SetRestockUI(this as Shelve);
            }
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
            if (interactor is Player player)
            {
                UIManager.Instance.HideRestock();
            }
            Debug.Log($"[Interactable] {gameObject.name} - {interactor} has exited the trigger area.");
        }
        else
        {
            Debug.LogWarning($"[Interactable] {gameObject.name} - Exit triggered by an unrecognized object or different interactor.");
        }
        
    }

    public void TryInteract()
    {
        Debug.Log($"[Interactable] {gameObject.name} - IsInteractable: {IsInteractable}");

        if (IsInteractable && CurrentInteractor != null)
        {
            Debug.Log($"[Interactable] {gameObject.name} - Interaction started with {CurrentInteractor.GetType().Name}");
            Debug.Log($"[Interactable] {gameObject.name} - CurrentInteractor: {CurrentInteractor}");
            Interact(CurrentInteractor);
            Debug.Log($"[Interactable] {gameObject.name} - Interaction completed.");
        }
        else
        {
            if (!IsInteractable)
            {
                Debug.LogWarning($"[Interactable] {gameObject.name} - Cannot interact: Object is not interactable.");
            }

            if (CurrentInteractor == null)
            {
                Debug.LogWarning($"[Interactable] {gameObject.name} - Cannot interact: No interactor assigned.");
            }
        }
    }

    public void SetInteractor(IInteractor interactor)
    {
        CurrentInteractor = interactor;
    }

    public virtual void OnFocus()
    {
        if (UIInteract != null)
        {
            UIInteract.gameObject.SetActive(true);
            Debug.Log($"[Interactable] {gameObject.name} - UI Prompt visible.");
        }
    }

    public virtual void OnUnfocus()
    {
        if (UIInteract != null)
        {
            UIInteract.gameObject.SetActive(false);
            Debug.Log($"[Interactable] {gameObject.name} - UI Prompt hidden.");
        }
    }

    public abstract void Interact(IInteractor interactor);
}

public interface IInteractor
{
    void SetInteractable(Interactable interactable);
    bool IsInteracting { get; set; }
    void Interact();
}
