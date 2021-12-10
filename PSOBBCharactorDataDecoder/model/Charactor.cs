using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PSOBBCharactorGetter
{
    class Charactor
    {

        Logger logger = Logger.GetInstance();
        Dictionary<String, String> itemCodes = ItemConfig.getItemCodes();

        // キャラクター名
        public string Name { get; }
        // キャラクターの種族
        public string Race { get; }
        // キャラクターのレベル
        public string Lavel { get; }
        // キャラクターの取得経験値
        public string Experience { get; }
        // キャラクターの所持品
        public List<string[]> Inventory { get; } = new List<string[]>();
        // キャラクターの倉庫
        public List<string[]> Bank { get; } = new List<string[]>();

        public Charactor(string hexCharactor, string slotNumber)
        {
            logger.print("charactor hex:" + hexCharactor);

            // キャラクターの名前をセット
            setName(hexCharactor);
            // キャラクターの種族をセット
            setRace(hexCharactor);
            // キャラクターのレベルをセット
            setLevel(hexCharactor);
            // キャラクターの経験値をセット
            setExperience(hexCharactor);
            // キャラクターの所持品をセット
            setInventory(hexCharactor.Substring(40, 1680), Inventory, 30, 56, slotNumber);
            // キャラクター倉庫アイテムをセット
            setInventory(hexCharactor.Substring(3600, 9600), Bank, 200, 48, slotNumber);
        }

        private void setName(string hexCharactor)
        {
        }

        private void setRace(string hexCharactor)
        {

        }
        private void setLevel(string hexCharactor)
        {

        }

        private void setExperience(string hexCharactor)
        {

        }

        /// <summary>
        /// 所持品のアイテムを取得するよ。
        /// 引数のStringは所持品の範囲だよ。
        /// </summary>
        /// <param name="inventoryRange">所持品の範囲</param>
        /// <param name="list">所持品を格納するリスト</param>
        /// <param name="max">アイテム数の最大値</param>
        ///  <param name="length">アイテム単位の長さ</param>
        private void setInventory(string inventoryRange, List<string[]> list, int max, int length, string SlotNumber)
        {
            logger.print("inventory area:" + inventoryRange);

            int index = 0;

            // 全アイテムエリアをアイテム単位でループする。
            for (int i = 0;i < max; i++)
            {
                // アイテムを取得
                string itemRange = inventoryRange.Substring(index, length);
                // アイテムコード取得
                string itemCode = itemRange.Substring(0, 6);

                logger.print($"item bag: {list.GetType().Name},item number:{i},item index:{index},item hex:{itemRange},item code:{itemCode}");

                // アイテムの種類を取得（武器、鎧、テクニックなど）
                int itemType = getItemType(itemCode);

                logger.print($"item bag: {list.GetType().Name},item number:{i},item type:{itemType}");

                // 空欄チェック
                if (isBlank(itemRange)) break;

                // アイテム情報を取得
                //string item = itemCode + getItem(itemType, itemCode, itemRange) + $"\tSlot:{SlotNumber}";
                string item = getItem(itemType, itemCode, itemRange);

                // 所持品のリストにアイテム情報を追加
                list.Add(new string[]{
                        itemCode,
                        item,
                        SlotNumber
                    }
                );

                // アイテム情報の開始位置を次のアイテムに更新
                index += length;
            }

        }
        private string getItem(int itemType, string itemCode, string itemRange)
        {
            switch(itemType) {
                case ((int)ItemConfig.ItemType.WEAPON):
                    return weapon(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.FRAME):
                    return armor(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.BARRIER):
                    return barrier(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.UNIT):
                    return unit(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.MAG):
                    return mag(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.DISK):
                    return disk(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.SRANK):
                    return sRankWeapon(itemCode, itemRange);
                case ((int)ItemConfig.ItemType.OTHER):
                    return other(itemCode, itemRange);
                default:
                    return $"unknown. ({itemCode}). There's a possibility that New Ephinea Item";
            }
        }

        private int getItemType(string itemRangeCode)
        {
            int itemCode = Convert.ToInt32(itemRangeCode, 16);

            // Sランク武器は4byteまでを参照する
            if (isSLank(Convert.ToInt32(itemRangeCode.Substring(0, 4), 16)))
            {
                return ((int)ItemConfig.ItemType.SRANK);
            }

            if (isWeapon(itemCode))
            {
                return ((int)ItemConfig.ItemType.WEAPON);
            }

            if (isArmor(itemCode))
            {
                return ((int)ItemConfig.ItemType.FRAME);
            }

            if (isBarrier(itemCode))
            {
                return ((int)ItemConfig.ItemType.BARRIER);
            }

            if (isUnit(itemCode))
            {
                return ((int)ItemConfig.ItemType.UNIT);
            }

            if (isMag(itemCode))
            {
                return ((int)ItemConfig.ItemType.MAG);
            }

            if (isDisk(Convert.ToInt32(itemRangeCode.Substring(0, 4), 16)))
            {
                return ((int)ItemConfig.ItemType.DISK);
            }

            return ((int)ItemConfig.ItemType.OTHER);
        }

        private bool isBlank(string itemRange)
        {
            // アイテム情報がすべて0だった場合は空欄。倉庫の空欄は途中FFFFFFFFが入る
            return (Regex.IsMatch(itemRange.Substring(0,39), @"^[0]+$") || Regex.IsMatch(itemRange.Substring(0, 39), @"^[0]+[F]+[0]+$"));
        }

        private bool isWeapon(int itemCode)
        {
            return (ItemConfig.WeaponRange[0] <= itemCode && itemCode <= ItemConfig.WeaponRange[1]);
        }
        private bool isCommonWeapon(string itemCode)
        {
            // コモン武器の最大アイテムコード以前であり、CommonWeaponsMaxCode
            return (Convert.ToInt32(itemCode, 16) <= ItemConfig.CommonWeaponContainsCode && Convert.ToInt32(itemCode.Substring(4, 2), 16) <= ItemConfig.CommonWeaponsMaxCode);

        }
        private bool isArmor(int itemCode)
        {
            return (ItemConfig.FrameRange[0] <= itemCode && itemCode <= ItemConfig.FrameRange[1]);
        }

        private bool isBarrier(int itemCode)
        {
            return (ItemConfig.BarrierRange[0] <= itemCode && itemCode <= ItemConfig.BarrierRange[1]);
        }
        private bool isUnit(int itemCode)
        {
            return (ItemConfig.UnitRange[0] <= itemCode && itemCode <= ItemConfig.UnitRange[1]);
        }

        private bool isMag(int itemCode)
        {
            return (ItemConfig.MagRange[0] <= itemCode && itemCode <= ItemConfig.MagRange[1]);
        }
        private bool isTool(int itemCode)
        {
            return (ItemConfig.ToolRange[0] <= itemCode && itemCode <= ItemConfig.ToolRange[1]);
        }
        private bool isDisk(int itemCode)
        {
            return (itemCode == ItemConfig.DiskCode);
        }
        private bool isSLank(int itemCode)
        {
            return (ItemConfig.SLankWeaponRange[0] <= itemCode && itemCode <= ItemConfig.SLankWeaponRange[1]);
        }

        private string weapon(string itemCode, string itemRange)
        {
            string name = getName(itemCode);
            int grinder = Convert.ToInt32(itemRange.Substring(6,2), 16);
            int native = getNative(itemRange);
            int aBeast = getABeast(itemRange);
            int machine = getMachine(itemRange);
            int dark = getDark(itemRange);
            int hit = getHit(itemRange);

            // グラインダーが1以上の場合
            string grinderLabel = null;
            if (grinder > 0)
            {
                grinderLabel = $" +{grinder}";
            }
           
            // コモン武器の場合はエレメントの設定をする。
            string element = null;
            if (isCommonWeapon(itemCode))
            {
                element = getElement(itemRange);
                if (element != null)
                {
                    return $"{name}{grinderLabel} [{element}] [{native}/{aBeast}/{machine}/{dark}|{hit}]";
                }
            }

            // レア武器の場合はエレメント非表示
            return $"{name}{grinderLabel} [{native}/{aBeast}/{machine}/{dark}|{hit}]";
        }

        private string armor(string itemCode, string itemRange)
        {
            string name = getName(itemCode);
            int slot = Convert.ToInt32(itemRange.Substring(10, 2), 16);
            int def = Convert.ToInt32(itemRange.Substring(12, 2), 16);
            string defMaxAddition = getAddition(name, ItemConfig.FramesAdditions, ((int)ItemConfig.AdditionType.DEF));
            int avoid = Convert.ToInt32(itemRange.Substring(16, 2), 16);
            string avoidMaxAddition = getAddition(name, ItemConfig.FramesAdditions, ((int)ItemConfig.AdditionType.AVOID));

            return $"{name} [{def}/{defMaxAddition} | {avoid}/{avoidMaxAddition}] [{slot}S]";
        }
        private string barrier(string itemCode, string itemRange)
        {
            string name = getName(itemCode);
            int def = Convert.ToInt32(itemRange.Substring(12, 2), 16);
            string defMaxAddition = getAddition(name, ItemConfig.ShieldAdditions, ((int)ItemConfig.AdditionType.DEF));
            int avoid = Convert.ToInt32(itemRange.Substring(16, 2), 16);
            string avoidMaxAddition = getAddition(name, ItemConfig.ShieldAdditions, ((int)ItemConfig.AdditionType.AVOID));

            return $"{name} [{def}/{defMaxAddition} | {avoid}/{avoidMaxAddition}]";
        }
        private string mag(string itemCode, string itemRange)
        {
            // まぐのアイテムコードは3バイト目に00を追加
            string name = getName(itemCode.Substring(0, 4) + "00");
            int level = Convert.ToInt32(itemRange.Substring(4, 2), 16);
            int sync = Convert.ToInt32(itemRange.Substring(32, 2), 16);
            int iq = Convert.ToInt32(itemRange.Substring(34, 2), 16);
            string collor = ItemConfig.MagCollorCodes[itemRange.Substring(38, 2)];
            double def = Convert.ToInt32(itemRange.Substring(10, 2) + itemRange.Substring(8, 2), 16) / 100;
            double pow = Convert.ToInt32(itemRange.Substring(14, 2) + itemRange.Substring(12, 2), 16) / 100;
            double dex = Convert.ToInt32(itemRange.Substring(18, 2) + itemRange.Substring(16, 2), 16) / 100;
            double mind = Convert.ToInt32(itemRange.Substring(22, 2) + itemRange.Substring(20, 2), 16) / 100;

            string[] pbs = getPbs(itemRange.Substring(6, 2) + itemRange.Substring(36, 2));

            return $"{name} LV{level} [{collor}] [{def}/{pow}/{dex}/{mind}] [{pbs[2]}|{pbs[0]}|{pbs[1]}]";
        }

        private string unit(string itemCode, string itemRange)
        {
            string name = getName(itemCode);
            return $"{name}";
        }
        private string disk(string itemCode, string itemRange)
        {
            string name = ItemConfig.DiskNameCodes[itemRange.Substring(8, 2)];
            int level = Convert.ToInt32(itemRange.Substring(4, 2), 16);
            return $"{name} LV{level} disk";
        }
        private string sRankWeapon(string itemCode, string itemRange)
        {
            // S武器のアイテムコードは行頭～4桁に00を追加
            string name = getName(itemCode.Substring(0,4) + "00");
            int grinder = Convert.ToInt32(itemRange.Substring(6, 2), 16);
            string element = getElement(itemRange);

            // グラインダーが1以上の場合は表示する
            string grinderLabel = null;
            if (grinder > 0)
            {
                grinderLabel = $" +{grinder}";
            }

            return $"S-RANK {name}{grinderLabel} [{element}]";
        }
        private string other(string itemCode, string itemRange)
        {
            string name = getName(itemCode);
            int number = Convert.ToInt32(itemRange.Substring(10, 2), 16);

            string numberLabel = null;
            if (number > 0)
            {
                numberLabel = $" x{number}";
            }

            return $"{name}{numberLabel}";
        }

        private int getNative(string itemRange)
        {
            return getAttribute(ItemConfig.AttributeType["native"], itemRange);
        }
        private int getABeast(string itemRange)
        {
            return getAttribute(ItemConfig.AttributeType["aBeast"], itemRange);
        }
        private int getMachine(string itemRange)
        {
            return getAttribute(ItemConfig.AttributeType["machine"], itemRange);
        }
        private int getDark(string itemRange)
        {
            return getAttribute(ItemConfig.AttributeType["dark"], itemRange);
        }
        private int getHit(string itemRange)
        {
            return getAttribute(ItemConfig.AttributeType["hit"], itemRange);
        }
        private int getAttribute(string attributeType, string itemRange)
        {
            string[] attributes =
            {
                // ひとつめの属性値
                itemRange.Substring(12,4),
                // ふたつめの属性値
                itemRange.Substring(16,4),
                // みっつめの属性値
                itemRange.Substring(20,4),
            };

            foreach (string attribute in attributes)
            {
                if (attribute.Substring(0, 2) == attributeType)
                {
                    return SByte.Parse(attribute.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            return 0;
        }
        private string getName(string itemCode)
        {
            if (itemCodes.ContainsKey(itemCode))
            {
                return itemCodes[itemCode];
            }
            return $"undefined. ({itemCode})";
        }

        private string getElement(string itemRange)
        {
            string elementCode = itemRange.Substring(8, 2);
            if (ItemConfig.ElementCodes.ContainsKey(elementCode))
            {
                return ItemConfig.ElementCodes[elementCode];
            }

            return "undefined";
        }

        private string getAddition(string name, Dictionary<string, int[]> additions, int type)
        {
            if (additions.ContainsKey(name))
            {
                return additions[name][type].ToString();
            }
            return "undefined";
        }
        private string[] getPbs(string pbsCode)
        {
            if (ItemConfig.PBs.ContainsKey(pbsCode))
            {
                return ItemConfig.PBs[pbsCode];
            }

            return new string[] { "undefined", "undefined", "undefined" };
        }
    }
}
