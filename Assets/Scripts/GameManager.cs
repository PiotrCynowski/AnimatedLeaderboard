using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public List<TextAsset> jsonFiles;
        int currentJsonIndex = 0;

        [SerializeField] LeaderboardSpriteSelector countryFlagsSelector;
        [SerializeField] LeaderboardSpriteSelector avatarSelector;
        [SerializeField] LeaderboardSpriteSelector podiumSelector;
        [SerializeField] LeaderboardView leaderboardView;

        [Range(5,15)]
        [SerializeField] int maxEntriesBeforeScroll = 10;

        LeaderboardPresenter presenter;
        PlayerInputActions inputActions;

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;

            inputActions = new();
        }

        void OnEnable()
        {
            inputActions.UI.TapScreen.performed += OnTapScreen;
            inputActions.UI.Enable();
        }

        void OnDisable()
        {
            inputActions.UI.TapScreen.performed -= OnTapScreen;
            inputActions.UI.Disable();
        }

        void Start()
        {
            if (leaderboardView == null)
            {
                Debug.LogError("Please assign leaderboard view");
                return;
            }

            leaderboardView.Init(maxEntriesBeforeScroll);
            leaderboardView.ExitButtonClicked += OnExitFromLeaderboard;

            if (countryFlagsSelector == null || avatarSelector == null || podiumSelector == null)
            {
                Debug.LogError("Please assign all sprite selectors");
                return;
            }

            if (currentJsonIndex >= jsonFiles.Count)
            {
                Debug.LogError("Please assign json files");
                return;
            }

            LeaderboardData model = new();
            presenter = new(model, leaderboardView, countryFlagsSelector, avatarSelector, podiumSelector);

            presenter.LoadData(jsonFiles[currentJsonIndex].text);
        }

        public void ReplaceJsonData(string jsonData)
        {
            presenter.LoadData(jsonData);
        }

        void OnTapScreen(InputAction.CallbackContext context)
        {
            inputActions.Disable();
            presenter.HandleTapScreen();
        }

        void OnExitFromLeaderboard()
        {
            ReplaceJsonData(GetNextJson().text);
            inputActions.Enable();
        }

        TextAsset GetNextJson()
        {
            currentJsonIndex = (currentJsonIndex + 1) % jsonFiles.Count;
            return jsonFiles[currentJsonIndex];
        }
    }
}