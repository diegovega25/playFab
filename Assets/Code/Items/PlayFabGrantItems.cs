using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ServerModels;
using PlayFab;

namespace Code.Items
{
    public class PlayFabGrantItems
    {
        public event Action<List<GrantedItemInstance>> OnSuccess;

        // Tenemos que añadir la directiva: ENABLE_PLAYFABSERVER_API
        public void GrantItems(string catalogId, List<ItemGrant> itemGrant)
        {
            var request = new GrantItemsToUsersRequest
            {
                CatalogVersion = catalogId,
                ItemGrants = itemGrant,
            };
            PlayFabServerAPI
                .GrantItemsToUsers(request,
                    OnGrantSuccess,
                    OnGetFailure
                );
        }

        private void OnGrantSuccess(GrantItemsToUsersResult result)
        {
            Debug.Log("OnGrantSuccess");
            OnSuccess?.Invoke(result.ItemGrantResults);
        }

        private void OnGetFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }
    }
}