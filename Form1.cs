using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeTai12_KTMT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Tạo mảng kí tự rỗng
        public char[] tam11 = null;

        //Click vào kết quả để hiển thị ra màn hình kết quả
        private void btnKetQua_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập đủ dữ liệu chưa
            if (txtNhapSoBit.Text != "" && txtNhapSoBiNhan.Text != "" && txtNhapSoNhan.Text != "")
            {
                //Lỗi không chứa toàn số
                if (kiemTraChuoiKiSo(txtNhapSoBit.Text) == false
                || kiemTraChuoiKiSo(txtNhapSoBiNhan.Text) == false
                || kiemTraChuoiKiSo(txtNhapSoNhan.Text) == false)
                {
                    MessageBox.Show("Vui lòng chỉ nhập vào số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtNhapSoBit.Text[0] == '-')
                {
                    MessageBox.Show("Số bit không được âm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
                else
                {
                    //Số bit
                    int soBit = Convert.ToInt32(txtNhapSoBit.Text);

                    //Số bị nhân
                    int sbn = Convert.ToInt32(txtNhapSoBiNhan.Text);
                    
                    //Số nhân
                    int sn = Convert.ToInt32(txtNhapSoNhan.Text);

                    //Trường hợp số nhân hoặc bị nhân quá lớn so với sức biểu diễn của số bit
                    if (-Math.Pow(2, soBit - 1) > sbn || Math.Pow(2, soBit - 1) - 1 < sbn ||
                        -Math.Pow(2, soBit - 1) > sn || Math.Pow(2, soBit - 1) - 1 < sn)
                    {
                        MessageBox.Show("Số bị nhân hoặc số nhân nằm ngoài giới hạn của bit!" +
                            "\nVui lòng nhập lại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        char[] soBiNhan = new char[soBit];
                        soBiNhan = chuyenHe10Sang2(sbn, soBiNhan, soBit);

                        char[] soNhan = new char[soBit];
                        soNhan = chuyenHe10Sang2(sn, soNhan, soBit);

                        txtSBNNP_TextChanged(sender, e);
                        txtSNNP_TextChanged(sender, e);

                        //Kết quả
                        char[] tam = tichNhiPhan(soBiNhan, soNhan, soBit);
                        string kq = new string(tam11);
                        for (int i = 0; i < tam.Length; i++)
                            kq += tam[i];
                        txtKetQua.Text = kq;
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Tính số bù 2 của số nhị phân
        public static char[] soBu2(char[] s)
        {
            char[] tam = new char[s.Length];
            for (int i = 0; i < s.Length; i++)
                tam[i] = s[i];
            for (int i = tam.Length - 1; i >= 0; i--)
                if (tam[i] == '1')
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (tam[j] == '0')
                            tam[j] = '1';
                        else
                            tam[j] = '0';
                    }
                    break;
                }
            return tam;
        }

        //Chuyển số thập phân sang nhị phân có n bit
        public static char[] chuyenHe10Sang2(int x, char[] s, int n)
        {
            int dau = 0;
            if (x < 0)
            {
                dau = 1;
                x = Math.Abs(x);
            }
            int i = n - 1;
            while (x != 0 && i >= 0)
            {
                s[i] = Convert.ToChar(x % 2 + 48);
                x /= 2;
                i--;
            }
            //Nếu chưa đủ n bit thì thêm vào cho đủ
            for (int j = 0; j < s.Length; j++)
            {
                if (s[j] == '\0')
                    s[j] = '0';
                else
                    break;
            }
            if (dau == 1)
                s = soBu2(s);
            return s;
        }

        //Xuất số nhị phân ra màn hình
        public static void xuatNhiPhan(char[] s)
        {
            foreach (char c in s)
                Console.Write("{0}", c);
        }

        //Công thức cộng nhị phân
        public static char phepCong(char a, char b)
        {
            if (a == '0' && b == '0' || a == '1' && b == '1')
                return '0';
            return '1';
        }

        //Phép cộng 2 số nhị phân
        public static char[] congNhiPhan(char[] a, char[] b, int n)
        {
            char[] kq = new char[n];
            //Cộng từ phải sang trái
            for (int i = kq.Length - 1; i >= 0; i--)
            {
                if (kq[i] == '\0')
                    kq[i] = '0';
                char tam = kq[i];
                kq[i] = phepCong(kq[i], phepCong(a[i], b[i]));
                if (a[i] == '1' && b[i] == '1' || phepCong(a[i], b[i]) == '1' && tam == '1')
                {
                    if (i == 0)
                        break;
                    kq[i - 1] = phepCong(kq[i - 1], '1');
                }
            }
            return kq;
        }

        //Chèn kí tự x vào đầu và xóa kí tự cuối
        public static char[] dichPhaiThemX(char[] s, char x)
        {
            for (int i = s.Length - 1; i > 0; i--)
                s[i] = s[i - 1];
            s[0] = x;
            return s;
        }

        //Hàm tính tích 2 số nhị phân
        public static char[] tichNhiPhan(char[] M, char[] Q, int n)
        {
            char[] tich = new char[n * 2];
            char[] A = new char[n];
            for (int i = 0; i < n; i++)
                A[i] = '0';
            char Q1 = '0';
            char[] sbM = soBu2(M);
             
            for (int i = 0; i < n; i++)
            {
                if (Q[Q.Length - 1] == Q1)
                {
                    //Dịch phải  A, Q, Q1
                    Q1 = Q[Q.Length - 1];
                    Q = dichPhaiThemX(Q, A[A.Length - 1]);
                    A = dichPhaiThemX(A, A[0]);
                }
                else if (Q[Q.Length - 1] > Q1) //10
                {
                    A = congNhiPhan(A, sbM, n);
                    Q1 = Q[Q.Length - 1];
                    Q = dichPhaiThemX(Q, A[A.Length - 1]);
                    A = dichPhaiThemX(A, A[0]);
                }
                else  //01
                {
                    A = congNhiPhan(A, M, n);
                    Q1 = Q[Q.Length - 1];
                    Q = dichPhaiThemX(Q, A[A.Length - 1]);
                    A = dichPhaiThemX(A, A[0]);
                }
            }
            A.CopyTo(tich, 0);
            Q.CopyTo(tich, A.Length);
            return tich;
        }

        //Kiểm tra trong mảng có toàn kí số hay không
        public static bool kiemTraChuoiKiSo(string s)
        {
            //Nếu kí tự đầu k phải là dấu hoặc số thì sai
            if (s[0] != '-' && s[0] != '+' && (s[0] < 48 || 57 < s[0]))
                return false;
            for (int i = 1; i < s.Length; i++)
                if (s[i] < 48 || 57 < s[i])
                    return false;
            return true;
        }
        

        //Xóa các dữ liệu
        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtKetQua.Clear();
            txtNhapSoBiNhan.Clear();
            txtNhapSoBit.Clear();
            txtNhapSoNhan.Clear();
            txtSBNNP.Clear();
            txtSNNP.Clear();
        }

        //Thoát chương trình
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dlr = MessageBox.Show("Bạn muốn thoát chương trình ??", "Thông Báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                this.Close();
                Application.Exit();
            }
        }

        //Xuất ra kết quả số bị nhân ở dạng nhị phân
        private void txtSBNNP_TextChanged(object sender, EventArgs e)
        {
            if (txtNhapSoBiNhan.Text != "")
            {
                int soBit = Convert.ToInt32(txtNhapSoBit.Text);

                int sbn = Convert.ToInt32(txtNhapSoBiNhan.Text);
                char[] soBiNhan = new char[soBit];
                soBiNhan = chuyenHe10Sang2(sbn, soBiNhan, soBit);

                string kq = new string(tam11);
                for (int i = 0; i < soBiNhan.Length; i++)
                    kq += soBiNhan[i];
                txtSBNNP.Text = kq;
            }    
        }

        //Xuất ra kết quả số nhân ở dạng nhị phân
        private void txtSNNP_TextChanged(object sender, EventArgs e)
        {
            if (txtNhapSoNhan.Text != "")
            {
                int soBit = Convert.ToInt32(txtNhapSoBit.Text);

                int sn = Convert.ToInt32(txtNhapSoNhan.Text);
                char[] soNhan = new char[soBit];
                soNhan = chuyenHe10Sang2(sn, soNhan, soBit);

                string kq = new string(tam11);
                for (int i = 0; i < soNhan.Length; i++)
                    kq += soNhan[i];
                txtSNNP.Text = kq;
            }    
        }
    }
}
