using System;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace LanguageConverter
{
    public class LanguageSource
    {
        private const string FilePre = "Lang.cs";
        List<XmlDocument> lst = new List<XmlDocument>();
        public void CreateSrc()
        {
            string[] files = Directory.GetFiles("Output","*.xml");
            foreach(string file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                lst.Add(doc);
            }
            CreateMethod();
        }

        private void XXGenerateCode()
        {
          

            //准备一个代码编译器单元

            CodeCompileUnit unit = new CodeCompileUnit();

            //准备必要的命名空间（这个是指要生成的类的空间）

            CodeNamespace sampleNamespace = new CodeNamespace("Xizhang.com");

            //导入必要的命名空间

            sampleNamespace.Imports.Add(new CodeNamespaceImport("System"));
            sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));

            //准备要生成的类的定义

            CodeTypeDeclaration Customerclass = new CodeTypeDeclaration("Customer");//替换节点

            //指定这是一个Class

            Customerclass.IsClass = true;

            Customerclass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;

            //把这个类放在这个命名空间下

            sampleNamespace.Types.Add(Customerclass);

            //把该命名空间加入到编译器单元的命名空间集合中

            unit.Namespaces.Add(sampleNamespace);

            //这是输出文件

            string outputFile = "Customer.cs";

            //添加字段

            CodeMemberField field = new CodeMemberField(typeof(System.String), "_Id");

            field.Attributes = MemberAttributes.Private;

            Customerclass.Members.Add(field);

            //添加属性

            CodeMemberProperty property = new CodeMemberProperty();

            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            property.Name = "Id";

            property.HasGet = true;

            property.HasSet = true;

            property.Type = new CodeTypeReference(typeof(System.String));

            property.Comments.Add(new CodeCommentStatement("这是Id属性"));

            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id")));

            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id"), new CodePropertySetValueReferenceExpression()));

            Customerclass.Members.Add(property);

            //添加方法（使用CodeMemberMethod)--此处略

            //添加构造器(使用CodeConstructor) --此处略

            //添加程序入口点（使用CodeEntryPointMethod） --此处略

            //添加事件（使用CodeMemberEvent) --此处略

            //添加特征(使用 CodeAttributeDeclaration)

            Customerclass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));

            //生成代码

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CodeGeneratorOptions options = new CodeGeneratorOptions();

            options.BracingStyle = "C";

            options.BlankLinesBetweenMembers = true;

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
            {

                provider.GenerateCodeFromCompileUnit(unit, sw, options);

            }

        
    }
   
        private string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append(" public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append(" {");
            sb.Append(Environment.NewLine);
            //方法部分
            sb.Append(" public string OutPut()");
            sb.Append(Environment.NewLine);
            sb.Append(" {");
            sb.Append(Environment.NewLine);
            sb.Append(" return \"Hello world!\";");
            sb.Append(Environment.NewLine);
            sb.Append(" }");
            //
            sb.Append(Environment.NewLine);
            sb.Append(" }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();
            Console.WriteLine(code);
            Console.WriteLine();

            return code;
        }

        private void CreateMethod()
        {
            //按照默认值XML获取
            string file = @"Output\\Lang-ZH-CN.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            //
          var asm=  doc.DocumentElement.SelectNodes("Assembly");
            foreach(XmlNode node in asm)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    //每个类节点
                    string type = child.Attributes["Type"].Value;
                    string[] clsType = type.Split('.');
                    StringBuilder sb = new StringBuilder();
                    sb.Append("using System;");
                    sb.Append(Environment.NewLine);
                    sb.Append("namespace " + clsType[0]);
                    sb.Append(Environment.NewLine);
                    sb.Append("{");
                    sb.Append(Environment.NewLine);
                    sb.AppendFormat("  partial class {0} ", clsType[1]);
                    sb.Append(Environment.NewLine);
                    sb.Append(" {");
                    sb.Append(Environment.NewLine);
                    //方法部分
                    foreach (var dc in lst)
                    {
                        sb.Append(Environment.NewLine);
                        string lname = dc.DocumentElement.SelectSingleNode("LanguageName").InnerText;
                        sb.AppendFormat(" public void LanguageTo{0}()", lname.Replace("-", "_"));
                        sb.Append(Environment.NewLine);
                        sb.Append(" {");
                        sb.Append(Environment.NewLine);
                        //方法内容
                        //查找同一个节点
                        var cur = dc.DocumentElement.SelectNodes("Assembly");
                        StringBuilder sbrMethod = new StringBuilder();
                        foreach (XmlNode curNode in cur)
                        {
                            if(node.Attributes["FileName"].Value == curNode.Attributes["FileName"].Value)
                            {
                                XmlNode nodefind = node.SelectSingleNode(child.Name);
                                //组成方法
                                CreateContent(nodefind, sbrMethod);
                            }


                        }
                        sb.Append(sbrMethod.ToString());
                       // sb.Append(" return \"Hello world!\";");
                        sb.Append(Environment.NewLine);
                        sb.Append(" }");
                        //
                    }

                    sb.Append(Environment.NewLine);
                    sb.Append(" }");
                    sb.Append(Environment.NewLine);
                    sb.Append("}");
                    //
                    string dllFile = node.Attributes["FileName"].Value;
                    dllFile = dllFile.Substring(0, dllFile.Length - 4);
                    DirectoryInfo directory = new DirectoryInfo("Output\\" +dllFile);
                    if(!directory.Exists)
                    {
                        directory.Create();
                    }
                    SaveFile(sb,directory.FullName,clsType[1]);
                    string code = sb.ToString();
                    Console.WriteLine(code);
                    Console.WriteLine();
                }
            }
        }
    
        /// <summary>
        /// 生成方法内容
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sbr"></param>

        private void CreateContent(XmlNode node,StringBuilder sbr)
        {
            if(node.Attributes!=null&&node.Attributes["Title"]!=null)
            {
                sbr.AppendFormat("{0}.{1}={2}", node.Name, node.Attributes["TitleName"].Value, node.Attributes["Title"].Value);
                sbr.Append(Environment.NewLine);
            }
            if(node.HasChildNodes)
            {
                foreach(XmlNode child in node.ChildNodes)
                {
                    CreateContent(child, sbr);
                }
            }
            else
            {

                sbr.AppendFormat("{0}.{1}={2}", node.Name, node.Attributes["TitleName"].Value, node.InnerText);
                sbr.Append(Environment.NewLine);

            }
        }

        private void SaveFile(StringBuilder sbr,string dir,string name)
        {
            //
            StringBuilder file = new StringBuilder();
            file.AppendFormat("{0}\\{1}.{2}", dir, name,FilePre);
            using(StreamWriter sw=new StreamWriter(file.ToString()))
            {
                sw.WriteLine(sbr);
            }
        }

    }
}
