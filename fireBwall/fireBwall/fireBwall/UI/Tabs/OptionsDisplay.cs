using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

using fireBwall.Configuration;
using fireBwall.Logging;

namespace fireBwall.UI.Tabs
{
    public partial class OptionsDisplay : DynamicUserControl
    {
        public OptionsDisplay():base()
        {
            multistring.SetString(Language.ENGLISH, "Display Icon Popups", "Display Icon Popups");
            multistring.SetString(Language.ENGLISH, "Language: ", "Language: ");
            multistring.SetString(Language.ENGLISH, "Start Minimized", "Start Minimized");
            multistring.SetString(Language.ENGLISH, "Interval>9", "The interval must be 10 minutes or greater");
            multistring.SetString(Language.ENGLISH, "Dev", "Developer Mode");
            multistring.SetString(Language.ENGLISH, "MaxLog", "Max Logs:");
            multistring.SetString(Language.ENGLISH, "MaxPLog", "Max Packet Logs:");
            multistring.SetString(Language.ENGLISH, "Theme", "Theme:");
            multistring.SetString(Language.ENGLISH, "MinInt", "Interval in Minutes:");
            multistring.SetString(Language.ENGLISH, "GenConf", "General Configuration");
            multistring.SetString(Language.ENGLISH, "UpConf", "Update Options");
            multistring.SetString(Language.ENGLISH, "EditCurrent", "Edit");
            multistring.SetString(Language.ENGLISH, "Check", "Check on Start Up");
            multistring.SetString(Language.ENGLISH, "Inter", "Check on Interval");
            multistring.SetString(Language.ENGLISH, "Load Module", "Load Module");
            multistring.SetString(Language.ENGLISH, "Load Theme", "Load Theme");

            multistring.SetString(Language.DUTCH, "Display Icon Popups", "Display pictogram Popups");
            multistring.SetString(Language.DUTCH, "Language: ", "Taal: ");
            multistring.SetString(Language.DUTCH, "Start Minimized", "Geminimaliseerd starten");
            multistring.SetString(Language.DUTCH, "Interval>9", "De interval moet 10 minuten of meer");
            multistring.SetString(Language.DUTCH, "Dev", "Ontwikkelaarsmodus");
            multistring.SetString(Language.DUTCH, "MaxLog", "Maximale Logs:");
            multistring.SetString(Language.DUTCH, "MaxPLog", "Maximale Packet Logs:");
            multistring.SetString(Language.DUTCH, "Theme", "Thema:");
            multistring.SetString(Language.DUTCH, "MinInt", "Interval in minuten:");
            multistring.SetString(Language.DUTCH, "GenConf", "Algemene configuratie");
            multistring.SetString(Language.DUTCH, "UpConf", "Bijwerkopties");
            multistring.SetString(Language.DUTCH, "EditCurrent", "Bewerken");
            multistring.SetString(Language.DUTCH, "Check", "Controleren op opstarten");
            multistring.SetString(Language.DUTCH, "Inter", "Controleren op Interval");
            multistring.SetString(Language.DUTCH, "Load Module", "Load Module");
            multistring.SetString(Language.DUTCH, "Load Theme", "Laden thema");

            multistring.SetString(Language.HEBREW, "Display Icon Popups", "הצג סמל הודעות מוקפצות");
            multistring.SetString(Language.HEBREW, "Language: ", "שפה:");
            multistring.SetString(Language.HEBREW, "Start Minimized", "הפעל במצב ממוזער");
            multistring.SetString(Language.HEBREW, "Interval>9", "מרווח הזמן חייב להיות 10 דקות ומעלה");
            multistring.SetString(Language.HEBREW, "Dev", "מצב מפתח");
            multistring.SetString(Language.HEBREW, "MaxLog", "יומני רישום של מקס:");
            multistring.SetString(Language.HEBREW, "MaxPLog", "יומני רישום של מנה מירבי:");
            multistring.SetString(Language.HEBREW, "Theme", "דירוג:");
            multistring.SetString(Language.HEBREW, "MinInt", "מרווח הזמן בדקות:");
            multistring.SetString(Language.HEBREW, "GenConf", "תצורה כלליות");
            multistring.SetString(Language.HEBREW, "UpConf", "אפשרויות עדכון");
            multistring.SetString(Language.HEBREW, "EditCurrent", "עריכה");
            multistring.SetString(Language.HEBREW, "Check", "בדיקת הפעלה");
            multistring.SetString(Language.HEBREW, "Inter", "בדוק מרווח");
            multistring.SetString(Language.HEBREW, "Load Module", "טעינת המודול");
            multistring.SetString(Language.HEBREW, "Load Theme", "ערכת נושא של עומס");

            multistring.SetString(Language.PORTUGUESE, "Display Icon Popups", "Mostrar Popups Icon");
            multistring.SetString(Language.PORTUGUESE, "Language: ", "Linguagem: ");
            multistring.SetString(Language.PORTUGUESE, "Start Minimized", "Iniciar minimizado");
            multistring.SetString(Language.PORTUGUESE, "Interval>9", "O intervalo deve ser de 10 minutos ou maior");
            multistring.SetString(Language.PORTUGUESE, "Dev", "Modo de desenvolvedor");
            multistring.SetString(Language.PORTUGUESE, "MaxLog", "Logs de Max:");
            multistring.SetString(Language.PORTUGUESE, "MaxPLog", "Max pacote Logs:");
            multistring.SetString(Language.PORTUGUESE, "Theme", "Tema:");
            multistring.SetString(Language.PORTUGUESE, "MinInt", "Intervalo, em minutos:");
            multistring.SetString(Language.PORTUGUESE, "GenConf", "Configuração geral");
            multistring.SetString(Language.PORTUGUESE, "UpConf", "Opções de atualização");
            multistring.SetString(Language.PORTUGUESE, "EditCurrent", "Editar");
            multistring.SetString(Language.PORTUGUESE, "Check", "Verificar no arranque");
            multistring.SetString(Language.PORTUGUESE, "Inter", "Verifique no intervalo");
            multistring.SetString(Language.PORTUGUESE, "Load Module", "Módulo de carga");
            multistring.SetString(Language.PORTUGUESE, "Load Theme", "Tema de carga");

            multistring.SetString(Language.CHINESE, "Display Icon Popups", "显示图标弹出窗口");
            multistring.SetString(Language.CHINESE, "Language: ", "語言標籤: ");
            multistring.SetString(Language.CHINESE, "Start Minimized", "最小化启动");
            multistring.SetString(Language.CHINESE, "Interval>9", "时间间隔必须 10 分钟或更大");
            multistring.SetString(Language.CHINESE, "Dev", "开发人员模式");
            multistring.SetString(Language.CHINESE, "MaxLog", "最大日志:");
            multistring.SetString(Language.CHINESE, "MaxPLog", "最大数据包日志:");
            multistring.SetString(Language.CHINESE, "Theme", "主题:");
            multistring.SetString(Language.CHINESE, "MinInt", "间隔分钟数:");
            multistring.SetString(Language.CHINESE, "GenConf", "常规配置");
            multistring.SetString(Language.CHINESE, "UpConf", "更新选项");
            multistring.SetString(Language.CHINESE, "EditCurrent", "编辑");
            multistring.SetString(Language.CHINESE, "Check", "启动时检查");
            multistring.SetString(Language.CHINESE, "Inter", "检查的时间间隔");
            multistring.SetString(Language.CHINESE, "Load Module", "加载模块");
            multistring.SetString(Language.CHINESE, "Load Theme", "负载主题");

            multistring.SetString(Language.GERMAN, "Display Icon Popups", "Anzeige Icon Popups");
            multistring.SetString(Language.GERMAN, "Language: ", "Sprachensiegel: ");
            multistring.SetString(Language.GERMAN, "Start Minimized", "Minimiert starten");
            multistring.SetString(Language.GERMAN, "Interval>9", "Das Intervall muss 10 Minuten oder mehr");
            multistring.SetString(Language.GERMAN, "Dev", "Entwicklermodus");
            multistring.SetString(Language.GERMAN, "MaxLog", "Max-Protokolle:");
            multistring.SetString(Language.GERMAN, "MaxPLog", "Max Packet Protokolle:");
            multistring.SetString(Language.GERMAN, "Theme", "Thema:");
            multistring.SetString(Language.GERMAN, "MinInt", "Das Intervall in Minuten:");
            multistring.SetString(Language.GERMAN, "GenConf", "Allgemeine Konfiguration");
            multistring.SetString(Language.GERMAN, "UpConf", "Update-Optionen");
            multistring.SetString(Language.GERMAN, "EditCurrent", "Bearbeiten");
            multistring.SetString(Language.GERMAN, "Check", "Überprüfen Sie auf der Start-Up");
            multistring.SetString(Language.GERMAN, "Inter", "Prüfen Sie auf Intervall");
            multistring.SetString(Language.GERMAN, "Load Module", "Modul");
            multistring.SetString(Language.GERMAN, "Load Theme", "Laden-Thema");

            multistring.SetString(Language.RUSSIAN, "Display Icon Popups", "Показать Иконка всплывающие окна");
            multistring.SetString(Language.RUSSIAN, "Language: ", "Язык этикетки: ");
            multistring.SetString(Language.RUSSIAN, "Start Minimized", "Запуск в свернутом виде");
            multistring.SetString(Language.RUSSIAN, "Interval>9", "Интервал должен составлять 10 минут или больше");
            multistring.SetString(Language.RUSSIAN, "Dev", "Режим разработчика");
            multistring.SetString(Language.RUSSIAN, "MaxLog", "Макс журналы:");
            multistring.SetString(Language.RUSSIAN, "MaxPLog", "Макс пакет журналы:");
            multistring.SetString(Language.RUSSIAN, "Theme", "Тема:");
            multistring.SetString(Language.RUSSIAN, "MinInt", "Интервал в минутах:");
            multistring.SetString(Language.RUSSIAN, "GenConf", "Общие настройки");
            multistring.SetString(Language.RUSSIAN, "UpConf", "Параметры обновления");
            multistring.SetString(Language.RUSSIAN, "EditCurrent", "Редактирование");
            multistring.SetString(Language.RUSSIAN, "Check", "Проверка на запуск");
            multistring.SetString(Language.RUSSIAN, "Inter", "Проверка на интервале");
            multistring.SetString(Language.RUSSIAN, "Load Module", "Загрузить модуль");
            multistring.SetString(Language.RUSSIAN, "Load Theme", "Загрузка тема");

            multistring.SetString(Language.SPANISH, "Display Icon Popups", "Mostrar ventanas emergentes Icono");
            multistring.SetString(Language.SPANISH, "Language: ", "Lenguaje de etiquetas: ");
            multistring.SetString(Language.SPANISH, "Start Minimized", "Iniciar minimizado");
            multistring.SetString(Language.SPANISH, "Interval>9", "El intervalo debe ser 10 minutos o mayor");
            multistring.SetString(Language.SPANISH, "Dev", "Modo de programador");
            multistring.SetString(Language.SPANISH, "MaxLog", "Registros de Max:");
            multistring.SetString(Language.SPANISH, "MaxPLog", "Registros de paquete Max:");
            multistring.SetString(Language.SPANISH, "Theme", "Tema:");
            multistring.SetString(Language.SPANISH, "MinInt", "Intervalo en minutos:");
            multistring.SetString(Language.SPANISH, "GenConf", "Configuración general");
            multistring.SetString(Language.SPANISH, "UpConf", "Opciones de actualización");
            multistring.SetString(Language.SPANISH, "EditCurrent", "Editar");
            multistring.SetString(Language.SPANISH, "Check", "Comprobar al inicio");
            multistring.SetString(Language.SPANISH, "Inter", "Comprobar el intervalo");
            multistring.SetString(Language.SPANISH, "Load Module", "Módulo de carga");
            multistring.SetString(Language.SPANISH, "Load Theme", "Tema de carga");

            multistring.SetString(Language.JAPANESE, "Display Icon Popups", "表示アイコンのポップアップ");
            multistring.SetString(Language.JAPANESE, "Language: ", "言語: ");
            multistring.SetString(Language.JAPANESE, "Start Minimized", "最小化を開始します。");
            multistring.SetString(Language.JAPANESE, "Interval>9", "間隔は 10 分する必要があります以上");
            multistring.SetString(Language.JAPANESE, "Dev", "開発者モード");
            multistring.SetString(Language.JAPANESE, "MaxLog", "最大ログ:");
            multistring.SetString(Language.JAPANESE, "MaxPLog", "最大パケット ログ:");
            multistring.SetString(Language.JAPANESE, "Theme", "テーマ:");
            multistring.SetString(Language.JAPANESE, "MinInt", "間隔を分単位:");
            multistring.SetString(Language.JAPANESE, "GenConf", "一般的な構成");
            multistring.SetString(Language.JAPANESE, "UpConf", "更新オプション");
            multistring.SetString(Language.JAPANESE, "EditCurrent", "編集");
            multistring.SetString(Language.JAPANESE, "Check", "起動を確認します。");
            multistring.SetString(Language.JAPANESE, "Inter", "間隔をチェックします。");
            multistring.SetString(Language.JAPANESE, "Load Module", "負荷モジュール");
            multistring.SetString(Language.JAPANESE, "Load Theme", "負荷のテーマ");

            multistring.SetString(Language.ITALIAN, "Display Icon Popups", "Visualizzazione icona pop-up");
            multistring.SetString(Language.ITALIAN, "Language: ", "Lingua: ");
            multistring.SetString(Language.ITALIAN, "Start Minimized", "Avvia ridotto a icona");
            multistring.SetString(Language.ITALIAN, "Interval>9", "L'intervallo deve essere di 10 minuti o superiore");
            multistring.SetString(Language.ITALIAN, "Dev", "Modalità sviluppatore");
            multistring.SetString(Language.ITALIAN, "MaxLog", "Registri di Max:");
            multistring.SetString(Language.ITALIAN, "MaxPLog", "Registri di pacchetti Max:");
            multistring.SetString(Language.ITALIAN, "Theme", "Tema:");
            multistring.SetString(Language.ITALIAN, "MinInt", "Intervallo in minuti:");
            multistring.SetString(Language.ITALIAN, "GenConf", "Configurazione generale");
            multistring.SetString(Language.ITALIAN, "UpConf", "Opzioni di aggiornamento");
            multistring.SetString(Language.ITALIAN, "EditCurrent", "Modifica");
            multistring.SetString(Language.ITALIAN, "Check", "Check su Start Up");
            multistring.SetString(Language.ITALIAN, "Inter", "Controllare in un intervallo");
            multistring.SetString(Language.ITALIAN, "Load Module", "Modulo di carico");
            multistring.SetString(Language.ITALIAN, "Load Theme", "Tema di carico");

            multistring.SetString(Language.FRENCH, "Display Icon Popups", "Affichage icône Popups");
            multistring.SetString(Language.FRENCH, "Language: ", "Langue: ");
            multistring.SetString(Language.FRENCH, "Start Minimized", "Démarrer minimisé");
            multistring.SetString(Language.FRENCH, "Interval>9", "L'intervalle doit être de 10 minutes ou plus");
            multistring.SetString(Language.FRENCH, "Dev", "Mode développeur");
            multistring.SetString(Language.FRENCH, "MaxLog", "Billes de Max:");
            multistring.SetString(Language.FRENCH, "MaxPLog", "Paquet de Max billes:");
            multistring.SetString(Language.FRENCH, "Theme", "Thème:");
            multistring.SetString(Language.FRENCH, "MinInt", "Intervalle en Minutes:");
            multistring.SetString(Language.FRENCH, "GenConf", "Configuration générale");
            multistring.SetString(Language.FRENCH, "UpConf", "Options de mise à jour");
            multistring.SetString(Language.FRENCH, "EditCurrent", "Edit");
            multistring.SetString(Language.FRENCH, "Check", "Vérifiez sur le démarrage");
            multistring.SetString(Language.FRENCH, "Inter", "Vérifiez sur l'intervalle");
            multistring.SetString(Language.FRENCH, "Load Module", "Module de charge");
            multistring.SetString(Language.FRENCH, "Load Theme", "Thème de la charge");

            InitializeComponent();
        }

