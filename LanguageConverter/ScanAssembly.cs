using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Text;

namespace LanguageConverter
{
    public  class ScanAssembly
    {
        readonly XmlDocument doc = new XmlDocument();
        readonly List<string> lstContent = new List<string>();
         

        /// <summary>
        /// 提取中文部分
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string IsChinese(string str)
        {
            //判断是否有中文 
            //Regex.IsMatch(CString, @"^[\u4e00-\u9fa5]+$");
            StringBuilder sbr = new StringBuilder();
            Regex reg = new Regex("[\u4e00-\u9fa5]+");
            foreach (Match v in reg.Matches(str))
                sbr.Append(v);
            return sbr.ToString();
        }

       /// <summary>
       /// 记录转换内容
       /// </summary>
       /// <param name="Value"></param>
        private void AddContent(string Value)
        {
            string cur = IsChinese(Regex.Replace(Value, @"\s", ""));
            if (!string.IsNullOrEmpty(cur))
            {
                if (!lstContent.Contains(cur))
                {
                    lstContent.Add(cur);
                }
            }
           
        }

        /// <summary>
        /// 获取显示属性
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private Child GetDisplay(Control control)
        {
            string title = control.Text;
            Child child = new Child();
            child.Name = "Text";
            child.Value = title;
            if (string.IsNullOrEmpty(title))
            {
                var p = control.GetType().GetProperty("Caption");
                if (p != null)
                {
                    title = p.GetValue(control).ToString();
                    child.Name = "Caption";
                }
                if (string.IsNullOrEmpty(title))
                {
                     p = control.GetType().GetProperty("Title");
                    if (p != null)
                    {
                        title = p.GetValue(control).ToString();
                        child.Name = "Title";
                    }
                }
            }
            child.Value = title;
            return child;
        }


        /// <summary>
        /// 查找所有控件内容
        /// </summary>
        /// <param name="control"></param>
        /// <param name="element"></param>
        private void Find(Control  control,XmlElement element)
        {
            if(control.HasChildren)
            {
                foreach (Control c in control.Controls)
                {
                    var node = doc.CreateElement(c.Name);
                    var v = GetDisplay(c);
                    node.SetAttribute("TitleName", v.Name);
                    node.InnerText = v.Value;
                    if (!string.IsNullOrEmpty(v.Value))
                    {
                        AddContent(v.Value);
                    }
                    element.AppendChild(node);
                    if (c.HasChildren)
                    {
                        Find(c, node);
                    }
                }
            }
        }


        /// <summary>
        /// 扫描程序集
        /// </summary>
        /// <param name="file"></param>
        public  void Add(string file)
        {
            if (doc.FirstChild == null)
            {
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                XmlElement root = doc.CreateElement("Language");
                doc.AppendChild(root);
            }
            var asm = Assembly.LoadFile(file);
            var types = asm.DefinedTypes.Where(X => (typeof(Control).IsAssignableFrom(X)));
            XmlElement p = doc.CreateElement("Assembly");
            //
            FileInfo fileInfo = new FileInfo(asm.Location);
            p.SetAttribute("FileName", fileInfo.Name);
            doc.DocumentElement.AppendChild(p);
            foreach (var tp in types)
            {
                var child = doc.CreateElement(tp.Name);
                //  asm.CreateInstance()
                child.SetAttribute("Type", tp.FullName);
                child.SetAttribute("Title", "");
                child.SetAttribute("TitleName", "");
                Control control = Activator.CreateInstance(tp, true) as Control;
                if(control!=null)
                {
                    var v = GetDisplay(control);
                    child.SetAttribute("Title", v.Value);
                    child.SetAttribute("TitleName", v.Name);
                    if (!string.IsNullOrEmpty(v.Value))
                    {
                        AddContent(v.Value);
                    }
                }
                p.AppendChild(child);
                //
                if (control != null)
                {
                    Find(control, child);
                }
            }

        }
    
       /// <summary>
       /// 保存
       /// </summary>
        public void Save()
        {
            doc.Save("Lang-ZH-CN.xml");
            using (StreamWriter sw = new StreamWriter("StringContent.csv",false,Encoding.Default)) 
            {
                sw.WriteLine("zh-cn");
                foreach(var lin in lstContent)
                {
                    sw.WriteLine(lin);
                }
            }
            lstContent.Clear();
        }
    
    }

    /// <summary>
    /// 临时变量
    /// </summary>
    public struct Child
    {
       public string Name;
      public  string Value;
    }
}
