using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class PlayFabLogin
    {
        public event Action<string> OnSuccess;
        public void Login()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)) 
                // Busca PlayFabSharedSettings para cambiar este valor
            {
                /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
                PlayFabSettings.staticSettings.TitleId = "86378";
            }

            var request = new LoginWithCustomIDRequest {CustomId = "GettingStartedGuide", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
            // SystemInfo.deviceUniqueIdentifier;
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Login");
            OnSuccess?.Invoke(result.PlayFabId);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }
    }
}
