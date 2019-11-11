using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesScript : MonoBehaviour {

	void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
	}

    void SignIn()
    {
        Social.localUser.Authenticate((success => { Debug.Log("User succesfully signed in."); }));
    }

    #region Leaderboard
    public static void AddScordToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, (success => { Debug.Log("Score sucessfully uploaded"); }));
    }

    public static void ShowLeaderboardsUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion
}
