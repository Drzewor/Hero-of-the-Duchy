using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CharacterStats;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using RPG.Combat;
using RPG.Saving;

public class InventoryManager : MonoBehaviour ,ISaveable, IPredicateEvaluator
{
    [SerializeField] private EquipmentPanel equipmentPanel;
    [SerializeField] private StatPanel statPanel;
    [SerializeField] private ItemTooltip itemTooltip;
    [SerializeField] private Image draggableItem;
    [SerializeField] private PlayerWeaponHandler weaponHandler;
    [SerializeField] private DropItemArea dropItemArea;
    [SerializeField] private ItemDropper itemDropper;
    [SerializeField] private EquipmentMenager equipmentMenager;
    [SerializeField] public Character character;
    [SerializeField] public Inventory inventory;
    [SerializeField] public GameObject EquipmentStats;
    [SerializeField] public GameObject CharacterPanel;
    [SerializeField] public CraftingWindow CraftingWindow;
    [SerializeField] public GameObject QuestWindow;
    private BaseItemSlot dragItemSlot;
    private ItemContainer openItemContainer;

    private void OnValidate()
    {
        if(itemTooltip == null) itemTooltip = FindObjectOfType<ItemTooltip>();
    }

    private void Awake() 
    {
        statPanel.SetStats
        (
            character.MaxHealth, character.MaxMana, character.Strength, character.Dexterity, character.Charisma, character.WeaponArmour, character.MagicArmour, character.ArmourPiercing
        );
        statPanel.UpdateStatValues();

        // Setup Events:
        // Right click
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentsPanelRightClick;
        // Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        CraftingWindow.OnPointerEnterEvent += ShowTooltip;
        // Pointer Exit
        inventory.OnPointerExiEvent += HideTooltip;
        equipmentPanel.OnPointerExiEvent += HideTooltip;
        CraftingWindow.OnPointerExitEvent += HideTooltip;
        // Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        // EndDrag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;     
        // Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        // Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
        dropItemArea.OnDropEvent += DropItemOutsideUI;
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if(itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(this);

            if(usableItem.IsConsumable)
            {
                itemSlot.Amount--;
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentsPanelRightClick(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        if (itemTooltip.gameObject.activeSelf)
		{
			itemTooltip.HideTooltip();
		}
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            float mouseX = Mouse.current.position.x.ReadValue();
            float mousey = Mouse.current.position.y.ReadValue();
            draggableItem.transform.position = new Vector2(mouseX,mousey);
            draggableItem.gameObject.SetActive(true);
        }
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        float mouseX = Mouse.current.position.x.ReadValue();
        float mousey = Mouse.current.position.y.ReadValue();
        draggableItem.transform.position = new Vector2(mouseX,mousey);
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if(dragItemSlot == null) return;

        if(dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }
        else if(dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }

    }

    private void DropItemOutsideUI()
    {
        if (dragItemSlot == null) return;

        if(dragItemSlot is EquipmentSlot)
        {
            EquippableItem item = (EquippableItem)dragItemSlot.Item;
            item.Unequip(character);
            if(item is ArmorItem)
            {
                equipmentMenager.UnequipItem(item.EquipmentType);
            }
            else if(item.EquipmentType == EquipmentType.MainHand)
            {
                UnequipWeapon();
            }
            statPanel.UpdateStatValues();
        }

        itemDropper.DropItem(dragItemSlot.Item, dragItemSlot.Amount);
        dragItemSlot.Item = null;
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddableStack = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStack, dragItemSlot.Amount);

        dropItemSlot.Amount += stacksToAdd;
        dragItemSlot.Amount -= stacksToAdd;
    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
        EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

 		if (dropItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) 
            {
                dragEquipItem.Equip(character);
                if(dragEquipItem is ArmorItem)
                {
                    equipmentMenager.EquipItem((ArmorItem)dragEquipItem);
                }
                else if(dragEquipItem.EquipmentType == EquipmentType.MainHand)
                {
                    EquipWeapon(dragEquipItem);
                }
            }

            if(dropEquipItem is ArmorItem)
            {
                equipmentMenager.UnequipItem(dropEquipItem.EquipmentType);
            }
			else if (dropEquipItem != null) 
            {
                dropEquipItem.Unequip(character);
            }
		}
		if (dragItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null)
            {
                dragEquipItem.Unequip(character);
                if(dragEquipItem is ArmorItem)
                {
                    equipmentMenager.UnequipItem(dragEquipItem.EquipmentType);
                }
                else if(dragEquipItem.EquipmentType == EquipmentType.MainHand)
                {
                    UnequipWeapon();
                }
                
            } 
			if (dropEquipItem != null)
            {
                dropEquipItem.Equip(character);
                if(dropEquipItem is ArmorItem)
                {
                    equipmentMenager.EquipItem((ArmorItem)dropEquipItem);
                }
                else if(dropEquipItem.EquipmentType == EquipmentType.MainHand)
                {
                    EquipWeapon(dropEquipItem);
                }
            } 
		}
        statPanel.UpdateStatValues();

