using UnityEngine;

public class Cart : Interactable
{
    public override void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent<Entity>(out var entity))
        {
            entity.HoldCart(this);
            IsInteractable = false;

            Debug.Log($"{entity.name} is Holding Cart {name}");
        }
    }
}
