using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public static class AnimatorUtil
{
    public static void MoveItems(Transform item, Transform target, float duration, Action onComplete)
    {
        item.transform.DOMove(target.position, duration)
            .SetEase(Ease.InOutBounce)
            .OnComplete(() =>
            {
                item.transform.position = target.position;
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
    public static void SetPosition(this NavMeshAgent agent, Vector3 targetPosition)
    {
        Vector3 from = agent.transform.position;

        if (IsPathClear(from, targetPosition))
        {
            agent.Warp(targetPosition);
            return;
        }

        Vector3 fallback = FindNearbyClearPosition(targetPosition, from);

        if (IsPathClear(from, fallback))
        {
            agent.Warp(fallback);
            return;
        }

        if (NavMesh.SamplePosition(targetPosition, out var hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            return;
        }

        Debug.LogWarning("NavMeshHelper: No clear or nearby NavMesh position found. Using raw target position.");
        agent.transform.position = targetPosition;
    }
}