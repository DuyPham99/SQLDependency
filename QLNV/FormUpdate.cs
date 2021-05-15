using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNV
{
    public partial class FormUpdate : Form
    {
        public delegate void NewHome();
        public event NewHome OnNewHome;
        String conn = "Data Source=LAPTOP-DOM;Initial Catalog=QLNV;Persist Security Info=True;User ID=sa;Password=123";
        SqlConnection con = null;

        public FormUpdate()
        {
            InitializeComponent();
            try
            {
                SqlClientPermission ss = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
                ss.Demand();
            }
            catch (Exception)
            {

                throw;
            }
            SqlDependency.Stop(conn);
            SqlDependency.Start(conn);
            con = new SqlConnection(conn);
        }

        private void Update_Load(object sender, EventArgs e)
        {
            OnNewHome += new NewHome(Form1_OnNewHome);//tab
            // TODO: This line of code loads data into the 'qLNVDataSet.NHANVIEN' table. You can move, or remove it, as needed.
            LoadData();
        }          

        public void reLoadData()
        {
            this.nHANVIENTableAdapter.Fill(this.qLNVDataSet.NHANVIEN);
            this.nHANVIENBindingSource.ResetBindings(false);
        }

        public void Add(String MANV, String HO, String TEN, 
                                            String PHAI, String DIACHI, String NGAYSINH, String LUONG)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            // typically obtained from user
            // input, but we take a short cut
            string custId = "FURIB";

            Console.WriteLine("\nCustomer Order History:\n");

            try
            {
                // create and open a connection object
                conn = new
                    SqlConnection("Data Source=LAPTOP-DOM;Initial Catalog=QLNV;Persist Security Info=True;User ID=sa;Password=123");
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    @"Sp_ThemNV", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@MANV", MANV));
                cmd.Parameters.Add(new SqlParameter("@HO", HO));
                cmd.Parameters.Add(new SqlParameter("@TEN", TEN));
                cmd.Parameters.Add(new SqlParameter("@PHAI", PHAI));
                cmd.Parameters.Add(new SqlParameter("@DIACHI", DIACHI));
                cmd.Parameters.Add(new SqlParameter("@NGAYSINH", NGAYSINH));
                cmd.Parameters.Add(new SqlParameter("@LUONG", LUONG));
                cmd.Parameters.Add(new SqlParameter("@MACN", "TPHCM"));
                // execute the command
                rdr = cmd.ExecuteReader();
                reLoadData();
                this.btnExit.PerformClick();
                MessageBox.Show("Thành công!");
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Errors[0].Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        public void Delete(String MANV)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                // create and open a connection object
                conn = new
                    SqlConnection("Data Source=LAPTOP-DOM;Initial Catalog=QLNV;Persist Security Info=True;User ID=sa;Password=123");
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    @"Sp_DeleteNV", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@MANV", MANV));
                // execute the command
                rdr = cmd.ExecuteReader();
                reLoadData();
                this.btnExit.PerformClick();
                MessageBox.Show("Thành công!");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        public void Update(String MANV, String HO, String TEN,
                                            String PHAI, String DIACHI, String NGAYSINH, String LUONG)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                // create and open a connection object
                conn = new
                    SqlConnection("Data Source=LAPTOP-DOM;Initial Catalog=QLNV;Persist Security Info=True;User ID=sa;Password=123");
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    @"Sp_UpdateNV", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@MANV", MANV));
                cmd.Parameters.Add(new SqlParameter("@HO", HO));
                cmd.Parameters.Add(new SqlParameter("@TEN", TEN));
                cmd.Parameters.Add(new SqlParameter("@PHAI", PHAI));
                cmd.Parameters.Add(new SqlParameter("@DIACHI", DIACHI));
                cmd.Parameters.Add(new SqlParameter("@NGAYSINH", NGAYSINH));
                cmd.Parameters.Add(new SqlParameter("@LUONG", LUONG));
                cmd.Parameters.Add(new SqlParameter("@MACN", "TPHCM"));
                // execute the command
                rdr = cmd.ExecuteReader();
                reLoadData();
                this.btnExit.PerformClick();
                MessageBox.Show("Thành công!");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validate() == false) return;
            Add(tbMaNV.Text, tbHo.Text, tbTen.Text, cbPhai.Text, tbDiaChi.Text, tbDate.Value.ToString("yyyyMMdd"), tbLuong.Text.Replace(".", ""));
        }

        String manv = "";
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbMaNV.Text == string.Empty) MessageBox.Show("Nhập Mã NV!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if ((MessageBox.Show("Bạn có muốn xóa?", "Warning!",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                Delete(this.manv);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnExit.PerformClick();
            if ( (e.RowIndex + 1) == (dataGridView1.Rows.Count) || e.RowIndex < 0) return;

            this.tbMaNV.ReadOnly = true;
            this.manv = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.tbMaNV.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.tbHo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            this.tbTen.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            this.cbPhai.SelectedIndex = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "NAM" ? 0 : 1;
            this.tbDiaChi.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            this.tbDate.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            this.tbLuong.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", double.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()));      
        }

        private void tbLuong_TextChanged(object sender, EventArgs e)
        { 
        }

        private void tbLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
            if(e.KeyChar == (char)13)
            {
                tbLuong.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", double.Parse(tbLuong.Text));
            }
        }

        private void tbMaNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == ' ';
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
            cbPhai.SelectedIndex = -1;
            tbDate.Value = DateTime.Now;
            reLoadData();
            this.tbMaNV.ReadOnly = false;
        }

        // SQL Dependency

        public void de_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency de = sender as SqlDependency;
            de.OnChange -= de_OnChange;
            if (OnNewHome != null)
            {
                OnNewHome();
            }
        }

        public void Form1_OnNewHome()
        {
            ISynchronizeInvoke i = (ISynchronizeInvoke)this;
            if (i.InvokeRequired)//tab
            {
                NewHome dd = new NewHome(Form1_OnNewHome);
                i.BeginInvoke(dd, null);
                return;
            }
            LoadData();
        }

        //Ham load data
        void LoadData()
        {
            DataTable dt = new DataTable();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT  MANV, HO, TEN, PHAI, DIACHI, NGAYSINH, LUONG, MACN, TRANGTHAI FROM[dbo].[NHANVIEN] WHERE TRANGTHAI = 'False'", con);
            cmd.Notification = null;

            SqlDependency de = new SqlDependency(cmd);
            de.OnChange += new OnChangeEventHandler(de_OnChange);

            dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            dataGridView1.DataSource = dt;
        }
        ///////////////////

        private void tbMaNV_Validating(object sender, CancelEventArgs e)
        {
        }

        private bool validate()
        {
            if (tbMaNV.Text == string.Empty)
            {
                MessageBox.Show("Nhập Mã NV!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (tbHo.Text == string.Empty)
            {
                MessageBox.Show("Nhập Họ!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (tbTen.Text == string.Empty)
            {
                MessageBox.Show("Nhập Tên!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (tbDiaChi.Text == string.Empty)
            {
                MessageBox.Show("Nhập Địa Chỉ!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (cbPhai.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập Phái!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (tbDate.Text == string.Empty)
            {
                MessageBox.Show("Nhập Ngày!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (tbLuong.Text == string.Empty)
            {
                MessageBox.Show("Nhập Lương!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (validate() == false) return;
            Update(tbMaNV.Text, tbHo.Text, tbTen.Text, cbPhai.Text, tbDiaChi.Text, tbDate.Value.ToString("yyyyMMdd"), tbLuong.Text.Replace(".", ""));
        }
    }
}
