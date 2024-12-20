using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace RPG.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TMP_Text itemNameText;
        [SerializeField] TMP_Text itemTypeText;
        [SerializeField] TMP_Text itemDescriptionText;


        public void ShowTooltip(Item item)
        {
            itemNameText.text = item.ItemName;
            itemTypeText.text = item.GetItemType();
            itemDescriptionText.text = item.GetDescription();

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}
