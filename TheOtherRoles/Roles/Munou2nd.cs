using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class Munou2nd: RoleBase<Munou2nd>
    {
        public static Color color = Color.grey;
        public static bool endGameFlag = false;
        public static bool randomColorFlag = false;
        public static Dictionary<byte, byte> randomPlayers = new Dictionary<byte, byte>();


        public Munou2nd()
        {
            RoleType = roleId = RoleId.Munou2nd;
        }

        public override void OnMeetingStart() { }
        public override void OnMeetingEnd()
        {
            if(PlayerControl.LocalPlayer.isRole(RoleId.Munou2nd) && PlayerControl.LocalPlayer.isAlive())
            {
                randomColors();
            }
        }

        public override void FixedUpdate()
        {
            // if(PlayerControl.LocalPlayer.isRole(RoleId.Munou2nd) && PlayerControl.LocalPlayer.isAlive())
            // {
            //     if(!randomColorFlag)
            //     {
            //         randomColors();
            //     }
            // }
        }
        public override void OnKill(PlayerControl target) { }
        public override void OnDeath(PlayerControl killer = null)
        {
            if(PlayerControl.LocalPlayer.isRole(RoleId.Munou2nd)) resetColors();
        }
        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

        public static void MakeButtons(HudManager hm) { }
        public static void SetButtonCooldowns() { }

        public static void Clear()
        {
            players = new List<Munou2nd>();
            randomPlayers = new Dictionary<byte, byte>();
            endGameFlag = false;
            resetColors();
        }

        public static void randomColors(){
            var allPlayers = PlayerControl.AllPlayerControls;
            List<byte> alivePlayers = new List<byte>();
            List<int> tempList = new List<int>();
            foreach(var p in allPlayers)
            {
                if(p.isAlive()) alivePlayers.Add(p.PlayerId);
            }
            foreach(byte id in alivePlayers)
            {
                if(id == PlayerControl.LocalPlayer.PlayerId) continue;
                var p = Helpers.playerById(id);
                int rnd;
                while(true){
                    rnd = TheOtherRoles.rnd.Next(alivePlayers.Count);
                    if(alivePlayers[rnd] == PlayerControl.LocalPlayer.PlayerId) continue;
                    if(!tempList.Contains(rnd))
                    {
                        tempList.Add(rnd);
                        break;
                    }
                }
                var to =Helpers.playerById((byte)alivePlayers[rnd]);
                MorphHandler.morphToPlayer(p, to);
            }
            randomColorFlag = true;
        }
        public static void resetColors(){
            var allPlayers = PlayerControl.AllPlayerControls;
            foreach(var p in allPlayers)
            {
                MorphHandler.morphToPlayer(p, p);
            }
            randomColorFlag = false;
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public class OnGameEndPatch
        {

            public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                    Munou2nd.endGameFlag = true;
            }
        }
    }
}