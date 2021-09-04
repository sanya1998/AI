using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Оболочка_ЭС
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}
		#region Поля
		const string folder_name = "Файлы";

		const string prefix_rTB = "rTB_";

		const string kb_name = "kb";
		const string facts_name = "facts";
		const string explanation_name = "explanation";

		const string file_kb_name = kb_name + ".txt";
		const string file_facts_name = facts_name + ".txt";
		const string file_explanation_name = explanation_name + ".txt";
		#endregion

		#region События формы и кнопок
		// Действия при загрузке формы
		private void FormMain_Load(object sender, EventArgs e)
		{
			Load_from_txt_in_rTB();
		}

		// Действия при закрытии формы
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Принудительно прервать поток
			if (bW_Start.IsBusy)
			{
				bW_Start.CancelAsync();
			}
		}

		// Клик по кнопке, чтобы загрузить обновления из файлов в rTB
		private void btn_load_from_txt_Click(object sender, EventArgs e)
		{
			Load_from_txt_in_rTB();
		}

		// Клик по кнопке, что запустить работу
		private void btn_Start_Click(object sender, EventArgs e)
		{
			btn_load_from_txt.Enabled = false;
			btn_Start.Enabled = false;
			rTB_kb.Enabled = false;
			rTB_facts.Enabled = false;

			// Запуск функции в отдельном потоке
			bW_Start.RunWorkerAsync();
		}
		#endregion

		#region Работа с файлами
		// Загрузка из текстовых файлов в richTextBox-ы
		private void Load_from_txt_in_rTB()
		{
			// Если папки еще нет, создать ее
			Directory.CreateDirectory(folder_name);

			// Cчитать (или создать) файлы file_kb_name, file_facts_name, file_explanation_name и записать в richTextBox-ы
			Txt_to_rTB(folder_name + @"/" + file_kb_name, ref rTB_kb);
			Txt_to_rTB(folder_name + @"/" + file_facts_name, ref rTB_facts);
			Txt_to_rTB(folder_name + @"/" + file_explanation_name, ref rTB_explanation);
		}
		// Считывание текстовых файлов и запись в rTB
		private void Txt_to_rTB(string path_txt, ref RichTextBox rTB)
		{
			byte[] bytes_array;
			using (FileStream fs = new FileStream(path_txt, FileMode.OpenOrCreate))
			{
				bytes_array = new byte[fs.Length];
				// Чтение
				fs.Read(bytes_array, 0, bytes_array.Length);
			}

			// Запись в rTB
			rTB.Text = Encoding.Default.GetString(bytes_array);

			// Поставить курсор в конец текста
			rTB.Select(rTB.Text.Length, 0);
		}
		// Изменение одного из rTB и запись в файл
		private void rTB_TextChanged(object sender, EventArgs e)
		{
			// Редактирование файла
			RichTextBox rTB = (RichTextBox)sender;
			string file_name;
			switch(rTB.Name)
			{
				case prefix_rTB + kb_name:
					file_name = file_kb_name;
					break;
				case prefix_rTB + facts_name:
					file_name = file_facts_name;
					break;
				case prefix_rTB + explanation_name:
					file_name = file_explanation_name;
					break;
				default:
					return;
			}
			// Запись в файл
			File.WriteAllText(folder_name + @"/" + file_name, rTB.Text, Encoding.Default);
		}
		#endregion

		#region Запуск
		// То, что будет выполняться в отдельном потоке
		private void bW_Start_DoWork(object sender, DoWorkEventArgs e)
		{
			// Заполнение рабочей памяти
			WorkMemory wm = new WorkMemory();
			List<Fact> facts_new = ConvertStringToFacts(rTB_facts.Text);
			wm.Add(facts_new);

			// Заполнение базы знаний
			KnowledgeBase kb = new KnowledgeBase();
			List<Rule> rules_new = ConvertStringToRules(rTB_kb.Text);
			kb.Add(rules_new);

			// Подсистема поиска решений
			DecisionFinder df = new DecisionFinder(wm, kb);

			// Подсистема объяснений
			Explanationer expl = df.Start();
			rTB_explanation.Text = expl.ToString();
		}

		// Формирование правил из текста
		private List<Rule> ConvertStringToRules(string text)
		{
			List<Rule> rules = new List<Rule>();
			// Деление текста по строкам c удалением пустых строк
			string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines != null)
			{
				foreach(var line in lines)
				{
					// Деление строки на условия и действия
					var conditions_str = line.Substring(0, line.IndexOf("=>"));
					var actions_str = line.Substring(line.IndexOf("=>")+2);

					// Представление условий и действий в виде фактов
					List<Fact> conditions = ConvertStringToFacts(conditions_str);
					List<Fact> actions = ConvertStringToFacts(actions_str);

					// Создание правила и добавление его в общий список. Так как нет удаления правил, можно id = count+1
					Rule rule = new Rule(rules.Count+1, conditions, actions);
					rules.Add(rule);
				}
			}
			return rules;
		}

		// Формирование фактов из текста
		private List<Fact> ConvertStringToFacts(string text)
		{
			List <Fact> facts = new List<Fact>();
			
			// w - любой алф-цифр символ
			// s - пробел
			// * 0 и более раз
			Regex regex = new Regex(@"\((\w*\s*)*\)");
			var matches = regex.Matches(text);
			foreach (Match match in matches)
			{
				Fact f = new Fact(match.Value);
				facts.Add(f);
			}
			
			return facts;
		}

		// Выполнится по завершении потока
		private void bW_Start_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btn_load_from_txt.Enabled = true;
			btn_Start.Enabled = true;
			rTB_kb.Enabled = true;
			rTB_facts.Enabled = true;
		}
		#endregion
	}
}
