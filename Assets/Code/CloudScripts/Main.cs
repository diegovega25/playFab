using System.Collections.Generic;
using System.Text;
using PlayFab.ServerModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

namespace Code.CloudScripts
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TextMeshProUGUI _output;
        private PlayFabLogin _playFabLogin;
        private string _playerId;

        private void Start()
        {
            _playFabLogin = new PlayFabLogin();
            _playFabLogin.OnSuccess += playerId => _playerId = playerId;
            DoLogin();
        }

        public void ExecuteButtton()
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "helloWorld",
                FunctionParameter = new
                {
                    name = _input.text
                }
            };
            PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnError);


        }

        private void OnError(PlayFabError obj)
        {
            throw new NotImplementedException();
        }

        private void OnExecuteSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
        {
            _output.text = result.FunctionResult.ToString();
            //throw new NotImplementedException();
        }
        private void DoLogin()
        {
            _playFabLogin.Login();
        }
    }
}
