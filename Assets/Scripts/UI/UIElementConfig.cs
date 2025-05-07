using UnityEngine;

namespace MNDRiN.UI
{
    [CreateAssetMenu(fileName = "UIElementConfig", menuName = "UI/ElementConfig")]
    public class UIElementConfig : ScriptableObject
    {
        [System.Serializable]
        public class EffectConfig
        {
            public Color color = Color.white;
            public Vector3 scale = Vector3.one;
            public Vector3 rotation = Vector3.zero;
            public float transitionDuration = 0.2f;
            public float scaleDuration = 0.2f;
            public float rotationDuration = 0.2f;
        }

        public EffectConfig onEnter;
        public EffectConfig onExit;
        public EffectConfig onClick;

        public AudioClip clickSound;
        public AudioClip enterSound;
        public AudioClip exitSound;
        public AudioClip downSound;
        public AudioClip upSound;
    }
}
