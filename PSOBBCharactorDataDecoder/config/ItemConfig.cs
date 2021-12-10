using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PSOBBCharactorGetter
{
    public static class ItemConfig
    {
        public static readonly int[] WeaponRange =  { 0x000000, 0x00ED00 };
        public static readonly int[] FrameRange = { 0x010100, 0x010158 };
        public static readonly int[] BarrierRange = { 0x010200, 0x0102A5 };
        public static readonly int[] UnitRange = { 0x010300, 0x010364 };
        public static readonly int[] MagRange = { 0x020000, 0x025200 };
        public static readonly int[] ToolRange = { 0x030000, 0x031A00 };
        public static readonly int[] MesetaRange = { 0x030000, 0x030102 };
        public static readonly int[] DiskRange = { 0x050000, 0x05121D };
        public static readonly int[] EphineaRange = { 0x031005, 0x031810 };
        public static readonly int[] SLankWeaponRange = { 0x0070, 0x0088 };
        public static readonly int DiskCode = 0x0302;
        // コモン武器が含まれている最小範囲
        public static readonly int CommonWeaponContainsCode = 0x000C03;
        // アイテムコード内のコモン武器を表す最大値
        public static readonly int CommonWeaponsMaxCode = 0x04;

        public enum ItemType {
            WEAPON = 1,
            FRAME = 2,
            BARRIER = 3,
            UNIT = 4,
            MAG = 5,
            DISK = 6,
            MESETA = 7,
            TOOL = 8,
            SRANK = 9,
            OTHER = 10,
            BLANK = 11,
        };

        public enum AdditionType
        {
            DEF = 0,
            AVOID = 1
        }
       public static readonly Dictionary<String, String[]> PBs = new Dictionary<String, String[]>()
        {
            {"0000", new string[] {"","",""} },
            {"0001", new string[] {"Farlla","",""} },
            {"0101", new string[] {"Estlla","",""} },
            {"0401", new string[] {"Leilla","",""} },
            {"1003", new string[] {"Farlla","Golla",""} },
            {"5007", new string[] {"Farlla","Golla","Pilla"} },
            {"D007", new string[] {"Farlla","Golla","Mylla & Youlla"} },
            {"1803", new string[] {"Farlla","Pilla",""} },
            {"5807", new string[] {"Farlla","Pilla","Golla"} },
            {"D807", new string[] {"Farlla","Pilla","Mylla & Youlla"} },
            {"2803", new string[] {"Farlla","Mylla & Youlla",""} },
            {"6807", new string[] {"Farlla","Mylla & Youlla","Golla"} },
            {"A807", new string[] {"Farlla","Mylla & Youlla", "Pilla" } },
            {"1103", new string[] {"Estlla","Golla",""} },
            {"5107", new string[] {"Estlla","Golla","Pilla"} },
            {"D107", new string[] {"Estlla","Golla","Mylla & Youlla"} },
            {"1903", new string[] {"Estlla","Pilla",""} },
            {"5907", new string[] {"Estlla","Pilla","Golla"} },
            {"D907", new string[] {"Estlla","Pilla","Mylla & Youlla"} },
            {"2903", new string[] {"Estlla","Mylla & Youlla",""} },
            {"6907", new string[] {"Estlla","Mylla & Youlla","Golla"} },
            {"A907", new string[] {"Estlla","Mylla & Youlla","Pilla"} },
            {"1403", new string[] {"Leilla","Golla",""} },
            {"9407", new string[] {"Leilla","Golla","Pilla"} },
            {"D407", new string[] {"Leilla","Golla","Mylla & Youlla"} },
            {"1C03", new string[] {"Leilla","Pilla",""} },
            {"9C07", new string[] {"Leilla","Pilla","Golla"} },
            {"DC07", new string[] {"Leilla","Pilla","Mylla & Youlla"} },
            {"2C03", new string[] {"Leilla","Mylla & Youlla",""} },
            {"AC07", new string[] {"Leilla","Mylla & Youlla","Golla"} },
            {"EC07", new string[] {"Leilla","Mylla & Youlla","Pilla"} },

        };

        public static readonly Dictionary<String, String> AttributeType = new Dictionary<String, String>()
        {
            { "native", "01" },
            { "aBeast", "02" },
            { "machine", "03" },
            { "dark", "04" },
            { "hit", "05" },
        };

        public static readonly Dictionary<String, String> MagCollorCodes = new Dictionary<String, String>()
        {
            { "00", "Red" },
            { "01", "Blue" },
            { "02", "Yellow" },
            { "03", "Green" },
            { "04", "Purple" },
            { "05", "Black" },
            { "06", "White" },
            { "07", "Cyan" },
            { "08", "Brown" },
            { "09", "Orange" },
            { "0A", "Slate Blue" },
            { "0B", "Olive" },
            { "0C", "Turqoise" },
            { "0D", "Fuschia" },
            { "0E", "Grey" },
            { "0F", "Cream" },
            { "10", "Pink" },
            { "11", "Dark Green" }
        };

        // S武器の種類の定義
        public static readonly Dictionary<String, String> SRankWeaponCodes = new Dictionary<String, String>()
        {
            { "007000", "SABER" },
            { "007100", "SWORD" },
            { "007200", "BLADE" },
            { "007300", "PARTISAN" },
            { "007400", "SLICER" },
            { "007500", "GUN" },
            { "007600", "RIFLE" },
            { "007700", "MECHGUN" },
            { "007800", "SHOT" },
            { "007900", "CANE" },
            { "007A00", "ROD" },
            { "007B00", "WAND" },
            { "007C00", "TWIN" },
            { "007D00", "CLAW" },
            { "007E00", "BAZOOKA" },
            { "007F00", "NEEDLE" },
            { "008000", "SCYTHE" },
            { "008100", "HAMMER" },
            { "008200", "MOON" },
            { "008300", "PSYCHOGUN" },
            { "008400", "PUNCH" },
            { "008500", "WINDMILL" },
            { "008600", "HARISEN" },
            { "008700", "J-BLADE" },
            { "008800", "J-CUTTER" }
        };

        // S武器の特殊の定義
        public static readonly Dictionary<String, String> ElementCodes = new Dictionary<String, String>()
        {
            { "00", "Unchanged/Nothing" },
            { "01", "Draw" },
            { "02", "Drain" },
            { "03", "Fill" },
            { "04", "Gush" },
            { "05", "Heart" },
            { "06", "Mind" },
            { "07", "Soul" },
            { "08", "Geist" },
            { "09", "Master's" },
            { "0A", "Lord's" },
            { "0B", "King's" },
            { "0C", "Charge" },
            { "0D", "Spirit" },
            { "0E", "Berserk" },
            { "0F", "Ice" },
            { "10", "Frost" },
            { "11", "Freeze" },
            { "12", "Blizzard" },
            { "13", "Bind" },
            { "14", "Hold" },
            { "15", "Seize" },
            { "16", "Arrest" },
            { "17", "Heat" },
            { "18", "Fire" },
            { "19", "Flame" },
            { "1A", "Burning" },
            { "1B", "Shock" },
            { "1C", "Thunder" },
            { "1D", "Storm" },
            { "1E", "Tempest" },
            { "1F", "Dim" },
            { "20", "Shadow" },
            { "21", "Dark" },
            { "22", "Hell" },
            { "23", "Panic" },
            { "24", "Riot" },
            { "25", "Havoc" },
            { "26", "Chaos" },
            { "27", "Devil's" },
            { "28", "Demon's" },
        };

        public static readonly Dictionary<String, int[]> FramesAdditions = new Dictionary<String, int[]>()
        {
            { "Frame",                          new int[] { 2,2 } },
            { "Armor",                          new int[] { 2,2 } },
            { "Psy Armor",                      new int[] { 3,2 } },
            { "Giga Frame",                     new int[] { 4,2 } },
            { "Soul Frame",                     new int[] { 4,2 } },
            { "Cross Armor",                    new int[] { 4,2 } },
            { "Solid Frame",                    new int[] { 4,2 } },
            { "Brave Armor",                    new int[] { 4,2 } },
            { "Hyper Frame",                    new int[] { 4,2 } },
            { "Grand Armor",                    new int[] { 4,2 } },
            { "Shock Frame",                    new int[] { 4,2 } },
            { "King&'s Frame",                  new int[] { 4,2 } },
            { "Dragon Frame",                   new int[] { 4,2 } },
            { "Absorb Armor",                   new int[] { 4,2 } },
            { "Protect Frame",                  new int[] { 4,2 } },
            { "General Armor",                  new int[] { 4,2 } },
            { "Perfect Frame",                  new int[] { 4,2 } },
            { "Valiant Frame",                  new int[] { 4,2 } },
            { "Imperial Armor",                 new int[] { 4,2 } },
            { "Holiness Armor",                 new int[] { 4,2 } },
            { "Guardian Armor",                 new int[] { 4,2 } },
            { "Divinity Armor",                 new int[] { 4,2 } },
            { "Ultimate Frame",                 new int[] { 4,2 } },
            { "Celestial Armor",                new int[] { 10,4 } },
            { "HUNTER FIELD",                   new int[] { 8,8 } },
            { "RANGER FIELD",                   new int[] { 8,8 } },
            { "FORCE FIELD",                    new int[] { 8,8 } },
            { "REVIVAL GARMENT",                new int[] { 5,10 } },
            { "SPIRIT GARMENT",                 new int[] { 7,5 } },
            { "STINK FRAME",                    new int[] { 85,85 } },
            { "D-PARTS ver1.01",                new int[] { 10,7 } },
            { "D-PARTS ver2.10",                new int[] { 10,8 } },
            { "PARASITE WEAR:De Rol",           new int[] { 0,0 } },
            { "PARASITE WEAR:Nelgal",           new int[] { 0,0 } },
            { "PARASITE WEAR:Vajulla",          new int[] { 0,0 } },
            { "SENSE PLATE",                    new int[] { 8,8 } },
            { "GRAVITON PLATE",                 new int[] { 8,0 } },
            { "ATTRIBUTE PLATE",                new int[] { 8,8 } },
            { "FLOWEN&'S FRAME",                new int[] { 10,10 } },
            { "CUSTOM FRAME ver.OO",            new int[] { 10,10 } },
            { "DB&'S ARMOR",                    new int[] { 10,10 } },
            { "GUARD WAVE",                     new int[] { 50,20 } },
            { "DF FIELD",                       new int[] { 50,20 } },
            { "LUMINOUS FIELD",                 new int[] { 50,20 } },
            { "CHU CHU FEVER",                  new int[] { 0,0 } },
            { "LOVE HEART",                     new int[] { 50,20 } },
            { "FLAME GARMENT",                  new int[] { 50,20 } },
            { "VIRUS ARMOR:Lafuteria",          new int[] { 0,0 } },
            { "BRIGHTNESS CIRCLE",              new int[] { 50,20 } },
            { "AURA FIELD",                     new int[] { 50,20 } },
            { "ELECTRO FRAME",                  new int[] { 50,20 } },
            { "SACRED CLOTH",                   new int[] { 50,20 } },
            { "SMOKING PLATE",                  new int[] { 5,20 } },
            { "STAR CUIRASS",                   new int[] { 7,5 } },
            { "BLACK HOUND CUIRASS",            new int[] { 0,0 } },
            { "MORNING PRAYER",                 new int[] { 10,20 } },
            { "BLACK ODOSHI DOMARU",            new int[] { 10,10 } },
            { "RED ODOSHI DOMARU",              new int[] { 10,10 } },
            { "BLACK ODOSHI RED NIMAIDOU",      new int[] { 10,10 } },
            { "BLUE ODOSHI VIOLET NIMAIDOU",    new int[] { 10,10 } },
            { "DIRTY LIFEJACKET",               new int[] { 0,0 } },
            { "KROE&'S SWEATER",                new int[] { 0,0 } },
            { "WEDDING DRESS",                  new int[] { 0,0 } },
            { "SONICTEAM ARMOR",                new int[] { 0,0 } },
            { "RED COAT",                       new int[] { 10,10 } },
            { "THIRTEEN",                       new int[] { 8,8 } },
            { "MOTHER GARB",                    new int[] { 15,5 } },
            { "MOTHER GARB+",                   new int[] { 15,5 } },
            { "DRESS PLATE",                    new int[] { 0,0 } },
            { "SWEETHEART",                     new int[] { 50,20 } },
            { "IGNITION CLOAK",                 new int[] { 8,8 } },
            { "CONGEAL CLOAK",                  new int[] { 8,8 } },
            { "TEMPEST CLOAK",                  new int[] { 8,8 } },
            { "CURSED CLOAK",                   new int[] { 8,8 } },
            { "SELECT CLOAK",                   new int[] { 8,8 } },
            { "SPIRIT CUIRASS",                 new int[] { 7,5 } },
            { "REVIVAL CURIASS",                new int[] { 5,10 } },
            { "ALLIANCE UNIFORM",               new int[] { 12,0 } },
            { "OFFICER UNIFORM",                new int[] { 14,0 } },
            { "COMMANDER UNIFORM",              new int[] { 16,0 } },
            { "CRIMSON COAT",                   new int[] { 12,12 } },
            { "INFANTRY GEAR",                  new int[] { 12,8 } },
            { "LIEUTENANT GEAR",                new int[] { 18,16 } },
            { "INFANTRY MANTLE",                new int[] { 10,10 } },
            { "LIEUTENANT MANTLE",              new int[] { 21,18 } },
            { "UNION FIELD",                    new int[] { 0,0 } },
            { "SAMURAI ARMOR",                  new int[] { 0,0 } },
            { "STEALTH SUIT",                   new int[] { 0,25 } },
        };

        public static readonly Dictionary<String, int[]> ShieldAdditions = new Dictionary<String, int[]>()
        {
            { "Barrier",                            new int[] { 5,5 } },
            { "Shield",                             new int[] { 5,5 } },
            { "Core Shield",                        new int[] { 5,5 } },
            { "Giga Shield",                        new int[] { 5,5 } },
            { "Soul Barrier",                       new int[] { 5,5 } },
            { "Hard Shield",                        new int[] { 5,5 } },
            { "Brave Barrier",                      new int[] { 5,5 } },
            { "Solid Shield",                       new int[] { 5,5 } },
            { "Flame Barrier",                      new int[] { 5,5 } },
            { "Plasma Barrier",                     new int[] { 5,5 } },
            { "Freeze Barrier",                     new int[] { 5,5 } },
            { "Psychic Barrier",                    new int[] { 5,5 } },
            { "General Shield",                     new int[] { 5,5 } },
            { "Protect Barrier",                    new int[] { 5,5 } },
            { "Glorious Shield",                    new int[] { 5,5 } },
            { "Imperial Barrier",                   new int[] { 5,5 } },
            { "Guardian Shield",                    new int[] { 5,5 } },
            { "Divinity Barrier",                   new int[] { 5,5 } },
            { "Ultimate Shield",                    new int[] { 5,5 } },
            { "Spiritual Shield",                   new int[] { 5,5 } },
            { "Celestial Shield",                   new int[] { 5,5 } },
            { "INVISIBLE GUARD",                    new int[] { 8,8 } },
            { "SACRED GUARD",                       new int[] { 8,8 } },
            { "S-PARTS ver1.16",                    new int[] { 8,8 } },
            { "S-PARTS ver2.01",                    new int[] { 7,7 } },
            { "LIGHT RELIEF",                       new int[] { 7,7 } },
            { "SHIELD OF DELSABER",                 new int[] { 7,7 } },
            { "FORCE WALL",                         new int[] { 10,10 } },
            { "RANGER WALL",                        new int[] { 10,10 } },
            { "HUNTER WALL",                        new int[] { 10,10 } },
            { "ATTRIBUTE WALL",                     new int[] { 10,10 } },
            { "SECRET GEAR",                        new int[] { 10,10 } },
            { "COMBAT GEAR",                        new int[] { 0,0 } },
            { "PROTO REGENE GEAR",                  new int[] { 7,7 } },
            { "REGENERATE GEAR",                    new int[] { 7,7 } },
            { "REGENE GEAR ADV.",                   new int[] { 7,7 } },
            { "FLOWEN&'S SHIELD",                   new int[] { 10,10 } },
            { "CUSTOM BARRIER ver.OO",              new int[] { 10,10 } },
            { "DB&'S SHIELD",                       new int[] { 10,10 } },
            { "RED RING",                           new int[] { 85,25 } },
            { "TRIPOLIC SHIELD",                    new int[] { 50,15 } },
            { "STANDSTILL SHIELD",                  new int[] { 50,15 } },
            { "SAFETY HEART",                       new int[] { 50,15 } },
            { "KASAMI BRACER",                      new int[] { 50,15 } },
            { "GODS SHIELD SUZAKU",                 new int[] { 0,0 } },
            { "GODS SHIELD GENBU",                  new int[] { 0,0 } },
            { "GODS SHIELD BYAKKO",                 new int[] { 0,0 } },
            { "GODS SHIELD SEIRYU",                 new int[] { 0,0 } },
            { "HUNTER&'S SHELL",                    new int[] { 50,15 } },
            { "RICO&'S GLASSES",                    new int[] { 0,0 } },
            { "RICO&'S EARRING",                    new int[] { 85,25 } },
            { "SECURE FEET",                        new int[] { 50,15 } },
            { "RESTA MERGE",                        new int[] { 5,5 } },
            { "ANTI MERGE",                         new int[] { 5,5 } },
            { "SHIFTA MERGE",                       new int[] { 5,5 } },
            { "DEBAND MERGE",                       new int[] { 5,5 } },
            { "FOIE MERGE",                         new int[] { 5,5 } },
            { "GIFOIE MERGE",                       new int[] { 5,5 } },
            { "RAFOIE MERGE",                       new int[] { 5,5 } },
            { "RED MERGE",                          new int[] { 5,5 } },
            { "BARTA MERGE",                        new int[] { 5,5 } },
            { "GIBARTA MERGE",                      new int[] { 5,5 } },
            { "RABARTA MERGE",                      new int[] { 5,5 } },
            { "BLUE MERGE",                         new int[] { 5,5 } },
            { "ZONDE MERGE",                        new int[] { 5,5 } },
            { "GIZONDE MERGE",                      new int[] { 5,5 } },
            { "RAZONDE MERGE",                      new int[] { 5,5 } },
            { "YELLOW MERGE",                       new int[] { 5,5 } },
            { "RECOVERY BARRIER",                   new int[] { 5,5 } },
            { "ASSIST  BARRIER",                    new int[] { 5,5 } },
            { "RED BARRIER",                        new int[] { 5,5 } },
            { "BLUE BARRIER",                       new int[] { 5,5 } },
            { "YELLOW BARRIER",                     new int[] { 5,5 } },
            { "WEAPONS GOLD SHIELD",                new int[] { 0,0 } },
            { "BLACK GEAR",                         new int[] { 5,5 } },
            { "WORKS GUARD",                        new int[] { 5,5 } },
            { "RAGOL RING",                         new int[] { 0,0 } },
            { "BLUE RING",                          new int[] { 5,5 } },
            { "GREEN RING",                         new int[] { 5,5 } },
            { "YELLOW RING",                        new int[] { 5,5 } },
            { "PURPLE RING",                        new int[] { 5,5 } },
            { "WHITE RING",                         new int[] { 5,5 } },
            { "BLACK RING",                         new int[] { 5,5 } },
            { "WEAPONS SILVER SHIELD",              new int[] { 0,0 } },
            { "WEAPONS COPPER SHIELD",              new int[] { 0,0 } },
            { "GRATIA",                             new int[] { 20,15 } },
            { "TRIPOLIC REFLECTOR",                 new int[] { 50,15 } },
            { "STRIKER PLUS",                       new int[] { 10,5 } },
            { "REGENERATE GEAR B.P.",               new int[] { 7,7 } },
            { "RUPIKA",                             new int[] { 10,20 } },
            { "YATA MIRROR",                        new int[] { 20,25 } },
            { "BUNNY EARS",                         new int[] { 0,0 } },
            { "CAT EARS",                           new int[] { 0,0 } },
            { "THREE SEALS",                        new int[] { 3,3 } },
            { "GOD&'S SHIELD &quot;KOURYU&quot;",   new int[] { 0,0 } },
            { "DF SHIELD",                          new int[] { 85,25 } },
            { "FROM THE DEPTHS",                    new int[] { 0,0 } },
            { "DE ROL LE SHIELD",                   new int[] { 75,75 } },
            { "HONEYCOMB REFLECTOR",                new int[] { 10,10 } },
            { "EPSIGUARD",                          new int[] { 75,75 } },
            { "ANGEL RING",                         new int[] { 0,0 } },
            { "UNION GUARD",                        new int[] { 0,0 } },
            { "STINK SHIELD",                       new int[] { 75,75 } },
            { "UNKNOWN_B",                          new int[] { 0,0 } },
            { "GENPEI",                             new int[] { 0,0 } },
        };

        public static readonly Dictionary<String, String> DiskNameCodes = new Dictionary<String, String>()
        {
            { "00", "Foie" },
            { "01", "Gifoie" },
            { "02", "Rafoie" },
            { "03", "Barta" },
            { "04", "Gibarta" },
            { "05", "Rabarta" },
            { "06", "Zonde" },
            { "07", "Gizonde" },
            { "08", "Razonde" },
            { "09", "Grants" },
            { "0A", "Deband" },
            { "0B", "Jellen" },
            { "0C", "Zalure" },
            { "0D", "Shifta" },
            { "0E", "Ryuker" },
            { "0F", "Resta" },
            { "10", "Anti" },
            { "11", "Reverser" },
            { "12", "Megid" },
        };

        public static Dictionary<String, String> getItemCodes()
        {

            Dictionary<String, String> dictionary = new Dictionary<String, String>();

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (StreamReader sr = new StreamReader(myAssembly.GetManifestResourceStream("PSOBBCharactorDataDecoder.resource.item.Item_code.list"), Encoding.UTF8)) {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // 行頭が0xの行だけ抜き取る
                    if (Regex.IsMatch(line, "^0x"))
                    {
                        // アイテムコードとアイテム名のディクショナリを作成する
                        // = より前をとる。アイテムコードは文字列として扱うため、文頭の0x２文字を削除する
                        string itemCode = line.Substring(0, line.IndexOf("=")).Substring(2);
                        // = 以降をとる
                        string itemValue = line.Substring(line.IndexOf("=") + 1);

                        dictionary[itemCode] = itemValue;
                    }
                }
            }

            // 初期ディクショナリとS武器ディクショナリを結合する
            dictionary = dictionary.Concat(SRankWeaponCodes).ToDictionary(x => x.Key, x => x.Value);

            return dictionary;
        }

    }
}
