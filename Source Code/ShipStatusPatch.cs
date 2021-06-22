using HarmonyLib;
using static TheOtherRoles.TheOtherRoles;
using Hazel;
using UnityEngine;

namespace TheOtherRoles {

    [HarmonyPatch(typeof(ShipStatus))]
    public class ShipStatusPatch {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo player) {
            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.Electrical) ? __instance.Systems[SystemTypes.Electrical] : null;
            if (systemType == null) return true;
            SwitchSystem switchSystem = systemType.TryCast<SwitchSystem>();
            if (switchSystem == null) return true;

            float num = (float)switchSystem.Value / 255f;
            
            if (player == null || player.IsDead) // IsDead
                __result = __instance.MaxLightRadius;
            else if (player.IsImpostor
                || (Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId && Jackal.hasImpostorVision)
                || (Sidekick.sidekick != null && Sidekick.sidekick.PlayerId == player.PlayerId && Sidekick.hasImpostorVision)
                || (Spy.spy != null && Spy.spy.PlayerId == player.PlayerId && Spy.hasImpostorVision)) // Impostor, Jackal/Sidekick or Spy with Impostor vision
                __result = __instance.MaxLightRadius * PlayerControl.GameOptions.ImpostorLightMod;
            else if (Lighter.lighter != null && Lighter.lighter.PlayerId == player.PlayerId && Lighter.lighterTimer > 0f) // if player is Lighter and Lighter has his ability active
                __result = Mathf.Lerp(__instance.MaxLightRadius * Lighter.lighterModeLightsOffVision, __instance.MaxLightRadius * Lighter.lighterModeLightsOnVision, num);
            else if (Trickster.trickster != null && Trickster.lightsOutTimer > 0f) {
                float lerpValue = 1f;
                if (Trickster.lightsOutDuration - Trickster.lightsOutTimer < 0.5f) lerpValue = Mathf.Clamp01((Trickster.lightsOutDuration - Trickster.lightsOutTimer) * 2);
                else if (Trickster.lightsOutTimer < 0.5) lerpValue = Mathf.Clamp01(Trickster.lightsOutTimer*2);
                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1 - lerpValue) * PlayerControl.GameOptions.CrewLightMod; // Instant lights out? Maybe add a smooth transition?
            }
            else
                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, num) * PlayerControl.GameOptions.CrewLightMod;
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.IsGameOverDueToDeath))]
        public static void Postfix2(ShipStatus __instance, ref bool __result)
        {
            __result = false;
        }

        private static int originalNumCommonTasksOption = 0;
        private static int originalNumShortTasksOption = 0;
        private static int originalNumLongTasksOption = 0;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        public static bool Prefix(ShipStatus __instance)
        {
            var commonTaskCount = __instance.CommonTasks.Count;
            var normalTaskCount = __instance.NormalTasks.Count;
            var longTaskCount = __instance.LongTasks.Count;
            originalNumCommonTasksOption = PlayerControl.GameOptions.NumCommonTasks;
            originalNumShortTasksOption = PlayerControl.GameOptions.NumShortTasks;
            originalNumLongTasksOption = PlayerControl.GameOptions.NumLongTasks;
            if(PlayerControl.GameOptions.NumCommonTasks > commonTaskCount) PlayerControl.GameOptions.NumCommonTasks = commonTaskCount;
            if(PlayerControl.GameOptions.NumShortTasks > normalTaskCount) PlayerControl.GameOptions.NumShortTasks = normalTaskCount;
            if(PlayerControl.GameOptions.NumLongTasks > longTaskCount) PlayerControl.GameOptions.NumLongTasks = longTaskCount;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        public static void Postfix3(ShipStatus __instance)
        {
            // Restore original settings after the tasks have been selected
            PlayerControl.GameOptions.NumCommonTasks = originalNumCommonTasksOption;
            PlayerControl.GameOptions.NumShortTasks = originalNumShortTasksOption;
            PlayerControl.GameOptions.NumLongTasks = originalNumLongTasksOption;
        }

        // Polusの湧き位置をランダムにする
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.SpawnPlayer))]
        public static void Postfix4(ShipStatus __instance, PlayerControl player, int numPlayers, bool initialSpawn){
            if(PlayerControl.GameOptions.MapId == 2 && CustomOptionHolder.polusRandomSpawn.getBool()){
                if(AmongUsClient.Instance.AmHost){
                    System.Random rand = new System.Random();
                    int randVal = rand.Next(0,6);
                    System.Console.WriteLine("spawnPlayer");
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RandomSpawn, Hazel.SendOption.Reliable, -1);
                    writer.Write((byte)player.Data.PlayerId);
                    writer.Write((byte)randVal);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.randomSpawn((byte)player.Data.PlayerId, (byte)randVal);
                }
                PolusAdditionalVents vents1 = new PolusAdditionalVents(new Vector3(36.54f, -21.77f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // Specimen
                PolusAdditionalVents vents2 = new PolusAdditionalVents(new Vector3(16.64f, -2.46f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // InitialSpawn
                PolusAdditionalVents vents3 = new PolusAdditionalVents(new Vector3(26.67f, -17.34f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // Vital
                //vents1.vent.Right = vents2.vent; // Specimen - InitialSpawn
                vents1.vent.Left = vents3.vent; // Specimen - Vital
                //vents2.vent.Right = vents1.vent; // InitialSpawn - Specimen
                vents2.vent.Center = vents3.vent; // InitialSpawn - Vital
                vents3.vent.Right = vents1.vent; // Vital - Specimen
                vents3.vent.Left = vents2.vent; // Vital - InitialSpawn
            }
        }
    }
}