using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace prKol_ind3_Moskvina_6
{
    public partial class Form1 : Form
    {
        List<Ukazatel> ukazatel = new List<Ukazatel>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string slovo = txtSl.Text.Trim();
            string str = txtStr.Text.Trim();

            if (string.IsNullOrEmpty(slovo))
            {
                MessageBox.Show("Введите слово!", "Ошибка");
                return;
            }

            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("Введите номера страниц!", "Ошибка");
                return;
            }

          
            string[] strM = str.Split(',');
            List<int> strList = new List<int>();

            foreach (string s in strM)
            {
                if (int.TryParse(s.Trim(), out int num))
                {
                    if (num >= 1 && num <= 1000) 
                    {
                        if (!strList.Contains(num))
                            strList.Add(num);
                    }
                }
            }

            if (strList.Count == 0 || strList.Count > 10)
            {
                MessageBox.Show("Количество страниц должно быть от 1 до 10!", "Ошибка");
                return;
            }

            strList.Sort();
            ukazatel.Add(new Ukazatel(slovo, strList));
            UpdateList();
            ClearInputs();

            MessageBox.Show("Слово добавлено!", "Успех");
        
    }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            ofd.Title = "Выберите файл с указателем";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(ofd.FileName);
                    ukazatel.Clear();

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string slovo = parts[0].Trim();
                            string[] strS = parts[1].Trim().Split(',');
                            List<int> strList = new List<int>();

                            foreach (string s in strS)
                            {
                                if (int.TryParse(s.Trim(), out int num))
                                {
                                    if (!strList.Contains(num))
                                        strList.Add(num);
                                }
                            }

                            if (strList.Count > 0 && strList.Count <= 10)
                            {
                                strList.Sort();
                                ukazatel.Add(new Ukazatel(slovo, strList));
                            }
                        }
                    }

                    UpdateList();
                    MessageBox.Show($"Загружено {ukazatel.Count} слов!", "Успех");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке файла: " + ex.Message, "Ошибка");
                }
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовые файлы|*.txt";
            sfd.Title = "Сохранить указатель в файл";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<string> lines = new List<string>();
                    foreach (Ukazatel u in ukazatel)
                    {
                        lines.Add($"{u.Slovo}: {string.Join(", ", u.Str)}");
                    }
                    File.WriteAllLines(sfd.FileName, lines);
                    MessageBox.Show("Указатель сохранен!", "Успех");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = Search.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                MessageBox.Show("Введите слово для поиска!", "Ошибка");
                return;
            }


            var result = ukazatel.FirstOrDefault(u => u.Slovo.Equals(search, StringComparison.OrdinalIgnoreCase));

            if (result != null)
            {
                lblResult.Text = $"Слово «{result.Slovo}» встречается на страницах: {string.Join(", ", result.Str)}";
            }
            else
            {
                lblResult.Text = $"Слово «{search}» не найдено!";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxUkazatel.SelectedItem == null)
            {
                MessageBox.Show("Выберите слово для удаления!", "Ошибка");
                return;
            }

            string selected = listBoxUkazatel.SelectedItem.ToString();
            string slovoDelete = selected.Split(':')[0].Trim();

            DialogResult dr = MessageBox.Show($"Удалить слово «{slovoDelete}»?",
                "Подтверждение", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                ukazatel.RemoveAll(u => u.Slovo == slovoDelete);
                UpdateList();
                lblResult.Text = "Слово удалено!";
                MessageBox.Show("Слово удалено!", "Успех");
            }
        }

        
        void UpdateList()
        {
            listBoxUkazatel.Items.Clear();
            foreach (Ukazatel u in ukazatel.OrderBy(u => u.Slovo)) 
            {
                listBoxUkazatel.Items.Add(u.ToString());
            }
        }

      
        void ClearInputs()
        {
            txtSl.Clear();
        }

        private void listBoxUkazatel_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBoxUkazatel.SelectedItem != null)
            {
                string selected = listBoxUkazatel.SelectedItem.ToString();
                lblResult.Text = selected;
            }
        }

        private void btnStat_Click(object sender, EventArgs e)
        {
            if (ukazatel.Count == 0)
            {
                MessageBox.Show("Указатель пуст!", "Статистика");
                return;
            }

            int maxStr = ukazatel.Max(u => u.Str.Count);
            int minStr = ukazatel.Min(u => u.Str.Count);
            double avgStr = ukazatel.Average(u => u.Str.Count);
            int totalWords = ukazatel.Count;

            MessageBox.Show($"Статистика:\n\n" +
                $"Всего слов: {totalWords}\n" +
                $"Макс страниц у слова: {maxStr}\n" +
                $"Мин страниц у слова: {minStr}\n" +
                $"Среднее кол-во страниц: {avgStr:F2}",
                "Статистика (LINQ)");
        }
    }
    }

