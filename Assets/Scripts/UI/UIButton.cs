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
        [SerializeField] private List<GraphicConfig> graphicConfigs;

        [Space(3)]
        [SerializeField] UnityEvent OnClick, OnHoverEnter, OnHoverExit;

        [System.Serializable]
        public class GraphicConfig
        {
            public Graphic graphic;
            public EventColors eventColors;
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

        private Dictionary<Graphic, Coroutine> activeCoroutines = new Dictionary<Graphic, Coroutine>();

      

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            foreach (var graphicConfig in graphicConfigs)
            {
                if (graphicConfig.graphic != null)
                {
                    if (activeCoroutines.ContainsKey(graphicConfig.graphic))
                    {
                        StopCoroutine(activeCoroutines[graphicConfig.graphic]);
                        activeCoroutines.Remove(graphicConfig.graphic);
                    }

                    Coroutine colorCoroutine = StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, graphicConfig.eventColors.clickColor, config.onClick.transitionDuration));
                    activeCoroutines.Add(graphicConfig.graphic, colorCoroutine);
                }
            }
            OnClick?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            foreach (var graphicConfig in graphicConfigs)
            {
                if (graphicConfig.graphic != null)
                {
                    if (activeCoroutines.ContainsKey(graphicConfig.graphic))
                    {
                        StopCoroutine(activeCoroutines[graphicConfig.graphic]);
                        activeCoroutines.Remove(graphicConfig.graphic);
                    }

                    Coroutine colorCoroutine = StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, graphicConfig.eventColors.enterColor, config.onEnter.transitionDuration));
                    activeCoroutines.Add(graphicConfig.graphic, colorCoroutine);
                }
            }
            OnHoverEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            foreach (var graphicConfig in graphicConfigs)
            {
                if (graphicConfig.graphic != null)
                {
                    if (activeCoroutines.ContainsKey(graphicConfig.graphic))
                    {
                        StopCoroutine(activeCoroutines[graphicConfig.graphic]);
                        activeCoroutines.Remove(graphicConfig.graphic);
                    }

                    Coroutine colorCoroutine = StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, graphicConfig.eventColors.exitColor, config.onExit.transitionDuration));
                    activeCoroutines.Add(graphicConfig.graphic, colorCoroutine);
                }
            }
            OnHoverExit?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            foreach (var graphicConfig in graphicConfigs)
            {
                if (graphicConfig.graphic != null)
                {
                    if (activeCoroutines.ContainsKey(graphicConfig.graphic))
                    {
                        StopCoroutine(activeCoroutines[graphicConfig.graphic]);
                        activeCoroutines.Remove(graphicConfig.graphic);
                    }

                    Coroutine colorCoroutine = StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, graphicConfig.eventColors.downColor, config.onEnter.transitionDuration));
                    activeCoroutines.Add(graphicConfig.graphic, colorCoroutine);
                }
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            foreach (var graphicConfig in graphicConfigs)
            {
                if (graphicConfig.graphic != null)
                {
                    if (activeCoroutines.ContainsKey(graphicConfig.graphic))
                    {
                        StopCoroutine(activeCoroutines[graphicConfig.graphic]);
                        activeCoroutines.Remove(graphicConfig.graphic);
                    }

                    Coroutine colorCoroutine = StartCoroutine(ColorTransition(graphicConfig.graphic, graphicConfig.graphic.color, graphicConfig.eventColors.upColor, config.onExit.transitionDuration));
                    activeCoroutines.Add(graphicConfig.graphic, colorCoroutine);
                }
            }
        }

        private IEnumerator ColorTransition(Graphic graphic, Color fromColor, Color toColor, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                graphic.color = Color.Lerp(fromColor, toColor, elapsedTime / duration);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            graphic.color = toColor;
        }
    }
}
