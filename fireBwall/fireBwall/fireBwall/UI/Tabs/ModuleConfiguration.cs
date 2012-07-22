using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using fireBwall.Filters.NDIS;
using System.Text;
using System.Windows.Forms;
using fireBwall.Configuration;
using fireBwall.Logging;

namespace fireBwall.UI.Tabs
{
    public partial class ModuleConfiguration : DynamicUserControl
    {
        List<KeyValuePair<bool, string>> moduleOrder = new List<KeyValuePair<bool, string>>();
        INDISFilter na;
        bool loading = false;
        readonly object padlock = new object();

        public ModuleConfiguration(INDISFilter na)
        {
            multistring.SetString(Language.ENGLISH, "Enable/Disable", "Enable/Disable");
            multistring.SetString(Language.ENGLISH, "Open Configuration", "Open Configuration");
            multistring.SetString(Language.ENGLISH, "Help", "Help");
            multistring.SetString(Language.ENGLISH, "Move Down", "Move Down");
            multistring.SetString(Language.ENGLISH, "Move Up", "Move Up");

            multistring.SetString(Language.DUTCH, "Enable/Disable", "Inschakelen/uitschakelen");
            multistring.SetString(Language.DUTCH, "Open Configuration", "Open configuratie");
            multistring.SetString(Language.DUTCH, "Help", "Help");
            multistring.SetString(Language.DUTCH, "Move Down", "Omlaag verplaatsen");
            multistring.SetString(Language.DUTCH, "Move Up", "Omhoog");

            multistring.SetString(Language.HEBREW, "Enable/Disable", "הפעל/בטל");
            multistring.SetString(Language.HEBREW, "Open Configuration", "תצורת פתוח");
            multistring.SetString(Language.HEBREW, "Help", "עזרה");
            multistring.SetString(Language.HEBREW, "Move Down", "הזז למטה");
            multistring.SetString(Language.HEBREW, "Move Up", "הזז למעלה");

            multistring.SetString(Language.PORTUGUESE, "Enable/Disable", "Activar / Desactivar");
            multistring.SetString(Language.PORTUGUESE, "Open Configuration", "Abrir Configuração");
            multistring.SetString(Language.PORTUGUESE, "Help", "Ajudar");
            multistring.SetString(Language.PORTUGUESE, "Move Down", "Mover para Baixo");
            multistring.SetString(Language.PORTUGUESE, "Move Up", "Mover para cima");

            multistring.SetString(Language.RUSSIAN, "Enable/Disable", "Включение / выключение");
            multistring.SetString(Language.RUSSIAN, "Open Configuration", "Открытая конфигурация");
            multistring.SetString(Language.RUSSIAN, "Help", "Помогите");
            multistring.SetString(Language.RUSSIAN, "Move Down", "спускать");
            multistring.SetString(Language.RUSSIAN, "Move Up", "вверх");

            multistring.SetString(Language.SPANISH, "Enable/Disable", "Activar / Desactivar");
            multistring.SetString(Language.SPANISH, "Open Configuration", "Abrir Configuración");
            multistring.SetString(Language.SPANISH, "Help", "Ayuda");
            multistring.SetString(Language.SPANISH, "Move Down", "Bajar");
            multistring.SetString(Language.SPANISH, "Move Up", "Subir");

            multistring.SetString(Language.CHINESE, "Enable/Disable", "启用/禁用");
            multistring.SetString(Language.CHINESE, "Open Configuration", "打开配置");
            multistring.SetString(Language.CHINESE, "Help", "帮助");
            multistring.SetString(Language.CHINESE, "Move Down", "下移");
            multistring.SetString(Language.CHINESE, "Move Up", "动起来");

            multistring.SetString(Language.GERMAN, "Enable/Disable", "Aktivieren / Deaktivieren");
            multistring.SetString(Language.GERMAN, "Open Configuration", "Konfiguration öffnen");
            multistring.SetString(Language.GERMAN, "Help", "Hilfe");
            multistring.SetString(Language.GERMAN, "Move Down", "Nach unten");
            multistring.SetString(Language.GERMAN, "Move Up", "Nach oben");

            multistring.SetString(Language.JAPANESE, "Enable/Disable", "有効化/無効化");
            multistring.SetString(Language.JAPANESE, "Open Configuration", "開いている構成");
            multistring.SetString(Language.JAPANESE, "Help", "ヘルプ");
            multistring.SetString(Language.JAPANESE, "Move Down", "下に移動します。");
            multistring.SetString(Language.JAPANESE, "Move Up", "上に移動します。");

            multistring.SetString(Language.ITALIAN, "Enable/Disable", "Attivare/disattivare");
            multistring.SetString(Language.ITALIAN, "Open Configuration", "Configurazione aperta");
            multistring.SetString(Language.ITALIAN, "Help", "Guida");
            multistring.SetString(Language.ITALIAN, "Move Down", "Spostare verso il basso");
            multistring.SetString(Language.ITALIAN, "Move Up", "Spostarsi verso l'alto");

            multistring.SetString(Language.FRENCH, "Enable/Disable", "Activer/désactiver");
            multistring.SetString(Language.FRENCH, "Open Configuration", "Configuration ouverte");
            multistring.SetString(Language.FRENCH, "Help", "Aide");
            multistring.SetString(Language.FRENCH, "Move Down", "Déplacer vers le bas");
            multistring.SetString(Language.FRENCH, "Move Up", "Déplacez vers le haut");

            this.na = na;
            moduleOrder = na.Modules.GetModuleOrder();
            InitializeComponent();
        }

