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
                //
                XmlElement name= doc.CreateElement("LanguageName");
                name.InnerText = "zh-cn";
                root.AppendChild(name);

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
            doc.Save(@"Output\\Lang-ZH-CN.xml");
            using (StreamWriter sw = new StreamWriter(@"Output\\LabelContent.csv", false,Encoding.Default)) 
            {
                sw.WriteLine("zh-cn");
                foreach(var lin in lstContent)
                {
                    sw.WriteLine(lin);
                }
            }
            lstContent.Clear();
        }
        
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="zh_cn"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Dictionary<string,string> GetConvert(int zh_cn=0,int index=0)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (StreamReader rd = new StreamReader(@"Output\\LabelContent.csv",Encoding.Default))
            {
                while (rd.Peek() != -1)
                {
                    string line = rd.ReadLine();
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        string[]  words = line.Split(',');
                        dic[words[zh_cn]] = words[index];
                    }

                }
            }
            return dic;
        }


        /// <summary>
        /// 创建语言XML
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        private void CreateXML(Dictionary<string, string> dic, string name)
        {
            string file = @"Output\\Lang-" + name.ToUpper() + ".xml";
            if(File.Exists(file))
            {
                File.Delete(file);
            }
            File.Copy(@"Output\\Lang-ZH-CN.xml",file);
            XmlDocument document = new XmlDocument();
            document.Load(file);
           var langName= document.DocumentElement.SelectSingleNode("LanguageName");
            if (langName != null)
            {
                langName.InnerText = name.ToLower();
            }

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                XmlElement element = (XmlElement)node;
                if(element.GetAttributeNode("Title")!=null)
                {
                    string v = element.GetAttribute("Title");
                    string ch= IsChinese(v);
                    if(!string.IsNullOrEmpty(ch))
                    {
                        //如果中间有空格
                        if (ch.Length != v.Length)
                        {
                            v=Regex.Replace(v, @"\s", "");
                        }
                        v = v.Replace(ch, dic[ch]);
                        element.SetAttribute("Title", v);
                    }
                    if (element.HasChildNodes)
                    {
                        Find(node, dic);
                    }
                }
                else if(node.HasChildNodes)
                {
                    Find(node,dic);
                }
                else
                {
                    string v = node.InnerText;
                    string ch = IsChinese(v);
                    if (!string.IsNullOrEmpty(ch))
                    {
                        //如果中间有空格
                        if (ch.Length != v.Length)
                        {
                            v = Regex.Replace(v, @"\s", "");
                        }
                        v = v.Replace(ch, dic[ch]);
                    }
                }
            }
            document.Save(file);
        }
        
        /// <summary>
        /// 替换显示
        /// </summary>
        /// <param name="child"></param>
        /// <param name="dic"></param>
        private void Find(XmlNode child,Dictionary<string,string> dic)
        {
            foreach (XmlNode node in child.ChildNodes)
            {
                if (node.Attributes != null && node.Attributes["Title"] != null)
                {
                    var Attri = node.Attributes["Title"];
                    string v = Attri.Value;
                    string ch = IsChinese(v);
                    if (!string.IsNullOrEmpty(ch))
                    {
                        //如果中间有空格
                        if (ch.Length != v.Length)
                        {
                            v = Regex.Replace(v, @"\s", "");
                        }
                        v = v.Replace(ch, dic[ch]);
                        Attri.Value = v;

                    }
                    //
                    if (node.HasChildNodes)
                    {
                        Find(node, dic);
                    }

                }
                else if (node.HasChildNodes)
                {
                    Find(node, dic);
                }
                else
                {
                    string v = node.InnerText;
                    string ch = IsChinese(v);
                    if (!string.IsNullOrEmpty(ch))
                    {
                        //如果中间有空格
                        if (ch.Length != v.Length)
                        {
                            v = Regex.Replace(v, @"\s", "");
                        }
                        v = v.Replace(ch, dic[ch]);
                        node.InnerText = v;
                    }
                }
            }
        }

        /// <summary>
        /// 处理XML
        /// </summary>
        public void LanguageXML()
        {
            List<string> lst = new List<string>();//获取所有语言代码
            using(StreamReader rd=new StreamReader(@"Output\\Language.csv"))
            {
                while(rd.Peek()!=-1)
                {
                    string line = rd.ReadLine();
                    string[] name = line.Split(',');
                    lst.Add(name[1]);
                }
            }
            //找到翻译顺序
            List<string> lstCode = new List<string>();
            using (StreamReader rd = new StreamReader(@"Output\\LabelContent.csv"))
            {
                while (rd.Peek() != -1)
                {
                    string line = rd.ReadLine();
                    if(!string.IsNullOrEmpty(line.Trim()))
                    {
                        string[] name = line.Split(',');
                        lstCode.AddRange(name);
                        break;
                    }
                 
                }
            }
            //按照代码名称生成XML
            int zh_cn = 0;
            zh_cn = lstCode.IndexOf("zh-cn");
            if (zh_cn > -1)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if(lst[i]=="zh-cn")
                    {
                        continue;
                    }
                    int index = lstCode.IndexOf(lst[i]);

                    if (index > -1)
                    {
                        var dic = GetConvert(zh_cn, index);
                        CreateXML(dic, lst[i]);
                    }

                }
            }
           
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
