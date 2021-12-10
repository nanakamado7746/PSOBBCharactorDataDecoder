using System;
using System.IO;
using System.Windows.Forms;

namespace PSOBBCharactorGetter
{
    public partial class Main : Form
    {

        Logger logger = Logger.GetInstance();

        public Main()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ダイアログを出してテキストボックスにフォルダを入力する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // ダイアログの説明文
                folderBrowserDialog.Description = "Please specify a folder.";
                // デフォルトのフォルダ
                folderBrowserDialog.SelectedPath = @"C:\Users\";
                // 「新しいフォルダーの作成する」ボタンを表示する（デフォルトはtrue）
                folderBrowserDialog.ShowNewFolderButton = true;
                //フォルダを選択するダイアログを表示する
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBox1.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        /// <summary>
        /// アイテムリストのテキストファイルを作るよ
        /// キャラクター単位のリストと全キャラクターのリストを作るよ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {

            // 入力欄のフォルダがない場合は不可
            if (!Directory.Exists(this.textBox1.Text))
            {
                label3.Text = "Please enter the folder.";
                return;
            }

            // ディレクトリを取得
            DirectoryInfo directoryInfo = new DirectoryInfo(this.textBox1.Text);
            // バイナリファイルを取得
            FileInfo[] fileInfo = directoryInfo.GetFiles("*.psochar");

            // バイナリファイルがない場合は不可
            if (fileInfo.Length == 0)
            {
                label3.Text = "Notthing Charactor Data File. (.psochar)";
                return;
            }

            // 実行中はボタンをオフにする
            disable();

            // デコード処理を実行する
            run(fileInfo);

            //　アプリの終了
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = dragFilePathArr[0];
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.All;
        }
    }
}
