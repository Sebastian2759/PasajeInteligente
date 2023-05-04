using Formulario_Pasaje_Inteligentee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formulario_Pasaje_Inteligentee
{
    public partial class FormCompanyEditor : Form
    {
        public int? id;
        T_Empresa T_Empresa = null;
        public FormCompanyEditor(int? id = null)
        {
            InitializeComponent();
            this.id = id;
            if (id!= null )
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            using (BD_PasajeInteligenteEntities db = new BD_PasajeInteligenteEntities())
            {
                T_Empresa = db.T_Empresa.Find(id);
                txtNombre.Text = T_Empresa.Nombre;
                txtCodigo.Text = Convert.ToString(T_Empresa.Codigo);
                txtDireccion.Text = T_Empresa.Direccion;
                txtTelefono.Text = T_Empresa.Telefono;
                txtCiudad.Text = T_Empresa.Ciudad;
                txtDepartamento.Text = T_Empresa.Departamento;
                txtPais.Text = T_Empresa.Pais;
            }
        }
        private void btnCrear_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1();
            using (BD_PasajeInteligenteEntities db = new BD_PasajeInteligenteEntities())
            {
                T_Empresa empresa = new T_Empresa();

                empresa.Nombre = txtNombre.Text;
                empresa.Codigo = Convert.ToInt32(txtCodigo.Text);
                empresa.Direccion = txtDireccion.Text;
                empresa.Telefono = txtTelefono.Text;
                empresa.Ciudad = txtCiudad.Text;
                empresa.Departamento = txtDepartamento.Text;
                empresa.Pais = txtPais.Text;
                
                
                if (id == null)
                {
                    empresa.Fec_cre = DateTime.Now;
                    db.T_Empresa.Add(empresa);
                }
                else
                {
                    empresa.Fec_mod = DateTime.Now;
                    empresa.EmpresaID = Convert.ToInt32(id);
                    db.Entry(empresa).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();   
                Form1.ShowDialog();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1();
            Form1.ShowDialog();
        }
    }
}
