using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using fireBwall.UI;
using fireBwall.Configuration;
using fireBwall.Utils;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ARPPoisoningProtection
{
    public partial class ArpPoisoningProtection : DynamicUserControl
    {
        ARPPoisoningProtectionModule saap;
        SerializableDictionary<IPAddr, MACAddr> cache = new SerializableDictionary<IPAddr, MACAddr>();

        public ArpPoisoningProtection(ARPPoisoningProtectionModule saap)
            : base()
        {
            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Save", "Save ARP Cache");
            multistring.SetString(lang, "Unsolicted", "Log Unsolicited");
            multistring.SetString(lang, "Attacks", "Log Attacks");
            multistring.SetString(lang, "Remove", "Remove Entry");
            multistring.SetString(lang, "Clear", "Clear Cache");
            multistring.SetString(lang, "Rectify", "Rectify Attacks");

            lang = Language.CHINESE;
            multistring.SetString(lang, "Save", "保存 ARP 缓存");
            multistring.SetString(lang, "Unsolicted", "未经请求的日志");
            multistring.SetString(lang, "Attacks", "日志攻击");
            multistring.SetString(lang, "Remove", "删除项");
            multistring.SetString(lang, "Clear", "清除缓存");
            multistring.SetString(lang, "Rectify", "纠正的攻击");

            lang = Language.DUTCH;
            multistring.SetString(lang, "Save", "ARP-Cache opslaan");
            multistring.SetString(lang, "Unsolicted", "Log ongevraagde");
            multistring.SetString(lang, "Attacks", "Log aanvallen");
            multistring.SetString(lang, "Remove", "Vermelding verwijderen");
            multistring.SetString(lang, "Clear", "Clear Cache");
            multistring.SetString(lang, "Rectify", "Aanvallen corrigeren");

            lang = Language.FRENCH;
            multistring.SetString(lang, "Save", "Sauver le Cache ARP");
            multistring.SetString(lang, "Unsolicted", "Journal non sollicité");
            multistring.SetString(lang, "Attacks", "Journal des attaques");
            multistring.SetString(lang, "Remove", "Supprimer l'entrée");
            multistring.SetString(lang, "Clear", "Vider le Cache");
            multistring.SetString(lang, "Rectify", "Corriger les attaques");

            lang = Language.GERMAN;
            multistring.SetString(lang, "Save", "ARP-Cache speichern");
            multistring.SetString(lang, "Unsolicted", "Protokoll unerwünschte");
            multistring.SetString(lang, "Attacks", "Protokoll-Angriffe");
            multistring.SetString(lang, "Remove", "Eintrag entfernen");
            multistring.SetString(lang, "Clear", "Cache löschen");
            multistring.SetString(lang, "Rectify", "Angriffe zu korrigieren");

            lang = Language.HEBREW;
            multistring.SetString(lang, "Save", "שמירת מטמון ARP");
            multistring.SetString(lang, "Unsolicted", "יומן רישום לא רצויה");
            multistring.SetString(lang, "Attacks", "התקפות יומן רישום");
            multistring.SetString(lang, "Remove", "הסרת ערך");
            multistring.SetString(lang, "Clear", "נקה מטמון");
            multistring.SetString(lang, "Rectify", "ליישר את התקפות");

            lang = Language.ITALIAN;
            multistring.SetString(lang, "Save", "Salvare la Cache ARP");
            multistring.SetString(lang, "Unsolicted", "Registro non richiesto");
            multistring.SetString(lang, "Attacks", "Attacchi di registro");
            multistring.SetString(lang, "Remove", "Rimuovere la voce");
            multistring.SetString(lang, "Clear", "Svuota Cache");
            multistring.SetString(lang, "Rectify", "Rettificare gli attacchi");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "Save", "ARP キャッシュを保存します。");
            multistring.SetString(lang, "Unsolicted", "不要なログ");
            multistring.SetString(lang, "Attacks", "ログの攻撃");
            multistring.SetString(lang, "Remove", "エントリを削除します。");
            multistring.SetString(lang, "Clear", "[キャッシュのクリア");
            multistring.SetString(lang, "Rectify", "攻撃を修正します。");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "Save", "Salvar Cache ARP");
            multistring.SetString(lang, "Unsolicted", "Registro não solicitado");
            multistring.SetString(lang, "Attacks", "Ataques de registro");
            multistring.SetString(lang, "Remove", "Remover entrada");
            multistring.SetString(lang, "Clear", "Limpar Cache");
            multistring.SetString(lang, "Rectify", "Rectificar os ataques");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "Save", "Сохранение кэша ARP");
            multistring.SetString(lang, "Unsolicted", "Журнал незапрошенные");
            multistring.SetString(lang, "Attacks", "Журнал атаки");
            multistring.SetString(lang, "Remove", "Удалить запись");
            multistring.SetString(lang, "Clear", "Очистить кэш");
            multistring.SetString(lang, "Rectify", "Исправить атаки");

            lang = Language.SPANISH;
            multistring.SetString(lang, "Save", "Guardar la caché ARP");
            multistring.SetString(lang, "Unsolicted", "Registro no solicitado");
            multistring.SetString(lang, "Attacks", "Ataques de registro");
            multistring.SetString(lang, "Remove", "Eliminar entrada");
            multistring.SetString(lang, "Clear", "Borrar caché");
            multistring.SetString(lang, "Rectify", "Rectificar los ataques");

            if (null != saap)
            {
                this.saap = saap;
                cache = saap.GetCache();
                saap.UpdatedArpCache += new System.Threading.ThreadStart(saap_UpdatedArpCache);
                InitializeComponent();
                saap_UpdatedArpCache();
            }
        }

        void saap_UpdatedArpCache()
        {
            if (listBox1.InvokeRequired)
            {
                cache = saap.GetCache();
                System.Threading.ThreadStart ts = new System.Threading.ThreadStart(saap_UpdatedArpCache);
                listBox1.Invoke(ts);
            }
            else
            {
                listBox1.Items.Clear();
                foreach (KeyValuePair<IPAddr, MACAddr> i in cache)
                {                    
                    listBox1.Items.Add(i.Value.ToString() + " -> " + i.Key.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxSave_CheckedChanged(object sender, EventArgs e)
        {
            saap.data.Save = checkBoxSave.Checked;
        }

        private void checkBoxLogUnsolicited_CheckedChanged(object sender, EventArgs e)
        {
            saap.data.LogUnsolic = checkBoxLogUnsolicited.Checked;
        }

        private void checkBoxLogPoisoning_CheckedChanged(object sender, EventArgs e)
        {
            saap.data.LogAttacks = checkBoxLogPoisoning.Checked;
        }

        public override void LanguageChanged()
        {
            checkBoxSave.Text = multistring.GetString("Save");
            checkBoxLogUnsolicited.Text = multistring.GetString("Unsolicted");
            checkBoxLogPoisoning.Text = multistring.GetString("Attacks");
            button1.Text = multistring.GetString("Remove");
            button2.Text = multistring.GetString("Clear");
            checkBoxRectify.Text = multistring.GetString("Rectify");
        }

        private void ArpPoisoningProtection_Load(object sender, EventArgs e)
        {
            checkBoxLogPoisoning.Checked = saap.data.LogAttacks;
            checkBoxLogUnsolicited.Checked = saap.data.LogUnsolic;
            checkBoxSave.Checked = saap.data.Save;
            checkBoxRectify.Checked = saap.data.RectifyAttacks;

            saap_UpdatedArpCache();
            ThemeChanged();
            LanguageChanged();
        }

        private void checkBoxRectify_CheckedChanged(object sender, EventArgs e)
        {
            saap.data.RectifyAttacks = checkBoxRectify.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string i = (string)listBox1.SelectedItem;
                    IPAddr ip = IPAddr.Parse(i.Split(' ')[2]);
                    cache.Remove(ip);
                    saap.UpdateCache(cache);
                    cache = saap.GetCache();
                    saap_UpdatedArpCache();
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cache.Clear();
            saap.UpdateCache(cache);
            cache = saap.GetCache();
            saap_UpdatedArpCache();
        }
    }
}
