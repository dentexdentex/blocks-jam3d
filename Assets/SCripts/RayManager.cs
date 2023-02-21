using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayManager : MonoBehaviour
{

    private int maxDistance = 10;
    public Transform[] targetPoints;
    public List<Player> playerList = new List<Player>();
    private int levelCounter = -9;
    void Start()
    {
       

    }
    IEnumerator LoadLevelWithDelay(int levelIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();
            levelCounter++;

            if (levelCounter == 0)
            {
                levelCounter = -9;
                StartCoroutine(LoadLevelWithDelay(SceneManager.GetActiveScene().buildIndex + 1, 3));
            }
        }
    }

   
    void Move()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.TryGetComponent(out Player player))
            {
                if (playerList.Count == 0)
                {
                    player.transform.DOJump(targetPoints[0].position, .1f, 1, 1f);
                    player.index = 0;
                    playerList.Add(player);
                    return;
                }

                if (playerList.Count == 1)
                {
                    player.transform.DOJump(targetPoints[playerList.Count].position, .1f, 1, 1f);
                    player.index = playerList.Count;
                    playerList.Add(player);
                    return;
                }

                //var lastPlayer = playerList.Where(x => x.currentColor == player.currentColor).LastOrDefault();
                Player lastPlayer = null;
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i].currentColor == player.currentColor)
                    {
                        lastPlayer = playerList[i];
                    }
                }
                
                if (playerList.Count >1)
                {
                    if (lastPlayer == null) //eğer oncesinde aynısından yoksa
                    {
                        player.transform.DOJump(targetPoints[playerList.Count].position, .1f, 1, 1f);
                        player.index = playerList.Count;
                        playerList.Add(player);
                        return;
                    }
                    else// (lastPlayer != null)
                    {
                        if (playerList.Count > lastPlayer.index + 1)
                            // playerlistemizde yeni ekleyeceğimiz kübün aynı renginnden en son olanının  yanında birisi var mı 
                        {
                            for (int i = lastPlayer.index+1; i < playerList.Count; i++) { 

                                var jumper = playerList[i]; // jumperı atarken 
                                jumper.index += 1; //  playerList.Count;
                                jumper.transform.DOJump(targetPoints[jumper.index].position, .1f, 1, 1f).OnComplete(() =>
                                {
                                    var totalSameColorsList = playerList.Where(x => x.currentColor == player.currentColor).ToList();
                                    if (totalSameColorsList.Count == 3)
                                    {
                                        for (int i = 0; i < totalSameColorsList.Count; i++)
                                        {
                                            playerList.Remove(totalSameColorsList[i]);
                                    
                                            totalSameColorsList[i].transform.DOScale(Vector3.zero,1f).OnComplete(()=> 
                                            {
                                                totalSameColorsList[i].gameObject.SetActive(false);           
                                            });
                                        }
                                        foreach (var currentPlayer in playerList)
                                        {
                                            currentPlayer.index -= 3;
                                            currentPlayer.transform.DOJump(targetPoints[currentPlayer.index].position,.1f,1,1f);
                                        }
                                    }
                                });
                            }
                            player.index = lastPlayer.index+1;
                            player.transform.DOJump(targetPoints[player.index].position, .1f, 1, 1f).OnComplete(() =>
                            {
                                var totalSameColorsList = playerList.Where(x => x.currentColor == player.currentColor).ToList();
                                if (totalSameColorsList.Count == 3)
                                {
                                    for (int i = 0; i < totalSameColorsList.Count; i++)
                                    {
                                        playerList.Remove(totalSameColorsList[i]);
                                    
                                        totalSameColorsList[i].transform.DOScale(Vector3.zero,1f).OnComplete(()=> 
                                        {
                                            totalSameColorsList[i].gameObject.SetActive(false);           
                                        });
                                    }
                                    foreach (var currentPlayer in playerList)
                                    {
                                        currentPlayer.index -= 3;
                                        currentPlayer.transform.DOJump(targetPoints[currentPlayer.index].position,.1f,1,1f);
                                    }
                                }
                            });
                            playerList.Insert(player.index,player);
                            return;
                            
                        }
                        else if(playerList.Count <= lastPlayer.index + 1)
                        {
                            var jumpIndex = lastPlayer.index + 1;
                            playerList.Add(player);
                            player.index = jumpIndex;
                            player.transform.DOJump(targetPoints[player.index].position, .1f, 1, 1f).OnComplete(() =>
                            {
                                var totalSameColorsList = playerList.Where(x => x.currentColor == player.currentColor).ToList();
                                if (totalSameColorsList.Count == 3)
                                {
                                    for (int i = 0; i < totalSameColorsList.Count; i++)
                                    {
                                        playerList.Remove(totalSameColorsList[i]);
                                    
                                        totalSameColorsList[i].transform.DOScale(Vector3.zero,1f).OnComplete(()=> 
                                        {
                                            totalSameColorsList[i].gameObject.SetActive(false);           
                                        });
                                    }
                                    foreach (var currentPlayer in playerList)
                                    {
                                        currentPlayer.index -= 3;
                                        currentPlayer.transform.DOJump(targetPoints[currentPlayer.index].position,.1f,1,1f);
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    }
}
