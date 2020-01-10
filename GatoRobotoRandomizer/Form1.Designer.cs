using System;

namespace GatoRobotoRandomizer {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.btn_reroll = new System.Windows.Forms.Button();
			this.btn_submit = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optb_beginner = new System.Windows.Forms.CheckBox();
			this.optb_advanced = new System.Windows.Forms.CheckBox();
			this.optb_phase_save = new System.Windows.Forms.CheckBox();
			this.optb_small_mech = new System.Windows.Forms.CheckBox();
			this.lbl_Version = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.text_seed = new System.Windows.Forms.TextBox();
			this.btn_restore = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.optb_100_possible = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Seed:";
			// 
			// btn_reroll
			// 
			this.btn_reroll.Image = global::GatoRobotoRandomizer.Properties.Resources.refresh_solid;
			this.btn_reroll.Location = new System.Drawing.Point(158, 8);
			this.btn_reroll.Name = "btn_reroll";
			this.btn_reroll.Size = new System.Drawing.Size(28, 27);
			this.btn_reroll.TabIndex = 2;
			this.btn_reroll.UseVisualStyleBackColor = true;
			this.btn_reroll.Click += new System.EventHandler(this.Btn_reroll_Click);
			// 
			// btn_submit
			// 
			this.btn_submit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_submit.Location = new System.Drawing.Point(192, 10);
			this.btn_submit.Name = "btn_submit";
			this.btn_submit.Size = new System.Drawing.Size(75, 23);
			this.btn_submit.TabIndex = 3;
			this.btn_submit.Text = "Rando!";
			this.btn_submit.UseVisualStyleBackColor = true;
			this.btn_submit.Click += new System.EventHandler(this.Btn_submit_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.optb_100_possible);
			this.groupBox1.Controls.Add(this.optb_beginner);
			this.groupBox1.Controls.Add(this.optb_advanced);
			this.groupBox1.Controls.Add(this.optb_phase_save);
			this.groupBox1.Controls.Add(this.optb_small_mech);
			this.groupBox1.Location = new System.Drawing.Point(14, 41);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(252, 137);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// optb_beginner
			// 
			this.optb_beginner.AutoSize = true;
			this.optb_beginner.Location = new System.Drawing.Point(6, 88);
			this.optb_beginner.Name = "optb_beginner";
			this.optb_beginner.Size = new System.Drawing.Size(97, 17);
			this.optb_beginner.TabIndex = 3;
			this.optb_beginner.Text = "Beginner Logic";
			this.toolTip1.SetToolTip(this.optb_beginner, "Restricts a few locations, mostly stuff you wouldn\'t think about trying in your f" +
        "irst few plays.");
			this.optb_beginner.UseVisualStyleBackColor = true;
			// 
			// optb_advanced
			// 
			this.optb_advanced.AutoSize = true;
			this.optb_advanced.Location = new System.Drawing.Point(6, 65);
			this.optb_advanced.Name = "optb_advanced";
			this.optb_advanced.Size = new System.Drawing.Size(128, 17);
			this.optb_advanced.TabIndex = 2;
			this.optb_advanced.Text = "Advanced Movement";
			this.toolTip1.SetToolTip(this.optb_advanced, "Holds a few locations that are either accessible early via big-brain minor glitch" +
        "es, like dash-jump storage.");
			this.optb_advanced.UseVisualStyleBackColor = true;
			// 
			// optb_phase_save
			// 
			this.optb_phase_save.AutoSize = true;
			this.optb_phase_save.Location = new System.Drawing.Point(6, 42);
			this.optb_phase_save.Name = "optb_phase_save";
			this.optb_phase_save.Size = new System.Drawing.Size(148, 17);
			this.optb_phase_save.TabIndex = 1;
			this.optb_phase_save.Text = "(Glitch) Phase Save State";
			this.toolTip1.SetToolTip(this.optb_phase_save, "After landing on a save-pad and entering suit at the same time, dash on the same " +
        "frame the game saves (white flash).");
			this.optb_phase_save.UseVisualStyleBackColor = true;
			// 
			// optb_small_mech
			// 
			this.optb_small_mech.AutoSize = true;
			this.optb_small_mech.Location = new System.Drawing.Point(6, 19);
			this.optb_small_mech.Name = "optb_small_mech";
			this.optb_small_mech.Size = new System.Drawing.Size(117, 17);
			this.optb_small_mech.TabIndex = 0;
			this.optb_small_mech.Text = "(Glitch) Small Mech";
			this.toolTip1.SetToolTip(this.optb_small_mech, "After landing on a save-pad and entering suit at the same time, eject from suit o" +
        "n the same frame the game saves (white flash).");
			this.optb_small_mech.UseVisualStyleBackColor = true;
			this.optb_small_mech.CheckedChanged += new System.EventHandler(this.Opt_smallMech_CheckedChanged);
			// 
			// lbl_Version
			// 
			this.lbl_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbl_Version.AutoSize = true;
			this.lbl_Version.Location = new System.Drawing.Point(11, 197);
			this.lbl_Version.Name = "lbl_Version";
			this.lbl_Version.Size = new System.Drawing.Size(34, 13);
			this.lbl_Version.TabIndex = 1;
			this.lbl_Version.Text = "v0.0a";
			// 
			// text_seed
			// 
			this.text_seed.Location = new System.Drawing.Point(50, 12);
			this.text_seed.MaxLength = 9;
			this.text_seed.Name = "text_seed";
			this.text_seed.Size = new System.Drawing.Size(100, 20);
			this.text_seed.TabIndex = 5;
			this.text_seed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Text_seed_KeyPress);
			// 
			// btn_restore
			// 
			this.btn_restore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_restore.Location = new System.Drawing.Point(158, 184);
			this.btn_restore.Name = "btn_restore";
			this.btn_restore.Size = new System.Drawing.Size(108, 23);
			this.btn_restore.TabIndex = 6;
			this.btn_restore.Text = "Restore to Vanilla";
			this.btn_restore.UseVisualStyleBackColor = true;
			this.btn_restore.Click += new System.EventHandler(this.Btn_restore_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(77, 184);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "View Logic";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// optb_100Possible
			// 
			this.optb_100_possible.AutoSize = true;
			this.optb_100_possible.Location = new System.Drawing.Point(6, 111);
			this.optb_100_possible.Name = "optb_100Possible";
			this.optb_100_possible.Size = new System.Drawing.Size(130, 17);
			this.optb_100_possible.TabIndex = 3;
			this.optb_100_possible.Text = "Ensure 100% Possible";
			this.toolTip1.SetToolTip(this.optb_100_possible, "Logic will ensure access to every location as well as ensuring a game completion." +
        "");
			this.optb_100_possible.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(280, 219);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btn_restore);
			this.Controls.Add(this.lbl_Version);
			this.Controls.Add(this.text_seed);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btn_submit);
			this.Controls.Add(this.btn_reroll);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Gato Roboto Randomizer";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btn_reroll;
		private System.Windows.Forms.Button btn_submit;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox optb_small_mech;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox text_seed;
		private System.Windows.Forms.Label lbl_Version;
		private System.Windows.Forms.CheckBox optb_advanced;
		private System.Windows.Forms.CheckBox optb_phase_save;
		private System.Windows.Forms.Button btn_restore;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox optb_beginner;
		private System.Windows.Forms.CheckBox optb_100_possible;
	}
}

