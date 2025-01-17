using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Inventories
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "RPG/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] Item[] items;

        public Item GetItemReference(string itemId)
        {
            foreach(Item item in items)
            {
                if(item.ID == itemId)
                {
                    return item;
                }
            }
            return null;
        }

        public Item GetItemCopy(string itemId)
        {
            Item item = GetItemReference(itemId);
            if(item == null) return null;
            return item.GetCopy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            LoadItems();
        }

        private void OnEnable() 
        {
            EditorApplication.projectChanged += LoadItems;
        }

        private void OnDisable() 
        {
            EditorApplication.projectChanged -= LoadItems;
        }

        private void LoadItems()
        {
            items = FindAssetsByType<Item>("Assets/Items");

        }

        public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
        {
            string type = typeof(T).ToString().Replace("UnityEngine.", "");

            string[] guids;
            if(folders == null || folders.Length == 0)
            {
                guids = AssetDatabase.FindAssets("t:" + type);
            }
            else
            {
                guids = AssetDatabase.FindAssets("t:" + type, folders);
            }

            T[] assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            return assets;
        }
#endif
    }

}