        /// <summary>
        /// Event for when displayTrayLogs changes state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayTrayLogs_CheckedChanged(object sender, EventArgs e)
        {
            GeneralConfiguration.Instance.ShowPopups = displayTrayLogs.Checked;
        }

        /// <summary>
        /// Language load for option settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsDisplay_Load(object sender, EventArgs e)
        {
            foreach (string theme in ThemeConfiguration.Instance.Schemes.Keys)
                themeBox.Items.Add(theme);
            themeBox.SelectedItem = GeneralConfiguration.Instance.CurrentTheme;

            switch (GeneralConfiguration.Instance.PreferredLanguage)
            {
                case null:
                case "en":
                    languageBox.SelectedIndex = 1;
                    break;
                case "pt":
                    languageBox.SelectedIndex = 6;
                    break;
                case "zh":
                    languageBox.SelectedIndex = 4;
                    break;
                case "de":
                    languageBox.SelectedIndex = 3;
                    break;
                case "ru":
                    languageBox.SelectedIndex = 5;
                    break;
                case "es":
                    languageBox.SelectedIndex = 2;
                    break;
                case "ja":
                    languageBox.SelectedIndex = 7;
                    break;
                case "fr":
                    languageBox.SelectedIndex = 8;
                    break;
                case "it":
                    languageBox.SelectedIndex = 9;
                    break;
                case "he":
                    languageBox.SelectedIndex = 10;
                    break;
                case "nl":
                    languageBox.SelectedIndex = 11;
                    break;
            }
            checkBox1.Checked = GeneralConfiguration.Instance.CheckUpdateOnStartup;
            checkBox2.Checked = GeneralConfiguration.Instance.IntervaledUpdateChecks;
            checkBox3.Checked = GeneralConfiguration.Instance.DeveloperMode;
            checkBoxStartMinimized.Checked = GeneralConfiguration.Instance.StartMinimized;
            textBox1.Text = GeneralConfiguration.Instance.IntervaledUpdateMinutes.ToString();
            displayTrayLogs.Checked = GeneralConfiguration.Instance.ShowPopups;

            maxLogsBox.Text = Convert.ToString(GeneralConfiguration.Instance.MaxLogs);
            maxPcapBox.Text = Convert.ToString(GeneralConfiguration.Instance.MaxPcapLogs);

            LanguageChanged();
            ThemeChanged();

            this.displayTrayLogs.CheckedChanged += new System.EventHandler(this.displayTrayLogs_CheckedChanged);
            this.languageBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.checkBoxStartMinimized.CheckedChanged += new System.EventHandler(this.checkBoxStartMinimized_CheckedChanged);
            this.themeBox.SelectedIndexChanged += new System.EventHandler(this.themeBox_SelectedIndexChanged);
            this.maxLogsBox.TextChanged += new System.EventHandler(this.maxLogsBox_TextChanged);
            this.maxPcapBox.TextChanged += new System.EventHandler(this.maxPcapBox_TextChanged);
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
        }

