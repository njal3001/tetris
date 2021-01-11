using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEnterScaling : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startScale;

    [SerializeField]
    private float scaleFactor = 1.1f;

    private void Awake() => startScale = transform.localScale;

    public void OnPointerEnter(PointerEventData eventData) => transform.localScale = startScale * scaleFactor;

    public void OnPointerExit(PointerEventData eventData) => OnDisable();

    private void OnDisable() => transform.localScale = startScale;

}
