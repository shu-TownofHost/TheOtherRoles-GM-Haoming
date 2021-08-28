using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using System.Reflection;
using System.Text;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles {
    public class CustomOptionHolder {
        public static string[] rates = new string[]{"0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"};
        public static string[] presets = new string[]{"Preset 1", "Preset 2", "Preset 3", "Preset 4", "Preset 5"};

        public static CustomOption presetSelection;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;

        public static CustomOption mafiaSpawnRate;
        public static CustomOption janitorClean;
        public static CustomOption janitorCooldown;

        public static CustomOption morphlingSpawnRate;
        public static CustomOption morphlingCooldown;
        public static CustomOption morphlingDuration;

        public static CustomOption camouflagerSpawnRate;
        public static CustomOption camouflagerCooldown;
        public static CustomOption camouflagerDuration;

        public static CustomOption vampireSpawnRate;
        public static CustomOption vampireKillDelay;
        public static CustomOption vampireCooldown;
        public static CustomOption vampireCanKillNearGarlics;

        public static CustomOption eraserSpawnRate;
        public static CustomOption eraserCooldown;
        public static CustomOption eraserCanEraseAnyone;

        public static CustomOption miniSpawnRate;
        public static CustomOption miniGrowingUpDuration;

        public static CustomOption loversSpawnRate;
        public static CustomOption loversImpLoverRate;
        public static CustomOption loversBothDie;
        public static CustomOption loversCanHaveAnotherRole;

        public static CustomOption guesserSpawnRate;
        public static CustomOption guesserIsImpGuesserRate;
        public static CustomOption guesserNumberOfShots;

        public static CustomOption jesterSpawnRate;
        public static CustomOption jesterCanCallEmergency;
        public static CustomOption jesterCanSabotage;
        public static CustomOption jesterArrow;

        public static CustomOption kitsuneSpawnRate;
        public static CustomOption madmateSpawnRate;
        public static CustomOption madmate2SpawnRate;
        public static CustomOption madmateCanDieToSheriff;
        public static CustomOption madmateArrow;
        public static CustomOption misimoSpawnRate;
        public static CustomOption misimoCooldown;
        public static CustomOption misimoDuration;
        public static CustomOption misimoInvisibleOn;
        public static CustomOption balladSpawnRate;
        public static CustomOption balladCooldown;
        public static CustomOption balladSetOnce;
        public static CustomOption balladTimer;
        public static CustomOption balladShowSealedVote;
        public static CustomOption predatorSpawnRate;
        public static CustomOption predatorInvisibleCooldown;
        public static CustomOption predatorInvisibleDuration;
        public static CustomOption predatorSpeedMultiplier;
        public static CustomOption predatorHatesGarlics;

        public static CustomOption bomberSpawnRate;
        public static CustomOption bomberPlantDuration;
        public static CustomOption bomberPlantCooldown;
        public static CustomOption bomberDefuseAfterMeeting;
        public static CustomOption bomberBombEffect;
        public static CustomOption bomberPlantSingleTarget;
        public static CustomOption bomberAOE;

        public static CustomOption trapperSpawnRate;
        public static CustomOption trapperCooldown;
        public static CustomOption trapperUnmoveable;
        public static CustomOption trapperTrapDuration;

        public static CustomOption mifuneSpawnRate;
        public static CustomOption mifuneCooldown;
        public static CustomOption mifuneDuration;
        public static CustomOption kanSpawnRate;
        public static CustomOption nottoriSpawnRate;
        public static CustomOption nottoriNeutral;
        public static CustomOption motarikeSpawnRate;
        public static CustomOption motarikeCooldown;
        public static CustomOption madScientistSpawnRate;
        public static CustomOption madScientistDistance;
        public static CustomOption madScientistDuration;
        public static CustomOption madScientistSabotage;

        public static CustomOption arsonistSpawnRate;
        public static CustomOption arsonistCooldown;
        public static CustomOption arsonistDuration;

        public static CustomOption jackalSpawnRate;
        public static CustomOption jackalKillCooldown;
        public static CustomOption jackalCreateSidekickCooldown;
        public static CustomOption jackalCanUseVents;
        public static CustomOption jackalCanCreateSidekick;
        public static CustomOption sidekickPromotesToJackal;
        public static CustomOption sidekickCanKill;
        public static CustomOption sidekickCanUseVents;
        public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
        public static CustomOption jackalCanCreateSidekickFromImpostor;
        public static CustomOption jackalAndSidekickHaveImpostorVision;

        public static CustomOption bountyHunterSpawnRate;
        public static CustomOption bountyHunterBountyDuration;
        public static CustomOption bountyHunterReducedCooldown;
        public static CustomOption bountyHunterPunishmentTime;
        public static CustomOption bountyHunterShowArrow;
        public static CustomOption bountyHunterArrowUpdateIntervall;

        public static CustomOption shifterSpawnRate;
        public static CustomOption shifterShiftsModifiers;

        public static CustomOption mayorSpawnRate;

        public static CustomOption engineerSpawnRate;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffCanKillNeutrals;

        public static CustomOption lighterSpawnRate;
        public static CustomOption lighterModeLightsOnVision;
        public static CustomOption lighterModeLightsOffVision;
        public static CustomOption lighterCooldown;
        public static CustomOption lighterDuration;

        public static CustomOption detectiveSpawnRate;
        public static CustomOption detectiveAnonymousFootprints;
        public static CustomOption detectiveFootprintIntervall;
        public static CustomOption detectiveFootprintDuration;
        public static CustomOption detectiveReportNameDuration;
        public static CustomOption detectiveReportColorDuration;

        public static CustomOption timeMasterSpawnRate;
        public static CustomOption timeMasterCooldown;
        public static CustomOption timeMasterRewindTime;
        public static CustomOption timeMasterShieldDuration;

        public static CustomOption medicSpawnRate;
        public static CustomOption medicShowShielded;
        public static CustomOption medicShowAttemptToShielded;
        public static CustomOption medicSetShieldAfterMeeting;

        public static CustomOption swapperSpawnRate;
        public static CustomOption swapperCanCallEmergency;
        public static CustomOption swapperCanOnlySwapOthers;

        public static CustomOption seerSpawnRate;
        public static CustomOption seerMode;
        public static CustomOption seerSoulDuration;
        public static CustomOption seerLimitSoulDuration;

        public static CustomOption hackerSpawnRate;
        public static CustomOption hackerCooldown;
        public static CustomOption hackerHackeringDuration;
        public static CustomOption hackerOnlyColorType;

        public static CustomOption trackerSpawnRate;
        public static CustomOption trackerUpdateIntervall;
        public static CustomOption trackerResetTargetAfterMeeting;

        public static CustomOption snitchSpawnRate;
        public static CustomOption snitchLeftTasksForReveal;
        public static CustomOption snitchIncludeTeamJackal;
        public static CustomOption snitchTeamJackalUseDifferentArrowColor;

        public static CustomOption spySpawnRate;
        public static CustomOption spyCanDieToSheriff;
        public static CustomOption spyImpostorsCanKillAnyone;
        public static CustomOption spyCanEnterVents;
        public static CustomOption spyHasImpostorVision;

        public static CustomOption tricksterSpawnRate;
        public static CustomOption tricksterPlaceBoxCooldown;
        public static CustomOption tricksterLightsOutCooldown;
        public static CustomOption tricksterLightsOutDuration;

        public static CustomOption cleanerSpawnRate;
        public static CustomOption cleanerCooldown;
        
        public static CustomOption warlockSpawnRate;
        public static CustomOption warlockCooldown;
        public static CustomOption warlockRootTime;

        public static CustomOption securityGuardSpawnRate;
        public static CustomOption securityGuardCooldown;
        public static CustomOption securityGuardTotalScrews;
        public static CustomOption securityGuardCamPrice;
        public static CustomOption securityGuardVentPrice;
        public static CustomOption munouSpawnRate;
        public static CustomOption fortuneTellerSpawnRate;
        public static CustomOption fortuneTellerNumTask;

        public static CustomOption baitSpawnRate;
        public static CustomOption baitHighlightAllVents;
        public static CustomOption baitReportDelay;

        public static CustomOption maxNumberOfMeetings;
        public static CustomOption blockSkippingInEmergencyMeetings;
        public static CustomOption noVoteIsSelfVote;
        public static CustomOption hidePlayerNames;
        public static CustomOption polusRandomSpawn;
        public static CustomOption ImpostorArrow;
        public static CustomOption ImpostorLocation;
        public static CustomOption ImpostorRoleInfo;
        public static CustomOption additionalVents;
        public static CustomOption polusSpecimenVital;
        public static CustomOption reportDistance;
		public static CustomOption haomingMunou;

        internal static Dictionary<byte, byte[]> blockedRolePairings = new Dictionary<byte, byte[]>();

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static void Load() {
            
            // Role Options
            presetSelection = CustomOption.Create(0, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), presets, null, true);

            // Using new id's for the options to not break compatibilty with older versions
            crewmateRolesCountMin = CustomOption.Create(300, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最小クルーメイト役職数"), 0f, 0f, 15f, 1f, null, true);
            crewmateRolesCountMax = CustomOption.Create(301, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最大クルーメイト役職数"), 0f, 0f, 15f, 1f);
            neutralRolesCountMin = CustomOption.Create(302, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最小第三陣営役職数"), 0f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(303, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最大第三陣営役職数"), 0f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(304, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最小インポスター役職数"), 0f, 0f, 3f, 1f);
            impostorRolesCountMax = CustomOption.Create(305, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "最大インポスター役職数"), 0f, 0f, 3f, 1f);
            

            mafiaSpawnRate = CustomOption.Create(10, cs(Janitor.color, "マフィア"), rates, null, true);
            janitorClean = CustomOption.Create(12, "ジェニターが死体を消せるOn/Off", false, mafiaSpawnRate);
            janitorCooldown = CustomOption.Create(11, "ジェニター死体消しクールダウン", 30f, 10f, 60f, 2.5f, janitorClean);

            morphlingSpawnRate = CustomOption.Create(20, cs(Morphling.color, "モーフィング"), rates, null, true);
            morphlingCooldown = CustomOption.Create(21, "変身クールダウン", 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
            morphlingDuration = CustomOption.Create(22, "変身効果時間", 10f, 1f, 60f, 0.5f, morphlingSpawnRate);

            camouflagerSpawnRate = CustomOption.Create(30, cs(Camouflager.color, "カモフラジャー"), rates, null, true);
            camouflagerCooldown = CustomOption.Create(31, "迷彩クールダウン", 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
            camouflagerDuration = CustomOption.Create(32, "迷彩効果時間", 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

            vampireSpawnRate = CustomOption.Create(40, cs(Vampire.color, "ヴァンパイア"), rates, null, true);
            vampireKillDelay = CustomOption.Create(41, "キルまでの遅延時間", 10f, 1f, 20f, 1f, vampireSpawnRate);
            vampireCooldown = CustomOption.Create(42, "ヴァンパイアキルクールダウン", 30f, 10f, 60f, 2.5f, vampireSpawnRate);
            vampireCanKillNearGarlics = CustomOption.Create(43, "ヴァンパイアがニンニクの近くでキルを行うことができる", true, vampireSpawnRate);

            eraserSpawnRate = CustomOption.Create(230, cs(Eraser.color, "イレイザー"), rates, null, true);
            eraserCooldown = CustomOption.Create(231, "役職消去クールダウン", 30f, 10f, 120f, 5f, eraserSpawnRate);
            eraserCanEraseAnyone = CustomOption.Create(232, "何の役職でも消すことができる", false, eraserSpawnRate);

            tricksterSpawnRate = CustomOption.Create(250, cs(Trickster.color, "トリックスター"), rates, null, true);
            tricksterPlaceBoxCooldown = CustomOption.Create(251, "ボックス設置クールダウン", 10f, 0f, 30f, 2.5f, tricksterSpawnRate);
            tricksterLightsOutCooldown = CustomOption.Create(252, "停電クールダウン", 30f, 10f, 60f, 5f, tricksterSpawnRate);
            tricksterLightsOutDuration = CustomOption.Create(253, "停電の効果時間", 15f, 5f, 60f, 2.5f, tricksterSpawnRate);

            cleanerSpawnRate = CustomOption.Create(260, cs(Cleaner.color, "クリーナー"), rates, null, true);
            cleanerCooldown = CustomOption.Create(261, "死体消去クールダウン", 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

            warlockSpawnRate = CustomOption.Create(270, cs(Cleaner.color, "ウォーロック"), rates, null, true);
            warlockCooldown = CustomOption.Create(271, "ウォーロックキルクールダウン", 30f, 10f, 60f, 2.5f, warlockSpawnRate);
            warlockRootTime = CustomOption.Create(272, "キル後の硬直時間", 5f, 0f, 15f, 1f, warlockSpawnRate);

            bountyHunterSpawnRate = CustomOption.Create(320, cs(BountyHunter.color, "賞金稼ぎ"), rates, null, true);
            bountyHunterBountyDuration = CustomOption.Create(321, "賞金首が更新されるまでの時間",  60f, 10f, 180f, 10f, bountyHunterSpawnRate);
            bountyHunterReducedCooldown = CustomOption.Create(322, "賞金首をキルした場合のクールダウン", 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate);
            bountyHunterPunishmentTime = CustomOption.Create(323, "賞金首以外をキルした場合のペナルティ", 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
            bountyHunterShowArrow = CustomOption.Create(324, "賞金首に矢印を表示する", true, bountyHunterSpawnRate);
            bountyHunterArrowUpdateIntervall = CustomOption.Create(325, "矢印の更新間隔", 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);


            misimoSpawnRate = CustomOption.Create(810, cs(Misimo.color, "Misimo"), rates, null, true);
            misimoCooldown = CustomOption.Create(811, "Misimoキルクールダウン", 20f, 5f, 60f, 2.5f, misimoSpawnRate);
            misimoDuration = CustomOption.Create(812, "自爆までのカウントダウン", 40f, 1f, 60f, 1f, misimoSpawnRate);
            misimoInvisibleOn = CustomOption.Create(813, "透明化能力 On/Off", true, misimoSpawnRate);


            balladSpawnRate = CustomOption.Create(860, cs(Ballad.color, "Ballad"), rates, null, true);
            balladCooldown = CustomOption.Create(861, "投票無効化Cooldown", 20f, 5f, 60f, 2.5f, balladSpawnRate);
            balladTimer = CustomOption.Create(862, "投票無効化有効時間", 120f, 5f, 180f, 2.5f, balladSpawnRate);
            balladSetOnce = CustomOption.Create(863, "一度しか無効化対象を選べない（効果は永続になる)", true, balladSpawnRate);
            balladShowSealedVote = CustomOption.Create(864, "無効化した投票を投票結果に表示する", true, balladSpawnRate);


            predatorSpawnRate = CustomOption.Create(820, cs(Predator.color, "プレデター"), rates, null, true);
            predatorInvisibleCooldown = CustomOption.Create(821, "透明化クールダウン", 20f, 0f, 60f, 2.5f, predatorSpawnRate);
            predatorInvisibleDuration = CustomOption.Create(822, "透明化効果時間", 10f, 1f, 60f, 1f, predatorSpawnRate);
            predatorSpeedMultiplier = CustomOption.Create(823, "移動速度増加倍率", 1f, 1.0f, 3.0f, 0.1f, predatorSpawnRate);
            predatorHatesGarlics = CustomOption.Create(824, "ニンニクの近くでカモフラージュ状態になる", true, predatorSpawnRate);


            bomberSpawnRate = CustomOption.Create(830, cs(Bomber.color, "爆弾魔"), rates, null, true);
            bomberPlantDuration = CustomOption.Create(831, "爆弾設置時間", 5f, 1f, 20f, 1.0f, bomberSpawnRate);
            bomberPlantCooldown = CustomOption.Create(832, "爆弾設置クールダウン", 10f, 0f, 60f, 2.5f, bomberSpawnRate);
            bomberDefuseAfterMeeting = CustomOption.Create(833, "会議後に爆弾を解除する", true, bomberSpawnRate);
            bomberBombEffect = CustomOption.Create(834, "爆弾のエフェクトを表示する", true, bomberSpawnRate);
            bomberPlantSingleTarget = CustomOption.Create(835,"一度に一人までしか爆弾をつけられない", true, bomberSpawnRate);
            bomberAOE = CustomOption.Create(836,"一定範囲内のクルーメイトをキルすることができる", true, bomberSpawnRate);


            trapperSpawnRate = CustomOption.Create(840, cs(Trapper.color, "トラッパー"), rates, null, true);
            trapperCooldown = CustomOption.Create(841, "トラップ再設置クールダウン", 10f, 0f, 60f, 2.5f, trapperSpawnRate);
            trapperUnmoveable = CustomOption.Create(842, "キルの代わりに動けなくする", true, trapperSpawnRate);
            trapperTrapDuration = CustomOption.Create(443, "移動不能時間", 15f, 0f, 60f, 2.5f, trapperUnmoveable);


            mifuneSpawnRate = CustomOption.Create(850, cs(Mifune.color, "御船"), rates, null, true);
            mifuneCooldown = CustomOption.Create(851, "千里眼クールダウン", 10f, 0f, 60f, 2.5f, mifuneSpawnRate);
            mifuneDuration = CustomOption.Create(852, "千里眼効果時間", 5f, 0f, 20f, 1.0f, mifuneSpawnRate);

            nottoriSpawnRate = CustomOption.Create(890, cs(Nottori.color, "乗っ取り"), rates, null, true);
            nottoriNeutral = CustomOption.Create(891, "第三陣営をクルーメイトとして表示する", true, nottoriSpawnRate);

            kanSpawnRate = CustomOption.Create(880, cs(Kan.color, "漢"), rates, null, true);

            motarikeSpawnRate = CustomOption.Create(910, cs(Kan.color, "モタリケ"), rates, null, true);
            motarikeCooldown = CustomOption.Create(911, "リスキーダイスクールダウン", 10f, 0f, 60f, 2.5f, motarikeSpawnRate);

            miniSpawnRate = CustomOption.Create(180, cs(Mini.color, "ミニ"), rates, null, true);
            miniGrowingUpDuration = CustomOption.Create(181, "ミニの成長にかかる時間", 400f, 100f, 1500f, 100f, miniSpawnRate);

            loversSpawnRate = CustomOption.Create(50, cs(Lovers.color, "恋人"), rates, null, true);
            loversImpLoverRate = CustomOption.Create(51, "恋人の片方がインポスターになる確率", rates, loversSpawnRate);
            loversBothDie = CustomOption.Create(52, "恋人の片方が死んだら療法が死ぬ", true, loversSpawnRate);
            loversCanHaveAnotherRole = CustomOption.Create(53, "恋人が他の役職を持つことができる", true, loversSpawnRate);

            guesserSpawnRate = CustomOption.Create(310, cs(Guesser.color, "ゲッサー"), rates, null, true);
            guesserIsImpGuesserRate = CustomOption.Create(311, "ゲッサーがインポスターになる確率", rates, guesserSpawnRate);
            guesserNumberOfShots = CustomOption.Create(312, "推測できる回数", 2f, 1f, 15f, 1f, guesserSpawnRate);

            jesterSpawnRate = CustomOption.Create(60, cs(Jester.color, "ジェスター"), rates, null, true);
            jesterCanCallEmergency = CustomOption.Create(61, "ジェスターが緊急会議を起こせる", true, jesterSpawnRate);
            jesterCanSabotage = CustomOption.Create(62, "ジェスターがサボタージュを行える", true, jesterSpawnRate);
            jesterArrow = CustomOption.Create(63, "死体位置にアローを表示する", true, jesterSpawnRate);

            madmateSpawnRate = CustomOption.Create(293, cs(Madmate.color, "マッドメイト"), rates, null, true);
            madmateCanDieToSheriff = CustomOption.Create(294, "シェリフに殺される", false, madmateSpawnRate);
            madmateArrow = CustomOption.Create(295, "死体位置にアローを表示する", false, madmateSpawnRate);
            madmate2SpawnRate = CustomOption.Create(296, cs(Madmate2.color, "マッドメイト2"), rates, null, true);

            madScientistSpawnRate = CustomOption.Create(901, cs(MadScientist.color, "マッドサイエンティスト"), rates, null, true);
            madScientistDistance = CustomOption.Create(902, "感染距離", 2.5f, 0.5f, 5.0f, 0.25f, madScientistSpawnRate);
            madScientistDuration = CustomOption.Create(903, "感染に必要な時間", 3f, 1f, 10f, 1f, madScientistSpawnRate);
            madScientistSabotage = CustomOption.Create(904, "サボタージュを行える", false, madScientistSpawnRate);

            kitsuneSpawnRate = CustomOption.Create(930, cs(FortuneTeller.color, "狐"), rates, null, true);

            arsonistSpawnRate = CustomOption.Create(290, cs(Arsonist.color, "放火魔"), rates, null, true);
            arsonistCooldown = CustomOption.Create(291, "ガソリンクールダウン", 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
            arsonistDuration = CustomOption.Create(292, "ガソリンをかけるのにかかる時間", 3f, 1f, 10f, 1f, arsonistSpawnRate);

            jackalSpawnRate = CustomOption.Create(220, cs(Jackal.color, "ジャッカル"), rates, null, true);
            jackalKillCooldown = CustomOption.Create(221, "ジャッカル/サイドキック キルクールダウン", 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCreateSidekickCooldown = CustomOption.Create(222, "ジャッカル　サイドキック作成クールダウン", 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCanUseVents = CustomOption.Create(223, "ジャッカルがベントを使うことができる", true, jackalSpawnRate);
            jackalCanCreateSidekick = CustomOption.Create(224, "ジャッカルがサイドキックを作ることができる", false, jackalSpawnRate);
            sidekickPromotesToJackal = CustomOption.Create(225, "ジャッカル死亡時にサイドキックがジャッカルに昇格する", false, jackalSpawnRate);
            sidekickCanKill = CustomOption.Create(226, "サイドキックがキルを行うことができる", false, jackalSpawnRate);
            sidekickCanUseVents = CustomOption.Create(227, "サイドキックがベントを使うことができる", true, jackalSpawnRate);
            jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(228, "ジャッカルに昇格したサイドキックがサイドキックを作ることができる", true, jackalSpawnRate);
            jackalCanCreateSidekickFromImpostor = CustomOption.Create(229, "ジャッカルがインポスターをサイドキックにすることができる", true, jackalSpawnRate);
            jackalAndSidekickHaveImpostorVision = CustomOption.Create(430, "ジャッカルとサイドキックがインポスターと同じ視界範囲を持つ", false, jackalSpawnRate);

            shifterSpawnRate = CustomOption.Create(70, cs(Shifter.color, "シフター"), rates, null, true);
            shifterShiftsModifiers = CustomOption.Create(71, "シフト時にシールド等の追加効果も移動する", false, shifterSpawnRate);

            mayorSpawnRate = CustomOption.Create(80, cs(Mayor.color, "市長"), rates, null, true);

            engineerSpawnRate = CustomOption.Create(90, cs(Engineer.color, "エンジニア"), rates, null, true);

            sheriffSpawnRate = CustomOption.Create(100, cs(Sheriff.color, "シェリフ"), rates, null, true);
            sheriffCooldown = CustomOption.Create(101, "シェリフクールダウン", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
            sheriffCanKillNeutrals = CustomOption.Create(102, "シェリフが第三陣営を殺すことができる", false, sheriffSpawnRate);


            lighterSpawnRate = CustomOption.Create(110, cs(Lighter.color, "ライター"), rates, null, true);
            lighterModeLightsOnVision = CustomOption.Create(111, "ライトON時の視界", 2f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterModeLightsOffVision = CustomOption.Create(112, "ライトOFF時の視界", 0.75f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterCooldown = CustomOption.Create(113, "ライタークールダウン", 30f, 5f, 120f, 5f, lighterSpawnRate);
            lighterDuration = CustomOption.Create(114, "ライト効果時間", 5f, 2.5f, 60f, 2.5f, lighterSpawnRate);

            detectiveSpawnRate = CustomOption.Create(120, cs(Detective.color, "探偵"), rates, null, true);
            detectiveAnonymousFootprints = CustomOption.Create(121, "足跡を匿名にする", false, detectiveSpawnRate); 
            detectiveFootprintIntervall = CustomOption.Create(122, "足跡の間隔", 0.5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveFootprintDuration = CustomOption.Create(123, "足跡が残る時間", 5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveReportNameDuration = CustomOption.Create(124, "探偵のレポート時に犯人の名前が得られる時間制限", 0, 0, 60, 2.5f, detectiveSpawnRate);
            detectiveReportColorDuration = CustomOption.Create(125, "探偵のレポート時に犯人のカラータイプが得られる時間制限", 20, 0, 120, 2.5f, detectiveSpawnRate);

            timeMasterSpawnRate = CustomOption.Create(130, cs(TimeMaster.color, "タイムマスター"), rates, null, true);
            timeMasterCooldown = CustomOption.Create(131, "タイムマスタークールダウン", 30f, 10f, 120f, 2.5f, timeMasterSpawnRate);
            timeMasterRewindTime = CustomOption.Create(132, "巻き戻し時間", 3f, 1f, 10f, 1f, timeMasterSpawnRate);
            timeMasterShieldDuration = CustomOption.Create(133, "シールドの効果時間", 3f, 1f, 20f, 1f, timeMasterSpawnRate);

            medicSpawnRate = CustomOption.Create(140, cs(Medic.color, "メディック"), rates, null, true);
            medicShowShielded = CustomOption.Create(143, "シールドを視認できる", new string[] {"全員", "シールド対象 + メディック", "メディック"}, medicSpawnRate);
            medicShowAttemptToShielded = CustomOption.Create(144, "シールド成功時に守られているプレイヤーに通知する", false, medicSpawnRate);
            medicSetShieldAfterMeeting = CustomOption.Create(145, "会議の後にシールドを有効化する", false, medicSpawnRate);

            swapperSpawnRate = CustomOption.Create(150, cs(Swapper.color, "スワッパー"), rates, null, true);
            swapperCanCallEmergency = CustomOption.Create(151, "スワッパーが緊急ボタンを押すことができる", false, swapperSpawnRate);
            swapperCanOnlySwapOthers = CustomOption.Create(152, "スワッパーは自身をスワップできない", false, swapperSpawnRate);

            seerSpawnRate = CustomOption.Create(160, cs(Seer.color, "霊能力者"), rates, null, true);
            seerMode = CustomOption.Create(161, "霊能力者　モード", new string[]{ "Show Death Flash + Souls", "Show Death Flash", "Show Souls"}, seerSpawnRate);
            seerLimitSoulDuration = CustomOption.Create(163, "魂の表示時間を制限する", false, seerSpawnRate);
            seerSoulDuration = CustomOption.Create(162, "魂の表示時間", 15f, 0f, 60f, 5f, seerLimitSoulDuration);
        
            hackerSpawnRate = CustomOption.Create(170, cs(Hacker.color, "ハッカー"), rates, null, true);
            hackerCooldown = CustomOption.Create(171, "ハッカークールダウン", 30f, 0f, 60f, 5f, hackerSpawnRate);
            hackerHackeringDuration = CustomOption.Create(172, "ハッカー効果時間", 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
            hackerOnlyColorType = CustomOption.Create(173, "カラータイプでしか見ることができない", false, hackerSpawnRate);

            trackerSpawnRate = CustomOption.Create(200, cs(Tracker.color, "トラッカー"), rates, null, true);
            trackerUpdateIntervall = CustomOption.Create(201, "トラッカー更新間隔", 5f, 2.5f, 30f, 2.5f, trackerSpawnRate);
            trackerResetTargetAfterMeeting = CustomOption.Create(202, "ミーティングの都度対象を選ぶことができる", false, trackerSpawnRate);
            snitchSpawnRate = CustomOption.Create(210, cs(Snitch.color, "スニッチ"), rates, null, true);
            snitchLeftTasksForReveal = CustomOption.Create(211, "インポスターにスニッチの位置が表示される残タスク数", 1f, 0f, 5f, 1f, snitchSpawnRate);
            snitchIncludeTeamJackal = CustomOption.Create(212, "ジャッカルの位置も表示する", false, snitchSpawnRate);
            snitchTeamJackalUseDifferentArrowColor = CustomOption.Create(213, "ジャッカルとインポスターで矢印の色を変更する", true, snitchIncludeTeamJackal);

            spySpawnRate = CustomOption.Create(240, cs(Spy.color, "スパイ"), rates, null, true);
            spyCanDieToSheriff = CustomOption.Create(241, "スパイがシェリフに殺される", false, spySpawnRate);
            spyImpostorsCanKillAnyone = CustomOption.Create(242, "インポスターがインポスターを殺すことができる", true, spySpawnRate);
            spyCanEnterVents = CustomOption.Create(243, "スパイがベントに入ることができる", false, spySpawnRate);
            spyHasImpostorVision = CustomOption.Create(244, "スパイがインポスターと同じ視界を持つ", false, spySpawnRate);

            securityGuardSpawnRate = CustomOption.Create(280, cs(SecurityGuard.color, "セキュリティーガード"), rates, null, true);
            securityGuardCooldown = CustomOption.Create(281, "セキュリティーガードクールダウン", 30f, 10f, 60f, 2.5f, securityGuardSpawnRate);
            securityGuardTotalScrews = CustomOption.Create(282, "ネジの本数", 7f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardCamPrice = CustomOption.Create(283, "カメラを作るのに必要なネジの数", 2f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardVentPrice = CustomOption.Create(284, "ベントを塞ぐのに必要なネジの数", 1f, 1f, 15f, 1f, securityGuardSpawnRate);

            baitSpawnRate = CustomOption.Create(330, cs(Bait.color, "囮"), rates, null, true);
            baitHighlightAllVents = CustomOption.Create(331, "どれかのベント使用時に全てのベントをハイライトする", false, baitSpawnRate);
            baitReportDelay = CustomOption.Create(332, "レポートまでの遅延時間", 0f, 0f, 10f, 1f, baitSpawnRate);
            munouSpawnRate = CustomOption.Create(870, cs(Munou.color, "無能"), rates, null, true);
            fortuneTellerSpawnRate = CustomOption.Create(920, cs(FortuneTeller.color, "占い師"), rates, null, true);
            fortuneTellerNumTask = CustomOption.Create(921, "占いに必要なタスク数", 3f, 1f, 10f, 1f, baitSpawnRate);

            // Other options
            maxNumberOfMeetings = CustomOption.Create(3, "緊急会議上限回数 (市長のものはカウントしない)", 10, 0, 15, 1, null, true);
            blockSkippingInEmergencyMeetings = CustomOption.Create(4, "緊急会議時にスキップを押せなくする", false);
            noVoteIsSelfVote = CustomOption.Create(5, "無投票時に自分に投票する", false, blockSkippingInEmergencyMeetings);
            hidePlayerNames = CustomOption.Create(6, "プレイヤーの名前を表示しない", false);
            reportDistance = CustomOption.Create(808, "レポート距離(5がデフォルト)", 5f, 1f, 10f, 0.5f, null, true);
            polusRandomSpawn = CustomOption.Create(801, "ランダム沸き位置(Polus)", true);
            polusSpecimenVital = CustomOption.Create(807, "バイタルをスペシメンに移動する(Polus)", true);
            additionalVents = CustomOption.Create(802, "追加ベント", true);
            ImpostorArrow = CustomOption.Create(803, "固まっているプレイヤー位置にアローを表示(インポスター)", true);
            ImpostorRoleInfo = CustomOption.Create(805, "ゲーム内役職一覧を表示(インポスター)", true);
            ImpostorLocation = CustomOption.Create(806, "他のプレイヤーの方向と距離を表示(インポスター)", true);
            haomingMunou = CustomOption.Create(804, "はおみんは無能", false);

            blockedRolePairings.Add((byte)RoleId.Vampire, new [] { (byte)RoleId.Warlock});
            blockedRolePairings.Add((byte)RoleId.Warlock, new [] { (byte)RoleId.Vampire});
            blockedRolePairings.Add((byte)RoleId.Spy, new [] { (byte)RoleId.Mini});
            blockedRolePairings.Add((byte)RoleId.Mini, new [] { (byte)RoleId.Spy});
            
        }
    }

    public class CustomOption {
        public static List<CustomOption> options = new List<CustomOption>();
        public static int preset = 0;

        public int id;
        public string name;
        public System.Object[] selections;

        public int defaultSelection;
        public ConfigEntry<int> entry;
        public int selection;
        public OptionBehaviour optionBehaviour;
        public CustomOption parent;
        public bool isHeader;

        // Option creation

        public CustomOption(int id, string name,  System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader) {
            this.id = id;
            this.name = parent == null ? name : "- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            selection = 0;
            if (id != 0) {
                entry = TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create(int id, string name, string[] selections, CustomOption parent = null, bool isHeader = false) {
            return new CustomOption(id, name, selections, "", parent, isHeader);
        }

        public static CustomOption Create(int id, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false) {
            List<float> selections = new List<float>();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, name, selections.Cast<object>().ToArray(), defaultValue, parent, isHeader);
        }

        public static CustomOption Create(int id, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false) {
            return new CustomOption(id, name, new string[]{"Off", "On"}, defaultValue ? "On" : "Off", parent, isHeader);
        }

        // Static behaviour

        public static void switchPreset(int newPreset) {
            CustomOption.preset = newPreset;
            foreach (CustomOption option in CustomOption.options) {
                if (option.id == 0) continue;

                option.entry = TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(), option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption) {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();
                }
            }
        }

        public static void ShareOptionSelections() {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null) return;
            foreach (CustomOption option in CustomOption.options) {
                MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptionSelection, Hazel.SendOption.Reliable);
                messageWriter.WritePacked((uint)option.id);
                messageWriter.WritePacked((uint)Convert.ToUInt32(option.selection));
                messageWriter.EndMessage();
            }
        }

        // Getter

        public int getSelection() {
            return selection;
        }

        public bool getBool() {
            return selection > 0;
        }

        public float getFloat() {
            return (float)selections[selection];
        }

        // Option changes

        public void updateSelection(int newSelection) {
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (optionBehaviour != null && optionBehaviour is StringOption stringOption) {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = selections[selection].ToString();

                if (AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer) {
                    if (id == 0) switchPreset(selection); // Switch presets
                    else if (entry != null) entry.Value = selection; // Save selection to config

                    ShareOptionSelections();// Share all selections
                }
           }
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch {
        public static void Postfix(GameOptionsMenu __instance) {
            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;

            List<OptionBehaviour> allOptions = __instance.Children.ToList();
            for (int i = 0; i < CustomOption.options.Count; i++) {
                CustomOption option = CustomOption.options[i];
                if (option.optionBehaviour == null) {
                    StringOption stringOption = UnityEngine.Object.Instantiate(template, template.transform.parent);
                    allOptions.Add(stringOption);

                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => {});
                    stringOption.TitleText.text = option.name;
                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();

                    option.optionBehaviour = stringOption;
                }
                option.optionBehaviour.gameObject.SetActive(true);
            }
            
            var commonTasksOption = allOptions.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if(commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = allOptions.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if(shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = allOptions.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if(longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
            
            __instance.Children = allOptions.ToArray();
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch {
        public static bool Prefix(StringOption __instance) {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => {});
            __instance.TitleText.text = option.name;
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.selections[option.selection].ToString();
            
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            CustomOption.ShareOptionSelections();
        }
    }


    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenuUpdatePatch
    {
        private static float timer = 1f;
        public static void Postfix(GameOptionsMenu __instance) {
            __instance.GetComponentInParent<Scroller>().YBounds.max = -0.5F + __instance.Children.Length * 0.55F; 
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            float offset = -7.85f;
            foreach (CustomOption option in CustomOption.options) {
                if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null) {
                    bool enabled = true;
                    var parent = option.parent;
                    while (parent != null && enabled) {
                        enabled = parent.selection != 0;
                        parent = parent.parent;
                    }
                    option.optionBehaviour.gameObject.SetActive(enabled);
                    if (enabled) {
                        offset -= option.isHeader ? 0.75f : 0.5f;
                        option.optionBehaviour.transform.localPosition = new Vector3(option.optionBehaviour.transform.localPosition.x, offset, option.optionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), "OnEnable")]
    class GameSettingMenuPatch {
        public static void Prefix(GameSettingMenu __instance) {
            __instance.HideForOnline = new Transform[]{};
        }

        public static void Postfix(GameSettingMenu __instance) {
            var mapNameTransform = __instance.AllItems.FirstOrDefault(x => x.gameObject.activeSelf && x.name.Equals("MapName", StringComparison.OrdinalIgnoreCase));
            if (mapNameTransform == null) return;

            var options = new Il2CppSystem.Collections.Generic.List<Il2CppSystem.Collections.Generic.KeyValuePair<string, int>>();
            for (int i = 0; i < GameOptionsData.MapNames.Length; i++) {
                var kvp = new Il2CppSystem.Collections.Generic.KeyValuePair<string, int>();
                kvp.key = GameOptionsData.MapNames[i];
                kvp.value = i;
                options.Add(kvp);
            }
            mapNameTransform.GetComponent<KeyValueOption>().Values = options;
        }
    }

    [HarmonyPatch(typeof(Constants), nameof(Constants.ShouldFlipSkeld))]
    class ConstantsShouldFlipSkeldPatch {
        public static bool Prefix(ref bool __result) {
            if (PlayerControl.GameOptions == null) return true;
            __result = PlayerControl.GameOptions.MapId == 3;
            return false;
        }
    }

    [HarmonyPatch] 
    class GameOptionsDataPatch
    {
        private static IEnumerable<MethodBase> TargetMethods() {
            return typeof(GameOptionsData).GetMethods().Where(x => x.ReturnType == typeof(string) && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(int));
        }

        private static void Postfix(ref string __result)
        {
            StringBuilder sb = new StringBuilder(__result);
            foreach (CustomOption option in CustomOption.options) {
                if (option.parent == null) {
                    if (option == CustomOptionHolder.crewmateRolesCountMin) {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Crewmate Roles");
                        var min = CustomOptionHolder.crewmateRolesCountMin.getSelection();
                        var max = CustomOptionHolder.crewmateRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    } else if (option == CustomOptionHolder.neutralRolesCountMin) {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Neutral Roles");
                        var min = CustomOptionHolder.neutralRolesCountMin.getSelection();
                        var max = CustomOptionHolder.neutralRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    } else if (option == CustomOptionHolder.impostorRolesCountMin) {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Impostor Roles");
                        var min = CustomOptionHolder.impostorRolesCountMin.getSelection();
                        var max = CustomOptionHolder.impostorRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    } else if ((option == CustomOptionHolder.crewmateRolesCountMax) || (option == CustomOptionHolder.neutralRolesCountMax) || (option == CustomOptionHolder.impostorRolesCountMax)) {
                        continue;
                    } else {
                        sb.AppendLine($"{option.name}: {option.selections[option.selection].ToString()}");
                    }
                    
                }
            }
            CustomOption parent = null;
            foreach (CustomOption option in CustomOption.options)
                if (option.parent != null) {
                    if (option.parent != parent) {
                        sb.AppendLine();
                        parent = option.parent;
                    }
                    sb.AppendLine($"{option.name}: {option.selections[option.selection].ToString()}");
                }

            var hudString = sb.ToString();

            int defaultSettingsLines = 19;
            int roleSettingsLines = defaultSettingsLines + 35;
            int detailedSettingsP1 = roleSettingsLines + 34;
            int detailedSettingsP2 = detailedSettingsP1 + 35;
            int detailedSettingsP3 = detailedSettingsP2 + 35;
            int end1 = hudString.TakeWhile(c => (defaultSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end2 = hudString.TakeWhile(c => (roleSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end3 = hudString.TakeWhile(c => (detailedSettingsP1 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end4 = hudString.TakeWhile(c => (detailedSettingsP2 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end5 = hudString.TakeWhile(c => (detailedSettingsP3 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int counter = TheOtherRolesPlugin.optionsPage;
            if (counter == 0) {
                hudString = hudString.Substring(0, end1) + "\n";   
            } else if (counter == 1) {
                hudString = hudString.Substring(end1 + 1, end2 - end1);
                // Temporary fix, should add a new CustomOption for spaces
                int gap = 1;
                int index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                hudString = hudString.Insert(index, "\n");
                gap = 5;
                index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                hudString = hudString.Insert(index, "\n");
                gap = 24;
                index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                hudString = hudString.Insert(index + 1, "\n");
                gap = 35;
                index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                hudString = hudString.Insert(index + 1, "\n");
            } else if (counter == 2) {
                hudString = hudString.Substring(end2 + 1, end3 - end2);
            } else if (counter == 3) {
                hudString = hudString.Substring(end3 + 1, end4 - end3);
            } else if (counter == 4) {
                hudString = hudString.Substring(end4 + 1, end5 - end4);
            } else if (counter == 5){
                hudString = hudString.Substring(end5 + 1);
            }

            hudString += $"\n Press tab for more... ({counter+1}/6)";
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            if(Input.GetKeyDown(KeyCode.Tab)) {
                TheOtherRolesPlugin.optionsPage = (TheOtherRolesPlugin.optionsPage + 1) % 6;
            }
        }
    }

    
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class GameSettingsScalePatch {
        public static void Prefix(HudManager __instance) {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f; 
        }
    }
}