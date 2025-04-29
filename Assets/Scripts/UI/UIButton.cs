using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MNDRiN.UI
{
    public class UIButton : UIElement
    {
        [Space(5)]
        [SerializeField] private List<GraphicConfig> graphicConfigs; // List of specific graphic configurations

        [Space(3)]
        [SerializeField] UnityEvent OnClick, OnHoverEnter, OnHoverExit;


        [System.Serializable]
        public class GraphicConfig
        {
            public Graphic graphic; // The graphic component (Image, Text, etc.)
            public EventColors eventColors; // Color configurations for different events
        }

        [System.Serializable]
        public class EventColors
        {
            public Color enterColor;
            public Color exitColor;
            public Color clickColor;
            public Color downColor;
            public Color upColor;
        }

        protected override void ApplyTransformEffects(UIElementConfig.EffectConfig effectConfig)
        {
            base.ApplyTransformEffects(effectConfig); // Apply base scale and rotation effects
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            ApplyGraphicEffects(g => g.eventColors.clickColor, config.onClick.transitionDuration);
            PlaySound(config.clickSound);
            OnClick?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            ApplyGraphicEffects(g => g.eventColors.enterColor, config.onEnter.transitionDuration);
            PlaySound(config.enterSound);
            OnHoverEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ApplyGraphicEffects(g => g.eventColors.exitColor, config.onExit.transitionDuration);
            PlaySound(config.exitSound);
            OnHoverExit?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            ApplyGraphicEffects(g => g.eventColors.downColor, config.onEnter.transitionDuration);
            PlaySound(config.downSound);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            ApplyGraphicEffects(g => g.eventColors.upColor, config.onExit.transitionDuration);
            PlaySound(config.upSound);
        }

        private void ApplyGraphicEffects(System.Func<GraphicConfig, Color> getColor, float transitionDuration)
        {
            if (graphicConfigs != null && graphicConfigs.Count > 0)
            {
                foreach (var graphicConfig in graphicConfigs)
                {
                    if (graphicConfig.graphic != null)
                    {
                        StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, getColor(graphicConfig), transitionDuration));
                    }
                }
            }
        }

        private IEnumerator ColorTransition(Graphic graphic, Color fromColor, Color toColor, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                graphic.color = Color.Lerp(fromColor, toColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            graphic.color = toColor;
        }
    }
}
