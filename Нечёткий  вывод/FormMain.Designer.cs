namespace Нечёткий__вывод
{
	partial class FormMain
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.tLP_main = new System.Windows.Forms.TableLayoutPanel();
			this.rTB_explanation = new System.Windows.Forms.RichTextBox();
			this.rTB_vars = new System.Windows.Forms.RichTextBox();
			this.label_explanation = new System.Windows.Forms.Label();
			this.label_vars = new System.Windows.Forms.Label();
			this.label_kb = new System.Windows.Forms.Label();
			this.rTB_kb = new System.Windows.Forms.RichTextBox();
			this.btn_load_from_txt = new System.Windows.Forms.Button();
			this.btn_Start = new System.Windows.Forms.Button();
			this.bW_Start = new System.ComponentModel.BackgroundWorker();
			this.tC_vars = new System.Windows.Forms.TabControl();
			this.tLP_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// tLP_main
			// 
			this.tLP_main.ColumnCount = 3;
			this.tLP_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
			this.tLP_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tLP_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tLP_main.Controls.Add(this.rTB_explanation, 2, 1);
			this.tLP_main.Controls.Add(this.rTB_vars, 1, 1);
			this.tLP_main.Controls.Add(this.label_explanation, 2, 0);
			this.tLP_main.Controls.Add(this.label_vars, 1, 0);
			this.tLP_main.Controls.Add(this.label_kb, 0, 0);
			this.tLP_main.Controls.Add(this.rTB_kb, 0, 1);
			this.tLP_main.Controls.Add(this.btn_load_from_txt, 0, 2);
			this.tLP_main.Controls.Add(this.btn_Start, 0, 3);
			this.tLP_main.Controls.Add(this.tC_vars, 0, 4);
			this.tLP_main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tLP_main.Location = new System.Drawing.Point(0, 0);
			this.tLP_main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tLP_main.Name = "tLP_main";
			this.tLP_main.RowCount = 5;
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
			this.tLP_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tLP_main.Size = new System.Drawing.Size(888, 481);
			this.tLP_main.TabIndex = 1;
			// 
			// rTB_explanation
			// 
			this.rTB_explanation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rTB_explanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.rTB_explanation.Location = new System.Drawing.Point(594, 41);
			this.rTB_explanation.Name = "rTB_explanation";
			this.rTB_explanation.ReadOnly = true;
			this.rTB_explanation.Size = new System.Drawing.Size(291, 210);
			this.rTB_explanation.TabIndex = 5;
			this.rTB_explanation.Text = "";
			this.rTB_explanation.TextChanged += new System.EventHandler(this.rTB_TextChanged);
			// 
			// rTB_vars
			// 
			this.rTB_vars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rTB_vars.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.rTB_vars.Location = new System.Drawing.Point(298, 41);
			this.rTB_vars.Name = "rTB_vars";
			this.rTB_vars.Size = new System.Drawing.Size(290, 210);
			this.rTB_vars.TabIndex = 4;
			this.rTB_vars.Text = "";
			this.rTB_vars.TextChanged += new System.EventHandler(this.rTB_TextChanged);
			// 
			// label_explanation
			// 
			this.label_explanation.AutoSize = true;
			this.label_explanation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_explanation.Location = new System.Drawing.Point(595, 0);
			this.label_explanation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label_explanation.Name = "label_explanation";
			this.label_explanation.Size = new System.Drawing.Size(289, 38);
			this.label_explanation.TabIndex = 2;
			this.label_explanation.Text = "explanation.txt";
			this.label_explanation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label_vars
			// 
			this.label_vars.AutoSize = true;
			this.label_vars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_vars.Location = new System.Drawing.Point(299, 0);
			this.label_vars.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label_vars.Name = "label_vars";
			this.label_vars.Size = new System.Drawing.Size(288, 38);
			this.label_vars.TabIndex = 1;
			this.label_vars.Text = "vars.txt";
			this.label_vars.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label_kb
			// 
			this.label_kb.AutoSize = true;
			this.label_kb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_kb.Location = new System.Drawing.Point(4, 0);
			this.label_kb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label_kb.Name = "label_kb";
			this.label_kb.Size = new System.Drawing.Size(287, 38);
			this.label_kb.TabIndex = 0;
			this.label_kb.Text = "kb.txt";
			this.label_kb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// rTB_kb
			// 
			this.rTB_kb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rTB_kb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.rTB_kb.Location = new System.Drawing.Point(3, 41);
			this.rTB_kb.Name = "rTB_kb";
			this.rTB_kb.Size = new System.Drawing.Size(289, 210);
			this.rTB_kb.TabIndex = 3;
			this.rTB_kb.Text = "";
			this.rTB_kb.TextChanged += new System.EventHandler(this.rTB_TextChanged);
			// 
			// btn_load_from_txt
			// 
			this.tLP_main.SetColumnSpan(this.btn_load_from_txt, 3);
			this.btn_load_from_txt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btn_load_from_txt.Location = new System.Drawing.Point(3, 257);
			this.btn_load_from_txt.Name = "btn_load_from_txt";
			this.btn_load_from_txt.Size = new System.Drawing.Size(882, 32);
			this.btn_load_from_txt.TabIndex = 6;
			this.btn_load_from_txt.Text = "Вставить в richTextBox-ы txt-файлы";
			this.btn_load_from_txt.UseVisualStyleBackColor = true;
			this.btn_load_from_txt.Click += new System.EventHandler(this.btn_load_from_txt_Click);
			// 
			// btn_Start
			// 
			this.tLP_main.SetColumnSpan(this.btn_Start, 3);
			this.btn_Start.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btn_Start.Location = new System.Drawing.Point(3, 295);
			this.btn_Start.Name = "btn_Start";
			this.btn_Start.Size = new System.Drawing.Size(882, 32);
			this.btn_Start.TabIndex = 7;
			this.btn_Start.Text = "Запуск";
			this.btn_Start.UseVisualStyleBackColor = true;
			this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
			// 
			// bW_Start
			// 
			this.bW_Start.WorkerSupportsCancellation = true;
			this.bW_Start.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bW_Start_DoWork);
			this.bW_Start.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bW_Start_RunWorkerCompleted);
			// 
			// tC_vars
			// 
			this.tLP_main.SetColumnSpan(this.tC_vars, 3);
			this.tC_vars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tC_vars.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tC_vars.Location = new System.Drawing.Point(3, 333);
			this.tC_vars.Name = "tC_vars";
			this.tC_vars.SelectedIndex = 0;
			this.tC_vars.Size = new System.Drawing.Size(882, 145);
			this.tC_vars.TabIndex = 8;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(888, 481);
			this.Controls.Add(this.tLP_main);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FormMain";
			this.Text = "Нечёткий вывод";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.tLP_main.ResumeLayout(false);
			this.tLP_main.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tLP_main;
		private System.Windows.Forms.RichTextBox rTB_explanation;
		private System.Windows.Forms.RichTextBox rTB_vars;
		private System.Windows.Forms.Label label_explanation;
		private System.Windows.Forms.Label label_vars;
		private System.Windows.Forms.Label label_kb;
		private System.Windows.Forms.RichTextBox rTB_kb;
		private System.Windows.Forms.Button btn_load_from_txt;
		private System.ComponentModel.BackgroundWorker bW_Start;
		private System.Windows.Forms.Button btn_Start;
		private System.Windows.Forms.TabControl tC_vars;
	}
}

