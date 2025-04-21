# MagUnityProject
 
# Prerequisites to run lib
- Must be running Unity LTS 2022.3.53 (Download found here: https://unity.com/releases/editor/whats-new/2022.3.53)
- Library relies on NewtonSoft Json dll, therefore this must be imported to your project with your preferred method, since this package is no longer supported on unity asset page I have instead used the manual method of downloading the correct dll for my system (https://www.newtonsoft.com/json) and dragging this into the project at the asset level.

# Product Catalogue
## ```void Init(string jsonString)```
### Params
- ```string jsonString``` : (required) string which contains jsonData that will be used to initialise products and bundles in game. (Use sample json for example of valid json structure).

## ```void Init(StreamReader reader)```
### Params
- ```StreamReader reader``` : (required) StreamReader which contains pointer to a stream that contains valid json data to be deserialised into products and bundles. (Use sample of json for example of valid json structure).

## ```IEnumerable<PurchasableItem> SortBy(SortByType sortByType, bool ascending)```
### Params
- ```enum SortByType sortByType``` : (required) sortByType has 3 valid types currently "Price, Name, Description" this will sort based on these fields and return bundles and products in that order
- ```bool ascending``` : (optional) ascending has a default of true, if set to false you will get the sorted values in decending order
### Return
- ```IEnumerable<PurchasableItem>``` : the result is the sorted list of products and bundles as an IEnumerable.

## ```IEnumerable<PurchasableItem> SortBy(params string[] itemOrder)```
### Params
- ```string[] itemOrder``` : (optional variable length) This field is used to sort products and bundles by item type, i.e. {coins, gems, ticket} if the item type is not included in the list it'll automatically be put to the back of the array unsorted.
### Return
- ```IEnumerable<PurchasableItem>``` : the result is the sorted list of products and bundles as an IEnumerable.

## ```IEnumerable<PurchasableItem> FilterBy(params string[] filterItems)```
### Params
- ```string[] filterItems``` : (optional variable length) This field is used to filter products and bundles by item type, i.e. {coins, gems, ticket} if the item type is not included in the list it'll not be returned in the resulting IEnumerable
### Return
- ```IEnumerable<PurchasableItem>``` : the result is the filtered list of products and bundles as an IEnumerable.

## ```IEnumerable<PurchasableItem> CustomFilterBy(Func<PurchasableItem, bool> filterFunc)```
### Params
- ```Func<PurchasableItem, bool> filterFunc``` : (required) This field is used to filter purchasable items loaded in from the json passed in via Init, you must account for the products of Bundles being accessed differently to products, to see an example of this check the UI implementation provided in the sample scene
### Return
- ```IEnumerable<PurchasableItem>``` : the result is the filtered list of products and bundles as an IEnumerable.

## ```IEnumerable<PurchasableItem> CustomSortBy(Func<PurchasableItem, object> sortFunc, bool ascending)```
### Params
- ```Func<PurchasableItem, object> sortFunc``` : (required) This field is used to sort purchasable items loaded in from the json passed in via Init, you must account for the products of Bundles being accessed differently to products, to see an example of this check the UI implementation provided in the sample scene
- ```bool ascending``` : (optional) ascending has a default of true, if set to false you will get the sorted values in decending order
### Return
- ```IEnumerable<PurchasableItem>``` : the result is the sorted list of products and bundles as an IEnumerable.

