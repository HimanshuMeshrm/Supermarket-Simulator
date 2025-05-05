using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MNDRiN.UI
{
    public class UIToggle : UIElement
    {
        [Space(5)]
        [SerializeField] private bool isOn;

        [Space(3)]
        [SerializeField] private List<UIButton.GraphicConfig> graphicConfigs;

        [Space(3)]
        [SerializeField] private RectTransform handle;
        [SerializeField] private Vector2 onPosition;
        [SerializeField] private Vector2 offPosition;
        [SerializeField] private float handleTransitionDuration = 0.15f;

        [Space(3)]
        [SerializeField] UnityEvent OnToggleOn, OnToggleOff;

        private Coroutine handleMoveRoutine;

       

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Toggle();
        }

        public void Toggle()
        {
            isOn = !isOn;
            Debug.Log("Toggled to: " + isOn);
            ApplyState();
            if (isOn) OnToggleOn?.Invoke();
            else OnToggleOff?.Invoke();
        }
      

        public void SetState(bool state, bool invokeEvents = false)
        {
            if (isOn == state) return;

            isOn = state;
            ApplyState();

            if (invokeEvents)
            {
                if (isOn) OnToggleOn?.Invoke();
                else OnToggleOff?.Invoke();
            }
        }

        private void ApplyState()
        {
            ApplyGraphicEffects();
            UpdateHandlePosition();
        }

        private void ApplyGraphicEffects()
        {
            if (graphicConfigs != null)
            {
                foreach (var graphicConfig in graphicConfigs)
                {
                    if (graphicConfig.graphic == null) continue;
                    Color toColor = isOn ? graphicConfig.eventColors.enterColor : graphicConfig.eventColors.exitColor;
                    StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, toColor, handleTransitionDuration));
                }
            }
        }

        private void UpdateHandlePosition()
        {
            if (handle == null) return;

            if (handleMoveRoutine != null)
                StopCoroutine(handleMoveRoutine);

            Vector2 target = isOn ? onPosition : offPosition;
            if (gameObject.activeInHierarchy)
            {
                handleMoveRoutine = StartCoroutine(HandleMove(handle, target, handleTransitionDuration));
            }
            else
            {
                handle.anchoredPosition = target;
            }

        }

        private IEnumerator HandleMove(RectTransform rect, Vector2 target, float duration)
        {
            RectTransform localHandle = rect;
            Vector2 start = localHandle.anchoredPosition;
            float t = 0f;
            while (t < duration)
            {
                localHandle.anchoredPosition = Vector2.Lerp(start, target, t / duration);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            localHandle.anchoredPosition = target;
            Debug.Log("Handle moved to: " + target);
        }

        private IEnumerator ColorTransition(Graphic graphic, Color fromColor, Color toColor, float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                graphic.color = Color.Lerp(fromColor, toColor, t / duration);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            graphic.color = toColor;
            Debug.Log("Color transitioned to: " + toColor);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                if (handle != null)
                {
                    handle.anchoredPosition = isOn ? onPosition : offPosition;
                    Debug.Log("Handle initial position set to: " + (isOn ? onPosition : offPosition));
                }

                if (graphicConfigs != null)
                {
                    foreach (var g in graphicConfigs)
                    {
                        if (g.graphic == null) continue;
                        g.graphic.color = isOn ? g.eventColors.enterColor : g.eventColors.exitColor;
                        Debug.Log("Graphic color set to: " + (isOn ? g.eventColors.enterColor : g.eventColors.exitColor));
                    }
                }
            }
        }

        public bool IsOn => isOn;
        
    }
}
