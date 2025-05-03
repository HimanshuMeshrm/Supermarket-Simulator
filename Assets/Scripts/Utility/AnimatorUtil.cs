using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public static class AnimatorUtil
{
    public static void MoveItems(List<Item> items, Transform target, float duration, Action onComplete)
    {
        int completed = 0;
        int total = items.Count;

        foreach (var item in items)
        {
            Vector3 endPosition = target.position;

            item.transform.DOMove(endPosition, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    item.transform.position = endPosition;
                    completed++;
                    if (completed >= total)
                        onComplete?.Invoke();
                });
        }
    }

}
public static class InventoryHelper
{
    public static void TransferItem(Inventory from, Inventory to, Transform receiver, float moveDuration, Action onComplete = null)
    {
        Item item = from.RemoveItem().GetComponent<Item>();
        if (item == null)
        {
            onComplete?.Invoke();
            return;
        }

        
        AnimatorUtil.MoveItems(new List<Item> { item }, receiver, moveDuration, () => {
            to.AddItem(item);
            onComplete?.Invoke();
        });
    }
}

public static class NavMeshHelper
{
    public static bool IsPathClear(Vector3 startPosition, Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(startPosition, targetPosition, NavMesh.AllAreas, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }

    public static Vector3 FindNearbyClearPosition(Vector3 target, Vector3 from, float searchRadius = 1.5f, int steps = 16)
    {
        for (int i = 0; i < steps; i++)
        {
            float angle = (360f / steps) * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * searchRadius;
            Vector3 candidate = target + offset;

            if (IsPathClear(from, candidate))
            {
                return candidate;
            }
        }

        return target; // fallback to original
    }

}