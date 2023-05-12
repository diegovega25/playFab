using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
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
                PlayFabSettings.staticSettings.TitleId = "AACE9";
            }

            var request = new LoginWithCustomIDRequest {CustomId = "GettingStartedGuide", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

            
            // SystemInfo.deviceUniqueIdentifier;
        }

        private void OnLoginSuccess(LoginResult result)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest{Keys = new List<string>{"IsInitialized"}},
                dataResult => {
                  //  Debug.Log(dataResult.Data);
                    if (!dataResult.Data.ContainsKey("IsInitialized")){
                        InitializeUser();
                    }
                }, error => {Debug.Log("Error en IsInitialized");}
            );

            Debug.Log("Login");
            OnSuccess?.Invoke(result.PlayFabId);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        }

        private void InitializeUser(){
            Debug.Log("jeje");
            /**************************************************************Test para obtener data**************************************************************************/
            GetTitleDataRequest request = new GetTitleDataRequest
            {
                Keys = new List<string>() {"InitialUserData"}
            };
            PlayFabClientAPI.GetTitleData(request, dataResult =>
                 {
                // Debug.Log(dataResult.Data["InitialUserData"]);
                 var data = dataResult.Data["InitialUserData"];
                 var initialUserData = JsonUtility.FromJson<InitialUserData>(data);
                  
                 PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest {
                     Amount = initialUserData.InitialSoftCurrency, VirtualCurrency = "SC" },
                    result => { Debug.Log("agregado el currency"); },
                    error => { Debug.Log("No agregado el currency"); }
                    );

                 PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest{
                     Data= new Dictionary<string, string>
                     {
                         {
                             "Tutorial", 
                             JsonUtility.ToJson(new TutorialConfiguration{IsEnabled = initialUserData.TutorialEnabled})
                         },
                         {
                             "IsInitialized",
                             JsonUtility.ToJson(new UserInitialized{IsInitialized = true})
                         }
                     }
                 },
                 result => { Debug.Log("agregado el initialized"); }, 
                        error => { Debug.Log("no agregado el initialized"); }
                  );
                    // Debug.Log(initialUserData.InitialSoftCurrency);
                }, error => {
                    Debug.Log("error obteniendo datos");
                }
            );
            /**************************************************************FIN Test para obtener data**************************************************************************/

        }
    }

    //Esta clase es para pasar los datos a JSON
    [Serializable]
    public class InitialUserData //los nombres deben ser los mismos de PlayFab
    {
        public int InitialSoftCurrency;
        public bool TutorialEnabled;
    }

    [Serializable]
    public class TutorialConfiguration
    {
        public bool IsEnabled;
    }

    [Serializable]
    public class UserInitialized
    {
        public bool IsInitialized;
    }
}
