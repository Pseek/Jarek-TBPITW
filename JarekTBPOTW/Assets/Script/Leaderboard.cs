using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using Unity.VisualScripting;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> times;

    public string publicLeaderboard = "ab4ef05bb257161227104325938aac1d956d6a666adbfff26740784fb755657b";

    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboard, ((msg) => {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for(int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                times[i].text = msg[i].Extra.ToString();
            }
        }));
    }

    public void SetLeaderboard(string username,int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboard, username, score, ((msg) => { GetLeaderboard(); }));
    }
}
    