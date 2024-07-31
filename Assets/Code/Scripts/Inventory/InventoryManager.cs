using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Singleton;
    public GameObject ItemContentImage;
    public Item[] Items;
    public GameObject ItemGame;
    public Transform ItemContentItem;
    public List<ItemInstance> Inventory { get; set; }
    public Text txtSlotInventory;
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;

        }
        else { }

    }

    public void SetTarget(GameObject target)
    {
        //currentTarget = target; // Đặt đối tượng Item
        if (ItemContentImage != null)
        {
            ItemContent itemContent1 = ItemContentImage.GetComponent<ItemContent>();
            if (itemContent1 != null)
            {
                itemContent1.HideArrow();
            }
        }

        ItemContentImage = target; // Đặt đối tượng quái vật hiện tại
        ItemContent itemContent2 = target.GetComponent<ItemContent>();
        if (itemContent2 != null)
        {
            itemContent2.ShowArrow();
            //itemContent2.OnPointerClick();
        }
    }
    private void Update()
    {
        txtSlotInventory.text = ItemContentItem.childCount + "/100";
    }
    private void OnEnable()
    {
        Load();
        UpdateInventory();
    }
    private void OnDisable()
    {
        DestroyItem();
    }
    void DestroyItem()
    {
        foreach (Transform remove in ItemContentItem)
        {
            Destroy(remove.gameObject);
        }
        
    }
    public void UpdateInventory()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(request, result =>
        {
            Inventory = result.Inventory;

            // Thực hiện các hành động khác sau khi lấy danh sách item thành công
            //Debug.LogError("item: " + result.Inventory);

        }, error =>
        {
            Debug.LogError("Lỗi khi lấy danh sách item: " + error.ErrorMessage);
        });
    }
    public void Load()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        GetCatalogItemsRequest catalogItemsRequest = new GetCatalogItemsRequest();
        GetCatalogItemsRequest catalogItemsRequestStone = new GetCatalogItemsRequest();
        GetCatalogItemsRequest catalogItemsRequestShop = new GetCatalogItemsRequest();
        GetCatalogItemsRequest catalogItemsRequestShopRuby = new GetCatalogItemsRequest();

        catalogItemsRequest.CatalogVersion = "Item";
        catalogItemsRequestStone.CatalogVersion = "Stone";
        catalogItemsRequestShop.CatalogVersion = "Shop";
        catalogItemsRequestShopRuby.CatalogVersion = "ShopRUBY";

        // Gửi yêu cầu lấy thông tin kho hàng của người chơi
        PlayFabClientAPI.GetUserInventory(request, result =>
        {
            List<ItemInstance> playerInventory = result.Inventory;

            // Gửi yêu cầu lấy thông tin mục trong Catalog với CatalogVersion là "PlayerGameShopGold"
            PlayFabClientAPI.GetCatalogItems(catalogItemsRequest, catalogResult =>
            {
                List<CatalogItem> catalogItems = catalogResult.Catalog;

                // Lặp qua mỗi mục trong kho hàng của người chơi
                foreach (Item editor in Items)
                {

                    foreach (ItemInstance playerItem in playerInventory)
                    {

                        if (editor.Iditem == playerItem.ItemId)
                        {
                            // Lặp qua mỗi mục trong Catalog
                            foreach (CatalogItem catalogItem in catalogItems)
                            {
                                // Kiểm tra xem mục trong kho hàng của người chơi có trong Catalog không
                                if (playerItem.ItemId == catalogItem.ItemId)
                                {


                                    GameObject o = Instantiate(ItemGame, ItemContentItem, false);
                                    ItemContent item = o.GetComponent<ItemContent>();
                                    item.txtcount.text = playerItem.RemainingUses.ToString();                                   
                                    item.ItemInstanceId = playerItem.ItemInstanceId;
                                    item.IdItem = playerItem.ItemId;
                                    item.NameItem = playerItem.DisplayName;
                                    item.IconItem.sprite = editor.sprite.sprite;

                                    if (playerItem.RemainingUses == null)
                                    {

                                    }
                                    else
                                    {
                                        item.count = (int)playerItem.RemainingUses;
                                    }
                                    
                                    item.DescriptionItem = catalogItem.Description;                                 
                                    // hạn sd
                                    item.Expiration = "Ngày tạo:" + playerItem.PurchaseDate.ToString();
                                }

                            }







                        }
                    }
                }

            }, catalogError =>
            {
                // Xử lý lỗi khi không thể lấy thông tin từ Catalog
            });
        }, error =>
        {
            // Xử lý lỗi khi không thể lấy thông tin kho hàng của người chơi
        });

        // Gửi yêu cầu lấy thông tin kho hàng của người chơi
        PlayFabClientAPI.GetUserInventory(request, result =>
        {
            List<ItemInstance> playerInventory = result.Inventory;

            // Gửi yêu cầu lấy thông tin mục trong Catalog với CatalogVersion là "PlayerGameShopGold"
            PlayFabClientAPI.GetCatalogItems(catalogItemsRequestStone, catalogResult =>
            {
                List<CatalogItem> catalogItems = catalogResult.Catalog;

                // Lặp qua mỗi mục trong kho hàng của người chơi
                foreach (Item editor in Items)
                {

                    foreach (ItemInstance playerItem in playerInventory)
                    {

                        if (editor.Iditem == playerItem.ItemId)
                        {
                            // Lặp qua mỗi mục trong Catalog
                            foreach (CatalogItem catalogItem in catalogItems)
                            {
                                // Kiểm tra xem mục trong kho hàng của người chơi có trong Catalog không
                                if (playerItem.ItemId == catalogItem.ItemId)
                                {


                                    GameObject o = Instantiate(ItemGame, ItemContentItem, false);
                                    ItemContent item = o.GetComponent<ItemContent>();
                                    item.txtcount.text = playerItem.RemainingUses.ToString();
                                    item.ItemInstanceId = playerItem.ItemInstanceId;
                                    item.IdItem = playerItem.ItemId;
                                    item.NameItem = playerItem.DisplayName;

                                    if (playerItem.RemainingUses == null)
                                    {

                                    }
                                    else
                                    {
                                        item.count = (int)playerItem.RemainingUses;
                                    }

                                    item.DescriptionItem = catalogItem.Description;
                                    // hạn sd
                                    item.Expiration = "Ngày tạo:" + playerItem.PurchaseDate.ToString();
                                }

                            }

                        }
                    }
                }
                

            }, catalogError =>
            {
                // Xử lý lỗi khi không thể lấy thông tin từ Catalog
            });
        }, error =>
        {
            // Xử lý lỗi khi không thể lấy thông tin kho hàng của người chơi
        });

        //Shop
        // Gửi yêu cầu lấy thông tin kho hàng của người chơi
        PlayFabClientAPI.GetUserInventory(request, result =>
        {
            List<ItemInstance> playerInventory = result.Inventory;

            // Gửi yêu cầu lấy thông tin mục trong Catalog với CatalogVersion là "PlayerGameShopGold"
            PlayFabClientAPI.GetCatalogItems(catalogItemsRequestShop, catalogResult =>
            {
                List<CatalogItem> catalogItems = catalogResult.Catalog;

                // Lặp qua mỗi mục trong kho hàng của người chơi
                foreach (Item editor in Items)
                {

                    foreach (ItemInstance playerItem in playerInventory)
                    {

                        if (editor.Iditem == playerItem.ItemId)
                        {
                            // Lặp qua mỗi mục trong Catalog
                            foreach (CatalogItem catalogItem in catalogItems)
                            {
                                // Kiểm tra xem mục trong kho hàng của người chơi có trong Catalog không
                                if (playerItem.ItemId == catalogItem.ItemId)
                                {


                                    GameObject o = Instantiate(ItemGame, ItemContentItem, false);
                                    ItemContent item = o.GetComponent<ItemContent>();
                                    item.txtcount.text = playerItem.RemainingUses.ToString();
                                    item.ItemInstanceId = playerItem.ItemInstanceId;
                                    item.IdItem = playerItem.ItemId;
                                    item.NameItem = playerItem.DisplayName;

                                    if (playerItem.RemainingUses == null)
                                    {

                                    }
                                    else
                                    {
                                        item.count = (int)playerItem.RemainingUses;
                                    }

                                    item.DescriptionItem = catalogItem.Description;
                                    // hạn sd
                                    item.Expiration = "Ngày tạo:" + playerItem.PurchaseDate.ToString();
                                }

                            }

                        }
                    }
                }

            }, catalogError =>
            {
                // Xử lý lỗi khi không thể lấy thông tin từ Catalog
            });
        }, error =>
        {
            // Xử lý lỗi khi không thể lấy thông tin kho hàng của người chơi
        });


        //ShopRUBY
        // Gửi yêu cầu lấy thông tin kho hàng của người chơi
        PlayFabClientAPI.GetUserInventory(request, result =>
        {
            List<ItemInstance> playerInventory = result.Inventory;

            // Gửi yêu cầu lấy thông tin mục trong Catalog với CatalogVersion là "PlayerGameShopGold"
            PlayFabClientAPI.GetCatalogItems(catalogItemsRequestShopRuby, catalogResult =>
            {
                List<CatalogItem> catalogItems = catalogResult.Catalog;

                // Lặp qua mỗi mục trong kho hàng của người chơi
                foreach (Item editor in Items)
                {

                    foreach (ItemInstance playerItem in playerInventory)
                    {

                        if (editor.Iditem == playerItem.ItemId)
                        {
                            // Lặp qua mỗi mục trong Catalog
                            foreach (CatalogItem catalogItem in catalogItems)
                            {
                                // Kiểm tra xem mục trong kho hàng của người chơi có trong Catalog không
                                if (playerItem.ItemId == catalogItem.ItemId)
                                {


                                    GameObject o = Instantiate(ItemGame, ItemContentItem, false);
                                    ItemContent item = o.GetComponent<ItemContent>();
                                    item.txtcount.text = playerItem.RemainingUses.ToString();
                                    item.ItemInstanceId = playerItem.ItemInstanceId;
                                    item.IdItem = playerItem.ItemId;
                                    item.NameItem = playerItem.DisplayName;

                                    if (playerItem.RemainingUses == null)
                                    {

                                    }
                                    else
                                    {
                                        item.count = (int)playerItem.RemainingUses;
                                    }

                                    item.DescriptionItem = catalogItem.Description;
                                    // hạn sd
                                    item.Expiration = "Ngày tạo:" + playerItem.PurchaseDate.ToString();
                                }

                            }

                        }
                    }
                }

            }, catalogError =>
            {
                // Xử lý lỗi khi không thể lấy thông tin từ Catalog
            });
        }, error =>
        {
            // Xử lý lỗi khi không thể lấy thông tin kho hàng của người chơi
        });
    }
}
