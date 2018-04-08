using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Cookbook
{
    public partial class frmMain : Form
    {
        SqlConnection connection;
        string connectionString;

        public frmMain()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Cookbook.Properties.Settings.CookbookConnectionString"].ConnectionString;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            PopulateRecipes();
        }

        private void PopulateRecipes()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Recipe ", connection))
            {
                DataTable recipeTable = new DataTable();
                adapter.Fill(recipeTable);
                lstRecipes.DisplayMember = "Name";
                lstRecipes.ValueMember = "Id";
                lstRecipes.DataSource = recipeTable;
            }
        }

        private void PopulateIngredients()
        {
            string query = @"SELECT Ingredient.Name FROM Ingredient
                            INNER JOIN RecipeIngredient ON RecipeIngredient.IngredientId = Ingredient.Id
                            WHERE RecipeId = @RecipeId";
            using (connection = new SqlConnection(connectionString))
            using(SqlCommand command = new SqlCommand(query,connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@RecipeId", lstRecipes.SelectedValue);
                DataTable ingredientTable = new DataTable();
                adapter.Fill(ingredientTable);
                lstIngredients.DisplayMember = "Name";
                lstIngredients.ValueMember = "Id";
                lstIngredients.DataSource = ingredientTable;
            }
        }

        private void lstRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateIngredients();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
