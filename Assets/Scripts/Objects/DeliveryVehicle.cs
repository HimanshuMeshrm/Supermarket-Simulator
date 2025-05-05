using UnityEngine;

public class DeliveryVehicle : Interactable
{
    [SerializeField] private float speed = 5f;

    public Inventory Inventory { get; private set; }
    public override void Interact(IInteractor interactor)
    {
        Player player = interactor as Player;



        foreach (var itemSpace in Inventory.Spaces)
        {
            if(player.Animator == null)
            {
                Debug.LogWarning("Player's cart is null.");
                return;
            }
            if (itemSpace.IsEmpty) continue;
            
            //ItemSpace item = player._currentCart
            Item item = itemSpace.Item;

        }
    }
}
