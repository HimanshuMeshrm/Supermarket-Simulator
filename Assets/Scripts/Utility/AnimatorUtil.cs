using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public static class AnimatorUtil
{
    public static void MoveItems(List<Transform> items, Transform target, float duration, Action onComplete)
    {
        int completed = 0;
        int total = items.Count;

        foreach (var item in items)
        {
            item.DOMove(target.position, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    item.SetParent(target);
                    item.localPosition = Vector3.zero;
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
        Transform item = from.RemoveItem();
        if (item == null)
        {
            onComplete?.Invoke();
            return;
        }

  
        AnimatorUtil.MoveItems(new List<Transform> { item }, receiver, moveDuration, () => {
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