		Item draggedItem = dragItemSlot.Item;
		int draggedItemAmount = dragItemSlot.Amount;

		dragItemSlot.Item = dropItemSlot.Item;
		dragItemSlot.Amount = dropItemSlot.Amount;

		dropItemSlot.Item = draggedItem;
		dropItemSlot.Amount = draggedItemAmount;
    }

    public void Equip(EquippableItem item)
    {
        if(inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(character);
                    statPanel.UpdateStatValues();
                }
                item.Equip(character);
                EquipWeapon(item);
                statPanel.UpdateStatValues();
                if(item is ArmorItem)
                {
                    equipmentMenager.EquipItem((ArmorItem)item);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if(inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(character);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
            if(item.EquipmentType == EquipmentType.MainHand)
            {
                UnequipWeapon();
            }
            if(item is ArmorItem)
            {
                equipmentMenager.UnequipItem(item.EquipmentType);
            }
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void TransferToItemContainer(BaseItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if(item != null && openItemContainer.CanAddItem(item))
        {
            inventory.RemoveItem(item);
            openItemContainer.AddItem(item);
        }
    }

    private void TransferToInventory(BaseItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if(item != null && inventory.CanAddItem(item))
        {
            openItemContainer.RemoveItem(item);
            inventory.AddItem(item);
        }
    }

    public void OpenItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = itemContainer;

        inventory.OnRightClickEvent -= InventoryRightClick;
        inventory.OnRightClickEvent += TransferToItemContainer;

        itemContainer.OnRightClickEvent += TransferToInventory;

        itemContainer.OnPointerEnterEvent += ShowTooltip;
        itemContainer.OnPointerExiEvent += HideTooltip;
        itemContainer.OnBeginDragEvent += BeginDrag;
        itemContainer.OnEndDragEvent += EndDrag;
        itemContainer.OnDragEvent += Drag;
        itemContainer.OnDropEvent += Drop;
    }

    public void CloseItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = null;

        inventory.OnRightClickEvent += InventoryRightClick;
        inventory.OnRightClickEvent -= TransferToItemContainer;

        itemContainer.OnRightClickEvent -= TransferToInventory;

        itemContainer.OnPointerEnterEvent -= ShowTooltip;
        itemContainer.OnPointerExiEvent -= HideTooltip;
        itemContainer.OnBeginDragEvent -= BeginDrag;
        itemContainer.OnEndDragEvent -= EndDrag;
        itemContainer.OnDragEvent -= Drag;
        itemContainer.OnDropEvent -= Drop;
    }

    private void LoadEquipped(EquippableItem item)
    {
        EquippableItem previousItem;
        equipmentPanel.AddItem(item, out previousItem);
        if(previousItem != null)
        {
            previousItem.Unequip(character);
            previousItem.Destroy();
        }
        
        item.Equip(character);
        EquipWeapon(item);
        if(item is ArmorItem)
        {
            equipmentMenager.EquipItem((ArmorItem)item);
        }
        statPanel.UpdateStatValues();
    }

    private void EquipWeapon(EquippableItem item)
    {
        if(item.EquipmentType == EquipmentType.MainHand || item == null)
        {
            weaponHandler.EquipWeapon((WeaponItem)item);
            return;
        }
    }

    private void UnequipWeapon()
    {
        EquipWeapon(null);
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
        if(predicate != "HasItem") return null;
        foreach(string parameter in parameters)
        {
            if(!inventory.HasItem(parameter))
            {
                return false;
            }
        }
        return true;
    }

    public object CaptureState()
    {
        var saveData = new ItemContainerSaveData(equipmentPanel.equipmentSlots.Length);
        
        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = equipmentPanel.equipmentSlots[i];

            if(itemSlot.Item == null) 
            {
                saveData.SavedSlots[i] = null;
            }
            else
            {
                saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount);
            }
        }

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (ItemContainerSaveData)state;

        if(saveData == null) return;

        equipmentPanel.Clear();

        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = equipmentPanel.equipmentSlots[i];
            ItemSlotSaveData savedSlot = saveData.SavedSlots[i];

            if(savedSlot == null)
            {
                continue;
            }
            else
            {
                Item item = inventory.itemDatabase.GetItemCopy(savedSlot.ItemID);
                LoadEquipped((EquippableItem)item);
            }
        }
    }
}
