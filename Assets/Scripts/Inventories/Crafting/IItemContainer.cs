namespace RPG.Inventories
{
    public interface IItemContainer 
    {
        int ItemCount(string itemID);
        bool HasItem(string itemID);
        Item RemoveItem(string itemID);
        bool RemoveItem(Item item);
        bool AddItem(Item item);
        bool CanAddItem(Item item, int amount = 1);
        void Clear();
    }
}

