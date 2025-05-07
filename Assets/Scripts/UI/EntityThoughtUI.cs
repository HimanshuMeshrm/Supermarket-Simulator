using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityThoughtUI : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private Image ThoughtIcon;
    [SerializeField] private TMP_Text ThoughtText;

    private Entity Entity;
    private Camera _camera;

    public void Follow(Entity entity)
    {
        this.Entity = entity;
    }

    public void SetIcon(Sprite image, string text = "")
    {
        ThoughtIcon.sprite = image;
        if (text == string.Empty)
        {
            ThoughtText.gameObject.SetActive(false);
        }
        else
        {
            ThoughtText.gameObject.SetActive(true);
            ThoughtText.text = text;
        }
    }

    private void Update()
    {
        if (_camera == null)
            _camera = Camera.main;

        if (Entity != null)
        {
            baseObject.transform.position = Entity.transform.position + Vector3.left + Vector3.up * 2.5f;


            Vector3 direction = _camera.transform.position - baseObject.transform.position;
            direction.y = 0;
            baseObject.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void ReturnToPool()
    {
        if (!Entity.gameObject.activeInHierarchy)
        {
            UIManager.Instance.ThoughtsUIPool.ReturnToPool(this.gameObject);
        }

    }
}
