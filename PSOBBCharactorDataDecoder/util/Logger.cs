using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace PSOBBCharactorGetter
{
    /// <summary>
    /// シングルトンのロガークラスだよ。
    /// 各クラスのフィールドで次のように呼び出してください。
    /// Logger logger = Logger.GetInstance();
    /// ロギングがONのときはログにも書き込むよ。
    /// 実行ファイルと同フォルダにloggingがある場合にロギングをONにするよ
    /// 標準出力はロギングがOFFでも出力するよ。
    /// 
    /// ２０２１年８月２５日　さくら　新規作成
    /// </summary>
    public sealed class Logger
    {
        // インスタンスの生成だよ
        private static Logger _logger = new Logger();
        // ロギング可否のフラグだよ
        private bool _doLogging;
        // ログファイル名だよ
        private const string LOGFILE = "log.txt";
        // ログに書き込む日時のフォーマットだよ
        private static readonly string DATETIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";

        public static Logger GetInstance()
        {
            return _logger;
        }

        /// <summary>
        /// コントラクタだよ。
        /// ログファイルを初期化をするよ
        /// </summary>
        private Logger()
        {
            // 実行ファイルと同フォルダにloggingがある場合
            if (File.Exists(@"logging"))
            {
                // ロギングをONにするよ
                _doLogging = true;

                // ログファイルの初期化だよ
                initialize();
            }
        }

        /// <summary>
        /// ログファイルを初期化するよ。
        /// </summary>
        private void initialize()
        {
            // 削除だよ
            delete();

            // 作成するよ
            create();

            // 読み取り専用にするよ。
            setReadonly();
        }

        /// <summary>
        /// 改行を挿入するよ
        /// 引数の数だけ改行するよ。
        /// 引数なしは一回だけ。
        /// ０以下はなにもしないよ
        /// </summary>
        public void br(int num = 1)
        {
            if (num < 1) return;

            string br = new StringBuilder().Insert(0, "\r\n", num - 1).ToString();
            write(br, false);
        }

        /// <summary>
        /// ロガーのインターフェースだよ。
        /// 文字列をそのまま書き込むよ。
        /// </summary>
        /// <param name="str"></param>
        public void print(string str)
        {
            write(str);
        }

        /// <summary>
        /// ロガーのインターフェースだよ。
        /// 数値をそのまま書き込むよ。
        /// </summary>
        /// <param name="str"></param>
        public void print(int str)
        {
            write(str.ToString());
        }

        /// <summary>
        /// ロガーのインターフェースだよ。
        /// 配列を文字列に変換して書き込むよ。
        /// array(
        /// array[1
        /// ,array[2]
        /// )
        /// って出るよ。
        /// </summary>
        /// <param name="arr"></param>
        public void print(string[] arr)
        {
            write("array(" + "\r\n"
                    + string.Join("\r\n", arr)
                + "\r\n)");
        }

        /// <summary>
        /// ロガーのインターフェースだよ。
        /// 文章にパラメータを与える感じで使うよ。
        /// Logger.print("明日は{0}の誕生日です。{1}をプレゼントしよう。,"えみちゃん", "HS")
        /// 明日はえみちゃんの誕生日です。HSをプレゼントしよう。
        /// デバグしてないので気を付けて。
        /// </summary>
        /// <param name="format"></param>
        /// <param name="obj"></param>
        public void print(string format, params object[] obj)
        {
            write(string.Format(format, obj));
        }

        /// <summary>
        /// 書き込むよ。
        /// 標準出力してからロギングするよ
        /// フォーマットはこんな感じだよ。
        /// [日時]　[クラス｜メソッド]　メッセージ
        /// [2021/08/22 14:33:34.142] [PSOChatLog.Logger|GetInstance] メッセージ
        /// </summary>
        /// <param name="message"></param>
        private void write(string message, bool enableFormat = true)
        {
            // ログのフォーマットを適用するよ
            if (enableFormat)
            {
                message = formatter(message);
            }

            // 標準出力だよ
            Console.WriteLine(message);

            if (_doLogging == false) return;

            // ログファイルへ書き込むよ
            logging(message);
        }

        /// <summary>
        /// ロギングの実体だよ。
        /// </summary>
        /// <param name="message"></param>
        private void logging(string message)
        {
            try
            {
                // ファイルを作成するよ
                create();

                // 書き込めるようにするよ
                setWritable();

                // 書くよ
                using (StreamWriter sw = new StreamWriter(
                        LOGFILE, true, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.WriteLine(message);
                }

                // 読み取り専用に戻すよ。
                setReadonly();
            }
            catch (IOException ex)
            {
                print(ex.StackTrace);
            }
        }

        private string formatter(string message)
        {
            // 日時だよ
            string datetime = DateTime.Now.ToString(DATETIME_FORMAT);
            // printをコールしたクラスメイトとかメソッド名、行番号だよ
            StackFrame sf = new StackFrame(3, true);
            string classname = sf.GetMethod().ReflectedType.FullName;
            string method = sf.GetMethod().Name;
            string lineNumber = sf.GetFileLineNumber().ToString();

            // LFをCRLFに変換するよ
            message = message.Replace("\n", "\r\n").Replace("\r\r", "\r");

            return $"[{datetime}] [{classname}|{method}|{lineNumber}] {message}";
        }

        private void delete()
        {
            try
            {
                // ログファイルがあったら消すよ
                if (File.Exists(LOGFILE))
                {
                    setWritable();
                    File.Delete(LOGFILE);
                }
            }
            catch (IOException ex)
            {
                print(ex.StackTrace);
            }
        }

        private void create()
        {
            try
            {
                // ログファイルがなかったら作るよ
                if (File.Exists(LOGFILE) == false)
                {
                    using (File.Create(LOGFILE)) { };
                }
            }
            catch (IOException ex)
            {
                print(ex.StackTrace);
            }
        }

        private void setReadonly()
        {
            File.SetAttributes(LOGFILE, File.GetAttributes(LOGFILE) | FileAttributes.ReadOnly);
        }

        private void setWritable()
        {
            File.SetAttributes(LOGFILE, File.GetAttributes(LOGFILE) & (~FileAttributes.ReadOnly));
        }
    }
}
