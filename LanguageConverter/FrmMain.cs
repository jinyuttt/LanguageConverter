using System.IO;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Text;

namespace LanguageConverter
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnOPen_Click(object sender, System.EventArgs e)
        {
            //
            openFileDialog1.Filter = "*dll|*.exe";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
                ScanAssembly scan = new ScanAssembly();
                scan.Add(txtFile.Text);
                scan.Save();
                MessageBox.Show("OK");
            }
        }

        private void BtnTest_Click(object sender, System.EventArgs e)
        {
            ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();
            using(StreamReader rd=new StreamReader("Lang.txt",System.Text.Encoding.Default))
            {
                int index = 0;
                string[] v = new string[2];
                while(rd.Peek()!=-1)
                {
                    string line = rd.ReadLine();
                    if(!string.IsNullOrEmpty(line.Trim()))
                    {
                        v[index] = line;
                        index++;
                        if(index%2==0)
                        {
                            dic[v[0]] = v[1];
                            index = 0;
                        }

                    }

                }
            }
            //
            StreamWriter sw = new StreamWriter("Language.csv",false,System.Text.Encoding.Default);
            StringBuilder sbr = new StringBuilder();
            foreach(var kv in dic)
            {
                sw.WriteLine(string.Format("{0},{1}", kv.Key, kv.Value));
                sbr.Append(kv.Value + ",");
            }
            sw.Close();

            sw = new StreamWriter("LanguageContent");
            sw.WriteLine(sbr.ToString());
            sw.Close();
        }

        private void Btn_XML_Click(object sender, System.EventArgs e)
        {
            ScanAssembly scan = new ScanAssembly();
            scan.LanguageXML();
        }

        private void btnSrc_Click(object sender, System.EventArgs e)
        {
            LanguageSource source = new LanguageSource();
            source.CreateSrc();
        }
    }
}
