namespace MLForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            predictButton = new Button();
            reTrainButton = new Button();
            compareButton = new Button();
            textBox1 = new TextBox();
            guessBox = new TextBox();
            confidenceBox = new TextBox();
            originalBox = new TextBox();
            reweightBox = new TextBox();
            diffBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            comboBox = new ComboBox();
            SuspendLayout();
            // 
            // predictButton
            // 
            predictButton.Location = new Point(72, 126);
            predictButton.Name = "predictButton";
            predictButton.Size = new Size(98, 35);
            predictButton.TabIndex = 0;
            predictButton.Text = "Predict";
            predictButton.UseVisualStyleBackColor = true;
            predictButton.Click += button1_Click;
            // 
            // reTrainButton
            // 
            reTrainButton.Location = new Point(72, 260);
            reTrainButton.Name = "reTrainButton";
            reTrainButton.Size = new Size(98, 35);
            reTrainButton.TabIndex = 1;
            reTrainButton.Text = "Re-Train";
            reTrainButton.UseVisualStyleBackColor = true;
            // 
            // compareButton
            // 
            compareButton.Location = new Point(72, 329);
            compareButton.Name = "compareButton";
            compareButton.Size = new Size(98, 35);
            compareButton.TabIndex = 2;
            compareButton.Text = "Compare Models";
            compareButton.UseVisualStyleBackColor = true;
            compareButton.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(72, 64);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(490, 23);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // guessBox
            // 
            guessBox.Location = new Point(455, 123);
            guessBox.Name = "guessBox";
            guessBox.Size = new Size(87, 23);
            guessBox.TabIndex = 4;
            // 
            // confidenceBox
            // 
            confidenceBox.Location = new Point(455, 172);
            confidenceBox.Name = "confidenceBox";
            confidenceBox.Size = new Size(87, 23);
            confidenceBox.TabIndex = 5;
            // 
            // originalBox
            // 
            originalBox.Location = new Point(455, 269);
            originalBox.Name = "originalBox";
            originalBox.Size = new Size(87, 23);
            originalBox.TabIndex = 6;
            // 
            // reweightBox
            // 
            reweightBox.Location = new Point(455, 312);
            reweightBox.Name = "reweightBox";
            reweightBox.Size = new Size(87, 23);
            reweightBox.TabIndex = 7;
            // 
            // diffBox
            // 
            diffBox.Location = new Point(455, 359);
            diffBox.Name = "diffBox";
            diffBox.Size = new Size(87, 23);
            diffBox.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 67);
            label1.Name = "label1";
            label1.Size = new Size(57, 15);
            label1.TabIndex = 9;
            label1.Text = "Feedback";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(381, 126);
            label2.Name = "label2";
            label2.Size = new Size(41, 15);
            label2.TabIndex = 10;
            label2.Text = "Guess:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(378, 175);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 11;
            label3.Text = "Confidence:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(351, 272);
            label4.Name = "label4";
            label4.Size = new Size(98, 15);
            label4.TabIndex = 12;
            label4.Text = "Original Weights:";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(339, 315);
            label5.Name = "label5";
            label5.Size = new Size(110, 15);
            label5.TabIndex = 13;
            label5.Text = "Re Trained Weights:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(370, 359);
            label6.Name = "label6";
            label6.Size = new Size(75, 15);
            label6.TabIndex = 14;
            label6.Text = "Weight Diffs:";
            label6.Click += label6_Click;
            // 
            // comboBox
            // 
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(72, 196);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(98, 23);
            comboBox.TabIndex = 15;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(618, 449);
            Controls.Add(comboBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(diffBox);
            Controls.Add(reweightBox);
            Controls.Add(originalBox);
            Controls.Add(confidenceBox);
            Controls.Add(guessBox);
            Controls.Add(textBox1);
            Controls.Add(compareButton);
            Controls.Add(reTrainButton);
            Controls.Add(predictButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button predictButton;
        private Button reTrainButton;
        private Button compareButton;
        private TextBox textBox1;
        private TextBox guessBox;
        private TextBox confidenceBox;
        private TextBox originalBox;
        private TextBox reweightBox;
        private TextBox diffBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private ComboBox comboBox;
    }
}
