using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMover : Singleton<SmoothMover>
{
    public enum EaseType { Linear, Smooth, Bounce, Elastic, Cartoon, Back, Punch, Shake, Custom }

    private readonly Dictionary<Transform, Coroutine> activeAnims = new();
    private readonly WaitForEndOfFrame wait = new();

    private void Awake()
    {
        _ = Instance;
    }
    public static void Move(Transform item, Transform target, float duration, Action onComplete = null)
    {
        Instance.InstancedMove(item, target, duration, EaseType.Smooth, true, false, false, false, Vector3.zero, null, onComplete);
    }

    public static void Move(Transform item, Transform target, float duration, EaseType ease, Action onComplete = null)
    {
        Instance.InstancedMove(item, target, duration, ease, true, false, false, false, Vector3.zero, null, onComplete);
    }

    public static void Move(
        Transform item,
        Transform target,
        float duration,
        EaseType ease = EaseType.Smooth,
        bool followTarget = true,
        bool animateRotation = false,
        bool animateScale = false,
        bool useLocalSpace = false,
        Vector3 positionOffset = default,
        AnimationCurve customCurve = null,
        Action onComplete = null)
    {
        Instance.InstancedMove(item, target, duration, ease, followTarget, animateRotation, animateScale, useLocalSpace, positionOffset, customCurve, onComplete);
    }

    private void InstancedMove(
        Transform item,
        Transform target,
        float duration,
        EaseType ease,
        bool followTarget,
        bool animateRotation,
        bool animateScale,
        bool useLocalSpace,
        Vector3 positionOffset,
        AnimationCurve customCurve,
        Action onComplete)
    {
        if (activeAnims.TryGetValue(item, out var running))
        {
            StopCoroutine(running);
            activeAnims.Remove(item);
        }

        var routine = StartCoroutine(Run(item, target, duration, ease, followTarget, animateRotation, animateScale, useLocalSpace, positionOffset, customCurve, () =>
        {
            activeAnims.Remove(item);
            onComplete?.Invoke();
        }));

        activeAnims[item] = routine;
    }

    private IEnumerator Run(
        Transform item,
        Transform target,
        float duration,
        EaseType ease,
        bool followTarget,
        bool animateRotation,
        bool animateScale,
        bool useLocalSpace,
        Vector3 offset,
        AnimationCurve custom,
        Action complete)
    {
        float time = 0f;
        Vector3 startPos = useLocalSpace ? item.localPosition : item.position;
        Quaternion startRot = item.rotation;
        Vector3 startScale = item.localScale;

        Vector3 GetTargetPos() => (useLocalSpace ? target.localPosition : target.position) + offset;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float eased = ease == EaseType.Custom && custom != null ? custom.Evaluate(t) : Ease(t, ease);

            Vector3 targetPos = followTarget ? GetTargetPos() : GetTargetPos();

            if (ease == EaseType.Shake)
            {
                Vector3 shake = UnityEngine.Random.insideUnitSphere * 0.1f;
                if (useLocalSpace)
                    item.localPosition = Vector3.LerpUnclamped(startPos, targetPos, eased) + shake;
                else
                    item.position = Vector3.LerpUnclamped(startPos, targetPos, eased) + shake;
            }
            else
            {
                if (useLocalSpace)
                    item.localPosition = Vector3.LerpUnclamped(startPos, targetPos, eased);
                else
                    item.position = Vector3.LerpUnclamped(startPos, targetPos, eased);
            }

            if (animateRotation)
                item.rotation = Quaternion.SlerpUnclamped(startRot, target.rotation, eased);

            if (animateScale)
                item.localScale = Vector3.LerpUnclamped(startScale, target.localScale, eased);

            yield return wait;
        }

        if (useLocalSpace)
            item.localPosition = GetTargetPos();
        else
            item.position = GetTargetPos();

        if (animateRotation) item.rotation = target.rotation;
        if (animateScale) item.localScale = target.localScale;
        
        if (!animateScale && item.localScale != startScale)
            Debug.LogWarning($"[SmoothMover] Scale changed unexpectedly on {item.name}");

        complete?.Invoke();
    }

    private float Ease(float t, EaseType type)
    {
        return type switch
        {
            EaseType.Linear => t,
            EaseType.Smooth => t * t * (3f - 2f * t),
            EaseType.Bounce => Mathf.Abs(Mathf.Sin(t * Mathf.PI * (2.2f - t))) * (1f - t) + t,
            EaseType.Elastic => Mathf.Sin(-13f * (t + 1f) * Mathf.PI / 2f) * Mathf.Pow(2f, -10f * t) + 1f,
            EaseType.Cartoon => Mathf.Sin(t * Mathf.PI * 0.5f) * (1.1f - 0.1f * Mathf.Cos(t * Mathf.PI * 4f)),
            EaseType.Back => t * t * (2.7f * t - 1.7f),
            EaseType.Punch => Mathf.Sin(t * Mathf.PI * 3f) * (1f - t),
            EaseType.Shake => t,
            _ => t,
        };
    }
}
