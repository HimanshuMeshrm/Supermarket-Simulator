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
        [SerializeField] protected Transform targetTransform; // Used for scaling and rotation
        [SerializeField] protected UIElementConfig config; // Configuration Scriptable Object

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

        protected virtual void ApplyTransformEffects(UIElementConfig.EffectConfig effectConfig)
        {
            if (targetTransform != null)
            {
                StopAllCoroutines();
                StartCoroutine(ScaleTransition(targetTransform.localScale, effectConfig.scale, effectConfig.scaleDuration));
                StartCoroutine(RotateTransition(targetTransform.rotation.eulerAngles, effectConfig.rotation, effectConfig.rotationDuration));
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
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                targetTransform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            targetTransform.localScale = toScale;
        }

        protected IEnumerator RotateTransition(Vector3 fromRotation, Vector3 toRotation, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Vector3 newRotation = Vector3.Lerp(fromRotation, toRotation, elapsedTime / duration);
                targetTransform.rotation = Quaternion.Euler(newRotation);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            targetTransform.rotation = Quaternion.Euler(toRotation);
        }
    }
}
