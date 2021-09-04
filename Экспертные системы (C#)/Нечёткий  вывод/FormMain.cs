using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Нечёткий__вывод
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
		const string vars_name = "vars";
		const string explanation_name = "explanation";

		const string file_kb_name = kb_name + ".txt";
		const string file_facts_name = vars_name + ".txt";
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
			rTB_vars.Enabled = false;

			// Запуск функции в отдельном потоке
			bW_Start.RunWorkerAsync();
		}
		#endregion
		#region Работа с файлами
		// Загрузка из текстовых файлов в richTextBox-ы
		private void Load_from_txt_in_rTB()
		{
			// Если папки еще нет, создать ее. (В этой функции нужно ее создать, потому что без нее вылетит исключение)
			Directory.CreateDirectory(folder_name);

			// Cчитать (или создать) файлы file_kb_name, file_facts_name, file_explanation_name и записать в richTextBox-ы
			Txt_to_rTB(folder_name + @"/" + file_kb_name, ref rTB_kb);
			Txt_to_rTB(folder_name + @"/" + file_facts_name, ref rTB_vars);
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
			switch (rTB.Name)
			{
				case prefix_rTB + kb_name:
					file_name = file_kb_name;
					break;
				case prefix_rTB + vars_name:
					file_name = file_facts_name;
					break;
				case prefix_rTB + explanation_name:
					file_name = file_explanation_name;
					break;
				default:
					return;
			}
			// Если папки нет, создать ее.
			Directory.CreateDirectory(folder_name);
			// Запись в файл
			File.WriteAllText(folder_name + @"/" + file_name, rTB.Text, Encoding.Default);
		}
		#endregion

		#region Запуск
		// Глобально для того, чтобы нарисовать переменные
		List<Variable> Variables;
		// То, что будет выполняться в отдельном потоке
		private void bW_Start_DoWork(object sender, DoWorkEventArgs e)
		{
			// Заполнение памяти переменных
			VariablesMemory vm = new VariablesMemory();
			Variables = ConvertToVariables(rTB_vars.Text);
			vm.Add(Variables);

			
			// Заполнение базы правил
			RulesBase rb = new RulesBase();
			List<Rule> rules = ConvertToRules(ref vm, rTB_kb.Text);
			rb.Add(rules);


			// Подсистема объяснений
			Explanationer expl = new Explanationer();


			// Процедура обработки нечетных множеств
			ProcessingProcedure pp = new ProcessingProcedure(ref vm, ref rb, ref expl);
			pp.Start();

			
			// Вывод объяснений в richTextBox
			rTB_explanation.Text = expl.ToString();
		}

		// Выполнится по завершении потока
		private void bW_Start_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(Variables != null)
				// Отрисовка нечетких значений на графике
				Picture(Variables);

			btn_load_from_txt.Enabled = true;
			btn_Start.Enabled = true;
			rTB_kb.Enabled = true;
			rTB_vars.Enabled = true;
		}
		
		// Отрисовка переменных в Chart
		private void Picture(List<Variable> variables)
		{
			// Удалить все вкладки
			tC_vars.TabPages.Clear();

			// Цикл по переменным
			foreach (var variable in variables)
			{
				// Создание новой вкладки
				TabPage tP = new TabPage
				{
					// Название вкладки
					Text = variable.Name + " (" + Math.Round(variable.Value, 3) + ") " + (variable.IsOutput? "(Output)":"(Input)")
				};

				// Создание чарта
				Chart chart = new Chart
				{
					Dock = DockStyle.Fill
				};

				// Создание места для графика
				ChartArea chartArea = new ChartArea();
				// Максимальные и минимальные значения осей
				chartArea.AxisX.Minimum = variable.FindMinimumX();
				chartArea.AxisX.Maximum = variable.FindMaximumX();
				chartArea.AxisY.Minimum = 0;
				//chartArea.AxisY.Maximum = variable.FindMaximumY(); // Если все значения равны 0, то интервал не определен
				chartArea.AxisY.Maximum = Math.Max(variable.FindMaximumY(), 0.000000000001); 

				// Массив цветов для использования
				Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Yellow, Color.Coral, Color.Brown, Color.Lime };
				// Номер последнего использованного цвета
				int num_last_color = 0;
				// Добавление каждого нечеткого значения
				foreach (var fuzzy in variable.FuzzyValues)
				{
					// Создание линии на графике
					Series series = new Series(fuzzy.Name)
					{
						// Тип линии
						ChartType = SeriesChartType.Line,
						// Цвет
						Color = colors[(num_last_color++) % colors.Length]
					};
					// Точки
					series.Points.AddXY(fuzzy.Begin, 0);
					series.Points.AddXY(fuzzy.Middle, fuzzy.Koef);
					series.Points.AddXY(fuzzy.End, 0);

					// Добавление линии нечеткого значения на график
					chart.Series.Add(series);
					chart.Legends.Add(series.LegendText);
				}

				// Добавление Места для графика в Чарт
				chart.ChartAreas.Add(chartArea);
				// Добавление Чарта во вкладку
				tP.Controls.Add(chart);
				// Добавление созданной вкладки
				tC_vars.TabPages.Add(tP);
			}
		}
		#endregion

		#region Конвертация из текста в правила, в переменные...
		// Формирование переменных из текста
		private List<Variable> ConvertToVariables(string text_var)
		{
			List<Variable> Variables = new List<Variable>();
			// Точки заменить на запятые, чтобы double смог спарсить
			text_var = text_var.Replace('.', ',');
			// Из текста в массив строк
			var lines = text_var.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines != null)
			{
				foreach (var line in lines)
				{
					// Из строки в слова
					var words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					// 1слово - "имяПеременной", 2слово - "число", (4х-1)слово - "значение", (4x)слово - "начало", (4x+1)слово - "середина", (4х+2)слово - "конец"
					if ((words.Length - 2) % 4 == 0)
					{
						string Name = words[0];
						// Нечеткие значения для переменной
						List<FuzzyValue> FuzzyValues = new List<FuzzyValue>();
						// Если "число" преобразовалось в double
						if (Double.TryParse(words[1], out double Value))
						{
							// Цикл по "значениям"
							for (var i = 2; i < words.Length; i += 4)
							{
								// Если удалось преобразовать "начало", "середина" и "конец" в double 
								if (Double.TryParse(words[i + 1], out double resultBegin)
									&& Double.TryParse(words[i + 2], out double resultMiddle)
									&& Double.TryParse(words[i + 3], out double resultEnd))
								{
									FuzzyValues.Add(new FuzzyValue(words[i], resultBegin, resultMiddle, resultEnd));
								}
							}
							Variables.Add(new Variable(Name, Value, FuzzyValues));
						}
					}
				}
			}
			return Variables;
		}

		// Формирование правил из текста
		private List<Rule> ConvertToRules(ref VariablesMemory vm, string text_rules)
		{
			List<Rule> rules = new List<Rule>();
			// Деление текста по строкам c удалением пустых строк
			string[] lines = text_rules.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines != null)
			{
				foreach (var line in lines)
				{
					// Деление строки на условия и действия
					var condition_str = line.Substring(0, line.IndexOf("=>"));
					var action_str = line.Substring(line.IndexOf("=>") + 2);

					// Представление условия и действия в виде KeyValuePair
					KeyValuePair<Variable, FuzzyValue> condition = ConvertToKeyValuePair(ref vm.Variables, condition_str);
					KeyValuePair<Variable, FuzzyValue> action = ConvertToKeyValuePair(ref vm.Variables, action_str);

					// Создание правила и добавление его в общий список. Так как нет удаления правил, можно id = count+1
					Rule rule = new Rule(rules.Count + 1, condition, action);
					rules.Add(rule);
				}
			}
			return rules;
		}

		// Формирование KeyValuePair<Variable, FuzzyValue> из текста
		private KeyValuePair<Variable, FuzzyValue> ConvertToKeyValuePair(ref List<Variable> Variables, string text)
		{
			// var_val[0] - имя переменной; var_val[1] - значение (имя нечеткого значения)
			var var_val = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			
			// Поиск переменной по имени
			Variable key = Variables.Find(x => x.Name == var_val[0]);

			// Поиск нечеткого значения по имени среди значений одной переменной
			FuzzyValue value = key.FuzzyValues.Find(x => x.Name == var_val[1]);

			return new KeyValuePair<Variable, FuzzyValue>(key, value);
		}
		#endregion
	}
}
