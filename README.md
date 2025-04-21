# MagUnityProject
 
# Prerequisites to run lib
- Must be running Unity LTS 2022.3.53 (Download found here: https://unity.com/releases/editor/whats-new/2022.3.53)
- Library relies on NewtonSoft Json dll, therefore this must be imported to your project with your preferred method, since this package is no longer supported on unity asset page I have instead used the manual method of downloading the correct dll for my system (https://www.newtonsoft.com/json) and dragging this into the project at the asset level.
- Copy files over from "ProductCatalogue" folder

# Project demo
- Clone repo from https://github.com/DavidHoskins/MagUnityProject
- Open project in Unity LTS 2022.3.53
- Open scene "SampleScene" inside /Assets/Scenes/
- Run game scene and observe buttons interacting with API sorting and filtering
- To alter Json update the file located in /Assets/Resources/json_test.json
- To try for yourself checkout the example code used in UIScript.cs 

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

# Push goals / Future concepts
## Compile into dll
- For future projects compiling down to a dll would make for much easier importing into projects, code left as is for easier testing of my code.
## Items not in Json
- Currently items are not stored in Json because the functionality boiled down to just being names ("Gems", "Tickets", "Coins", etc.) however if this were to change it would be worth while creating a Item object in json which we could connect with the products on initialisation.
## Custom sorting & filtering incomplete
- While custom sorting and filter does work it basically just exposes the backend to the developers (hence the use of the names CustomFilterBy and CustomSortBy), it might be worth instead exposing more filtering & sorting options to the devs to allow them more freedom in filtering and sorting without exposing the backend as much, such things as allowing for filtering by fields which are determinded by class structure instead of implementation.
## Filtering & Sorting
- Currently this system only supports filtering or sorting this is due to the system using a reference to "_items" and creating IEnumerables based off of that, if instead we passed in an IEnumerable / List to the functions we could then allow for combining filtering and sorting in anyway the dev sees fit, I ignored this method however as I didn't think this would be needed for the currently implementation and exposed possible unintended bugs that might harm the stability of the end program.
