using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
     private Button _btn;
 
     private void Awake()
     {
         _btn = GetComponent<Button>();
     }
 
     public void OnPointerEnter(PointerEventData eventData)
     {
         _btn.OnSelect(eventData);
     }
 
     public void OnPointerExit(PointerEventData eventData)
    {
         _btn.OnDeselect(eventData);
    }
}
