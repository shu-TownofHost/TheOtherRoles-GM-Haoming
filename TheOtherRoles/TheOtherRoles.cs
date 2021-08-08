using System.Net;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using TheOtherRoles.Objects;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public static class TheOtherRoles
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void clearAndReloadRoles() {
            Jester.clearAndReload();
            Mayor.clearAndReload();
            Engineer.clearAndReload();
            Sheriff.clearAndReload();
            Lighter.clearAndReload();
            Godfather.clearAndReload();
            Mafioso.clearAndReload();
            Janitor.clearAndReload();
            Detective.clearAndReload();
            TimeMaster.clearAndReload();
            Medic.clearAndReload();
            Shifter.clearAndReload();
            Swapper.clearAndReload();
            Lovers.clearAndReload();
            Seer.clearAndReload();
            Morphling.clearAndReload();
            Camouflager.clearAndReload();
            Hacker.clearAndReload();
            Mini.clearAndReload();
            Tracker.clearAndReload();
            Vampire.clearAndReload();
            Snitch.clearAndReload();
            Jackal.clearAndReload();
            Sidekick.clearAndReload();
            Eraser.clearAndReload();
            Spy.clearAndReload();
            Trickster.clearAndReload();
            Cleaner.clearAndReload();
            Warlock.clearAndReload();
            SecurityGuard.clearAndReload();
            Arsonist.clearAndReload();
            Guesser.clearAndReload();
            BountyHunter.clearAndReload();
            Bait.clearAndReload();
            Madmate.clearAndReload();
            Madmate2.clearAndReload();
            Misimo.clearAndReload();
            Ballad.clearAndReload();
            Predator.clearAndReload();
            Bomber.clearAndReload();
            Trapper.clearAndReload();
            Mifune.clearAndReload();
            Munou.clearAndReload();
            SoulPlayer.clearAndReload();
            ImpostorPlayer.clearAndReload();
        }
        public static class ImpostorPlayer{
            public static List<Arrow> arrows = new List<Arrow>();
            public static float updateTimer = 1.0f;
            public static void clearAndReload(){
                arrows = new List<Arrow>();
                updateTimer = 1.0f;
            }
        }
        public static class SoulPlayer {
            public static bool toggle = false;
            public static void senrigan(){
                if(toggle){
                    toggle = !toggle;
                    Camera.main.orthographicSize /= 6f;
                    // HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
                    // HudManager.Instance.KillButton.gameObject.SetActive(true);
                    // HudManager.Instance.UseButton.gameObject.SetActive(true);
                    // HudManager.Instance.ReportButton.gameObject.SetActive(true);
                }else{
                    toggle = !toggle;
                    Camera.main.orthographicSize *= 6f;
                    // HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
                    // HudManager.Instance.KillButton.gameObject.SetActive(false);
                    // HudManager.Instance.UseButton.gameObject.SetActive(true);
                    // HudManager.Instance.ReportButton.gameObject.SetActive(false);
                }
            }
            public static void clearAndReload() {
                toggle = false;
            }
        }
        public static class Nottori{
            public static PlayerControl nottori;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            public static void clearAndReload(){
                nottori = null;
            }
        }

        public static class Kan{
            public static PlayerControl kan;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            public static void clearAndReload(){
                kan = null;
            }
        }

        public static class Munou{
            public static PlayerControl munou;
            public static Color color = new Color(255f/255f, 255f/255f, 255f/255f, 1);
            public static void clearAndReload(){
                munou = null;
            }
        }

        public static class Mifune {
            public static PlayerControl mifune;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite buttonSprite;
            public static float cooldown = 30f;
            public static float duration = 1f;
            public static bool toggle = false;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MifuneButton.png", 115f);
                return buttonSprite;
            }
            
            public static void senrigan(){
                if(toggle){
                    toggle = !toggle;
                    Camera.main.orthographicSize /= 4f;
                    HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
                    HudManager.Instance.KillButton.gameObject.SetActive(true);
                    HudManager.Instance.UseButton.gameObject.SetActive(true);
                    HudManager.Instance.ReportButton.gameObject.SetActive(true);
                }else{
                    toggle = !toggle;
                    Camera.main.orthographicSize *= 4f;
                    HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
                    HudManager.Instance.KillButton.gameObject.SetActive(true);
                    HudManager.Instance.UseButton.gameObject.SetActive(true);
                    HudManager.Instance.ReportButton.gameObject.SetActive(true);
                }
            }

            public static void clearAndReload() {
                cooldown = CustomOptionHolder.mifuneCooldown.getFloat();
                duration = CustomOptionHolder.mifuneDuration.getFloat();
                toggle = false;
                mifune = null;
            }
        }
        public static class Trapper {
            public static PlayerControl trapper;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite trapButtonSprite;
            private static Sprite unsetButtonSprite;
            public static float cooldown = 30f;
            public static Vector3 trap;
            public static Vector3 zero = new Vector3(0, 0, 0);
            public static float baseTrueSpeed = 0.0f;
            public static bool meetingFlag = false;
            public static Vector3 basePos = new Vector3(0, 0, 0);
            public static Sprite getTrapButtonSprite() {
                if (trapButtonSprite) return trapButtonSprite;
                trapButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapperTrapButton.png", 115f);
                return trapButtonSprite;
            }
            public static Sprite getUnsetButtonSprite() {
                if (unsetButtonSprite) return unsetButtonSprite;
                unsetButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrapperUnsetButton.png", 115f);
                return unsetButtonSprite;
            }
            public static void setTrap(){
                trap = PlayerControl.LocalPlayer.transform.position;
                new TrapEffect(PlayerControl.LocalPlayer);
            }
            public static void unsetTrap(){
                if(trap != Trapper.zero){
                    if(TrapEffect.trapeffects.Count > 0){
                        TrapEffect.trapeffects[TrapEffect.trapeffects.Count-1].trapeffect.SetActive(false);
                        TrapEffect.trapeffects.RemoveAt(TrapEffect.trapeffects.Count-1);
                    }
                    trap = new Vector3(0, 0, 0);
                }
            }
            public static Vector3 getButtonPos(){
                if(CustomOptionHolder.trapperUnmoveable.getBool())
                    return new Vector3(-2.6f, 0f, 0f);
                return new Vector3(-2.6f, 0f, 0f);
            }

            public static void clearAndReload() {
                trapper = null;
                trap = new Vector3(0, 0, 0);
                cooldown = CustomOptionHolder.trapperCooldown.getFloat();
                baseTrueSpeed = 0.0f;
                basePos = new Vector3(0, 0, 0);
                meetingFlag = false;
            }
        }

        public static class Bomber {
            public static PlayerControl bomber;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite plantButtonSprite;
            private static Sprite detonateButtonSprite;
            public static List<PlayerControl> targets;
            public static PlayerControl currentTarget;
            public static PlayerControl plantTarget;
            public static float plantDuration;
            public static float plantCooldown;

            public static bool isSet;
            public static bool isAOE;
            public static float cooldown = 30f;
            public static Dictionary<byte, PoolablePlayer> plantedIcons = new Dictionary<byte, PoolablePlayer>();
            public static Sprite getDetonateButtonSprite() {
                if (detonateButtonSprite) return detonateButtonSprite;
                detonateButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DetonateButton.png", 115f);
                return detonateButtonSprite;
            }
            public static Sprite getPlantBombButtonSprite() {
                if (plantButtonSprite) return plantButtonSprite;
                plantButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlantBombButton.png", 115f);
                return plantButtonSprite;
            }

            public static void clearAndReload() {
                bomber = null;
                targets = new List<PlayerControl>();
                isSet = false;
                plantTarget = null;
                currentTarget = null;
                foreach (PoolablePlayer p in plantedIcons.Values) {
                    if (p != null && p.gameObject != null) { 
                        UnityEngine.Object.Destroy(p.gameObject);
                    }
                }
                plantDuration = CustomOptionHolder.bomberPlantDuration.getFloat();
                plantCooldown = CustomOptionHolder.bomberPlantCooldown.getFloat();
                isAOE = CustomOptionHolder.bomberAOE.getBool();
            }
            public static void setTarget(){
                if(Bomber.plantTarget != null)
                    Bomber.plantedIcons[Bomber.plantTarget.PlayerId].setSemiTransparent(false);
                    Bomber.targets.Add(Bomber.plantTarget);
                Bomber.plantTarget = null;
            }

            public static void detonate(){
                foreach(PlayerControl target in targets){
                    if(!target.Data.IsDead){
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BomberKill, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(target.Data.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.bomberKill(target.Data.PlayerId);
                        if(isAOE){
                            foreach(PlayerControl player in PlayerControl.AllPlayerControls){
                                if(Vector2.Distance(target.transform.position, player.transform.position) <= 1f && !player.Data.IsDead && player != target){
                                    killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BomberKill, Hazel.SendOption.Reliable, -1);
                                    killWriter.Write(player.Data.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                                    RPCProcedure.bomberKill(player.Data.PlayerId);
                                }
                            }
                        }
                    }
                }

                targets = new List<PlayerControl>();
                try{
                    foreach (PoolablePlayer p in plantedIcons.Values) {
                        p.setSemiTransparent(true);
                    }
                }catch(Exception e){
                    System.Console.WriteLine(e);
                }

            }
        }
        public static class Ballad {
            public static PlayerControl ballad;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite buttonSprite;


            public static PlayerControl target;
            public static PlayerControl currentTarget;
            public static int meetingCount;
            public static int expirationCount;
            public static bool isSet;
            public static float cooldown = 30f;
            public static Dictionary<byte, PoolablePlayer> sealedIcons = new Dictionary<byte, PoolablePlayer>();
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.BalladButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                ballad = null;
                target = null;
                isSet = false;
                meetingCount = 0;
                expirationCount = 0;
                foreach (PoolablePlayer p in sealedIcons.Values) {
                    if (p != null && p.gameObject != null) { 
                        UnityEngine.Object.Destroy(p.gameObject);
                    }
                }
            }
            public static void setTarget(){
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BalladSetTarget, Hazel.SendOption.Reliable, -1);
                writer.Write(Ballad.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.balladSetTarget(Ballad.currentTarget.PlayerId);
                Ballad.isSet = true;
                Ballad.expirationCount = Ballad.meetingCount;
                if(Ballad.target != null)
                    MapOptions.playerIcons[Ballad.target.PlayerId].setSemiTransparent(false);
            }
            public static void unsetTarget(){
                if(Ballad.target != null)
                    MapOptions.playerIcons[Ballad.target.PlayerId].setSemiTransparent(true);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BalladSetTarget, Hazel.SendOption.Reliable, -1);
                writer.Write(Ballad.ballad.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.balladSetTarget(Ballad.ballad.PlayerId);
            }
        }
        public static class Misimo {
            public static PlayerControl misimo;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite buttonSprite;
            private static Sprite buttonSpriteInvisible;
            public static float cooldown = 15f;
            public static float duration = 30f;
            public static bool visibility = true;
            public static bool isCountdown = false;
            public static float invisibleCooldown = 20f;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MisimoSelfDestructButton.png", 115f);
                return buttonSprite;
            }
            public static Sprite getButtonSpriteInvisible() {
                if (buttonSpriteInvisible) return buttonSpriteInvisible;
                buttonSpriteInvisible = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MisimoInvisibleButton.png", 115f);
                return buttonSpriteInvisible;
            }

            public static void clearAndReload() {
                cooldown = CustomOptionHolder.misimoCooldown.getFloat();
                duration = CustomOptionHolder.misimoDuration.getFloat();
                isCountdown = false;
                visibility = true;
                misimo = null;
            }

            public static void selfDestruct(){
                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MisimoKill, Hazel.SendOption.Reliable, -1); killWriter.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                RPCProcedure.misimoKill(targetId);
            }
            public static void invisible(){
                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MisimoInvisible, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            public static void visible(){
                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MisimoVisible, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
        public static class Predator {
            public static PlayerControl predator;
            public static bool visibility = true;
            public static float invisibleDuration = 10.0f;
            public static float invisibleCooldown = 10.0f;
            public static Color color = new Color(255f / 255f, 00f / 255f, 00f / 255f, 1);
            private static Sprite buttonSpriteInvisible;
            private static Sprite buttonSpriteVisible;
            public static float baseSpeed = 0;
            public static float baseTrueSpeed = 0;
            public static uint hatId = 0;
            public static uint petId = 0;
            public static uint skinId = 0;
            public static int colorId = 0;
            public static string name = "";
            public static bool garlicsActive;
            public static Sprite getButtonSpriteInvisible() {
                if (buttonSpriteInvisible) return buttonSpriteInvisible;
                buttonSpriteInvisible = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MisimoInvisibleButton.png", 115f);
                return buttonSpriteInvisible;
            }
            public static Sprite getButtonSpriteVisible() {
                if (buttonSpriteVisible) return buttonSpriteVisible;
                buttonSpriteVisible = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MisimoVisibleButton.png", 115f);
                return buttonSpriteVisible;
            }
            public static void clearAndReload() {
                invisibleCooldown = CustomOptionHolder.predatorInvisibleCooldown.getFloat();
                invisibleDuration = CustomOptionHolder.predatorInvisibleDuration.getFloat();
                baseSpeed = 0;
                predator = null;
                visibility = true;
                Vampire.localPlacedGarlic = false;
                garlicsActive = CustomOptionHolder.predatorHatesGarlics.getBool();
            }
            public static void invisible(){
                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PredatorInvisible, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.predatorInvisible(targetId);
            }
            public static void visible(){
                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PredatorVisible, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.predatorVisible(targetId);
            }

        }

        public static class Madmate {
            public static PlayerControl madmate;
            public static Color color = new Color(167f / 255f, 87f / 255f, 168f / 255f, 1);

            public static void clearAndReload() {
                madmate = null;
            }
        }
        public static class Madmate2 {
            public static PlayerControl madmate2;
            public static Color color = new Color(167f / 255f, 87f / 255f, 168f / 255f, 1);

            public static void clearAndReload() {
                madmate2 = null;
            }
        }

        public static class Jester {
            public static PlayerControl jester;
            public static Color color = new Color32(236, 98, 165, byte.MaxValue);

            public static bool triggerJesterWin = false;
            public static bool canCallEmergency = true;
            public static bool canSabotage = true;

            public static void clearAndReload() {
                jester = null;
                triggerJesterWin = false;
                canCallEmergency = CustomOptionHolder.jesterCanCallEmergency.getBool();
                canSabotage = CustomOptionHolder.jesterCanSabotage.getBool();
            }
        }

        public static class Mayor {
            public static PlayerControl mayor;
            public static Color color = new Color32(32, 77, 66, byte.MaxValue);

            public static void clearAndReload() {
                mayor = null;
            }
        }

        public static class Engineer {
            public static PlayerControl engineer;
            public static Color color = new Color32(0, 40, 245, byte.MaxValue);
            public static bool usedRepair;
            private static Sprite buttonSprite;

            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.RepairButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                engineer = null;
                usedRepair = false;
            }
        }

        public static class Godfather {
            public static PlayerControl godfather;
            public static Color color = Palette.ImpostorRed;

            public static void clearAndReload() {
                godfather = null;
            }
        }

        public static class Mafioso {
            public static PlayerControl mafioso;
            public static Color color = Palette.ImpostorRed;

            public static void clearAndReload() {
                mafioso = null;
            }
        }


        public static class Janitor {
            public static PlayerControl janitor;
            public static bool clean = false;
            public static Color color = Palette.ImpostorRed;

            public static float cooldown = 30f;

            private static Sprite buttonSprite;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CleanButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                janitor = null;
                cooldown = CustomOptionHolder.janitorCooldown.getFloat();
                clean = CustomOptionHolder.janitorClean.getBool();
            }
        }

        public static class Sheriff {
            public static PlayerControl sheriff;
            public static Color color = new Color32(248, 205, 70, byte.MaxValue);

            public static float cooldown = 30f;
            public static bool canKillNeutrals = false;
            public static bool spyCanDieToSheriff = false;
            public static bool madmateCanDieToSheriff = false;

            public static PlayerControl currentTarget;

            public static void clearAndReload() {
                sheriff = null;
                currentTarget = null;
                cooldown = CustomOptionHolder.sheriffCooldown.getFloat();
                canKillNeutrals = CustomOptionHolder.sheriffCanKillNeutrals.getBool();
                spyCanDieToSheriff = CustomOptionHolder.spyCanDieToSheriff.getBool();
                madmateCanDieToSheriff = CustomOptionHolder.madmateCanDieToSheriff.getBool();
            }
        }

        public static class Lighter {
            public static PlayerControl lighter;
            public static Color color = new Color32(238, 229, 190, byte.MaxValue);
            
            public static float lighterModeLightsOnVision = 2f;
            public static float lighterModeLightsOffVision = 0.75f;

            public static float cooldown = 30f;
            public static float duration = 5f;

            public static float lighterTimer = 0f;

            private static Sprite buttonSprite;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.LighterButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                lighter = null;
                lighterTimer = 0f;
                cooldown = CustomOptionHolder.lighterCooldown.getFloat();
                duration = CustomOptionHolder.lighterDuration.getFloat();
                lighterModeLightsOnVision = CustomOptionHolder.lighterModeLightsOnVision.getFloat();
                lighterModeLightsOffVision = CustomOptionHolder.lighterModeLightsOffVision.getFloat();
            }
        }

        public static class Detective {
            public static PlayerControl detective;
            public static Color color = new Color32(45, 106, 165, byte.MaxValue);

            public static float footprintIntervall = 1f;
            public static float footprintDuration = 1f;
            public static bool anonymousFootprints = false;
            public static float reportNameDuration = 0f;
            public static float reportColorDuration = 20f;
            public static float timer = 6.2f;

            public static void clearAndReload() {
                detective = null;
                anonymousFootprints = CustomOptionHolder.detectiveAnonymousFootprints.getBool();
                footprintIntervall = CustomOptionHolder.detectiveFootprintIntervall.getFloat();
                footprintDuration = CustomOptionHolder.detectiveFootprintDuration.getFloat();
                reportNameDuration = CustomOptionHolder.detectiveReportNameDuration.getFloat();
                reportColorDuration = CustomOptionHolder.detectiveReportColorDuration.getFloat();
                timer = 6.2f;
            }
        }
    }

    public static class TimeMaster {
        public static PlayerControl timeMaster;
        public static Color color = new Color32(112, 142, 239, byte.MaxValue);

        public static bool reviveDuringRewind = false;
        public static float rewindTime = 3f;
        public static float shieldDuration = 3f;
        public static float cooldown = 30f;

        public static bool shieldActive = false;
        public static bool isRewinding = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TimeShieldButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            timeMaster = null;
            isRewinding = false;
            shieldActive = false;
            rewindTime = CustomOptionHolder.timeMasterRewindTime.getFloat();
            shieldDuration = CustomOptionHolder.timeMasterShieldDuration.getFloat();
            cooldown = CustomOptionHolder.timeMasterCooldown.getFloat();
        }
    }

    public static class Medic {
        public static PlayerControl medic;
        public static PlayerControl shielded;
        public static PlayerControl futureShielded;
        
        public static Color color = new Color32(126, 251, 194, byte.MaxValue);
        public static bool usedShield;

        public static int showShielded = 0;
        public static bool showAttemptToShielded = false;
        public static bool setShieldAfterMeeting = false;

        public static Color shieldedColor = new Color32(0, 221, 255, byte.MaxValue);
        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ShieldButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            medic = null;
            shielded = null;
            futureShielded = null;
            currentTarget = null;
            usedShield = false;
            showShielded = CustomOptionHolder.medicShowShielded.getSelection();
            showAttemptToShielded = CustomOptionHolder.medicShowAttemptToShielded.getBool();
            setShieldAfterMeeting = CustomOptionHolder.medicSetShieldAfterMeeting.getBool();
        }
    }

    public static class Shifter {
        public static PlayerControl shifter;
        public static Color color = new Color32(102, 102, 102, byte.MaxValue);

        public static PlayerControl futureShift;
        public static PlayerControl currentTarget;
        public static bool shiftModifiers = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ShiftButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            shifter = null;
            currentTarget = null;
            futureShift = null;
            shiftModifiers = CustomOptionHolder.shifterShiftsModifiers.getBool();
        }
    }

    public static class Swapper {
        public static PlayerControl swapper;
        public static Color color = new Color32(134, 55, 86, byte.MaxValue);
        private static Sprite spriteCheck;
        public static bool canCallEmergency = false;
        public static bool canOnlySwapOthers = false;

        public static byte playerId1 = Byte.MaxValue;
        public static byte playerId2 = Byte.MaxValue;

        public static Sprite getCheckSprite() {
            if (spriteCheck) return spriteCheck;
            spriteCheck = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SwapperCheck.png", 150f);
            return spriteCheck;
        }

        public static void clearAndReload() {
            swapper = null;
            playerId1 = Byte.MaxValue;
            playerId2 = Byte.MaxValue;
            canCallEmergency = CustomOptionHolder.swapperCanCallEmergency.getBool();
            canOnlySwapOthers = CustomOptionHolder.swapperCanOnlySwapOthers.getBool();
        }
    }

    public static class Lovers {
        public static PlayerControl lover1;
        public static PlayerControl lover2;
        public static Color color = new Color32(232, 57, 185, byte.MaxValue);

        public static bool bothDie = true;
        // Lovers save if next to be exiled is a lover, because RPC of ending game comes before RPC of exiled
        public static bool notAckedExiledIsLover = false;

        public static bool existing() {
            return lover1 != null && lover2 != null && !lover1.Data.Disconnected && !lover2.Data.Disconnected;
        }

        public static bool existingAndAlive() {
            return existing() && !lover1.Data.IsDead && !lover2.Data.IsDead && !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
        }

        public static bool existingWithKiller() {
            return existing() && (lover1 == Jackal.jackal     || lover2 == Jackal.jackal
                               || lover1 == Sidekick.sidekick || lover2 == Sidekick.sidekick
                               || lover1.Data.IsImpostor      || lover2.Data.IsImpostor);
        }

        public static bool hasAliveKillingLover(this PlayerControl player) {
            if (!Lovers.existingAndAlive() || !existingWithKiller())
                return false;
            return (player != null && (player == lover1 || player == lover2));
        }

        public static void clearAndReload() {
            lover1 = null;
            lover2 = null;
            notAckedExiledIsLover = false;
            bothDie = CustomOptionHolder.loversBothDie.getBool();
        }

        public static PlayerControl getPartner(this PlayerControl player) {
            if (player == null)
                return null;
            if (lover1 == player)
                return lover2;
            if (lover2 == player)
                return lover1;
            return null;
        }
    }

    public static class Seer {
        public static PlayerControl seer;
        public static Color color = new Color32(97, 178, 108, byte.MaxValue);
        public static List<Vector3> deadBodyPositions = new List<Vector3>();

        public static float soulDuration = 15f;
        public static bool limitSoulDuration = false;
        public static int mode = 0;

        private static Sprite soulSprite;
        public static Sprite getSoulSprite() {
            if (soulSprite) return soulSprite;
            soulSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        public static void clearAndReload() {
            seer = null;
            deadBodyPositions = new List<Vector3>();
            limitSoulDuration = CustomOptionHolder.seerLimitSoulDuration.getBool();
            soulDuration = CustomOptionHolder.seerSoulDuration.getFloat();
            mode = CustomOptionHolder.seerMode.getSelection();
        }
    }

    public static class Morphling {
        public static PlayerControl morphling;
        public static Color color = Palette.ImpostorRed;
        private static Sprite sampleSprite;
        private static Sprite morphSprite;
    
        public static float cooldown = 30f;
        public static float duration = 10f;

        public static PlayerControl currentTarget;
        public static PlayerControl sampledTarget;
        public static PlayerControl morphTarget;
        public static float morphTimer = 0f;

        public static void resetMorph() {
            morphTarget = null;
            morphTimer = 0f;
            if (morphling == null) return;
            morphling.SetName(morphling.Data.PlayerName);
            morphling.SetHat(morphling.Data.HatId, (int)morphling.Data.ColorId);
            Helpers.setSkinWithAnim(morphling.MyPhysics, morphling.Data.SkinId);
            morphling.SetPet(morphling.Data.PetId);
            morphling.CurrentPet.Visible = morphling.Visible;
            morphling.SetColor(morphling.Data.ColorId);
        }

        public static void clearAndReload() {
            resetMorph();
            morphling = null;
            currentTarget = null;
            sampledTarget = null;
            morphTarget = null;
            morphTimer = 0f;
            cooldown = CustomOptionHolder.morphlingCooldown.getFloat();
            duration = CustomOptionHolder.morphlingDuration.getFloat();
        }

        public static Sprite getSampleSprite() {
            if (sampleSprite) return sampleSprite;
            sampleSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SampleButton.png", 115f);
            return sampleSprite;
        }

        public static Sprite getMorphSprite() {
            if (morphSprite) return morphSprite;
            morphSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MorphButton.png", 115f);
            return morphSprite;
        }
    }

    public static class Camouflager {
        public static PlayerControl camouflager;
        public static Color color = Palette.ImpostorRed;
    
        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float camouflageTimer = 0f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CamoButton.png", 115f);
            return buttonSprite;
        }

        public static void resetCamouflage() {
            camouflageTimer = 0f;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                if (p == null) continue;
                if (Morphling.morphling == null || Morphling.morphling != p) {
                    p.SetName(p.Data.PlayerName);
                    p.SetHat(p.Data.HatId, (int)p.Data.ColorId);
                    Helpers.setSkinWithAnim(p.MyPhysics, p.Data.SkinId);
                    p.SetPet(p.Data.PetId);
                    p.CurrentPet.Visible = p.Visible;
                    p.SetColor(p.Data.ColorId);
                }
            }
        }

        public static void clearAndReload() {
            resetCamouflage();
            camouflager = null;
            camouflageTimer = 0f;
            cooldown = CustomOptionHolder.camouflagerCooldown.getFloat();
            duration = CustomOptionHolder.camouflagerDuration.getFloat();
        }
    }

    public static class Hacker {
        public static PlayerControl hacker;
        public static Color color = new Color32(117, 250, 76, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 10f;
        public static bool onlyColorType = false;
        public static float hackerTimer = 0f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HackerButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            hacker = null;
            hackerTimer = 0f;
            cooldown = CustomOptionHolder.hackerCooldown.getFloat();
            duration = CustomOptionHolder.hackerHackeringDuration.getFloat();
            onlyColorType = CustomOptionHolder.hackerOnlyColorType.getBool();
        }
    }

    public static class Mini {
        public static PlayerControl mini;
        public static Color color = Color.white;
        public const float defaultColliderRadius = 0.2233912f;
            public const float defaultColliderOffset = 0.3636057f;

        public static float growingUpDuration = 400f;
        public static DateTime timeOfGrowthStart = DateTime.UtcNow;
        public static bool triggerMiniLose = false;

        public static void clearAndReload() {
            mini = null;
            triggerMiniLose = false;
            growingUpDuration = CustomOptionHolder.miniGrowingUpDuration.getFloat();
            timeOfGrowthStart = DateTime.UtcNow;
        }

        public static float growingProgress() {
            if (timeOfGrowthStart == null) return 0f;

            float timeSinceStart = (float)(DateTime.UtcNow - timeOfGrowthStart).TotalMilliseconds;
            return Mathf.Clamp(timeSinceStart/(growingUpDuration*1000), 0f, 1f);
        }

        public static bool isGrownUp() {
            return growingProgress() == 1f;
        }
    }

    public static class Tracker {
        public static PlayerControl tracker;
        public static Color color = new Color32(100, 58, 220, byte.MaxValue);

        public static float updateIntervall = 5f;
        public static bool resetTargetAfterMeeting = false;

        public static PlayerControl currentTarget;
        public static PlayerControl tracked;
        public static bool usedTracker = false;
        public static float timeUntilUpdate = 0f;
        public static Arrow arrow = new Arrow(Color.blue);

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrackerButton.png", 115f);
            return buttonSprite;
        }

        public static void resetTracked() {
            currentTarget = tracked = null;
            usedTracker = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.blue);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }

        public static void clearAndReload() {
            tracker = null;
            resetTracked();
            timeUntilUpdate = 0f;
            updateIntervall = CustomOptionHolder.trackerUpdateIntervall.getFloat();
            resetTargetAfterMeeting = CustomOptionHolder.trackerResetTargetAfterMeeting.getBool();
        }
    }

    public static class Vampire {
        public static PlayerControl vampire;
        public static Color color = Palette.ImpostorRed;

        public static float delay = 10f;
        public static float cooldown = 30f;
        public static bool canKillNearGarlics = true;
        public static bool localPlacedGarlic = false;
        public static bool garlicsActive = true;

        public static PlayerControl currentTarget;
        public static PlayerControl bitten; 
        public static bool targetNearGarlic = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.VampireButton.png", 115f);
            return buttonSprite;
        }

        private static Sprite garlicButtonSprite;
        public static Sprite getGarlicButtonSprite() {
            if (garlicButtonSprite) return garlicButtonSprite;
            garlicButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.GarlicButton.png", 115f);
            return garlicButtonSprite;
        }

        public static void clearAndReload() {
            vampire = null;
            bitten = null;
            targetNearGarlic = false;
            localPlacedGarlic = false;
            currentTarget = null;
            garlicsActive = CustomOptionHolder.vampireSpawnRate.getSelection() > 0;
            delay = CustomOptionHolder.vampireKillDelay.getFloat();
            cooldown = CustomOptionHolder.vampireCooldown.getFloat();
            canKillNearGarlics = CustomOptionHolder.vampireCanKillNearGarlics.getBool();
        }
    }

    public static class Snitch {
        public static PlayerControl snitch;
        public static Color color = new Color32(184, 251, 79, byte.MaxValue);

        public static List<Arrow> localArrows = new List<Arrow>();
        public static int taskCountForReveal = 1;
        public static bool includeTeamJackal = false;
        public static bool teamJackalUseDifferentArrowColor = true;


        public static void clearAndReload() {
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                    UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
            taskCountForReveal = Mathf.RoundToInt(CustomOptionHolder.snitchLeftTasksForReveal.getFloat());
            includeTeamJackal = CustomOptionHolder.snitchIncludeTeamJackal.getBool();
            teamJackalUseDifferentArrowColor = CustomOptionHolder.snitchTeamJackalUseDifferentArrowColor.getBool();
            snitch = null;
        }
    }

    public static class Jackal {
        public static PlayerControl jackal;
        public static Color color = new Color32(0, 180, 235, byte.MaxValue);
        public static PlayerControl fakeSidekick;
        public static PlayerControl currentTarget;
        public static List<PlayerControl> formerJackals = new List<PlayerControl>();
        
        public static float cooldown = 30f;
        public static float createSidekickCooldown = 30f;
        public static bool canUseVents = true;
        public static bool canCreateSidekick = true;
        public static Sprite buttonSprite;
        public static bool jackalPromotedFromSidekickCanCreateSidekick = true;
        public static bool canCreateSidekickFromImpostor = true;
        public static bool hasImpostorVision = false;

        public static Sprite getSidekickButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SidekickButton.png", 115f);
            return buttonSprite;
        }

        public static void removeCurrentJackal() {
            if (!formerJackals.Any(x => x.PlayerId == jackal.PlayerId)) formerJackals.Add(jackal);
            jackal = null;
            currentTarget = null;
            fakeSidekick = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
        }

        public static void clearAndReload() {
            jackal = null;
            currentTarget = null;
            fakeSidekick = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
            canUseVents = CustomOptionHolder.jackalCanUseVents.getBool();
            canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick.getBool();
            jackalPromotedFromSidekickCanCreateSidekick = CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick.getBool();
            canCreateSidekickFromImpostor = CustomOptionHolder.jackalCanCreateSidekickFromImpostor.getBool();
            formerJackals.Clear();
            hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
        }
        
    }

    public static class Sidekick {
        public static PlayerControl sidekick;
        public static Color color = new Color32(0, 180, 235, byte.MaxValue);

        public static PlayerControl currentTarget;

        public static float cooldown = 30f;
        public static bool canUseVents = true;
        public static bool canKill = true;
        public static bool promotesToJackal = true;
        public static bool hasImpostorVision = false;

        public static void clearAndReload() {
            sidekick = null;
            currentTarget = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            canUseVents = CustomOptionHolder.sidekickCanUseVents.getBool();
            canKill = CustomOptionHolder.sidekickCanKill.getBool();
            promotesToJackal = CustomOptionHolder.sidekickPromotesToJackal.getBool();
            hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
        }
    }

    public static class Eraser {
        public static PlayerControl eraser;
        public static Color color = Palette.ImpostorRed;

        public static List<PlayerControl> futureErased = new List<PlayerControl>();
        public static PlayerControl currentTarget;
        public static float cooldown = 30f;
        public static bool canEraseAnyone = false; 

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.EraserButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            eraser = null;
            futureErased = new List<PlayerControl>();
            currentTarget = null;
            cooldown = CustomOptionHolder.eraserCooldown.getFloat();
            canEraseAnyone = CustomOptionHolder.eraserCanEraseAnyone.getBool();
        }
    }
    
    public static class Spy {
        public static PlayerControl spy;
        public static Color color = Palette.ImpostorRed;

        public static bool impostorsCanKillAnyone = true;
        public static bool canEnterVents = false;
        public static bool hasImpostorVision = false;

        public static void clearAndReload() {
            spy = null;
            impostorsCanKillAnyone = CustomOptionHolder.spyImpostorsCanKillAnyone.getBool();
            canEnterVents = CustomOptionHolder.spyCanEnterVents.getBool();
            hasImpostorVision = CustomOptionHolder.spyHasImpostorVision.getBool();
        }
    }

    public static class Trickster {
        public static PlayerControl trickster;
        public static Color color = Palette.ImpostorRed;
        public static float placeBoxCooldown = 30f;
        public static float lightsOutCooldown = 30f;
        public static float lightsOutDuration = 10f;
        public static float lightsOutTimer = 0f;

        private static Sprite placeBoxButtonSprite;
        private static Sprite lightOutButtonSprite;
        private static Sprite tricksterVentButtonSprite;

        public static Sprite getPlaceBoxButtonSprite() {
            if (placeBoxButtonSprite) return placeBoxButtonSprite;
            placeBoxButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlaceJackInTheBoxButton.png", 115f);
            return placeBoxButtonSprite;
        }

        public static Sprite getLightsOutButtonSprite() {
            if (lightOutButtonSprite) return lightOutButtonSprite;
            lightOutButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.LightsOutButton.png", 115f);
            return lightOutButtonSprite;
        }

        public static Sprite getTricksterVentButtonSprite() {
            if (tricksterVentButtonSprite) return tricksterVentButtonSprite;
            tricksterVentButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TricksterVentButton.png", 115f);
            return tricksterVentButtonSprite;
        }

        public static void clearAndReload() {
            trickster = null;
            lightsOutTimer = 0f;
            placeBoxCooldown = CustomOptionHolder.tricksterPlaceBoxCooldown.getFloat();
            lightsOutCooldown = CustomOptionHolder.tricksterLightsOutCooldown.getFloat();
            lightsOutDuration = CustomOptionHolder.tricksterLightsOutDuration.getFloat();
            JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
        }

    }

    public static class Cleaner {
        public static PlayerControl cleaner;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CleanButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            cleaner = null;
            cooldown = CustomOptionHolder.cleanerCooldown.getFloat();
        }
    }

    public static class Warlock {

        public static PlayerControl warlock;
        public static Color color = Palette.ImpostorRed;

        public static PlayerControl currentTarget;
        public static PlayerControl curseVictim;
        public static PlayerControl curseVictimTarget;
        public static PlayerControl curseKillTarget;

        public static float cooldown = 30f;
        public static float rootTime = 5f;

        private static Sprite curseButtonSprite;
        private static Sprite curseKillButtonSprite;

        public static Sprite getCurseButtonSprite() {
            if (curseButtonSprite) return curseButtonSprite;
            curseButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CurseButton.png", 115f);
            return curseButtonSprite;
        }

        public static Sprite getCurseKillButtonSprite() {
            if (curseKillButtonSprite) return curseKillButtonSprite;
            curseKillButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CurseKillButton.png", 115f);
            return curseKillButtonSprite;
        }

        public static void clearAndReload() {
            warlock = null;
            currentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
            curseKillTarget = null;
            cooldown = CustomOptionHolder.warlockCooldown.getFloat();
            rootTime = CustomOptionHolder.warlockRootTime.getFloat();
        }

        public static void resetCurse() {
            HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
            HudManagerStartPatch.warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
            HudManagerStartPatch.warlockCurseButton.killButtonManager.TimerText.color = Palette.EnabledColor;
            currentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
            curseKillTarget = null;
        }
    }

    public static class SecurityGuard {
        public static PlayerControl securityGuard;
        public static Color color = new Color32(195, 178, 95, byte.MaxValue);

        public static float cooldown = 30f;
        public static int remainingScrews = 7;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;
        public static int placedCameras = 0;
        public static Vent ventTarget = null;

        private static Sprite closeVentButtonSprite;
        public static Sprite getCloseVentButtonSprite() {
            if (closeVentButtonSprite) return closeVentButtonSprite;
            closeVentButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CloseVentButton.png", 115f);
            return closeVentButtonSprite;
        }

        private static Sprite placeCameraButtonSprite;
        public static Sprite getPlaceCameraButtonSprite() {
            if (placeCameraButtonSprite) return placeCameraButtonSprite;
            placeCameraButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlaceCameraButton.png", 115f);
            return placeCameraButtonSprite;
        }

        private static Sprite animatedVentSealedSprite;
        public static Sprite getAnimatedVentSealedSprite() {
            if (animatedVentSealedSprite) return animatedVentSealedSprite;
            animatedVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.AnimatedVentSealed.png", 160f);
            return animatedVentSealedSprite;
        }

        private static Sprite staticVentSealedSprite;
        public static Sprite getStaticVentSealedSprite() {
            if (staticVentSealedSprite) return staticVentSealedSprite;
            staticVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.StaticVentSealed.png", 160f);
            return staticVentSealedSprite;
        }

        public static void clearAndReload() {
            securityGuard = null;
            ventTarget = null;
            placedCameras = 0;
            cooldown = CustomOptionHolder.securityGuardCooldown.getFloat();
            totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.securityGuardTotalScrews.getFloat());
            camPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamPrice.getFloat());
            ventPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardVentPrice.getFloat());
        }
    }

    public static class Arsonist {
        public static PlayerControl arsonist;
        public static Color color = new Color32(238, 112, 46, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 3f;
        public static bool triggerArsonistWin = false;

        public static PlayerControl currentTarget;
        public static PlayerControl douseTarget;
        public static List<PlayerControl> dousedPlayers = new List<PlayerControl>();

        private static Sprite douseSprite;
        public static Sprite getDouseSprite() {
            if (douseSprite) return douseSprite;
            douseSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DouseButton.png", 115f);
            return douseSprite;
        }

        private static Sprite igniteSprite;
        public static Sprite getIgniteSprite() {
            if (igniteSprite) return igniteSprite;
            igniteSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.IgniteButton.png", 115f);
            return igniteSprite;
        }

        public static bool dousedEveryoneAlive() {
            return PlayerControl.AllPlayerControls.ToArray().All(x => { return x == Arsonist.arsonist || x.Data.IsDead || x.Data.Disconnected || Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId); });
        }

        public static void clearAndReload() {
            arsonist = null;
            currentTarget = null;
            douseTarget = null; 
            triggerArsonistWin = false;
            dousedPlayers = new List<PlayerControl>();
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
            cooldown = CustomOptionHolder.arsonistCooldown.getFloat();
            duration = CustomOptionHolder.arsonistDuration.getFloat();
        }
    }

    public static class Guesser {
        public static PlayerControl guesser;
        public static Color color = new Color32(255, 255, 0, byte.MaxValue);
        private static Sprite targetSprite;

        public static int remainingShots = 2;

        public static Sprite getTargetSprite() {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TargetIcon.png", 150f);
            return targetSprite;
        }

        public static void clearAndReload() {
            guesser = null;
            
            remainingShots = Mathf.RoundToInt(CustomOptionHolder.guesserNumberOfShots.getFloat());
        }
    }

    public static class BountyHunter {
        public static PlayerControl bountyHunter;
        public static Color color = Palette.ImpostorRed;

        public static Arrow arrow;
        public static float bountyDuration = 30f;
        public static bool showArrow = true;
        public static float bountyKillCooldown = 0f;
        public static float punishmentTime = 15f;
        public static float arrowUpdateIntervall = 10f;

        public static float arrowUpdateTimer = 0f;
        public static float bountyUpdateTimer = 0f;
        public static PlayerControl bounty;
        public static TMPro.TextMeshPro cooldownText;

        public static void clearAndReload() {
            arrow = new Arrow(color);
            bountyHunter = null;
            bounty = null;
            arrowUpdateTimer = 0f;
            bountyUpdateTimer = 0f;
            if (arrow != null && arrow.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = null;
            if (cooldownText != null && cooldownText.gameObject != null) UnityEngine.Object.Destroy(cooldownText.gameObject);
            cooldownText = null;
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }


            bountyDuration = CustomOptionHolder.bountyHunterBountyDuration.getFloat();
            bountyKillCooldown = CustomOptionHolder.bountyHunterReducedCooldown.getFloat();
            punishmentTime = CustomOptionHolder.bountyHunterPunishmentTime.getFloat();
            showArrow = CustomOptionHolder.bountyHunterShowArrow.getBool();
            arrowUpdateIntervall = CustomOptionHolder.bountyHunterArrowUpdateIntervall.getFloat();
        }
    }

    public static class Bait {
        public static PlayerControl bait;
        public static Color color = new Color32(0, 247, 255, byte.MaxValue);

        public static bool highlightAllVents = false;
        public static float reportDelay = 0f;

        public static bool reported = false;

        public static void clearAndReload() {
            bait = null;
            reported = false;
            highlightAllVents = CustomOptionHolder.baitHighlightAllVents.getBool();
            reportDelay = CustomOptionHolder.baitReportDelay.getFloat();
        }
    }
}