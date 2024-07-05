namespace InjectorGUI
{
    partial class Injector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Injector));
            Inject = new Button();
            InjectBtn = new Button();
            DllSelector = new CheckedListBox();
            AddDLLBtn = new Button();
            RemoveDllBtn = new Button();
            SelectProcess = new ComboBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // Inject
            // 
            Inject.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(Inject, "Inject");
            Inject.Name = "Inject";
            Inject.UseVisualStyleBackColor = true;
            // 
            // InjectBtn
            // 
            InjectBtn.BackColor = SystemColors.Control;
            resources.ApplyResources(InjectBtn, "InjectBtn");
            InjectBtn.Name = "InjectBtn";
            InjectBtn.UseVisualStyleBackColor = false;
            // 
            // DllSelector
            // 
            DllSelector.BackColor = SystemColors.Control;
            DllSelector.BorderStyle = BorderStyle.FixedSingle;
            DllSelector.CheckOnClick = true;
            resources.ApplyResources(DllSelector, "DllSelector");
            DllSelector.FormattingEnabled = true;
            DllSelector.Name = "DllSelector";
            // 
            // AddDLLBtn
            // 
            AddDLLBtn.BackColor = SystemColors.Control;
            resources.ApplyResources(AddDLLBtn, "AddDLLBtn");
            AddDLLBtn.Name = "AddDLLBtn";
            AddDLLBtn.UseVisualStyleBackColor = false;
            AddDLLBtn.Click += AddDLLBtn_Click;
            // 
            // RemoveDllBtn
            // 
            RemoveDllBtn.BackColor = SystemColors.Control;
            resources.ApplyResources(RemoveDllBtn, "RemoveDllBtn");
            RemoveDllBtn.Name = "RemoveDllBtn";
            RemoveDllBtn.UseVisualStyleBackColor = false;
            RemoveDllBtn.Click += RemoveDllBtn_Click;
            // 
            // SelectProcess
            // 
            SelectProcess.DropDownStyle = ComboBoxStyle.DropDownList;
            resources.ApplyResources(SelectProcess, "SelectProcess");
            SelectProcess.FormattingEnabled = true;
            SelectProcess.Name = "SelectProcess";
            SelectProcess.SelectedIndexChanged += SelectProcess_SelectedIndexChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.BackColor = Color.White;
            label1.Name = "label1";
            // 
            // Injector
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            Controls.Add(label1);
            Controls.Add(SelectProcess);
            Controls.Add(RemoveDllBtn);
            Controls.Add(AddDLLBtn);
            Controls.Add(DllSelector);
            Controls.Add(InjectBtn);
            Controls.Add(Inject);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Injector";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Show;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button Inject;
        private Button InjectBtn;
        private CheckedListBox DllSelector;
        private Button AddDLLBtn;
        private Button RemoveDllBtn;
        private ComboBox SelectProcess;
        private Label label1;
    }
}
