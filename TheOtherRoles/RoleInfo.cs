using HarmonyLib;
using System.Linq;
using System;
using System.Collections.Generic;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;

namespace TheOtherRoles
{
    public class RoleInfo {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public RoleId roleId;

        public RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId) {
            this.color = color;
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
        }

        public static RoleInfo jester = new RoleInfo("ジェスター", Jester.color, "Get voted out", "Get voted out", RoleId.Jester);
        public static RoleInfo mayor = new RoleInfo("市長", Mayor.color, "Your vote counts twice", "Your vote counts twice", RoleId.Mayor);
        public static RoleInfo engineer = new RoleInfo("エンジニア",  Engineer.color, "Maintain important systems on the ship", "Repair the ship", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("シェリフ", Sheriff.color, "Shoot the <color=#FF1919FF>Impostors</color>", "Shoot the Impostors", RoleId.Sheriff);
        public static RoleInfo lighter = new RoleInfo("ライター", Lighter.color, "Your light never goes out", "Your light never goes out", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("ゴッドファーザー", Godfather.color, "Kill all Crewmates", "Kill all Crewmates", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("マフィオソ", Mafioso.color, "Work with the <color=#FF1919FF>Mafia</color> to kill the Crewmates", "Kill all Crewmates", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("ジェニター", Janitor.color, "Work with the <color=#FF1919FF>Mafia</color> by hiding dead bodies", "Hide dead bodies", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("モーフィング", Morphling.color, "Change your look to not get caught", "Change your look", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("カモフラジャー", Camouflager.color, "Camouflage and kill the Crewmates", "Hide among others", RoleId.Camouflager);
        public static RoleInfo vampire = new RoleInfo("ヴァンパイア", Vampire.color, "Kill the Crewmates with your bites", "Bite your enemies", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("イレイザー", Eraser.color, "Kill the Crewmates and erase their roles", "Erase the roles of your enemies", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("トリックスター", Trickster.color, "Use your jack-in-the-boxes to surprise others", "Surprise your enemies", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("クリーナー", Cleaner.color, "Kill everyone and leave no traces", "Clean up dead bodies", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("ウォーロック", Warlock.color, "Curse other players and kill everyone", "Curse and kill everyone", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("賞金稼ぎ", BountyHunter.color, "Hunt your Bounty down", "Hunt your Bounty down", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("探偵", Detective.color, "Find the <color=#FF1919FF>Impostors</color> by examining footprints", "Examine footprints", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("タイムマスター", TimeMaster.color, "Save yourself with your time shield", "Use your time shield", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("メディック", Medic.color, "Protect someone with your shield", "Protect other players", RoleId.Medic);
        public static RoleInfo shifter = new RoleInfo("シフター", Shifter.color, "Shift your role", "Shift your role", RoleId.Shifter);
        public static RoleInfo swapper = new RoleInfo("スワッパー", Swapper.color, "Swap votes to exile the <color=#FF1919FF>Impostors</color>", "Swap votes", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("霊能力者", Seer.color, "You will see players die", "You will see players die", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("ハッカー", Hacker.color, "Hack systems to find the <color=#FF1919FF>Impostors</color>", "Hack to find the Impostors", RoleId.Hacker);
        public static RoleInfo niceMini = new RoleInfo("良いミニ", Mini.color, "No one will harm you until you grow up", "No one will harm you", RoleId.Mini);
        public static RoleInfo evilMini = new RoleInfo("悪いミニ", Palette.ImpostorRed, "No one will harm you until you grow up", "No one will harm you", RoleId.Mini);
        public static RoleInfo tracker = new RoleInfo("トラッカー", Tracker.color, "Track the <color=#FF1919FF>Impostors</color> down", "Track the Impostors down", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("スニッチ", Snitch.color, "Finish your tasks to find the <color=#FF1919FF>Impostors</color>", "Finish your tasks", RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo("ジャッカル", Jackal.color, "Kill all Crewmates and <color=#FF1919FF>Impostors</color> to win", "Kill everyone", RoleId.Jackal);
        public static RoleInfo sidekick = new RoleInfo("サイドキック", Sidekick.color, "Help your Jackal to kill everyone", "Help your Jackal to kill everyone", RoleId.Sidekick);
        public static RoleInfo spy = new RoleInfo("スパイ", Spy.color, "Confuse the <color=#FF1919FF>Impostors</color>", "Confuse the Impostors", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("セキュリティーガード", SecurityGuard.color, "Seal vents and place cameras", "Seal vents and place cameras", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("放火魔", Arsonist.color, "Let them burn", "Let them burn", RoleId.Arsonist);
        public static RoleInfo goodGuesser = new RoleInfo("良いゲッサー", Guesser.color, "Guess and shoot", "Guess and shoot", RoleId.Guesser);
        public static RoleInfo badGuesser = new RoleInfo("悪いゲッサー", Palette.ImpostorRed, "Guess and shoot", "Guess and shoot", RoleId.Guesser);
        public static RoleInfo bait = new RoleInfo("囮", Bait.color, "Bait your enemies", "Bait your enemies", RoleId.Bait);
        public static RoleInfo impostor = new RoleInfo("インポスター", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "Sabotage and kill everyone"), "Sabotage and kill everyone", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("クルーメイト", Color.white, "Find the Impostors", "Find the Impostors", RoleId.Crewmate);
        public static RoleInfo lover = new RoleInfo("恋人", Lovers.color, $"You are in love", $"You are in love", RoleId.Lover);
        public static RoleInfo misimo = new RoleInfo("Misimo", Misimo.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Misimo);
        public static RoleInfo ballad = new RoleInfo("Ballad", Ballad.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Ballad);
        public static RoleInfo predator= new RoleInfo("プレデター", Ballad.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Predator);
        public static RoleInfo bomber = new RoleInfo("爆弾魔", Bomber.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Bomber);
        public static RoleInfo trapper = new RoleInfo("トラッパー", Trapper.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Trapper);
        public static RoleInfo mifune = new RoleInfo("御船", Mifune.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Mifune);
        public static RoleInfo kan = new RoleInfo("漢", Kan.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Kan);
        public static RoleInfo nottori = new RoleInfo("乗っ取り", Nottori.color, "Kill All Crewmates", "Kill All Crewmates", RoleId.Nottori);
        public static RoleInfo madmate = new RoleInfo("マッドメイト", Madmate.color, "Help Impostors", "Help Impostors", RoleId.Madmate);
        public static RoleInfo madmate2 = new RoleInfo("マッドメイト", Madmate2.color, "Help Impostors", "Help Impostors", RoleId.Madmate2);
        public static RoleInfo munou = new RoleInfo("無能", Munou.color, "???????????", "???????????", RoleId.Munou);
        public static RoleInfo madScientist = new RoleInfo("マッドサイエンティスト", MadScientist.color, "Infect All Crewmates and Impostors", "Infect All Crewmatest and Impostors", RoleId.MadScientist);

        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            godfather,
            mafioso,
            janitor,
            morphling,
            camouflager,
            vampire,
            eraser,
            trickster,
            cleaner,
            warlock,
            bountyHunter,
            misimo,
            ballad,
            predator,
            bomber,
            trapper,
            mifune,
			kan,
			nottori,
            madmate,
            madmate2,
			munou,
			madScientist,
            niceMini,
            evilMini,
            goodGuesser,
            badGuesser,
            lover,
            jester,
            arsonist,
            jackal,
            sidekick,
            crewmate,
            shifter,
            mayor,
            engineer,
            sheriff,
            lighter,
            detective,
            timeMaster,
            medic,
            swapper,
            seer,
            hacker,
            tracker,
            snitch,
            spy,
            securityGuard,
            bountyHunter,
            bait
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p) {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Special roles
            if (p == Misimo.misimo) infos.Add(misimo);
            if (p == Ballad.ballad) infos.Add(ballad);
            if (p == Predator.predator) infos.Add(predator);
            if (p == Bomber.bomber) infos.Add(bomber);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Mifune.mifune) infos.Add(mifune);
            if (p == Kan.kan) infos.Add(kan);
            if (p == Nottori.nottori) infos.Add(nottori);
            if (p == Madmate.madmate) infos.Add(madmate);
            if (p == Madmate2.madmate2) infos.Add(madmate2);
            if (p == Munou.munou) infos.Add(munou);
            if (p == MadScientist.madScientist) infos.Add(madScientist);
            if (p == Jester.jester) infos.Add(jester);
            if (p == Mayor.mayor) infos.Add(mayor);
            if (p == Engineer.engineer) infos.Add(engineer);
            if (p == Sheriff.sheriff) infos.Add(sheriff);
            if (p == Lighter.lighter) infos.Add(lighter);
            if (p == Godfather.godfather) infos.Add(godfather);
            if (p == Mafioso.mafioso) infos.Add(mafioso);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Morphling.morphling) infos.Add(morphling);
            if (p == Camouflager.camouflager) infos.Add(camouflager);
            if (p == Vampire.vampire) infos.Add(vampire);
            if (p == Eraser.eraser) infos.Add(eraser);
            if (p == Trickster.trickster) infos.Add(trickster);
            if (p == Cleaner.cleaner) infos.Add(cleaner);
            if (p == Warlock.warlock) infos.Add(warlock);
            if (p == Detective.detective) infos.Add(detective);
            if (p == TimeMaster.timeMaster) infos.Add(timeMaster);
            if (p == Medic.medic) infos.Add(medic);
            if (p == Shifter.shifter) infos.Add(shifter);
            if (p == Swapper.swapper) infos.Add(swapper);
            if (p == Seer.seer) infos.Add(seer);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Mini.mini) infos.Add(p.Data.IsImpostor ? evilMini : niceMini);
            if (p == Tracker.tracker) infos.Add(tracker);
            if (p == Snitch.snitch) infos.Add(snitch);
            if (p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == p.PlayerId))) infos.Add(jackal);
            if (p == Sidekick.sidekick) infos.Add(sidekick);
            if (p == Spy.spy) infos.Add(spy);
            if (p == SecurityGuard.securityGuard) infos.Add(securityGuard);
            if (p == Arsonist.arsonist) infos.Add(arsonist);
            if (p == Guesser.guesser) infos.Add(p.Data.IsImpostor ? badGuesser : goodGuesser);
            if (p == BountyHunter.bountyHunter) infos.Add(bountyHunter);
            if (p == Bait.bait) infos.Add(bait);

            // Default roles
            if (infos.Count == 0 && p.Data.IsImpostor) infos.Add(impostor); // Just Impostor
            if (infos.Count == 0 && !p.Data.IsImpostor) infos.Add(crewmate); // Just Crewmate

            // Modifier
            if (p == Lovers.lover1|| p == Lovers.lover2) infos.Add(lover);

            return infos;
        }
    }
}