        void UpdateView()
        {
            if (this.checkedListBoxModules.InvokeRequired)
            {
                System.Threading.ThreadStart ts = new System.Threading.ThreadStart(UpdateView);
                this.checkedListBoxModules.Invoke(ts);
            }
            else
            {
                lock (padlock)
                {
                    loading = true;
                    checkedListBoxModules.Items.Clear();
                    for (int x = 0; x < moduleOrder.Count; x++)
                    {
                        checkedListBoxModules.Items.Add(moduleOrder[x].Value, moduleOrder[x].Key);
                    }
                    loading = false;
                }
            }
        }

        public override void LanguageChanged()
        {
            buttonEnable.Text = multistring.GetString("Enable/Disable");
            buttonOpenConfiguration.Text = multistring.GetString("Open Configuration");
            buttonHelp.Text = multistring.GetString("Help");
            buttonMoveDown.Text = multistring.GetString("Move Down");
            buttonMoveUp.Text = multistring.GetString("Move Up");
        }

        private void ModuleConfiguration_Load(object sender, EventArgs e)
        {
            loading = true;
            UpdateView();
            LanguageChanged();
            ThemeChanged();

            loading = false;
            if (checkedListBoxModules.Items.Count != 0)
            {
                checkedListBoxModules.SelectedIndex = 0;
            }
        }

        private void buttonEnable_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = checkedListBoxModules.SelectedIndex;
                if (moduleOrder[checkedListBoxModules.SelectedIndex].Key)
                {
                    moduleOrder[checkedListBoxModules.SelectedIndex] = new KeyValuePair<bool, string>(false, moduleOrder[checkedListBoxModules.SelectedIndex].Value);
                }
                else
                {
                    moduleOrder[checkedListBoxModules.SelectedIndex] = new KeyValuePair<bool, string>(true, moduleOrder[checkedListBoxModules.SelectedIndex].Value);
                }                
                na.Modules.UpdateModuleOrder(moduleOrder);
                moduleOrder = na.Modules.GetModuleOrder();
                UpdateView();
                checkedListBoxModules.SelectedIndex = temp;
            }
            catch (Exception ne)
            {
                LogCenter.Instance.LogException(ne);
            }
        }

        private void checkedListBoxModules_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
            lock (padlock)
            {
                if (!loading && moduleOrder.Count > e.Index && e.Index != -1)
                {
                    moduleOrder[e.Index] = new KeyValuePair<bool, string>(e.NewValue == CheckState.Checked, moduleOrder[e.Index].Value);
                    na.Modules.UpdateModuleOrder(moduleOrder);
                    moduleOrder = na.Modules.GetModuleOrder();
                }
            }
        }

        private void buttonOpenConfiguration_Click(object sender, EventArgs e)
        {            
            try
            {
                if (moduleOrder[checkedListBoxModules.SelectedIndex].Key)
                {
                    DynamicUserControl uc = na.Modules.GetModule(checkedListBoxModules.SelectedIndex).GetUserInterface();
                    if (uc != null)
                    {
                        DynamicForm f = new DynamicForm();
                        f.Size = new System.Drawing.Size(640, 480);
                        f.Text = na.GetAdapterInformation().Name + ": " + na.Modules.GetModule(checkedListBoxModules.SelectedIndex).MetaData.GetMeta().Name + " - " + na.Modules.GetModule(checkedListBoxModules.SelectedIndex).MetaData.GetMeta().Version;
                        f.Controls.Add(uc);
                        f.Show();
                        f.ThemeChanged();
                    }
                }
            }
            catch (Exception ne)
            {
                LogCenter.Instance.LogException(ne);
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBoxModules.SelectedIndex != 0)
                {
                    KeyValuePair<bool, string> temp = moduleOrder[checkedListBoxModules.SelectedIndex];
                    moduleOrder.RemoveAt(checkedListBoxModules.SelectedIndex);
                    moduleOrder.Insert(checkedListBoxModules.SelectedIndex - 1, temp);
                    na.Modules.UpdateModuleOrder(moduleOrder);
                    moduleOrder = na.Modules.GetModuleOrder();
                    int newIndex = checkedListBoxModules.SelectedIndex - 1;
                    UpdateView();
                    checkedListBoxModules.SelectedIndex = newIndex;
                }
            }
            catch (Exception ne)
            {
                LogCenter.Instance.LogException(ne);
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBoxModules.SelectedIndex != moduleOrder.Count - 1)
                {
                    KeyValuePair<bool, string> temp = moduleOrder[checkedListBoxModules.SelectedIndex];
                    moduleOrder.RemoveAt(checkedListBoxModules.SelectedIndex);
                    moduleOrder.Insert(checkedListBoxModules.SelectedIndex + 1, temp);
                    na.Modules.UpdateModuleOrder(moduleOrder);
                    moduleOrder = na.Modules.GetModuleOrder();
                    int newIndex = checkedListBoxModules.SelectedIndex + 1;
                    UpdateView();
                    checkedListBoxModules.SelectedIndex = newIndex;
                }
            }
            catch (Exception ne)
            {
                LogCenter.Instance.LogException(ne);
            }
        }

        /// <summary>
        /// Builds the module Help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicForm f = new DynamicForm();
                f.Size = new System.Drawing.Size(640, 480);
                f.Text = "Help";
                Help uc = new Help(checkedListBoxModules.SelectedItem);
                uc.Dock = DockStyle.Fill;
                f.Controls.Add(uc);
                f.Show();
                f.ThemeChanged();
                uc.ThemeChanged();
            }
            catch (Exception ne)
            {
                LogCenter.Instance.LogException(ne);
            }
        }
    }
}