        public override void LanguageChanged()
        {
            displayTrayLogs.Text = multistring.GetString("Display Icon Popups");
            languageLabel.Text = multistring.GetString("Language: ");
            checkBoxStartMinimized.Text = multistring.GetString("Start Minimized");
            label4.Text = multistring.GetString("Interval>9");
            checkBox3.Text = multistring.GetString("Dev");
            label5.Text = multistring.GetString("MaxLog");
            label2.Text = multistring.GetString("MaxPLog");
            label1.Text = multistring.GetString("Theme");
            label3.Text = multistring.GetString("MinInt");
            groupBox2.Text = multistring.GetString("GenConf");
            groupBox1.Text = multistring.GetString("UpConf");
            button1.Text = multistring.GetString("EditCurrent");
            checkBox1.Text = multistring.GetString("Check");
            checkBox2.Text = multistring.GetString("Inter");
            buttonLoadModule.Text = multistring.GetString("Load Module");
            buttonLoadTheme.Text = multistring.GetString("Load Theme");
        }

        public override void ThemeChanged()
        {
            themeBox.Items.Clear();
            foreach (string theme in ThemeConfiguration.Instance.Schemes.Keys)
                themeBox.Items.Add(theme);
            themeBox.SelectedItem = GeneralConfiguration.Instance.CurrentTheme;
            ThemeConfiguration.Instance.SetColorScheme(this);
        }

