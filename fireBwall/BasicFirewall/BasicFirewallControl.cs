using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using fireBwall.UI;
using fireBwall.Logging;
using fireBwall.Configuration;

namespace BasicFirewall
{
    public partial class BasicFirewallControl : DynamicUserControl
    {
        BasicFirewallModule basicfirewall;

        public BasicFirewallControl(BasicFirewallModule basicfirewall):base()
        {
            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Add Rule", "Add Rule");
            multistring.SetString(lang, "Edit Rule", "Edit Rule");
            multistring.SetString(lang, "Remove Rule", "Remove Rule");
            multistring.SetString(lang, "Move Down", "Move Down");
            multistring.SetString(lang, "Move Up", "Move Up");

            lang = Language.CHINESE;
            multistring.SetString(lang, "Add Rule", "添加规则");
            multistring.SetString(lang, "Remove Rule", "删除规则");
            multistring.SetString(lang, "Move Down", "向下移动");
            multistring.SetString(lang, "Move Up", "向上移动");
            multistring.SetString(lang, "Edit Rule", "编辑规则");

            lang = Language.DUTCH;
            multistring.SetString(lang, "Add Rule", "Regel toevoegen");
            multistring.SetString(lang, "Remove Rule", "Regel verwijderen");
            multistring.SetString(lang, "Move Down", "Omlaag verplaatsen");
            multistring.SetString(lang, "Move Up", "Omhoog");
            multistring.SetString(lang, "Edit Rule", "Regel bewerken");

            lang = Language.FRENCH;
            multistring.SetString(lang, "Add Rule", "Ajoutez la règle");
            multistring.SetString(lang, "Remove Rule", "Supprimer la règle");
            multistring.SetString(lang, "Move Down", "Déplacer vers le bas");
            multistring.SetString(lang, "Move Up", "Déplacez vers le haut");
            multistring.SetString(lang, "Edit Rule", "Modifier la règle");

            lang = Language.GERMAN;
            multistring.SetString(lang, "Add Rule", "Regel hinzufügen");
            multistring.SetString(lang, "Remove Rule", "Regel entfernen");
            multistring.SetString(lang, "Move Down", "Nach unten");
            multistring.SetString(lang, "Move Up", "Nach oben");
            multistring.SetString(lang, "Edit Rule", "Regel bearbeiten");

            lang = Language.HEBREW;
            multistring.SetString(lang, "Add Rule", "הוספת כלל");
            multistring.SetString(lang, "Remove Rule", "הסרת כלל");
            multistring.SetString(lang, "Move Down", "הזז למטה");
            multistring.SetString(lang, "Move Up", "הזז למעלה");
            multistring.SetString(lang, "Edit Rule", "עריכת כלל");

            lang = Language.ITALIAN;
            multistring.SetString(lang, "Add Rule", "Aggiungi regola");
            multistring.SetString(lang, "Remove Rule", "Rimuovere regola");
            multistring.SetString(lang, "Move Down", "Spostare verso il basso");
            multistring.SetString(lang, "Move Up", "Spostarsi verso l'alto");
            multistring.SetString(lang, "Edit Rule", "Modifica regola");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "Add Rule", "ルールを追加します。");
            multistring.SetString(lang, "Remove Rule", "規則を削除します。");
            multistring.SetString(lang, "Move Down", "下に移動します。");
            multistring.SetString(lang, "Move Up", "上に移動します。");
            multistring.SetString(lang, "Edit Rule", "規則を編集します。");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "Add Rule", "Adicionar regra");
            multistring.SetString(lang, "Remove Rule", "Remover regra");
            multistring.SetString(lang, "Move Down", "Mover para baixo");
            multistring.SetString(lang, "Move Up", "Mover para cima");
            multistring.SetString(lang, "Edit Rule", "Editar regra");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "Add Rule", "Добавить правило");
            multistring.SetString(lang, "Remove Rule", "Удаление правила");
            multistring.SetString(lang, "Move Down", "Переместить вниз");
            multistring.SetString(lang, "Move Up", "Переместить вверх");
            multistring.SetString(lang, "Edit Rule", "Изменение правила");

            lang = Language.SPANISH;
            multistring.SetString(lang, "Add Rule", "Agregar regla");
            multistring.SetString(lang, "Remove Rule", "Eliminar regla");
            multistring.SetString(lang, "Move Down", "Mover hacia abajo");
            multistring.SetString(lang, "Move Up", "Mover hacia arriba");
            multistring.SetString(lang, "Edit Rule", "Editar regla");

            this.basicfirewall = basicfirewall;
            InitializeComponent();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index != 0)
                {
                    Rule rule = (Rule)listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    index--;
                    listBox1.Items.Insert(index, rule);
                    listBox1.SelectedIndex = index;
                    List<Rule> r = new List<Rule>();
                    foreach (object ru in listBox1.Items)
                    {
                        r.Add((Rule)ru);
                    }

                    basicfirewall.InstanceGetRuleUpdates(r);
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditRule aer = new AddEditRule();
                if (aer.ShowDialog() == DialogResult.OK)
                {
                    listBox1.Items.Add(aer.NewRule);
                    List<Rule> r = new List<Rule>();
                    foreach (object rule in listBox1.Items)
                    {
                        r.Add((Rule)rule);
                    }

                    basicfirewall.InstanceGetRuleUpdates(r);
                }
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null) return;
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

                List<Rule> r = new List<Rule>();
                foreach (object rule in listBox1.Items)
                {
                    r.Add((Rule)rule);
                }

                basicfirewall.InstanceGetRuleUpdates(r);
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }

        public override void LanguageChanged()
        {
            button1.Text = multistring.GetString("Add Rule");
            button2.Text = multistring.GetString("Remove Rule");
            buttonMoveDown.Text = multistring.GetString("Move Down");
            buttonMoveUp.Text = multistring.GetString("Move Up");
            editButton.Text = multistring.GetString("Edit Rule");
        }

        private void BasicFirewallControl_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "String";
            List<Rule> r = new List<Rule>(basicfirewall.rules);
            foreach (Rule rule in r)
            {
                listBox1.Items.Add(rule);
            }           

            ThemeChanged();
            LanguageChanged();
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index != listBox1.Items.Count - 1)
                {
                    Rule rule = (Rule)listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    index++;
                    listBox1.Items.Insert(index, rule);
                    listBox1.SelectedIndex = index;
                    List<Rule> r = new List<Rule>();
                    foreach (object ru in listBox1.Items)
                    {
                        r.Add((Rule)ru);
                    }

                    basicfirewall.InstanceGetRuleUpdates(r);
                }
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }

        /// <summary>
        /// Edits the selected rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            try
            {
                int idx = listBox1.SelectedIndex;
                Rule tmp = basicfirewall.rules[idx];
                AddEditRule aer = new AddEditRule(tmp);

                // show dialog and confirm changes
                if (aer.ShowDialog() == DialogResult.OK)
                {
                    // insert the new rule
                    listBox1.Items[idx] = aer.NewRule;
                    List<Rule> r = new List<Rule>();
                    foreach (object rule in listBox1.Items)
                    {
                        r.Add((Rule)rule);
                    }

                    basicfirewall.InstanceGetRuleUpdates(r);
                }
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }
    }
}
