using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PSOBBCharactorGetter
{
    partial class Main
    {
        private void disable()
        {
            textBox1.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void run(FileInfo[] fileInfo)
        {

            // 処理済みカウント用。count / max　で処理状況を計算する
            double max = fileInfo.Length;
            double count = 0;
            // 全アイテムリスト
            List<string[]> allItemsList = new List<string[]>();

            // キャラクターデータファイルをループ
            foreach (FileInfo binaryFile in fileInfo)
            {
                logger.print("target charactor binary file: " + binaryFile.FullName);

                // 出力ファイル名
                string encodedFile = binaryFile.FullName + ".txt";

                // バイナリファイルを読み込んで処理する
                using (FileStream fileStream = File.OpenRead(binaryFile.FullName))
                {

                    // バイナリの長さに合わせてバイト配列の初期化
                    byte[] bytes = new byte[fileStream.Length];
                    // バイナリファイルの読込
                    fileStream.Read(bytes, 0, (int)fileStream.Length);

                    // スロット番号を取得
                    string slotNumber = getSlotNumber(binaryFile.FullName);

                    // キャラクタークラス作成
                    Charactor charactor = new Charactor(BitConverter.ToString(bytes).Replace("-", string.Empty), slotNumber);

                    // ファイルに書き込み
                    write(charactor, encodedFile);

                    // 全アイテムリストに追加
                    allItemsList.AddRange(charactor.Inventory);
                    allItemsList.AddRange(charactor.Bank);

                    //　処理の状況（％）を表示する
                    count += 1;
                    double processed = count / max * 100;
                    label3.Text = $"....{Math.Ceiling(processed)}%";
                    label3.Refresh();
                    
                }

                // ソートする前にリストを配列にする
                string[][] allItemsArray = allItemsList.ToArray();
                // 全アイテムリストをソート
                Array.Sort(allItemsArray, (a, b) => Convert.ToInt32(a[0], 16) - Convert.ToInt32(b[0], 16));
                // 全アイテムリストの書き込み
                write(allItemsArray, this.textBox1.Text + "\\All_Charactor_Items.txt");

            }
        }

        private string getSlotNumber(string fileName)
        {
            Match match = Regex.Match(fileName, @"(?<= ).+(?=\.)");
            if (match.Success)
            {
                return match.Value;
            }

            return "unknown";
        }

        private void write(Charactor charactor, string file)
        {
            // 出力ファイルの初期化
            initializEncodedFile(file);

            // ファイルへの書き込み
            using (StreamWriter writer = new StreamWriter(file, true, Encoding.GetEncoding("UTF-8")))
            {
                writer.WriteLine("############################################");
                writer.WriteLine("#  INVENTORY                               #");
                writer.WriteLine("############################################");
                foreach (string[] item in charactor.Inventory)
                {
                    writer.WriteLine(item[1]);
                }
                writer.WriteLine("############################################");
                writer.WriteLine("#  BANK                                    #");
                writer.WriteLine("############################################");
                foreach (string[] item in charactor.Bank)
                {
                    writer.WriteLine(item[1]);
                }
            }

            //　出力ファイルを読み取り専用する
            setReadonly(file);
        }

        private void write(string[][] array, string file)
        {

            // 出力ファイルの初期化
            initializEncodedFile(file);

            // ファイルを開く
            using (StreamWriter writer = new StreamWriter(file, false, Encoding.GetEncoding("UTF-8")))
            {
                foreach (string[] item in array)
                {
                    writer.WriteLine(item[1] + $"\tSlot:{item[2]}");
                }
            }

            //　出力ファイルを読み取り専用する
            setReadonly(file);
        }
        private void initializEncodedFile(string file)
        {
            if (File.Exists(file))
            {
                // ファイルが存在する場合は書き込み可にする
                setWritable(file);

                // ファイルを空にする
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    fileStream.SetLength(0);
                }
            }
            else
            {
                // 存在しない場合は作成
                using (File.Create(file));
            }
        }

        private void setWritable(string file)
        {
            File.SetAttributes(file, File.GetAttributes(file) & (~FileAttributes.ReadOnly));
        }

        private void setReadonly(string file)
        {
            // ファイルが存在したときは読み取り専用にする
            if (File.Exists(file))
            {
                File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
            }
        }

    }
}
