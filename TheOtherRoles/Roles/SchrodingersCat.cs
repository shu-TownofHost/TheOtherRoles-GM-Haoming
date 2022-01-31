using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.Patches.PlayerControlFixedUpdatePatch;

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
        public override void FixedUpdate()
        {
            if (player == PlayerControl.LocalPlayer && !isTeamJackalAlive())
            {
                currentTarget = setTarget();
                setPlayerOutline(currentTarget, Sheriff.color);
            }
        }

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
                player.Revive();
                DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                for (int i = 0; i < array.Length; i++) {
                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == player.PlayerId) {
                        array[i].gameObject.active = false;
                    }     
                }
            }
        }

        public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

        private static CustomButton jackalKillButton;
        public static PlayerControl currentTarget;
        public static void MakeButtons(HudManager hm)
        {
                jackalKillButton = new CustomButton(
                () =>
                {
                    if (Helpers.checkMuderAttemptAndKill(PlayerControl.LocalPlayer, SchrodingersCat.currentTarget) == MurderAttemptResult.SuppressKill) return;

                    jackalKillButton.Timer = jackalKillButton.MaxTimer;
                    Jackal.currentTarget = null;
                },
                () => { return  jackalFlag && !isTeamJackalAlive() && PlayerControl.LocalPlayer.isRole(RoleId.SchrodingersCat) && PlayerControl.LocalPlayer.isAlive(); },
                () => { return SchrodingersCat.currentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { jackalKillButton.Timer = jackalKillButton.MaxTimer; },
                hm.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                hm,
                hm.KillButton,
                KeyCode.Q
            );
        }
        public static void SetButtonCooldowns()
        {
            jackalKillButton.MaxTimer = Jackal.cooldown;
        }

        public static void Clear()
        {
            players = new List<SchrodingersCat>();
            impostorFlag = false;
            crewFlag = false;
            jackalFlag = false;
        }

        public static bool isTeamJackalAlive()
        {
            foreach(var p in PlayerControl.AllPlayerControls)
            {
                if(p.isRole(RoleId.Jackal) && p.isAlive()){
                    return true;
                }
                else if(p.isRole(RoleId.Sidekick) && p.isAlive()){
                    return true;
                }
            }
            return false;
        }
    }
}