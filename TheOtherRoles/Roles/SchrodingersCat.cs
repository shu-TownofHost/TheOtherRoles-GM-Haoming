using HarmonyLib;
using Hazel;
using System;
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
        public enum exileType
        {
            None = 0,
            Crew = 1,
            Random = 2,
        }
        public static Color color = Color.grey;
        public static bool impostorFlag = false;
        public static bool jackalFlag = false;
        public static bool crewFlag = false;
        public static float killCooldown {get {return CustomOptionHolder.schrodingersCatKillCooldown.getFloat();}}
        public static bool becomesImpostor {get {return CustomOptionHolder.schrodingersCatBecomesImpostor.getBool();}}
        public static exileType becomesWhichTeamsOnExiled {get {return (exileType)CustomOptionHolder.schrodingersCatBecomesWhichTeamsOnExiled.getSelection();}}
        public static bool cantKillUntilLastOne {get {return CustomOptionHolder.schrodingersCatCantKillUntilLastOne.getBool();}}
        public static bool killsKiller {get {return CustomOptionHolder.schrodingersCatKillsKiller.getBool();}}
        public static bool justDieOnKilledByCrew {get {return CustomOptionHolder.schrodingersCatJustDieOnKilledByCrew.getBool();}}
        public static PlayerControl killer = null;

        public SchrodingersCat()
        {
            RoleType = roleId = RoleType.SchrodingersCat;
        }

        public override void OnMeetingStart() { }
        public override void OnMeetingEnd() 
        {
            if (PlayerControl.LocalPlayer.isRole(RoleType.SchrodingersCat))
                PlayerControl.LocalPlayer.SetKillTimerUnchecked(killCooldown);
        }
        public override void FixedUpdate()
        {
            if (player == PlayerControl.LocalPlayer && jackalFlag)
            {
                if(!isTeamJackalAlive() || !cantKillUntilLastOne)
                {
                    currentTarget = setTarget();
                    setPlayerOutline(currentTarget, Sheriff.color);
                }
            }
            if (player == PlayerControl.LocalPlayer && impostorFlag && !isLastImpostor() && cantKillUntilLastOne)
            {
                HudManager.Instance.KillButton.SetTarget(null);
            }
        }

        public override void OnKill(PlayerControl target) 
        {
            if (PlayerControl.LocalPlayer == player && impostorFlag)
                player.SetKillTimerUnchecked(killCooldown);
        }
        public override void OnDeath(PlayerControl killer = null)
        { 
            // 占い師の画面では呪殺したことを分からなくするために自殺処理させているので注意すること
            if(impostorFlag|| jackalFlag|| crewFlag) return;
            if(((killer != null && killer.isCrew()) || killer.isRole(RoleType.SchrodingersCat)) && justDieOnKilledByCrew) return;
            if(killer == null)
            {
                if(becomesWhichTeamsOnExiled == exileType.Random)
                {
                    int rndVal = Jackal.jackal != null ? rnd.Next(0, 2): rnd.Next(0, 1);
                    switch(rndVal)
                    {
                        case 0:
                            setCrewFlag();
                            break;
                        case 1:
                            setImpostorFlag();
                            break;
                        case 2:
                            setJackalFlag();
                            break;
                        default:
                            setCrewFlag();
                            break;
                    }
                }
                else if(becomesWhichTeamsOnExiled == exileType.Crew)
                {
                    setCrewFlag();
                }
                return;
            }
            else
            {
                bool isCrewOrSchrodingersCat = killer.isCrew() || killer.isRole(RoleType.SchrodingersCat);
                if(killer.isImpostor())
                {
                    setImpostorFlag();
                    if(becomesImpostor)
                        DestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Impostor);
                }
                else if(killer.isRole(RoleType.Jackal))
                {
                    setJackalFlag();
                }
                else if(isCrewOrSchrodingersCat)
                {
                    setCrewFlag();
                }

                // EndGamePatchでゲームを終了させないために先にkillerに値を代入する
                if(SchrodingersCat.killsKiller && !isCrewOrSchrodingersCat)
                    SchrodingersCat.killer = killer;


                // 蘇生する
                player.Revive();
                // 死体を消す
                DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                for (int i = 0; i < array.Length; i++) {
                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == player.PlayerId) {
                        array[i].gameObject.active = false;
                    }     
                }
                if(SchrodingersCat.killsKiller && !isCrewOrSchrodingersCat)
                {
                    if(PlayerControl.LocalPlayer == killer){
                        // 死亡までのカウントダウン
                        TMPro.TMP_Text text;
                        RoomTracker roomTracker =  HudManager.Instance?.roomTracker;
                        GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);
                        UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                        gameObject.transform.SetParent(HudManager.Instance.transform);
                        gameObject.transform.localPosition = new Vector3(0, -1.8f, gameObject.transform.localPosition.z);
                        gameObject.transform.localScale = Vector3.one * 3f;
                        text = gameObject.GetComponent<TMPro.TMP_Text>();
                        HudManager.Instance.StartCoroutine(Effects.Lerp(15f, new Action<float>((p) => {
                            string message = (15 -(p * 15f)).ToString("0");
                            bool even = ((int)(p * 15f / 0.25f)) % 2 == 0; // Bool flips every 0.25 seconds
                            string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                            text.text = prefix + message + "</color>";
                            if (text != null) text.color = even ? Color.yellow : Color.red;
                            if (p == 1f && text != null && text.gameObject != null) {
                                if(SchrodingersCat.killer != null && SchrodingersCat.killer.isAlive())
                                {
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SchrodingersCatSuicide, Hazel.SendOption.Reliable, -1);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.schrodingersCatSuicide();
                                    SchrodingersCat.killer = null;
                                }
                                UnityEngine.Object.Destroy(text.gameObject);
                            }
                        })));
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
                () => { return isJackalButtonEnable(); },
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
            jackalKillButton.MaxTimer = killCooldown;
        }

        public static void Clear()
        {
            players = new List<SchrodingersCat>();
            impostorFlag = false;
            crewFlag = false;
            jackalFlag = false;
            RoleInfo.schrodingersCat.color = color;
            killer = null;
        }

        public static void setImpostorFlag()
        {
            impostorFlag = true;
            RoleInfo.schrodingersCat.color = Palette.ImpostorRed;
        }

        public static void setCrewFlag()
        {
            crewFlag = true;
            RoleInfo.schrodingersCat.color = Color.white;
        }

        public static void setJackalFlag()
        {
            jackalFlag = true;
            RoleInfo.schrodingersCat.color = Jackal.color;
        }

        public static bool isJackalButtonEnable()
        {
            if(jackalFlag && PlayerControl.LocalPlayer.isRole(RoleType.SchrodingersCat) && PlayerControl.LocalPlayer.isAlive())
            {
                if(!isTeamJackalAlive() || !cantKillUntilLastOne )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isTeamJackalAlive()
        {
            foreach(var p in PlayerControl.AllPlayerControls)
            {
                if(p.isRole(RoleType.Jackal) && p.isAlive()){
                    return true;
                }
                else if(p.isRole(RoleType.Sidekick) && p.isAlive()){
                    return true;
                }
            }
            return false;
        }

        public static bool isLastImpostor()
        {
            foreach(var p in PlayerControl.AllPlayerControls)
            {
                if(PlayerControl.LocalPlayer != p && p.isImpostor() && p.isAlive()) return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
        class PlayerControlCmdReportDeadBodyPatch
        {
            public static void Prefix(PlayerControl __instance)
            {
                // 時限爆弾よりも前にミーティングが来たら直後に死亡する
                if(killer != null && killsKiller){
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SchrodingersCatSuicide, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.schrodingersCatSuicide();
                    killer = null;
                }
            }
        }
    }
}