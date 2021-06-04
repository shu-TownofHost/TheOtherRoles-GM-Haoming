using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Hazel;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles {
    [HarmonyPatch(typeof(DoorCardSwipeGame), nameof(DoorCardSwipeGame.Begin))]
    public class DoorCardSwipeGamePatch{
        [HarmonyPrefix]
        public static void Prefix(DoorCardSwipeGame __instance, PlayerTask task){
            System.Console.WriteLine("DoorCardSwipeGameBegin");
            if(Mifune.mifune != null && PlayerControl.LocalPlayer == Mifune.mifune){
                if(Mifune.toggle){
                    Mifune.senrigan();
                }
            }
            if(PlayerControl.LocalPlayer.Data.IsDead){
                if(SoulPlayer.toggle){
                    SoulPlayer.senrigan();
                }
            }
        }
    }

    [HarmonyPatch(typeof(DoorBreakerGame), nameof(DoorBreakerGame.Start))]
    public class DoorBreakerGamePatch{
        [HarmonyPrefix]
        public static void Prefix(DoorBreakerGame __instance){
            System.Console.WriteLine("DoorbreakerGameBegin");
            if(Mifune.mifune != null && PlayerControl.LocalPlayer == Mifune.mifune){
                if(Mifune.toggle){
                    Mifune.senrigan();
                }
            }
            if(PlayerControl.LocalPlayer.Data.IsDead){
                if(SoulPlayer.toggle){
                    SoulPlayer.senrigan();
                }
            }
        }

    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CoStartMeeting))]
    class StartMeetingPatch {
        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)]GameData.PlayerInfo meetingTarget) {
            if(Mifune.mifune != null && PlayerControl.LocalPlayer == Mifune.mifune){
                if(Mifune.toggle){
                    Mifune.senrigan();
                }
            }
        }
    }

    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    class MinigameBeginPatch{
        static void Prefix(Minigame __instance){
            if(Mifune.mifune != null && PlayerControl.LocalPlayer == Mifune.mifune){
                if(Mifune.toggle){
                    Mifune.senrigan();
                }
            }
            if(PlayerControl.LocalPlayer.Data.IsDead){
                if(SoulPlayer.toggle){
                    SoulPlayer.senrigan();
                }
            }
        }
    }
     [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
     public static class MifuneVentUsePatch {
         public static void Prefix(Vent __instance) {
             // ベントに入ると千里眼を解除する
             if(Mifune.mifune != null && PlayerControl.LocalPlayer == Mifune.mifune){
                 if(Mifune.toggle){
                     Mifune.senrigan();
                 }
             }
         }
     }
}