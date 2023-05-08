using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ServerModels;
using UnityEngine;

namespace Code.Items
{
    public class PlayFabRemoveItems
    {
        public event Action OnSuccess;

        public void RevokeItems(List<RevokeInventoryItem> itemsToRevoke)
        {
            var request = new RevokeInventoryItemsRequest
            {
                Items = itemsToRevoke
            };
            PlayFabServerAPI
                .RevokeInventoryItems(request,
                    OnRevokeInventoryItemsSuccess,
                    OnGetFailure
                );
        }

        private void OnRevokeInventoryItemsSuccess(RevokeInventoryItemsResult result)
        {
            Debug.Log("OnGetUserInventorySuccess");
            OnSuccess?.Invoke();
        }


        private void OnGetFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }
    }
}