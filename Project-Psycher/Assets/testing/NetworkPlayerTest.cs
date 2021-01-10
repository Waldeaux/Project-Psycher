using Assets.testing;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerTest : NetworkBehaviour
{
    public int connId;
    public List<PlayerData> playerData;
    [SyncVar]
    public long startingTime;

    [SyncVar(hook = "ForcePlayerData")]
    PlayerStruct authPlayerData;

    private enum State
    {
        normal,
        playerOverride
    }

    private State currentState = State.normal;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //authPlayerData = new PlayerStruct(new Vector3(0, 0, 0), 0, transform.rotation);
        if (isServer)
        {
            playerData = new List<PlayerData>();
            for (int x = 0; x < 8; x++)
            {
                playerData.Add(null);
            }
        }
        //CmdSetKinematic();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (State.normal):
                NormalUpdate();
                break;
            case (State.playerOverride):
                PlayerOverrideUpdate();
                break;
        }
    }

    void NormalUpdate()
    {
        if (hasAuthority)
        {
            Vector3 planarMovement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"));
            Vector3 upMovement = Vector3.Project(rb.velocity, transform.up) - transform.up * 9.8f * Time.deltaTime;
            //transform.position += (planarMovement * 5 + upMovement)*Time.deltaTime;
            rb.velocity = planarMovement * 5 + upMovement;
            //CmdUpdateMovement(planarMovement);
            if (Input.GetKeyDown(KeyCode.R))
            {
                CmdUpdateAuthPlayerData();
            }
        }
    }

    void PlayerOverrideUpdate()
    {
        long tickDifference = DateTime.Now.Ticks - authPlayerData.frame*10000000 + startingTime;
        float timer = (float)tickDifference / 5000000;
        transform.position = Vector3.Lerp(transform.position, authPlayerData.position,timer);
        transform.rotation = Quaternion.Slerp(transform.rotation, authPlayerData.rotation, timer);
        Vector3 difference = authPlayerData.position - transform.position;
        if (difference.magnitude < 0.5f)
        {
            transform.position = authPlayerData.position;
            transform.rotation = authPlayerData.rotation;
            currentState = State.normal;
        }
    }

    [Command]
    void CmdUpdateMovement(Vector3 movementVector)
    {
        Vector3 upMovement = Vector3.Project(rb.velocity, transform.up) - transform.up * 9.8f * Time.deltaTime;
        rb.velocity = movementVector*5 + upMovement;
    }

    [Command]
    void CmdSendCurrentFrame(int frame, long ticks)
    {
        Debug.Log(ticks);
        Debug.Log(DateTime.Now.Ticks);
        //Debug.Log(frame);
    }

    [Command]
    void CmdUpdatePlayerData(PlayerStruct playerFrameData)
    {
        PlayerData compPlayerData = null;
        print(playerFrameData.frame);
        for(int x = 0; x < playerData.Count; x++)
        {
            if(playerData[x] != null)
            {
                compPlayerData = playerData[x];
                break;
            }
        }
        int currentFrame = CalculateFrame(DateTime.Now.Ticks);
        int mostRecentFrame = 0;
        if (compPlayerData != null) { 
            mostRecentFrame = compPlayerData.Frame;
        }
        if(mostRecentFrame - 7 > playerFrameData.frame)
        {
            print("frame outside of scope");
            return;
        }
        if (mostRecentFrame < playerFrameData.frame)
        {
            int frameDifference = Mathf.Min(playerFrameData.frame - mostRecentFrame,8);
            playerData.RemoveRange(8 - frameDifference, frameDifference);
            for(int x = 1; x < frameDifference; x++)
            {
                playerData.Insert(0, null);
            }
            PlayerData insertData = new PlayerData() {
                Position = playerFrameData.position,
                Frame = playerFrameData.frame,
                Rotation = playerFrameData.rotation
            };
            playerData.Insert(0, insertData);
            
        }
        print("-----------------------------------");
        foreach(PlayerData data in playerData)
        {
            if (data != null)
            {
                print(data.Position);
            }
            else
            {
                print("null");
            }
        }
    }

    [Command]
    void CmdUpdateAuthPlayerData()
    {
        //authPlayerData = new PlayerStruct(new Vector3(0, 5, 0),DateTime.Now.Ticks, Quaternion.Euler(new Vector3(0,45,0)));
    }
    void ForcePlayerData(PlayerStruct oldData, PlayerStruct newData)
    {
        currentState = State.playerOverride;
    }

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            long ticks = DateTime.Now.Ticks;
            int currentFrame = CalculateFrame(ticks - startingTime);
            //CmdSendCurrentFrame(currentFrame,ticks);
            PlayerStruct newPlayerData = new PlayerStruct(rb.position, currentFrame, rb.rotation);
            
            //CmdUpdatePlayerData(newPlayerData);
        }
    }

    int CalculateFrame(long tick)
    {
        return (int)(tick / 10000 / 16.667f);
    }

    [Command]
    void CmdSetKinematic()
    {
        RpcSetKinematic();
    }

    [ClientRpc]
    public void RpcSetKinematic()
    {
        if (!hasAuthority)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
            }
        }
    }
}
