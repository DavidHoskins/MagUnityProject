using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

class ProductCatalogue
{
    private List<PurchasableItem> m_items = new List<PurchasableItem>();
    public enum SortByType  {Price, Name, Description};

    public void Init(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogError("JSON string is null or empty.");
            return;
        }
        FromJson(jsonString);
    }

    public void Init(StreamReader reader)
    {
        if (reader == null)
        {
            Debug.LogError("StreamReader is null.");
            return;
        }
        string jsonString = reader.ReadToEnd();
        FromJson(jsonString);
    }

    private void FromJson(string jsonString)
    {
        JsonAdaptor jsonAdaptor = JsonConvert.DeserializeObject<JsonAdaptor>(jsonString);
        if(jsonAdaptor != null)
        {
            if(jsonAdaptor.Bundles != null)
            {
                m_items.AddRange(jsonAdaptor.Bundles);
            }
            if (jsonAdaptor.Products != null)
            {
                m_items.AddRange(jsonAdaptor.Products);
            }
        }
    }

    public IEnumerable<PurchasableItem> SortBy(SortByType sortByType, bool ascending = true)
    {
        switch (sortByType)
        {
            case SortByType.Name:
                return ascending ? m_items.OrderBy(item => item.Name) : m_items.OrderBy(item => item.Name).Reverse();
            case SortByType.Price:
                return ascending ? m_items.OrderBy(item => item.Price) : m_items.OrderBy(item => item.Price).Reverse();
            case SortByType.Description:
                return ascending ? m_items.OrderBy(item => item.Description) : m_items.OrderBy(item => item.Description).Reverse();
            default:
                Debug.LogWarning("SortByType is undefined. Returning unsorted items.");
                return m_items;
        }
    }

    public IEnumerable<PurchasableItem> SortBy(params string[] itemOrder)
    {
        if (itemOrder.Length == 0)
        {
            Debug.LogWarning("No item order provided. Returning unsorted items.");
            return m_items;
        }

        IOrderedEnumerable<PurchasableItem> sortedItems = m_items.OrderBy(item => { return sortItemsFunc(item, itemOrder); });
        return sortedItems;
    }

    private int sortItemsFunc(PurchasableItem item, params string[] itemOrder)
    {
        // Sort for bundles
        if(item is Bundle bundle)
        {
            foreach (Product product in bundle.Products)
            {
                int bundleIndex = Array.IndexOf(itemOrder, product.Name);
                if(bundleIndex != -1)
                    return bundleIndex;
            }
            return itemOrder.Count() + 1; // If not found, place at the end
        }

        // Sort for products
        int index = Array.IndexOf(itemOrder, item.Name);
        if (index == -1)
            return itemOrder.Count() + 1; // If not found, place at the end
        return index;
    }

    public IEnumerable<PurchasableItem> FilterBy(params string[] filterItems)
    {
        if (filterItems.Length == 0)
        {
            Debug.LogWarning("No item order provided. Returning unsorted items.");
            return m_items;
        }

        IEnumerable<PurchasableItem> filteredItems = m_items.Where(item => { return filterItemsFunc(item, filterItems); });

        return filteredItems;
    }

    private bool filterItemsFunc(PurchasableItem item, params string[] filterItems)
    {
        if(item is Bundle bundle)
        {
            foreach (Product product in bundle.Products)
            {
                if (filterItems.Contains(product.Name))
                    return true;
            }
            return false; // If no products in the bundle match, exclude the bundle
        }
        return filterItems.Contains(item.Name);
    }

    public IEnumerable<PurchasableItem> CustomFilterBy(Func<PurchasableItem, bool> filterFunc, bool ascending = true)
    {
        return ascending ? m_items.Where(filterFunc) : m_items.Where(filterFunc).Reverse();
    }

    public IEnumerable<PurchasableItem> CustomSortBy(Func<PurchasableItem, object> sortFunc, bool ascending = true)
    {
        IOrderedEnumerable<PurchasableItem> sortedItems = m_items.OrderBy(sortFunc);
        return ascending ? sortedItems : sortedItems.Reverse();
    }
}