        /// <summary>
        /// Let the user select their language!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (languageBox.SelectedIndex)
            {
                case 0:
                    GeneralConfiguration.Instance.PreferredLanguage = "en";
                    break;
                case 1:
                    GeneralConfiguration.Instance.PreferredLanguage = "en";
                    break;
                case 2:
                    GeneralConfiguration.Instance.PreferredLanguage = "es";
                    break;
                case 3:
                    GeneralConfiguration.Instance.PreferredLanguage = "de";
                    break;
                case 4:
                    GeneralConfiguration.Instance.PreferredLanguage = "zh";
                    break;
                case 5:
                    GeneralConfiguration.Instance.PreferredLanguage = "ru";
                    break;
                case 6:
                    GeneralConfiguration.Instance.PreferredLanguage = "pt";
                    break;
                case 7:
                    GeneralConfiguration.Instance.PreferredLanguage = "ja";
                    break;
                case 8:
                    GeneralConfiguration.Instance.PreferredLanguage = "fr";
                    break;
                case 9:
                    GeneralConfiguration.Instance.PreferredLanguage = "it";
                    break;
                case 10:
                    GeneralConfiguration.Instance.PreferredLanguage = "he";
                    break;
                case 11:
                    GeneralConfiguration.Instance.PreferredLanguage = "nl";
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            GeneralConfiguration.Instance.IntervaledUpdateChecks = checkBox2.Checked;
            GeneralConfiguration.Instance.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GeneralConfiguration.Instance.CheckUpdateOnStartup = checkBox1.Checked;
            GeneralConfiguration.Instance.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            uint v = 0;
            if (uint.TryParse(textBox1.Text, out v) && v >= 10)
            {
                GeneralConfiguration.Instance.IntervaledUpdateMinutes = v;
                GeneralConfiguration.Instance.Save();
            }
        }

