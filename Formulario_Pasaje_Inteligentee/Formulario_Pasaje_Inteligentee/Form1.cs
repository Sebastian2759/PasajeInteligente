using Formulario_Pasaje_Inteligentee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Formulario_Pasaje_Inteligentee
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            MonstrarDatos();
        }
        private void MonstrarDatos()
        {
            using (BD_PasajeInteligenteEntities db = new BD_PasajeInteligenteEntities())
            {
                var lst = from d in db.T_Empresa
                          select d;

                dataGridView1.DataSource = lst.ToList();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            FormCompanyEditor form = new FormCompanyEditor();
            form.ShowDialog();
        }

        private int GetId()
        {
            return int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            int? id = GetId();
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;
            if (selectedRows.Count > 1)
            {
                lblError.Text = "No puede seleccionar mas de un campo";
            }
            else if (id != null)
            {
                FormCompanyEditor form = new FormCompanyEditor(id);
                form.ShowDialog();
            }
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;
            T_Empresa empresa = new T_Empresa();
            int id = 0;
            // Recorrer las filas seleccionadas y obtener el valor del campo ID de cada una
            foreach (DataGridViewRow row in selectedRows)
            {
                 id = Convert.ToInt32(row.Cells["EmpresaID"].Value);
                
                using (BD_PasajeInteligenteEntities db = new BD_PasajeInteligenteEntities())
                {
                    empresa = db.T_Empresa.Find(id);
                    db.T_Empresa.Remove(empresa);
                    db.SaveChanges();
                   
                }
            }
            MonstrarDatos();

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            using (BD_PasajeInteligenteEntities db = new BD_PasajeInteligenteEntities())
            {
                string filtro = txtFiltro.Text;

                // Crear una consulta LINQ para filtrar por el campo "Nombre"
                var consulta = from empresa in db.T_Empresa
                               where empresa.Nombre.Contains(filtro)
                               select empresa;

                // Convertir los resultados de la consulta en una lista de objetos T_Empresa
                List<T_Empresa> empresasFiltradas = consulta.ToList();

                // Convertir la lista de objetos T_Empresa a un DataTable
                DataTable tablaFiltrada = ConvertToDataTable(empresasFiltradas);

                // Asignar la tabla filtrada como fuente de datos del DataGridView
                dataGridView1.DataSource = tablaFiltrada;
            }



        }

        private DataTable ConvertToDataTable(List<T_Empresa> empresasFiltradas)
        {
            // Crear un nuevo DataTable
            DataTable dataTable = new DataTable();

            // Obtener las propiedades del modelo
            PropertyInfo[] propiedades = typeof(T_Empresa).GetProperties();

            // Agregar las columnas al DataTable
            foreach (PropertyInfo propiedad in propiedades)
            {
                dataTable.Columns.Add(propiedad.Name, Nullable.GetUnderlyingType(propiedad.PropertyType) ?? propiedad.PropertyType);
            }

            // Agregar las filas al DataTable
            foreach (T_Empresa objeto in empresasFiltradas)
            {
                DataRow row = dataTable.NewRow();

                foreach (PropertyInfo propiedad in propiedades)
                {
                    row[propiedad.Name] = propiedad.GetValue(objeto) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
