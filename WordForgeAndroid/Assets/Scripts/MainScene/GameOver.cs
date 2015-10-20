using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    private string ScoreInput = string.Empty;
    private string ScoreEnding = "/api/Cardbox/WordForgeScoreSubmit/";
    private string UserName;
    private int score;

    [SerializeField]
    private GameObject InputFieldString;

    [SerializeField]
    private GameObject ScoreGO;
    // Use this for initialization
    void Start () {
#if UNITY_EDITOR
        ScoreInput = "http://midsportsacademydev.azurewebsites.net/" + ScoreEnding;
#else
        ScoreInput = Application.absoluteURL.Substring(0, Application.absoluteURL.Length - 14) + ScoreEnding;

#endif
        score = GameObject.FindGameObjectWithTag("dontDestroy").GetComponent<LoadingScreen>().score;
        ScoreGO.GetComponent<Text>().text = "Score: " + score;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void PlayAgain()
    {
        Application.LoadLevel(2);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void SubmitScore()
    {
        StartCoroutine(SubmitIENum());
    }

    public void SetUsername()
    {
        UserName = InputFieldString.GetComponent<InputField>().text;
    }

    IEnumerator SubmitIENum()
    {
        var responseForm = new WWWForm();
        responseForm.AddField("Username", UserName);
        responseForm.AddField("Score", score);

        WWW response = new WWW(ScoreInput,responseForm);

        yield return response;
        if (response.error != null)
        {
            Debug.Log(response.error);
        }
        else
        {
            Debug.Log(response.text);
        }
    }
}
