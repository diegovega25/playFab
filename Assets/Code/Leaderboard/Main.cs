using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Leaderboard
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Button _getLeaderboardButton;
        [SerializeField] private Button _getLeaderboardAroundPlayerButton;
        [SerializeField] private Button _getPlayerScoreButton;
        [SerializeField] private Button _addPlayerScoreButton;
        [SerializeField] private TextMeshProUGUI _resultsText;
       
        private const string LeaderboardName = "LeaderboardTest";

        private string _playerId;
        
        private PlayFabLogin _playFabLogin;
        private PlayFabUpdatePlayerStatistics _playFabUpdatePlayerStatistics;
        private PlayFabGetLeaderboardAroundPlayer _playFabGetLeaderboardAroundPlayer;
        private PlayFabGetLeaderboard _playFabGetLeaderboard;

        private void Start()
        {
            AddListeners();
            CreatePlayFabServices();
            DoLogin();
        }

        private void CreatePlayFabServices()
        {
            _playFabLogin = new PlayFabLogin();
            _playFabLogin.OnSuccess += playerId => _playerId = playerId;

            _playFabUpdatePlayerStatistics = new PlayFabUpdatePlayerStatistics();

            _playFabGetLeaderboardAroundPlayer = new PlayFabGetLeaderboardAroundPlayer();
            _playFabGetLeaderboardAroundPlayer.OnSuccess += result => _resultsText.SetText(result);

            _playFabGetLeaderboard = new PlayFabGetLeaderboard();
            _playFabGetLeaderboard.OnSuccess += result => _resultsText.SetText(result);
        }

        private void DoLogin()
        {
            _playFabLogin.Login();
        }

        private void AddListeners()
        {
            _getLeaderboardButton.onClick.AddListener(OnGetLeaderboardButtonPressed);
            _getLeaderboardAroundPlayerButton.onClick.AddListener(OnGetLeaderboardAroundPlayerButtonPressed);
            _getPlayerScoreButton.onClick.AddListener(OnGetPlayerScoreButtonPressed);
            _addPlayerScoreButton.onClick.AddListener(OnAddPlayerScoreButtonPressed);
        }

        private void OnAddPlayerScoreButtonPressed()
        {
            _playFabUpdatePlayerStatistics
               .UpdatePlayerStatistics(LeaderboardName, 100);
        }

        private void OnGetPlayerScoreButtonPressed()
        {
            _playFabGetLeaderboardAroundPlayer
               .GetLeaderboardAroundPlayer(_playerId, 1, LeaderboardName);
        }

        private void OnGetLeaderboardAroundPlayerButtonPressed()
        {
            _playFabGetLeaderboardAroundPlayer
               .GetLeaderboardAroundPlayer(_playerId, 3, LeaderboardName);
        }

        private void OnGetLeaderboardButtonPressed()
        {
            _playFabGetLeaderboard
               .GetLeaderboardEntries(0, 10, LeaderboardName);
        }
    }
}
