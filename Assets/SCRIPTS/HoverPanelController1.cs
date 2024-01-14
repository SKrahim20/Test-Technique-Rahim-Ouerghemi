using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverPanel;

    void Start()
    {       
        hoverPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {      
        hoverPanel.SetActive(false);
    }
}
