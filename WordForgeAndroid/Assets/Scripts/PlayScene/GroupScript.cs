using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GroupScript : MonoBehaviour {

    private float lastFall = 0;
    private string outputWord;
    public GameObject SpawnerGO;
    private const int LOWERCOUNT = -10;
    private const int UPPERCOUNT = 10;

    public GameObject RightSide;
    public GameObject LeftSide;

    private int count;

    public int level;
	// Use this for initialization
	void Start () {
        if (!isValidGridPos(transform.position))
        {
            Application.LoadLevel(4);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gameObject.GetComponent<createdScript>().tilePosition == 1)
            {
                transform.position += new Vector3(-1, 0, 0);

                var outputPos = transform.position;
                if (LeftSide != null)
                {
                    if(LeftSide.GetComponent<createdScript>().Spread == true)
                    {
                        outputPos += new Vector3(-2, 0, 0);
                    }
                    else
                    {
                        outputPos += new Vector3(-1, 0, 0);
                    }
                    
                    LeftSide.transform.position += new Vector3(-1, 0, 0);
                }
                if(RightSide != null)
                {

                    RightSide.transform.position += new Vector3(-1, 0, 0);
                }
                if (isValidGridPos(outputPos))
                {
                    if(RightSide != null)
                    {
                        if (isValidGridPos(RightSide.transform.position))
                        {
                            if (LeftSide != null)
                            {
                                if (isValidGridPos(LeftSide.transform.position))
                                {
                                    updateGrid();
                                }
                                else
                                {
                                    transform.position += new Vector3(1, 0, 0);
                                    if (RightSide != null)
                                    {
                                        RightSide.transform.position += new Vector3(1, 0, 0);
                                    }
                                    if (LeftSide != null)
                                    {
                                        LeftSide.transform.position += new Vector3(1, 0, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            transform.position += new Vector3(1, 0, 0);
                            if (RightSide != null)
                            {
                                RightSide.transform.position += new Vector3(1, 0, 0);
                            }
                            if (LeftSide != null)
                            {
                                LeftSide.transform.position += new Vector3(1, 0, 0);
                            }
                        }
                    }
                    else
                    {
                        updateGrid();
                    }
                    
                }
                else
                {
                    transform.position += new Vector3(1, 0, 0);
                    if (RightSide != null)
                    {
                        RightSide.transform.position += new Vector3(1, 0, 0);
                    }
                    if (LeftSide != null)
                    {
                        LeftSide.transform.position += new Vector3(1, 0, 0);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gameObject.GetComponent<createdScript>().tilePosition == 1)
            {
                transform.position += new Vector3(1, 0, 0);

                var outputPos = transform.position;
                if (RightSide != null)
                {
                    if (RightSide.GetComponent<createdScript>().Spread == true)
                    {
                        outputPos += new Vector3(2, 0, 0);
                    }
                    else
                    {
                        outputPos += new Vector3(1, 0, 0);
                    }
                    RightSide.transform.position += new Vector3(1, 0, 0);
                }
                if (LeftSide != null)
                {
                    LeftSide.transform.position += new Vector3(1, 0, 0);
                }
                if (isValidGridPos(outputPos))
                {
                    if (RightSide != null)
                    {
                        if (isValidGridPos(RightSide.transform.position))
                        {
                            if (LeftSide != null)
                            {
                                if (isValidGridPos(LeftSide.transform.position))
                                {
                                    updateGrid();
                                }
                                else
                                {
                                    transform.position += new Vector3(-1, 0, 0);
                                    if (RightSide != null)
                                    {
                                        RightSide.transform.position += new Vector3(-1, 0, 0);
                                    }
                                    if (LeftSide != null)
                                    {
                                        LeftSide.transform.position += new Vector3(-1, 0, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            transform.position += new Vector3(-1, 0, 0);
                            if (RightSide != null)
                            {
                                RightSide.transform.position += new Vector3(-1, 0, 0);
                            }
                            if (LeftSide != null)
                            {
                                LeftSide.transform.position += new Vector3(-1, 0, 0);
                            }
                        }
                    }
                    else
                    {
                        updateGrid();
                    }
                }
                else
                {
                    transform.position += new Vector3(-1, 0, 0);
                    if (RightSide != null)
                    {
                        RightSide.transform.position += new Vector3(-1, 0, 0);
                    }
                    if (LeftSide != null)
                    {
                        LeftSide.transform.position += new Vector3(-1, 0, 0);
                    }
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= (0.5 + (0.5 / level)))
        {

            transform.position += new Vector3(0, -1, 0);

            if (isValidGridPos(transform.position))
            {
                updateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);

                //WordCheck and Delete any GameObjects that form a word


                checkWords();

                enabled = false;
            }
            lastFall = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if(gameObject.GetComponent<createdScript>().tilePosition == 1)
            {

                if (RightSide != null && LeftSide == null)
                {
                    if (RightSide.GetComponent<GroupScript>().enabled == true)
                    {
                        if (RightSide.GetComponent<createdScript>().Spread == true)
                        {
                            RightSide.transform.position += new Vector3(-1, 0, 0);
                            if (isValidGridPos(RightSide.transform.position))
                            {
                                updateGrid();
                                RightSide.GetComponent<createdScript>().Spread = false;
                            }
                            else
                            {
                                RightSide.transform.position += new Vector3(1, 0, 0);
                            }
                        }
                        else
                        {
                            RightSide.transform.position += new Vector3(1, 0, 0);
                            if (isValidGridPos(RightSide.transform.position))
                            {
                                updateGrid();
                                RightSide.GetComponent<createdScript>().Spread = true;
                            }
                            else
                            {
                                RightSide.transform.position += new Vector3(-1, 0, 0);
                            }
                        }
                    }
                }
                else if (LeftSide != null && RightSide == null)
                {
                    if (LeftSide.GetComponent<GroupScript>().enabled == true)
                    {
                        if (LeftSide.GetComponent<createdScript>().Spread == true)
                        {
                            LeftSide.transform.position += new Vector3(1, 0, 0);
                            if (isValidGridPos(LeftSide.transform.position))
                            {
                                updateGrid();
                                LeftSide.GetComponent<createdScript>().Spread = false;
                            }
                            else
                            {
                                LeftSide.transform.position += new Vector3(-1, 0, 0);
                            }
                        }
                        else
                        {
                            LeftSide.transform.position += new Vector3(-1, 0, 0);
                            if (isValidGridPos(LeftSide.transform.position))
                            {
                                updateGrid();
                                LeftSide.GetComponent<createdScript>().Spread = true;
                            }
                            else
                            {
                                LeftSide.transform.position += new Vector3(1, 0, 0);
                            }
                        }
                    }
                }
                else if (LeftSide != null && RightSide != null)
                {
                    if (LeftSide.GetComponent<GroupScript>().enabled == true && RightSide.GetComponent<GroupScript>().enabled == true)
                    {
                        if (count == 0)
                        {
                            RightSide.transform.position += new Vector3(1, 0, 0);
                            if (isValidGridPos(RightSide.transform.position))
                            {

                                updateGrid();
                                RightSide.GetComponent<createdScript>().Spread = true;
                                count = 1;

                            }
                            else
                            {
                                RightSide.transform.position += new Vector3(-1, 0, 0);
                                RightSide.GetComponent<createdScript>().Spread = false;
                            }
                        }
                        else if (count == 1)
                        {
                            LeftSide.transform.position += new Vector3(-1, 0, 0);
                            if (isValidGridPos(LeftSide.transform.position))
                            {
                                updateGrid();
                                LeftSide.GetComponent<createdScript>().Spread = true;
                                count = 2;

                            }
                            else
                            {
                                LeftSide.transform.position += new Vector3(1, 0, 0);
                                LeftSide.GetComponent<createdScript>().Spread = false;
                            }

                        }
                        else if (count == 2)
                        {
                            RightSide.transform.position += new Vector3(-1, 0, 0);
                            if (isValidGridPos(RightSide.transform.position))
                            {
                                updateGrid();
                                RightSide.GetComponent<createdScript>().Spread = false;
                                count = 3;

                            }
                            else
                            {
                                RightSide.GetComponent<createdScript>().Spread = true;
                                RightSide.transform.position += new Vector3(1, 0, 0);
                            }
                        }
                        else if (count == 3)
                        {
                            LeftSide.transform.position += new Vector3(1, 0, 0);
                            if (isValidGridPos(LeftSide.transform.position))
                            {
                                updateGrid();
                                LeftSide.GetComponent<createdScript>().Spread = false;
                                count = 0;

                            }
                            else
                            {
                                LeftSide.GetComponent<createdScript>().Spread = true;
                                LeftSide.transform.position += new Vector3(-1, 0, 0);
                            }
                        }
                    }
                }
            }
        }
	}

    bool isValidGridPos(Vector2 input)
    {
        Vector2 v = GridScript.roundVec(input);

        if (!GridScript.insideBorder(v)) return false;

        if (GridScript.grid[(int)v.x, (int)v.y] != null)
        {
            if (GridScript.grid[(int)v.x, (int)v.y].transform.GetComponentInParent<GroupScript>().enabled)
            {
                return true;
            }
            if (GridScript.grid[(int)v.x, (int)v.y].transform.parent != transform)
            {
                return false;
            }

        }

        return true;
    }

    void updateGrid()
    {
        for (int y = 0; y < GridScript.h; y++)
        {
            for (int x = 0; x < GridScript.w; x++)
            {
                if (GridScript.grid[x, y] != null)
                {
                    if (GridScript.grid[x, y].transform.parent == transform)
                    {
                        GridScript.grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform child in transform)
        {
            Vector2 v = GridScript.roundVec(child.position);
            GridScript.grid[(int)v.x, (int)v.y] = child.gameObject;
        }
    }

    void checkWords()
    {
        

        var outputList = new List<GameObject>();
        var lower = -11;
        var upper = 11;
        var previousCount = 0;
        var containsCheck = string.Empty;
        var countlist = new List<int>();



        for (int i = 0; i > LOWERCOUNT; i--)
        {
            outputWord = string.Empty;
            outputList.Clear();
            for (int j = 0; j < UPPERCOUNT; j++)
            {
                if (i != j || i == 0)
                {
                    var count = j + i;
                    //if (countlist.Contains(count) == false)
                    //{
                    countlist.Add(count);
                    var x = (int)gameObject.transform.position.x + count;
                    var y = (int)gameObject.transform.position.y;

                    if (x > 9 || x < 0) continue;


                    if (x > lower && x < upper)
                    {
                        if (GridScript.grid[x, y] != null)
                        {
                            //Debug.Log(x + " " + y);
                            if (count < previousCount)
                            {
                                outputWord = GridScript.grid[x, y].GetComponentInParent<createdScript>().assignedChar + outputWord;
                                previousCount = count;
                            }
                            else
                            {
                                outputWord += GridScript.grid[x, y].GetComponentInParent<createdScript>().assignedChar;
                                previousCount = count;
                            }

                            outputList.Add(GridScript.grid[x, y].transform.parent.gameObject);

                            if (containsCheck == "")
                            {
                                containsCheck = GridScript.grid[x, y].GetComponentInParent<createdScript>().assignedChar.ToString();
                            }

                            
                            if (!checkifExists(outputWord))
                            {
                                if (outputWord.Contains(containsCheck) == true)
                                {
                                    SpawnerGO.GetComponent<SPawner>().wordList.Add(outputWord);
                                    var addtoList = new wordOutput
                                    {
                                        word = outputWord,
                                        xcoordstart = (int)outputList.First().transform.position.x,
                                        ycoordstart = (int)outputList.First().transform.position.y,
                                        isHorizontal = true                                        
                                    };
                                    SpawnerGO.GetComponent<SPawner>().wordsListed.Add(addtoList);
                                    //var listed = SpawnerGO.GetComponent<SPawner>().wordDictionary[outputWord];
                                }
                            }

                        }
                        else
                        {
                            if (count > previousCount)
                            {
                                upper = x;
                            }
                            if (count < previousCount)
                            {
                                lower = x;
                            }

                        }


                        // }
                    }
                }
            }
        }



        checkWordsVert();
    }

    void checkWordsVert()
    {


        var outputList = new List<GameObject>();
        var containsCheck = string.Empty;
        var countlist = new List<int>();
        outputWord = string.Empty;
        outputList.Clear();
        for(var i = 0; i < UPPERCOUNT; i++)
        {
            var x = (int)gameObject.transform.position.x;
            var y = (int)gameObject.transform.position.y - i;

            if (y < 0) continue;

            if(GridScript.grid[x,y] != null)
            {
                
                    outputWord += GridScript.grid[x, y].GetComponentInParent<createdScript>().assignedChar;
                    outputList.Add(GridScript.grid[x, y].transform.parent.gameObject);

                    SpawnerGO.GetComponent<SPawner>().wordList.Add(outputWord);
                    var addtoList = new wordOutput
                    {
                        word = outputWord,
                        xcoordstart = (int)outputList.First().transform.position.x,
                        ycoordstart = (int)outputList.First().transform.position.y
                    };
                    SpawnerGO.GetComponent<SPawner>().wordsListed.Add(addtoList);
            }
        }
        var tilePos = gameObject.GetComponent<createdScript>().tilePosition;
        FindObjectOfType<SPawner>().wordCheck(tilePos);
    }

    bool checkifExists(string word)
    {
        foreach(var item in SpawnerGO.GetComponent<SPawner>().wordsListed)
        {
            if (word == item.word)
            {
                return true;
            }

        }
        return false;
    }
}