        private void checkBoxStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            GeneralConfiguration.Instance.StartMinimized = checkBoxStartMinimized.Checked;
            GeneralConfiguration.Instance.Save();
        }

        private void themeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThemeConfiguration.Instance.ChangeTheme((string)themeBox.SelectedItem);
            ConfigurationManagement.Instance.SaveAllConfigurations();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorSchemeEditor cse = new ColorSchemeEditor(GeneralConfiguration.Instance.CurrentTheme);
            cse.Width = 640;
            cse.Height = 480;
            cse.Show();
        }

        /// <summary>
        /// Stores the new max log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxLogsBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                uint tmp = Convert.ToUInt32(maxLogsBox.Text);
                if (tmp < UInt32.MaxValue)
                {
                    GeneralConfiguration.Instance.MaxLogs = tmp;
                    GeneralConfiguration.Instance.Save();
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Stores the new max pcap log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxPcapBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                uint tmp = Convert.ToUInt32(maxPcapBox.Text);
                if (tmp < UInt32.MaxValue)
                {
                    GeneralConfiguration.Instance.MaxPcapLogs = tmp;
                    GeneralConfiguration.Instance.Save();
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            GeneralConfiguration.Instance.DeveloperMode = checkBox3.Checked;
            GeneralConfiguration.Instance.Save();
        }

        private void buttonLoadModule_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Module DLL (*.dll)|*.dll";
            ofd.InitialDirectory = ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "modules";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (fireBwall.Filters.NDIS.INDISFilter filter in ProcessingConfiguration.Instance.NDISFilterList.GetAllAdapters())
                {
                    filter.Modules.LoadModule(ofd.FileName);
                    filter.Modules.UpdateModuleOrder();
                }
            }
        }

        private void buttonLoadTheme_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.InitialDirectory = ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ThemeConfiguration.Instance.Load(ofd.FileName, true);
            }
        }
    }
}
