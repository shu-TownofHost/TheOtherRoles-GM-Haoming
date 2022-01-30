using HarmonyLib;
using Hazel;
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
    public class SchrodingersCat : RoleBase<SchrodingersCat>
    {
        public static Color color = Color.grey;
        public static bool impostorFlag = false;
        public static bool jackalFlag = false;
        public static bool crewFlag = false;

        public SchrodingersCat()
        {
            RoleType = roleId = RoleId.SchrodingersCat;
        }

        public override void OnMeetingStart() { }
        public override void OnMeetingEnd() { }
        public override void FixedUpdate() { }
        public override void OnKill(PlayerControl target) { }
        public override void OnDeath(PlayerControl killer = null)
        {
            if(impostorFlag|| jackalFlag|| crewFlag) return;
            if(killer == null)
            {
                crewFlag = true;
            }
            else
            {
                if(killer.isImpostor())
                {
                    impostorFlag = true;
                }
                else if(killer.isRole(RoleId.Jackal))
                {
                    jackalFlag = true;
                }
                else if(killer.isRole(RoleId.Sheriff))
                {
                    crewFlag = true;
                }
                PlayerControl.LocalPlayer.Revive();
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CleanBody, Hazel.SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.cleanBody(PlayerControl.LocalPlayer.PlayerId);
            }
        }

        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

        public static void MakeButtons(HudManager hm) { }
        public static void SetButtonCooldowns() { }

        public static void Clear()
        {
            players = new List<SchrodingersCat>();
            impostorFlag = false;
            crewFlag = false;
            jackalFlag = false;
        }
    }
}