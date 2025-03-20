using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static bool bCaught = false;
    public static bool ice_semi_key = false;
    public static bool lava_key = false;
    public static bool ice_key = false;
    public static bool invisible_key = false;
    public static bool checkpoint_normal = false;
    public static bool checkpoint_ice = false;
    public static bool checkpoint_lava = false;
    public static bool player_normal = false;
    public static bool player_ice = false;
    public static bool player_lava = false;

    public static void ResetVariables()
    {
        bCaught = false;
        ice_semi_key = false;
        lava_key = false;
        ice_key = false;
        invisible_key = false;
        checkpoint_normal = false;
        checkpoint_ice = false;
        checkpoint_lava = false;
        player_normal = false;
        player_ice = false;
        player_lava = false;
    }

    public static void DroppedCheckpoint()
    {
        checkpoint_normal = player_normal;
        checkpoint_ice = player_ice;
        checkpoint_lava = player_lava;
    }

    public static void ObtainedLavaKey()
    {
        lava_key = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.ChangeKeyMaterial("Final_Door/Cube2/Sphere", "lava");
        CheckWin();
    }

    public static void PushedIceKey()
    {
        ice_key = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.ChangeKeyMaterial("Final_Door/Cube4/Sphere", "ice");
        CheckWin();
    }

    public static void PushedInvisibleKey()
    {
        invisible_key = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.ChangeKeyMaterial("Final_Door/Cube1/Sphere", "invisible");
        CheckWin();
    }

    static void CheckWin()
    {
        if (lava_key && ice_key && invisible_key)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.Win();
        }
    }
}
