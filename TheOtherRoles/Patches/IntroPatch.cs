using HarmonyLib;
using System;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.TheOtherRolesGM;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Objects;
using BepInEx.IL2CPP.Utils.Collections;

namespace TheOtherRoles.Patches {
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutsceneOnDestroyPatch
    {
        public static void Prefix(IntroCutscene __instance) {
            // Generate and initialize player icons
            if (PlayerControl.LocalPlayer != null && HudManager.Instance != null)
            {
                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.localPosition.x, HudManager.Instance.UseButton.transform.localPosition.y, HudManager.Instance.UseButton.transform.localPosition.z);
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    GameData.PlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, HudManager.Instance.transform);
                    player.UpdateFromPlayerOutfit(p.Data.DefaultOutfit, p.Data.IsDead);
                    player.SetFlipX(true);
                    player.PetSlot.gameObject.SetActive(false);
                    player.NameText.text = p.Data.DefaultOutfit.PlayerName;
                    MapOptions.playerIcons[p.PlayerId] = player;

                    if (PlayerControl.LocalPlayer == BountyHunter.bountyHunter)
                    {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, 0f, 0);
                        player.transform.localScale = Vector3.one * 0.4f;
                        player.gameObject.SetActive(false);
                    }
                    else if (PlayerControl.LocalPlayer == GM.gm)
                    {
                        player.transform.localPosition = Vector3.zero;
                        player.transform.localScale = Vector3.one * 0.3f;
                        player.setSemiTransparent(false);
                        player.gameObject.SetActive(false);
                    }
                    else
                    {
                        player.gameObject.SetActive(false);
                    }
                }
            }

            // Force Bounty Hunter to load a new Bounty when the Intro is over
            if (BountyHunter.bounty != null && PlayerControl.LocalPlayer == BountyHunter.bountyHunter) {
                BountyHunter.bountyUpdateTimer = 0f;
                if (HudManager.Instance != null) {
                    Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.localPosition.x, HudManager.Instance.UseButton.transform.localPosition.y, HudManager.Instance.UseButton.transform.localPosition.z) + new Vector3(-0.25f, 1f, 0);
                    BountyHunter.cooldownText = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(HudManager.Instance.KillButton.cooldownTimerText, HudManager.Instance.transform);
                    BountyHunter.cooldownText.alignment = TMPro.TextAlignmentOptions.Center;
                    BountyHunter.cooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -1f, -1f);
                    BountyHunter.cooldownText.gameObject.SetActive(true);
                }
            }

            Arsonist.updateIcons();
            Morphling.resetMorph();
            Camouflager.resetCamouflage();

            if (PlayerControl.LocalPlayer == GM.gm && !GM.hasTasks)
            {
                PlayerControl.LocalPlayer.clearAllTasks();
            }

            if (PlayerControl.LocalPlayer.isGM())
            {
                HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
                HudManager.Instance.ReportButton.gameObject.SetActiveRecursively(false);
                HudManager.Instance.ReportButton.SetActive(false);
                HudManager.Instance.ReportButton.graphic.enabled = false;
                HudManager.Instance.ReportButton.enabled = false;
                HudManager.Instance.ReportButton.graphic.sprite = null;
                HudManager.Instance.ReportButton.buttonLabelText.enabled = false;
                HudManager.Instance.ReportButton.buttonLabelText.SetText("");

                HudManager.Instance.roomTracker.gameObject.SetActiveRecursively(false);
                HudManager.Instance.roomTracker.text.enabled = false;
                HudManager.Instance.roomTracker.text.SetText("");
                HudManager.Instance.roomTracker.enabled = false;
            }

            // ベントを追加する
            AdditionalVents.AddAdditionalVents();

            // スペシメンにバイタルを移動する
            SpecimenVital.moveVital();

            //タスクバグ修正
            if(PlayerControl.GameOptions.MapId == 4 && CustomOptionHolder.airshipEnableWallCheck.getBool())
            {
                var objects = UnityEngine.GameObject.FindObjectsOfType<Console>().ToList();
                objects.Find(x=> x.name == "task_garbage1").checkWalls = true;
                objects.Find(x=> x.name == "task_garbage2").checkWalls = true;
                objects.Find(x=> x.name == "task_garbage3").checkWalls = true;
                objects.Find(x=> x.name == "task_garbage4").checkWalls = true;
                objects.Find(x=> x.name == "task_garbage5").checkWalls = true;
                objects.Find(x=> x.name == "task_shower").checkWalls = true;
                objects.Find(x=> x.name == "task_developphotos").checkWalls = true;
                objects.Find(x=> x.name == "DivertRecieve" && x.Room == SystemTypes.Armory).checkWalls = true;
                objects.Find(x=> x.name == "DivertRecieve" && x.Room == SystemTypes.MainHall).checkWalls = true;
            }

            // 最初から一人の場合はLast Impostorになる
            if(AmongUsClient.Instance.AmHost)
            {
                LastImpostor.promoteToLastImpostor();
            }
        }
    }

    [HarmonyPatch]
    class IntroPatch {
        public static void setupIntroTeamIcons(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            // Intro solo teams
            if (PlayerControl.LocalPlayer.isNeutral() || PlayerControl.LocalPlayer == GM.gm) {
                var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                soloTeam.Add(PlayerControl.LocalPlayer);
                yourTeam = soloTeam;
            }

            // Don't show the GM
            if (!PlayerControl.LocalPlayer.isGM())
            {
                var newTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                foreach (PlayerControl p in yourTeam)
                {
                    if (p != GM.gm)
                        newTeam.Add(p);
                }
                yourTeam = newTeam;
            }

            // Add the Spy to the Impostor team (for the Impostors)
            if (Spy.spy != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
                var fakeImpostorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); // The local player always has to be the first one in the list (to be displayed in the center)
                fakeImpostorTeam.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in players) {
                    if (PlayerControl.LocalPlayer != p && (p == Spy.spy || p.Data.Role.IsImpostor))
                        fakeImpostorTeam.Add(p);
                }
                yourTeam = fakeImpostorTeam;
            }
        }

        public static void setupIntroTeam(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
            RoleInfo roleInfo = infos.Where(info => info.roleType != RoleType.Lovers).FirstOrDefault();
            if (roleInfo == null) return;
            if (PlayerControl.LocalPlayer.isNeutral() || PlayerControl.LocalPlayer.isGM())
            {
                __instance.BackgroundBar.material.color = roleInfo.color;
                __instance.TeamTitle.text = roleInfo.name;
                __instance.TeamTitle.color = roleInfo.color;
                __instance.ImpostorText.text = "";
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        class SetUpRoleTextPatch {
            public static bool Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
            {
                if (!CustomOptionHolder.activateRoles.getBool()) return true; // Don't override the intro of the vanilla roles
                __result = setupRole(__instance).WrapToIl2Cpp();
                return false;
            }

            private static IEnumerator setupRole(IntroCutscene __instance)
            {
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer, new RoleType[] { RoleType.Lovers });
                RoleInfo roleInfo = infos.FirstOrDefault();
                if (roleInfo == RoleInfo.fortuneTeller && FortuneTeller.numTasks > 0)
                {
                    roleInfo = RoleInfo.crewmate;
                }

                Helpers.log($"{roleInfo.name}");
                Helpers.log($"{roleInfo.introDescription}");

                __instance.YouAreText.color = roleInfo.color;
                __instance.RoleText.text = roleInfo.name;
                __instance.RoleText.color = roleInfo.color;
                __instance.RoleBlurbText.text = roleInfo.introDescription;
                __instance.RoleBlurbText.color = roleInfo.color;

                if (PlayerControl.LocalPlayer.hasModifier(ModifierType.Madmate))
                {
                    if (roleInfo == RoleInfo.crewmate)
                    {
                        __instance.RoleText.text = ModTranslation.getString("madmate");
                    }
                    else
                    {
                        __instance.RoleText.text = ModTranslation.getString("madmatePrefix") + __instance.RoleText.text;
                    }
                    __instance.YouAreText.color = Madmate.color;
                    __instance.RoleText.color = Madmate.color;
                    __instance.RoleBlurbText.text = ModTranslation.getString("madmateIntroDesc");
                    __instance.RoleBlurbText.color = Madmate.color;
                }

                if (infos.Any(info => info.roleType == RoleType.Lovers)) {
                    PlayerControl otherLover = PlayerControl.LocalPlayer.getPartner();
                	__instance.RoleBlurbText.text += "\n" + Helpers.cs(Lovers.color, String.Format(ModTranslation.getString("loversFlavor"), otherLover?.Data?.PlayerName ?? ""));
                } 

                // 従来処理
                SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.Data.Role.IntroSound, false, 1f);
                __instance.YouAreText.gameObject.SetActive(true);
                __instance.RoleText.gameObject.SetActive(true);
                __instance.RoleBlurbText.gameObject.SetActive(true);

                if (__instance.ourCrewmate == null)
                {
                    __instance.ourCrewmate = __instance.CreatePlayer(0, 1, PlayerControl.LocalPlayer.Data, false);
                    __instance.ourCrewmate.gameObject.SetActive(false);
                }
                __instance.ourCrewmate.gameObject.SetActive(true);
                __instance.ourCrewmate.transform.localPosition = new Vector3(0f, -1.05f, -18f);
                __instance.ourCrewmate.transform.localScale = new Vector3(1f, 1f, 1f);
                yield return new WaitForSeconds(2.5f);
                __instance.YouAreText.gameObject.SetActive(false);
                __instance.RoleText.gameObject.SetActive(false);
                __instance.RoleBlurbText.gameObject.SetActive(false);
                __instance.ourCrewmate.gameObject.SetActive(false);

                yield break;
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeam(__instance, ref yourTeam);
            }
        }
    }
}

