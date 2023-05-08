using System.Collections.Generic;
using System.Text;
using PlayFab.ServerModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Items
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Button _getInventoryButton;
        [SerializeField] private Button _grantRandomItemButton;
        [SerializeField] private Button _removeRandomItemButton;
        [SerializeField] private TextMeshProUGUI _inventoryText;


        private string _playerId;
        private const string Catalog = "Main";

        private PlayFabLogin _playFabLogin;
        private PlayFabGetInventory _playFabGetInventory;
        private PlayFabGrantItems _playFabGrantItems;
        private PlayFabRemoveItems _playFabRemoveItems;
        private List<string> _userItems;

        private void Start()
        {
            _userItems = new List<string>();
            AddListeners();
            CreatePlayFabServices();
            DoLogin();
        }

        private void CreatePlayFabServices()
        {
            _playFabLogin = new PlayFabLogin();
            _playFabLogin.OnSuccess += playerId => _playerId = playerId;

            _playFabGetInventory = new PlayFabGetInventory();
            _playFabGetInventory.OnSuccess += OnGetInventory;

            _playFabGrantItems = new PlayFabGrantItems();
            _playFabGrantItems.OnSuccess += OnGrantItems;

            _playFabRemoveItems = new PlayFabRemoveItems();
            _playFabRemoveItems.OnSuccess += OnRemoveItems;
        }

        private void UpdateInventoryText()
        {
            var text = new StringBuilder();
            foreach (var item in _userItems)
            {
                text.AppendLine($"{item}");
            }

            _inventoryText.SetText(text.ToString());
        }

        private void OnRemoveItems()
        {
            _userItems.RemoveAt(0);
            UpdateInventoryText();
        }

        private void OnGrantItems(List<GrantedItemInstance> result)
        {
            foreach (var grantedItemInstance in result)
            {
                _userItems.Add(grantedItemInstance.ItemInstanceId);
            }
            UpdateInventoryText();
        }

        private void OnGetInventory(List<ItemInstance> result)
        {
            foreach (var itemInstance in result)
            {
                _userItems.Add(itemInstance.ItemInstanceId);
            }
            UpdateInventoryText();
        }

        private void DoLogin()
        {
            _playFabLogin.Login();
        }

        private void AddListeners()
        {
            _getInventoryButton.onClick.AddListener(OnGetInventoryButtonPressed);
            _grantRandomItemButton.onClick.AddListener(OnGrantRandomItemButton);
            _removeRandomItemButton.onClick.AddListener(OnRemoveRandomItemButton);
        }

        private void OnRemoveRandomItemButton()
        {
            var revokeInventoryItems = new List<RevokeInventoryItem>
            {
                new RevokeInventoryItem
                {
                    PlayFabId = _playerId,
                    ItemInstanceId = _userItems[0]
                }
            };
            _playFabRemoveItems.RevokeItems(revokeInventoryItems);
        }

        private void OnGrantRandomItemButton()
        {
            var itemGrants = new List<ItemGrant>
            {
                new ItemGrant
                {
                    PlayFabId = _playerId,
                    ItemId = Random.Range(0, 100) % 2 == 0 ? "Item001" : "Item002",
                    Data = new Dictionary<string, string> {{"Key2", "Value2"}}
                }
            };
            _playFabGrantItems.GrantItems(Catalog, itemGrants);
        }

        private void OnGetInventoryButtonPressed()
        {
            _playFabGetInventory.GetInventory(_playerId);
        }
    }
}