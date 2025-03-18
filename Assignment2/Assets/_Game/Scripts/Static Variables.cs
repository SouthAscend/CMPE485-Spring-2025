using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static bool bCaught = false;
    public static bool ice_semi_key = false;
    public static bool lava_key = false;
    public static bool ice_key = false;
    public static bool checkpoint_normal = false;
    public static bool checkpoint_ice = false;
    public static bool checkpoint_lava = false;
    public static bool player_normal = false;
    public static bool player_ice = false;
    public static bool player_lava = false;

    public static void DroppedCheckpoint()
    {
        checkpoint_normal = player_normal;
        checkpoint_ice = player_ice;
        checkpoint_lava = player_lava;
    }
}
