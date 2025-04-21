using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class UIScript : MonoBehaviour
{
    public GameObject productPrefab;
    public GameObject bundlePrefab;
    public Transform canvas;

    public GameObject ButtonPrefab;

    private ProductCatalogue productCatalogue;

    private IEnumerable<PurchasableItem> currentProductList;

    void Awake()
    {
        productCatalogue = new ProductCatalogue();
        StreamReader reader = new StreamReader("Assets/Resources/test_json.json");
        productCatalogue.Init(reader: reader);
        reader.Close();
    }

    void Start()
    {
        string[] itemOrder = { "Gems", "Coins" };
        currentProductList = productCatalogue.SortBy(itemOrder: itemOrder);
        GenerateProductUI();
        GenerateButtonUI();
    }

    void DestroyUI()
    {
        for (int i = canvas.childCount - 1; i >= 0; i--)
        {
            GameObject child = canvas.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    void GenerateButtonUI()
    {
        int buffer = 10;
        Vector2 uiPosition = new Vector2(0,0);
        GameObject button = GenerateButtonElement(ButtonPrefab, "Filter by Gems", uiPosition, () =>
        {
            string[] filterItems = { "Gems" };
            currentProductList = productCatalogue.FilterBy(filterItems: filterItems);
            DestroyUI();
            GenerateProductUI();
            GenerateButtonUI();
        });
        uiPosition.x += GetUIElementWidth(button) + buffer; // Adjust the x position for the next UI element
        GameObject otherButton = GenerateButtonElement(ButtonPrefab, "Sort by price", uiPosition, () =>
        {
            currentProductList = productCatalogue.SortBy(sortByType: ProductCatalogue.SortByType.Price, ascending: true);
            DestroyUI();
            GenerateProductUI();
            GenerateButtonUI();
        });
        uiPosition.x += GetUIElementWidth(otherButton) + buffer; // Adjust the x position for the next UI element
        GameObject thirdButton = GenerateButtonElement(ButtonPrefab, "Filter only Bundles", uiPosition, () =>
        {
            currentProductList = productCatalogue.CustomFilterBy(filterFunc: (item) =>
            {
                if (item is Bundle)
                {
                    return true;
                }
                return false;
            });
            DestroyUI();
            GenerateProductUI();
            GenerateButtonUI();
        });

        uiPosition.x += GetUIElementWidth(thirdButton) + buffer; // Adjust the x position for the next UI element
        GameObject fourthButton = GenerateButtonElement(ButtonPrefab, "Sort products before bundles", uiPosition, () =>
        {
            currentProductList = productCatalogue.CustomSortBy(sortFunc: (item) =>
            {
                if (item is Bundle bundle)
                {
                    return 1;
                }
                return 0;
            });
            DestroyUI();
            GenerateProductUI();
            GenerateButtonUI();
        });
    }

    GameObject GenerateButtonElement(GameObject buttonPrefab, string buttonText, Vector2 uiPosition, ButtonScript.ButtonClickHandler buttonClickHandler)
    {
        GameObject button = Instantiate(buttonPrefab, canvas);
        button.transform.localPosition = uiPosition;
        button.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = buttonText;
        button.GetComponent<ButtonScript>().buttonClickHandler = buttonClickHandler;
        return button;
    }

    float GetCanvasWidth()
    {
        RectTransform rectTransform = canvas.GetComponent<RectTransform>();
        return rectTransform.rect.width;
    }

    float GetCanvasHeight()
    {
        RectTransform rectTransform = canvas.GetComponent<RectTransform>();
        return rectTransform.rect.height;
    }

    float GetUIElementWidth(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return rectTransform.rect.width;
    }

    float GetUIElementHeight(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return rectTransform.rect.height;
    }

    void GenerateProductUI()
    {
        Vector2 uiPosition = new Vector2(-GetCanvasWidth() / 2, GetCanvasHeight() / 2);
        int buffer = 10; // Buffer space between UI elements

        // Adjust the initial position to start from the top left corner of the canvas
        uiPosition.y -= GetUIElementHeight(productPrefab) + buffer;
        uiPosition.x += GetUIElementWidth(productPrefab) + buffer; 
        foreach (PurchasableItem item in currentProductList)
        {
            Debug.Log(item.Name);
            GameObject uiGameObj = GenerateProductUIElement(item, uiPosition);
            uiPosition.x += GetUIElementWidth(uiGameObj) + buffer; // Adjust the x position for the next UI element
            if (uiPosition.x + GetUIElementWidth(uiGameObj) > GetCanvasWidth() / 2)
            {
                uiPosition.x = -GetCanvasWidth() / 2; // Reset x position
                uiPosition.y -= GetUIElementHeight(uiGameObj) + buffer; // Move down to the next row
            }

        }
    }

    GameObject GenerateProductUIElement(PurchasableItem item, Vector2 uiPosition)
    {
        GameObject uiElement = null;
        if (item is Product product)
        {
            uiElement = Instantiate(productPrefab, canvas);
            uiElement.transform.localPosition = uiPosition;
            uiElement.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = product.Name;
            uiElement.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = product.Price.ToString();
            uiElement.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = product.Description;
            uiElement.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = product.Quantity.ToString();
        }
        else if (item is Bundle bundle)
        {
            uiElement = Instantiate(bundlePrefab, canvas);
            uiElement.transform.localPosition = uiPosition;
            uiElement.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = bundle.Name;
            uiElement.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = bundle.Price.ToString();
            uiElement.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = bundle.Description;

            generateBundleUIText(uiElement, bundle, "Coins");
            generateBundleUIText(uiElement, bundle, "Gems");
            generateBundleUIText(uiElement, bundle, "Tickets");
        }
        return uiElement;
    }

    private void generateBundleUIText(GameObject uiElement, Bundle bundle, string fieldName)
    {
        Product product = bundle.Products.Find(p => p.Name == fieldName);
        TextMeshProUGUI textField = uiElement.transform.Find(fieldName).GetComponent<TextMeshProUGUI>();
        if(product == null)
        {
            textField.enabled = false;
            return;
        }
        textField.text = $"{product.Name}: {product.Quantity}";
    }
}
