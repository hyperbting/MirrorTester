using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerAction localPlayer;

    public string hostPlayerName;

    private void Awake()
    {
        localPlayer.isLocalPlayer = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerAction playerPrefab;
    public PlayerAction Instantiate()
    {
        var go = Instantiate(playerPrefab);

        return go.GetComponent<PlayerAction>();
    }
}
