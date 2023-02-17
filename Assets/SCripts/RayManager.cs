using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayManager : MonoBehaviour
{

    private int maxDistance = 10;
    public Transform[] targetPoints;
    public List<Player> playerList = new List<Player>();
    private int currentTargetIndex;

    void Start()
    {
        currentTargetIndex = 0;
    }

   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            move();
        }
    }
    private bool isss;

    void move()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,  out hit,maxDistance))
            {
                if (hit.collider.TryGetComponent(out Player player))
                {
                    if(playerList.Count == 0)
                    {
                        player.transform.DOJump(targetPoints[0].position, .1f, 1, 1f);
                        player.index = 0;
                        playerList.Add(player);
                        return;
                    }
                    var lastPlayer = playerList.Where(x => x.currentColor == player.currentColor).LastOrDefault();
                    if(lastPlayer is not null)
                    {
                        var nextIndex = lastPlayer.index + 1;
                        if(playerList.Count -1 >= nextIndex)
                        {
                            if (playerList[nextIndex] != null)
                            {
                                var currentPlayersNextIndex = playerList[nextIndex].index + 1;
                                playerList[nextIndex].transform.DOJump(targetPoints[currentPlayersNextIndex].position, .1f, 1, 1f);
                                playerList[nextIndex].index = currentPlayersNextIndex;
                            }
                        }
                        player.index = nextIndex;
                        playerList.Add(player);
                        player.transform.DOJump(targetPoints[nextIndex].position, 0.1f, 1, 1f).OnComplete(() =>
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
                    else
                    {
                        var nextIndex = playerList.LastOrDefault().index +1;
                        player.transform.DOJump(targetPoints[nextIndex].position, 0.1f, 1, 1f);
                        player.index = nextIndex;
                        playerList.Add(player);
                    }
                }
            }

        }
    }

