using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GatoRobotoRandomizer {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			IO.Path = ".";
		}

		private void Form1_Load(object sender, EventArgs e) {
			IO.Log("Macro Dump:");
			foreach (RandoMacro macro in RandoData.Macros.Values) {
				IO.Log($"--{macro.Name} Post: '{macro.LogicPost}'");
				IO.Log($"--{macro.Name} Base: '{macro.LogicBase}'\n");
			}

			promptForBackup();
			IO.LogClear();
			lbl_Version.Text = $"v{RandoSettings.Version}";
		}

		private void promptForBackup() {
			//Look for the right map files
			if (!IO.CheckFiles()) {
				MessageBox.Show("Map files not found. Are you sure this program is in the Gato Roboto root directory?", "UH-OH!");
				return;
			}

			//If we haven't chosen whether or not to backup, do so then backup if yes.
			if (!Properties.Settings.Default.bln_backupPrompted && !Directory.Exists(IO.BackupPath)) {
				DialogResult dr = MessageBox.Show("Make a backup of the Gato Roboto files? (Don't do this if you've already played the rando)", "Backups", MessageBoxButtons.YesNo);
				if (dr == DialogResult.Yes) {
					IO.MakeBackups();
				}

				Properties.Settings.Default.bln_backupPrompted = true;
				Properties.Settings.Default.Save();
			}
		}

		private void Text_seed_KeyPress(object sender, KeyPressEventArgs e) {
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
				e.Handled = true;
			}
		}

		private void Btn_reroll_Click(object sender, EventArgs e) {
			text_seed.Text = RandoSettings.NewSeed().ToString();
		}

		private bool smallMechWarning = false;

		private void Opt_smallMech_CheckedChanged(object sender, EventArgs e) {
			if (!smallMechWarning) {
				MessageBox.Show("If you use this glitch and you haven't found dash, MAKE SURE TO KILL THE HOT BOYS before clearing the lava tubes.\n\nTheir rooms crash the game if lava's been cleared, and you won't be able to access Vent or the item under HB2 otherwise.", "Glitch Warning!");
				smallMechWarning = true;
			}
		}

		private void Btn_submit_Click(object sender, EventArgs e) {
			IO.Log("Rando Click");
			DialogResult dr = MessageBox.Show("Continue with Randomization? This will overwrite the map files.", "Please Insert Quarter", MessageBoxButtons.YesNo);
			if (dr == DialogResult.No) {
				IO.Log("msgBox: No");
				return;
			}
			IO.Log("msgBox: Yes");

			IO.Log("Parse Seed");
			if (!int.TryParse(text_seed.Text, out int seed)) {
				seed = RandoSettings.NewSeed();
				text_seed.Text = seed.ToString();
			}

			IO.Log($"Seed: {RandoSettings.Seed}");

			IO.Log("Load Settings");
			//Load Settings
			RandoSettings.Seed = seed;
			RandoSettings.Options_bool["OPTB_small_mech"] = optb_small_mech.Checked;
			RandoSettings.Options_bool["OPTB_phase_save"] = optb_phase_save.Checked;
			RandoSettings.Options_bool["OPTB_advanced"] = optb_advanced.Checked;
			RandoSettings.Options_bool["OPTB_not_beginner"] = !optb_beginner.Checked;
			RandoSettings.Options_bool["OPTB_not_100"] = !optb_100_possible.Checked;

			IO.Log("Rando Inst");
			//Randomize me Captain
			Randomizer rando = new Randomizer();

			IO.Log("Rando dot Rando");
			List<RandoLocation> locations = rando.Randomize();

			IO.Log("IO Write");
			IO.WriteData(locations);

			IO.Log("Rando Done!");
			MessageBox.Show("Randomization Complete!", "Enjoy ;)");
		}

		private void Btn_restore_Click(object sender, EventArgs e) {
			IO.Log("Restore Click");
			DialogResult dr = MessageBox.Show("This doesn't actually restore your backups, but it will really place the items in their vanilla locations. Continue?", "Please Remove Quarter", MessageBoxButtons.YesNo);
			if (dr == DialogResult.No) {
				IO.Log("msgBox: No");
				return;
			}
			IO.Log("msgBox: Yes");

			IO.Log("Init Data");
			List<RandoLocation> locs = RandoData.GetAllLocations();
			List<RandoItem> itms = RandoData.GetAllItems();

			IO.Log("Loop default data");
			foreach (RandoItem item in itms) {
				locs.First(v => v.ID == item.OrigLoc).PlaceItem(item);
			}

			IO.Log("IO Write");
			IO.WriteData(locs.ToList(), true);

			IO.Log("Restore Done!");
			MessageBox.Show("Unrandomization Complete!", "I hope you hate this! ;)");
		}

		private void Button1_Click(object sender, EventArgs e) {
			string note = "And('&') takes precendence over Or('|').\nPEMDAS is to Arithmetic as AndOr is to Logic\n\n";
			MessageBox.Show(note + Logic.Dumps(), "Cheater?");
		}
	}
}
