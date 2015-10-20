using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Linq;


public class SPawner : MonoBehaviour {

    public GameObject[] items;
    public List<string> wordList = new List<string>();
    public List<wordOutput> wordsListed;
    public List<string> wordConfirm = new List<string>();

    [SerializeField]
    private char[] alphabetArray;

    [SerializeField]
    private int[] pointsArray;

    [SerializeField]
    private float[] alphabetWeights;

    private XmlDocument xmlDoc;
    [SerializeField]
    private TextAsset XMLInput;
    [SerializeField]
    private Sprite[] alphabetSprites;

    [SerializeField]
    private GameObject NextLetter;

    [SerializeField]
    private GameObject BonusWord;

    private List<int> nextLetter = new List<int>();

    private WordContainer wordCollection;

    [SerializeField]
    private GameObject scoreOutput;

    [SerializeField]
    private GameObject levelOutput;

    private List<int> tilePositionList = new List<int>();

    private GameObject createdItem;
    private GameObject middleObject;

    private int scoreCounter;

    private float randomOutput;

    private int level = 1;

    // Use this for initialization
    void Start () {
        wordsListed = new List<wordOutput>();
        firstSpawn();
        GridScript.scoreVisual = scoreOutput;
        GridScript.scoreText = scoreOutput.GetComponent<Text>();
        levelOutput.GetComponent<Text>().text = level.ToString();
        wordCollection = WordContainer.Load(Path.Combine(Application.dataPath, "Resources/Clean Dictionary.xml"));
        newWord();


    }


    // Update is called once per frame
    void Update()
    {

    }

    void newWord()
    {
        int random = Random.Range(1, 276747);
        var word = wordCollection.Words[random];
        if(word.Length > 10)
        {
            newWord();
        }
        else
        {
            setnewWord(word);
        }
        
    }

    void setnewWord(string word)
    {
        BonusWord.GetComponent<Text>().text = word;
    }

    public void firstSpawn()
    {
        int j = 0;


        tilePositionList.Add(1);
        var createdItem = Instantiate(items[j], transform.position, Quaternion.identity) as GameObject;

        var index = randomiseLetter();

        createdItem.GetComponent<createdScript>().assignedChar = alphabetArray[index];
        createdItem.GetComponent<createdScript>().points = pointsArray[index];
        createdItem.GetComponent<createdScript>().tilePosition = 1;
        createdItem.GetComponent<GroupScript>().SpawnerGO = gameObject;
        createdItem.GetComponent<GroupScript>().level = level;
        createdItem.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = alphabetSprites[index];

        nextLetter.Add(randomiseLetter());
        NextLetter.GetComponent<Text>().text = alphabetArray[nextLetter[0]].ToString();
    }

    public int randomiseLetter()
    {
        float TotalFreq = 0;

        for (int i = 0; i < alphabetArray.Length; i++)
        {
            TotalFreq += alphabetWeights[i];
        }
        var roll = Random.Range(0, TotalFreq);

        var index = -1;

        for (int i = 0; i < alphabetArray.Length; i++)
        {
            if (roll <= alphabetWeights[i])
            {
                index = i;
                break;
            }

            roll -= alphabetWeights[i];
        }

        if (index == -1) index = alphabetWeights.Length - 1;

        return index;
    }

