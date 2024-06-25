using System;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        int i = 0;

        public struct Employee
        {
            public string NCNSS;     // National Insurance Number
            public string Nom;       // Name
            public string Prenom;    // Surname
            public string Fonction;  // Job Title
            public string TypeEmploye; // Employee Type
            public DateTime dateNais; // Date of Birth
            public DateTime DateEmb;  // Employment Date
            public double Salaire;
            public bool Sex;         // Gender (true for Male, false for Female)
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Ecole Racine - TSDI - 2023 - 2024 -";
            // Ajout des éléments aux listes déroulantes
            comboBox1.Items.AddRange(new string[] { "Ingénieur", "Technicien", "Agent" });
            comboBox2.Items.AddRange(new string[] { "Développeur", "Designer", "Directeur", "Comptable", "Chef de projet" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Vérification si tous les champs essentiels sont remplis
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                comboBox1.SelectedItem == null ||
                comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Champ Vide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Création d'un objet Employee à partir des données saisies
            Employee employee = new Employee
            {
                NCNSS = textBox1.Text,
                Nom = textBox2.Text,
                Prenom = textBox3.Text,
                dateNais = dateTimePicker1.Value,
                Fonction = comboBox1.SelectedItem.ToString(),
                TypeEmploye = comboBox2.SelectedItem.ToString(),
                Salaire = double.Parse(textBox6.Text),
                Sex = checkBox1.Checked  // Assignation du sexe en fonction de checkBox1
            };

            DialogResult result = MessageBox.Show("Voulez-vous ajouter cet employé ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                i++;
                dataGridView1.Rows.Add(employee.NCNSS, employee.Nom, employee.Prenom, employee.Sex ? "Male" : "Female", employee.dateNais.ToShortDateString(), employee.DateEmb.ToShortDateString(), employee.Fonction, employee.TypeEmploye, employee.Salaire);
                MessageBox.Show("Employé ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                string newNcnss = Prompt.ShowDialog("Modifier le Ncnss:", "Modifier le Ncnss");
                string newNom = Prompt.ShowDialog("Modifier le Nom:", "Modifier le Nom");
                string newPrenom = Prompt.ShowDialog("Modifier le prénom:", "Modifier le Prénom");
                double newSalaire = Convert.ToDouble(Prompt.ShowDialog("Modifier le salaire:", "Modifier le salaire"));

                selectedRow.Cells["Ncnss"].Value = newNcnss;
                selectedRow.Cells["Nom"].Value = newNom;
                selectedRow.Cells["Prenom"].Value = newPrenom;
                selectedRow.Cells["Salaire"].Value = newSalaire;

                MessageBox.Show("Modification réussie.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne à modifier.", "Aucune Ligne Sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["Salaire"].Value != null)
                {
                    double value;
                    if (double.TryParse(row.Cells["Salaire"].Value.ToString(), out value))
                    {
                        total += value;
                    }
                }
            }
            textBox5.Text = total.ToString() + " DH";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Effacer les champs de saisie et réinitialiser les cases à cocher
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // Validation de l'entrée pour NCNSS pour s'assurer que seuls des chiffres sont saisis
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Veuillez entrer uniquement des chiffres.", "Caractères Invalides", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Text = string.Empty;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Gestion des états des cases à cocher
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // Gestion des états des cases à cocher
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser uniquement les chiffres et un point décimal dans le champ de salaire
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Empêcher plus d'un point décimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}