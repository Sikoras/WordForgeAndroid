using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;
using System;

public class LeaderboardScreen : MonoBehaviour {

    private string LeaderInput = string.Empty;
    private string LeaderEnding = "/api/Cardbox/WordForgeLeaderBoard/";
    private List<string> LeaderPlayers = new List<string>();
    private List<int> LeaderScores = new List<int>();
    [SerializeField]
    private GameObject LeaderPlayerOutput;
    [SerializeField]
    private GameObject LeaderScoreOutput;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        LeaderInput = "http://midsportsacademydev.azurewebsites.net/" + LeaderEnding;
#else
        LeaderInput = Application.absoluteURL.Substring(0, Application.absoluteURL.Length - 14) + LeaderEnding;

#endif
        Refresh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoBack()
    {
        Application.LoadLevel(1);
    }

    public void Refresh()
    {
        StartCoroutine(GetLeaderBoard());
    }

    IEnumerator GetLeaderBoard()
    {
        WWW response = new WWW(LeaderInput);

        yield return response;
        if(response.error != null)
        {
            Debug.Log(response.error);
        }
        else
        {
            Debug.Log(response.text);
            
            var textImport = response.text.Substring(1,response.text.Length-2);
            textImport = textImport.Replace(@"\", "");
            Debug.Log(textImport);

            if(LeaderPlayers.Count > 0)
            {
                LeaderPlayers.Clear();
            }
            if(LeaderScores.Count > 0)
            {
                LeaderScores.Clear();
            }

            try
            {
                var jsonObject = Json.Deserialize(textImport) as Dictionary<string, object>;
                var item = jsonObject["Leaders"];
                var outputList = item as List<object>;

                for (int i = 0; i < outputList.Count; i++)
                {
                    var outItem = outputList[i] as Dictionary<string, object>;

                    LeaderPlayers.Add((outItem["Username"]).ToString());
                    LeaderScores.Add(int.Parse(outItem["Points"].ToString()));
                }
                if(LeaderPlayerOutput != null && LeaderScoreOutput != null)
                {
                    var playerOut = LeaderPlayerOutput.GetComponent<Text>().text;
                    var scoresOut = LeaderScoreOutput.GetComponent<Text>().text;

                    playerOut = "";
                    scoresOut = "";

                    for(int i = 0; i < LeaderPlayers.Count; i++)
                    {
                        playerOut = playerOut + "\n" + LeaderPlayers[i];
                        scoresOut = scoresOut + "\n" + LeaderScores[i].ToString();
                    }
                    LeaderPlayerOutput.GetComponent<Text>().text = "";
                    LeaderScoreOutput.GetComponent<Text>().text = "";
                    LeaderPlayerOutput.GetComponent<Text>().text = playerOut;
                    LeaderScoreOutput.GetComponent<Text>().text = scoresOut;
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex);
            }
            /*foreach (var item in LeaderPlayers)
            {
                LeaderPlayerOutput.GetComponent<Text>().text += "/n" + item;
            }
            foreach (var item in LeaderScores)
            {
                LeaderScoreOutput.GetComponent<Text>().text += "/n" + item;
            }*/
        }
    }
}
