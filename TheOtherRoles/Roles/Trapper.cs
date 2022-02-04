using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public class Trapper : RoleBase<Trapper>
    {
        public static Color color = Palette.CrewmateBlue;
        public static Sprite trapButtonSprite;
        public static bool placeTrap;
        public static AudioClip test;

        public static bool meetingFlag;
        public static float baseTrueSpeed = 0.0f;
        public static Dictionary<GameObject, byte> activatedTrap;
        

        public Trapper()
        {
            RoleType = roleId = RoleId.NoRole;
        }

        public override void OnMeetingStart()
        {
            meetingFlag = true;
        }
        public override void OnMeetingEnd() 
        {
            unsetTrap();
            meetingFlag = false;
        }

        public override void FixedUpdate() 
        {
            if (PlayerControl.LocalPlayer.isRole(RoleId.Trapper))
            {
                // トラップを踏んだプレイヤーを動けなくする 
                foreach(var p in PlayerControl.AllPlayerControls)
                {
                    if(p == PlayerControl.LocalPlayer) continue;
                    var p1 = p.transform.localPosition;
                    int index = 0;
                    foreach(var trapeffect in TrapEffect.trapeffects)
                    {
                        if(activatedTrap.Keys.Contains(trapeffect.trapeffect)) continue;
                        var p2 = trapeffect.trapeffect.transform.localPosition;
                        var distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5)
                        {
                            activatedTrap.Add(trapeffect.trapeffect, p.PlayerId);
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ActivateTrap, Hazel.SendOption.Reliable, -1);
                            writer.Write(PlayerControl.LocalPlayer.PlayerId);
                            writer.Write(p.PlayerId);
                            writer.Write((byte)index);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.activateTrap(PlayerControl.LocalPlayer.PlayerId, p.PlayerId, (byte)index);
                        }
                    }
                    index++;
                }

                // トラップにかかっているプレイヤーを救出する
                HashSet<GameObject> listRemove = new HashSet<GameObject>();
                foreach(var trap in activatedTrap.Keys)
                {
                    Vector3 p1 = trap.transform.position;
                    foreach(var player in PlayerControl.AllPlayerControls)
                    {
                        if (player.PlayerId == activatedTrap[trap]) continue;
                        Vector3 p2 = player.transform.position;
                        float distance = Vector3.Distance(p1, p2);
                        if(distance < 0.5) listRemove.Add(trap);
                    }
                }
                foreach(var trap in listRemove)
                {
                    activatedTrap.Remove(trap);
                    trap.SetActive(false);
                    GameObject.Destroy(trap);
                    TrapEffect.trapeffects.RemoveAll(x => x.trapeffect==trap);
                }
            }
        }
    public override void OnKill(PlayerControl target) { }
    public override void OnDeath(PlayerControl killer = null) { }
    public override void HandleDisconnect(PlayerControl player, DisconnectReasons reason) { }

    public static CustomButton trapperSetTrapButton;
    public static void MakeButtons(HudManager hm)
    {
        trapperSetTrapButton = new CustomButton(
            () => { // ボタンが押された時に実行
                Trapper.setTrap();
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
            },
            () => { /*ボタン有効になる条件*/return PlayerControl.LocalPlayer.isRole(RoleId.Trapper) && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { /*ボタンが使える条件*/
                return PlayerControl.LocalPlayer.CanMove;
            },
            () => { /*ミーティング終了時*/
                trapperSetTrapButton.Timer = trapperSetTrapButton.MaxTimer;
                Trapper.unsetTrap();
                TrapEffect.clearTrapEffects();
            },
            Trapper.getTrapButtonSprite(),
            new Vector3(-2.6f, 0f, 0f),
            hm,
            hm.AbilityButton,
            KeyCode.F
        );
    }
    public static void SetButtonCooldowns()
    {
        trapperSetTrapButton.MaxTimer = 5f;
    }

        public static void Clear()
        {
            players = new List<Trapper>();
            test = FileImporter.ImportWAVAudio("TheOtherRoles.Resources.test.wav", false);
            meetingFlag = false;
            activatedTrap = new Dictionary<GameObject, byte>();
        }

        public static Sprite getTrapButtonSprite() {
            if (trapButtonSprite) return trapButtonSprite;
            trapButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapperTrapButton.png", 115f);
            return trapButtonSprite;
        }
        public static void setTrap(){
            var pos = PlayerControl.LocalPlayer.transform.position;
            byte[] buff = new byte[sizeof(float) * 2];
            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceTrap, Hazel.SendOption.Reliable);
            writer.WriteBytesAndSize(buff);
            writer.EndMessage();
            RPCProcedure.placeTrap(buff);
        }
        public static void unsetTrap(){
            TrapEffect.clearTrapEffects();
        }
        
    }
}