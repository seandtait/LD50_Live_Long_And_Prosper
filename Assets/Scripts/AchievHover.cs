using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AchievHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI achievText;
    [SerializeField] private Player player;
    [SerializeField] private int achievIndex;

    private void Start()
    {
        achievText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (player.achievements[achievIndex].activeSelf)
        {
            // Show
            achievText.text = player.achievements[achievIndex].name;
        } else
        {
            // ???
            achievText.text = "???";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        achievText.text = "";
    }
}