    public void newSpawn()
    {
        int j = 0;

        createdItem = null;
        for (int i = 0; i < nextLetter.Count; i++)
        {
            var newV3 = new Vector3();
            if (i != 2)
            {
                tilePositionList.Add(i + 1);
                newV3 = new Vector3(transform.position.x + i, transform.position.y, transform.position.z);
            }
            else
            {
                newV3 = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            }
            var createdItems = Instantiate(items[j], newV3, Quaternion.identity) as GameObject;

            if (createdItem == null)
            {
                createdItem = createdItems;
                middleObject = createdItems;
            }
            else
            {
                if (middleObject.GetComponent<GroupScript>().RightSide == null)
                {
                    middleObject.GetComponent<GroupScript>().RightSide = createdItems;
                    createdItems.GetComponent<GroupScript>().LeftSide = middleObject;
                    createdItem = createdItems;
                }
                else
                {
                    middleObject.GetComponent<GroupScript>().LeftSide = createdItems;
                    createdItems.GetComponent<GroupScript>().RightSide = middleObject;
                    createdItem = createdItems;
                }
            }

            var index = nextLetter[i];
            levelOutput.GetComponent<Text>().text = level.ToString();
            createdItem.GetComponent<createdScript>().tilePosition = i + 1;
            createdItem.GetComponent<createdScript>().assignedChar = alphabetArray[index];
            createdItem.GetComponent<GroupScript>().SpawnerGO = gameObject;
            createdItem.GetComponent<GroupScript>().level = level;
            createdItem.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = alphabetSprites[index];
            createdItem.GetComponent<createdScript>().points = pointsArray[index];
        }
        randomOutput = Random.Range(0, 100);
        nextLetter.Clear();
        var stringOutput = string.Empty;
        if (level > 1)
        {
            if (level > 2)
            {
                if (randomOutput > 35)
                {
                    for (int p = 0; p < 3; p++)
                    {
                        nextLetter.Add(randomiseLetter());
                        stringOutput += alphabetArray[nextLetter[p]].ToString();

                    }
                }
                else if (randomOutput > 10 && randomOutput <= 35)
                {
                    for (int p = 0; p < 2; p++)
                    {
                        nextLetter.Add(randomiseLetter());
                        stringOutput += alphabetArray[nextLetter[p]].ToString();
                    }
                }
                else if (randomOutput <= 10)
                {
                    nextLetter.Add(randomiseLetter());
                    stringOutput += alphabetArray[nextLetter[0]].ToString();
                }
            }
            else
            {
                if (randomOutput > 25)
                {
                    for (int p = 0; p < 2; p++)
                    {
                        nextLetter.Add(randomiseLetter());
                        stringOutput += alphabetArray[nextLetter[p]].ToString();
                    }
                }
                else
                {
                    nextLetter.Add(randomiseLetter());
                    stringOutput += alphabetArray[nextLetter[0]].ToString();
                }
            }
        }
        else
        {
            nextLetter.Add(randomiseLetter());
            stringOutput += alphabetArray[nextLetter[0]].ToString();
        }


        NextLetter.GetComponent<Text>().text = stringOutput;

        if (stringOutput == string.Empty)
        {
            Debug.Log("Stop Here");
        }


    }

    public void wordCheck(int pos)
    {
        var countList = new List<int>();
        if (tilePositionList.Contains(pos))
        {
            tilePositionList.Remove(pos);
        }
        for(var i = 0; i < wordList.Count; i++)
        {
            var item = wordList[i];
            var found = wordCollection.Words.FirstOrDefault(t => t == item);
            
            if (found == null) continue;
            if (found.Length < 3) continue;
            wordsListed[i].isValid = true;
        }
        if (tilePositionList.Count == 0)
        {
            wordDelete(true);
        }
        
    }

    public void wordDelete(bool empty)
    {
        for(int i = 0; i < wordsListed.Count; i++)
        {
            if (wordsListed[i].isValid)
            {
                /*var list = wordsListed[i].ListGO;
                int count = list.Count;
                for (int j = 0; j < count; j++)
                {

                    int x = (int)list[j].transform.position.x;
                    int y = (int)list[j].transform.position.y;
                    GridScript.deleteTiles(x, y);
                }*/
                    var wordlength = wordsListed[i].word.Length;
                for (var j = 0; j < wordlength; j++)
                {
                    if (wordsListed[i].isHorizontal)
                    {
                        var x = wordsListed[i].xcoordstart + j;
                        var y = wordsListed[i].ycoordstart;
                        if (GridScript.grid[x, y] != null)
                        {
                            var pointsoutput = GridScript.grid[x, y].transform.parent.gameObject.GetComponent<createdScript>().points;
                            if(wordsListed[i].word == BonusWord.GetComponent<Text>().text)
                            {
                                pointsoutput += pointsoutput;
                                newWord();
                            }
                            scoreCounter += GridScript.deleteTiles(x, y, pointsoutput);

                            if (scoreCounter >= 25)
                            {
                                scoreCounter = scoreCounter - 25;
                                level = level + 1;
                                levelOutput.GetComponent<Text>().text = level.ToString();
                            }
                        }
                    }
                    else
                    {
                        var x = wordsListed[i].xcoordstart;
                        var y = wordsListed[i].ycoordstart -j;
                        if (GridScript.grid[x, y] != null)
                        {
                            var pointsoutput = GridScript.grid[x, y].transform.parent.gameObject.GetComponent<createdScript>().points;
                            if (wordsListed[i].word == BonusWord.GetComponent<Text>().text)
                            {
                                pointsoutput += pointsoutput;
                                newWord();
                            }
                            scoreCounter += GridScript.deleteTiles(x, y, pointsoutput);

                            if (scoreCounter >= 25)
                            {
                                scoreCounter = scoreCounter - 25;
                                level = level + 1;
                                levelOutput.GetComponent<Text>().text = level.ToString();
                            }
                        }
                    }

                }

            }
        }
        GridScript.decreaseallRows();
        wordList.Clear();
        wordsListed.Clear();
        wordConfirm.Clear();
        if (GridScript.valid == false)
        {
            Debug.Log(tilePositionList.Count);
            if (empty == true)
            {
                newSpawn();
            }
            if(tilePositionList.Count == 0)
            {
                newSpawn();
            }
            
        }
    }



}
