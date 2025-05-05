using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MNDRiN.UI
{
    public abstract class UIElement : MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] protected Transform targetTransform;
        [SerializeField] protected UIElementConfig config;

        private Coroutine scaleCoroutine;
        private Coroutine rotateCoroutine;

        protected virtual void Awake()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ApplyTransformEffects(config.onClick);
            PlaySound(config.clickSound);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            ApplyTransformEffects(config.onEnter);
            PlaySound(config.enterSound);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            ApplyTransformEffects(config.onExit);
            PlaySound(config.exitSound);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            ApplyTransformEffects(config.onClick);
            PlaySound(config.downSound);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            ApplyTransformEffects(config.onExit);
            PlaySound(config.upSound);
        }

        private Vector3 lastScaleTarget;
        private Vector3 lastRotateTarget;

        private void ApplyTransformEffects(UIElementConfig.EffectConfig effectConfig)
        {
            if (targetTransform == null) return;

            if (effectConfig.scale != lastScaleTarget)
            {
                if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
                scaleCoroutine = StartCoroutine(ScaleTransition(targetTransform.localScale, effectConfig.scale, effectConfig.scaleDuration));
                lastScaleTarget = effectConfig.scale;
            }

            if (effectConfig.rotation != lastRotateTarget)
            {
                if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
                rotateCoroutine = StartCoroutine(RotateTransition(targetTransform.rotation.eulerAngles, effectConfig.rotation, effectConfig.rotationDuration));
                lastRotateTarget = effectConfig.rotation;
            }
        }



        protected virtual void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }
        }
        protected IEnumerator ScaleTransition(Vector3 fromScale, Vector3 toScale, float duration)
        {
           // Debug.Log($"[ScaleCoroutine] START from: {fromScale} to: {toScale}, duration: {duration}");

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                targetTransform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
               // Debug.Log($"[ScaleCoroutine] Lerp: {targetTransform.localScale}");
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            targetTransform.localScale = toScale;
           // Debug.Log($"[ScaleCoroutine] END at: {targetTransform.localScale}");
        }

        protected IEnumerator RotateTransition(Vector3 fromRotation, Vector3 toRotation, float duration)
        {
           // Debug.Log($"[RotateCoroutine] START from: {fromRotation} to: {toRotation}, duration: {duration}");

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Vector3 newRotation = Vector3.Lerp(fromRotation, toRotation, elapsedTime / duration);
                targetTransform.rotation = Quaternion.Euler(newRotation);
              //  Debug.Log($"[RotateCoroutine] Lerp: {newRotation}");
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            targetTransform.rotation = Quaternion.Euler(toRotation);
           // Debug.Log($"[RotateCoroutine] END at: {targetTransform.rotation.eulerAngles}");
        }

    }
}
