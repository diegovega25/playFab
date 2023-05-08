using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ServerModels;
using UnityEngine;

namespace Code.Items
{
    public class PlayFabGetInventory
    {
        public event Action<List<ItemInstance>> OnSuccess;

        public void GetInventory(string userId)
        {
            var request = new GetUserInventoryRequest
            {
                PlayFabId = userId
            };
            PlayFabServerAPI
                .GetUserInventory(request,
                    OnGetUserInventorySuccess,
                    OnGetFailure
                );
        }

        private void OnGetUserInventorySuccess(GetUserInventoryResult result)
        {
            Debug.Log("OnGetUserInventorySuccess");
            OnSuccess?.Invoke(result.Inventory);
        }


        private void OnGetFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }
    }
}