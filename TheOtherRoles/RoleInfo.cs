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

        public static RoleInfo jester = new RoleInfo("ジェスター", Jester.color, "追放されろ", "追放されろ", RoleId.Jester);
        public static RoleInfo mayor = new RoleInfo("市長", Mayor.color, "投票が2票としてカウントされる", "投票が2票としてカウントされる", RoleId.Mayor);
        public static RoleInfo engineer = new RoleInfo("エンジニア",  Engineer.color, "船の重要なシステムをメンテナンスしろ", "船を直せ", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("シェリフ", Sheriff.color, "<color=#FF1919FF>インポスター</color>を撃て", "インポスターを撃て", RoleId.Sheriff);
        public static RoleInfo lighter = new RoleInfo("ライター", Lighter.color, "明かりは消えない", "明かりは消えない", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("ゴッドファーザー", Godfather.color, "全てのクルーメイトを殺せ", "全てのクルーメイトを殺せ", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("マフィオソ", Mafioso.color, "<color=#FF1919FF>マフィア</color>の一員として全てのクルーメイトを殺せ", "全てのクルーメイトを殺せ", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("ジェニター", Janitor.color, "<color=#FF1919FF>マフィア</color>の一員として死体を隠せ", "死体を隠せ", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("モーフィング", Morphling.color, "見た目を変えて逃げ切れ", "見た目を変えろ", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("カモフラジャー", Camouflager.color, "迷彩中に殺せ", "みんなの姿を隠せ", RoleId.Camouflager);
        public static RoleInfo vampire = new RoleInfo("ヴァンパイア", Vampire.color, "クルーメイトを噛み殺せ", "お前の敵を噛め", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("イレイザー", Eraser.color, "奴らの役職を消す、そして殺せ", "敵の役職を消せ", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("トリックスター", Trickster.color, "ジャックインザボックスで奴らを驚かせろ", "敵を驚かせろ", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("クリーナー", Cleaner.color, "敵を殺して痕跡を消せ", "死体を片付けろ", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("ウォーロック", Warlock.color, "呪いでプレイヤーを操って全員殺せ", "呪いで全員殺せ", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("賞金稼ぎ", BountyHunter.color, "賞金首を殺せ", "賞金首を殺せ", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("探偵", Detective.color, "足跡を追って<color=#FF1919FF>インポスター</color>を探せ", "足跡を追跡しろ", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("タイムマスター", TimeMaster.color, "自身の身はタイムシールドで守れ", "タイムシールドを使え", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("メディック", Medic.color, "シールドで味方を守れ", "シールドで味方を守れ", RoleId.Medic);
        public static RoleInfo shifter = new RoleInfo("シフター", Shifter.color, "役職をシフトしろ", "役職をシフトしろ", RoleId.Shifter);
        public static RoleInfo swapper = new RoleInfo("スワッパー", Swapper.color, "投票を入れ替えて<color=#FF1919FF>インポスター</color>を追放しろ", "投票を入れ替えろ", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("霊能力者", Seer.color, "仲間の死を見るだろう", "仲間の死を見るだろう", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("ハッカー", Hacker.color, "システムをハックして<color=#FF1919FF>インポスター</color>を見つけろ", "ハックしてインポスターを見つけろ", RoleId.Hacker);
        public static RoleInfo niceMini = new RoleInfo("良いミニ", Mini.color, "成長するまでは誰もあなたを傷つけられない", "誰もあなたを傷つけられない", RoleId.Mini);
        public static RoleInfo evilMini = new RoleInfo("悪いミニ", Palette.ImpostorRed, "成長するまでは誰もあなたを傷つけられない", "誰もあなたを傷つけられない", RoleId.Mini);
        public static RoleInfo tracker = new RoleInfo("トラッカー", Tracker.color, "<color=#FF1919FF>インポスター</color>を追跡しろ", "インポスターを追跡しろ", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("スニッチ", Snitch.color, "タスクを終わらせて<color=#FF1919FF>インポスター</color>を発見しろ", "タスクを終わらせろ", RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo("ジャッカル", Jackal.color, "全てのクルーメイトと<color=#FF1919FF>インポスター</color>を殺せ", "全員殺せ", RoleId.Jackal);
        public static RoleInfo sidekick = new RoleInfo("サイドキック", Sidekick.color, "ジャッカルが全員殺すのをサポートせよ", "ジャッカルが全員殺すのをサポートせよ", RoleId.Sidekick);
        public static RoleInfo spy = new RoleInfo("スパイ", Spy.color, "<color=#FF1919FF>インポスター</color>を困惑させろ", "インポスターを困惑させろ", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("セキュリティーガード", SecurityGuard.color, "ベントを塞いでカメラを設置しろ", "ベントを塞いでカメラを設置しろ", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("放火魔", Arsonist.color, "全員燃やせ", "全員燃やせ", RoleId.Arsonist);
        public static RoleInfo goodGuesser = new RoleInfo("良いゲッサー", Guesser.color, "推測して撃て", "推測して撃て", RoleId.Guesser);
        public static RoleInfo badGuesser = new RoleInfo("悪いゲッサー", Palette.ImpostorRed, "推測して撃て", "推測して", RoleId.Guesser);
        public static RoleInfo bait = new RoleInfo("囮", Bait.color, "敵をおびき寄せろ", "敵をおびき寄せろ", RoleId.Bait);
        public static RoleInfo impostor = new RoleInfo("インポスター", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "サボタージュをして全員殺せ"), "サボタージュをして全員殺せ", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("クルーメイト", Color.white, "インポスターを見つけ出せ", "インポスターを見つけ出せ", RoleId.Crewmate);
        public static RoleInfo lover = new RoleInfo("恋人", Lovers.color, $"恋愛中", $"恋愛中", RoleId.Lover);
        public static RoleInfo misimo = new RoleInfo("Misimo", Misimo.color, "時間内に全員殺しきれ", "時間内に全員殺しきれ", RoleId.Misimo);
        public static RoleInfo ballad = new RoleInfo("Ballad", Ballad.color, "投票を誤魔化せ", "投票を誤魔化せ", RoleId.Ballad);
        public static RoleInfo predator= new RoleInfo("プレデター", Ballad.color, "透明になって殺せ", "透明になって殺せ", RoleId.Predator);
        public static RoleInfo bomber = new RoleInfo("爆弾魔", Bomber.color, "爆弾を設置しろ", "爆弾を設置しろ", RoleId.Bomber);
        public static RoleInfo trapper = new RoleInfo("トラッパー", Trapper.color, "罠を設置しろ", "罠を設置しろ", RoleId.Trapper);
        public static RoleInfo mifune = new RoleInfo("御船", Mifune.color, "千里眼でマップを見渡せ", "千里眼でマップを見渡せ", RoleId.Mifune);
        public static RoleInfo kan = new RoleInfo("漢", Kan.color, "疑ってきたやつを殺せ", "疑ってきたやつを殺せ", RoleId.Kan);
        public static RoleInfo motarike = new RoleInfo("モタリケ", Motarike.color, "ちょっといいトコ見てみたいーーーー♪", "もーー1回！！", RoleId.Kan);
        public static RoleInfo nottori = new RoleInfo("乗っ取り", Nottori.color, "役職を殺して乗っ取れ", "役職を殺して乗っ取れ", RoleId.Nottori);
        public static RoleInfo madmate = new RoleInfo("マッドメイト", Madmate.color, "インポスターをサポートしろ", "インポスターをサポートしろ", RoleId.Madmate);
        public static RoleInfo madmate2 = new RoleInfo("マッドメイト", Madmate2.color, "インポスターをサポートしろ", "インポスターをサポートしろ", RoleId.Madmate2);
        public static RoleInfo munou = new RoleInfo("無能", Munou.color, "???????????", "???????????", RoleId.Munou);
        public static RoleInfo fortuneTeller = new RoleInfo("占い師", FortuneTeller.color, "占いでインポスターを見つけろ", "タスクをして占え", RoleId.FortuneTeller);
        public static RoleInfo madScientist = new RoleInfo("マッドサイエンティスト", MadScientist.color, "全員を感染させろ", "全員を感染させろ", RoleId.MadScientist);

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
            motarike,
            nottori,
            madmate,
            madmate2,
            munou,
			fortuneTeller,
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
            if (p == Motarike.motarike) infos.Add(motarike);
            if (p == Nottori.nottori) infos.Add(nottori);
            if (p == Madmate.madmate) infos.Add(madmate);
            if (p == Madmate2.madmate2) infos.Add(madmate2);
            if (p == Munou.munou) infos.Add(munou);
            if (p == FortuneTeller.fortuneTeller) infos.Add(fortuneTeller);
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
