using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExcelData;

namespace ConfigTool.Java
{
    public class JavaGenerator
    {
        public static void GenDataManager(ExcelData excelData, string outputPath, string package)
        {
            outputPath = outputPath + "/" + package.Replace('.', '/');
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//auto generated");
            sb.AppendLine("//don't edit");
            //包名
            sb.AppendLine(string.Format("package {0};", package));

            //引用
            sb.AppendLine("import java.io.FileInputStream;");
            sb.AppendLine("import java.io.InputStream;");
            sb.AppendLine("import java.lang.reflect.Method;");

            sb.AppendLine();
            sb.AppendLine("public class DataManager {");

            //枚举
            sb.AppendLine();
            sb.AppendLine("\tpublic enum EConfigType{");
            for (int i = 0; i < excelData.tableList.Count; i++)
            {
                TableData table = excelData.tableList[i];
                string name = table.tableName.Substring(2);
                name = Util.InitialToUpper(name);
                sb.AppendLine(string.Format("\t\t{0}({1}){2}"
                    , name, i == 0 ? 1 : i * 2, i == excelData.tableList.Count - 1 ? ";" : ","));
            }
            sb.AppendLine("\t\tpublic int value;");
            sb.AppendLine();
            sb.AppendLine("\t\tprivate EConfigType(int value){");
            sb.AppendLine("\t\t\tthis.value = value;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine();

            //定义
            sb.AppendLine("\tprivate static DataManager _instance;");
            sb.AppendLine("\tpublic static DataManager getInstance() {");
            sb.AppendLine("\t\tif (_instance == null)");
            sb.AppendLine("\t\t\t_instance = new DataManager();");
            sb.AppendLine("\t\treturn _instance;");
            sb.AppendLine("\t}");
            sb.AppendLine("\tprivate String _protoPath;");
            sb.AppendLine("\tprivate String _protoExtension;");
            sb.AppendLine();

            //变量
            for (int i = 0; i < excelData.tableList.Count; i++)
            {
                TableData table = excelData.tableList[i];
                string className = TableNameToClassName(table.tableName);
                string containerTypeName = table.tableName + ProtobufGen.CONTAINER_NAME_EXTEND;
                string containerName = table.tableName.Substring(2) + ProtobufGen.CONTAINER_NAME_EXTEND;
                sb.AppendLine(string.Format("\tprivate {0}.{1} {2};"
                    , className
                    , containerTypeName
                    , containerName));
            }
            sb.AppendLine();

            //初始化
            sb.AppendLine("\tpublic void Init(String protoPath, String protoExtension) {");
            sb.AppendLine("\t\t_protoPath = protoPath;");
            sb.AppendLine("\t\t_protoExtension = protoExtension;");
            for (int i = 0; i < excelData.tableList.Count; i++)
            {
                TableData table = excelData.tableList[i];
                string containerName = table.tableName.Substring(2) + ProtobufGen.CONTAINER_NAME_EXTEND;
                sb.AppendLine(string.Format("\t\t{0} = Load(\"{1}\");", containerName, table.tableName));
            }
            sb.AppendLine("\t}");
            sb.AppendLine();

            //获取config函数
            string outerName = Util.InitialToUpper(ProtobufGen.ALL_CONFIG_NAME) + "." + ProtobufGen.ALL_CONFIG_NAME;
            sb.AppendLine(string.Format("\tpublic {0} GetConfig(int configType) {1}"
                , outerName, "{"));
            sb.AppendLine(string.Format("\t\t{0}.Builder builder = {0}.newBuilder();", outerName));

            for (int i = 0; i < excelData.tableList.Count; i++)
            {
                TableData table = excelData.tableList[i];
                string name = table.tableName.Substring(2);
                string containerName = name + ProtobufGen.CONTAINER_NAME_EXTEND;
                string upContainerName = Util.InitialToUpper(containerName);
                name = Util.InitialToUpper(name);
                sb.AppendLine(string.Format("\t\tif ((configType & EConfigType.{0}.value) != 0)", name));
                sb.AppendLine(string.Format("\t\t\tbuilder.set{0}({1});", upContainerName, containerName));
            }
            sb.AppendLine("\t\treturn builder.build();");
            sb.AppendLine("\t}");
            sb.AppendLine();

            //Load函数
            sb.AppendLine("\tprivate <T> T Load(String name) {");
            sb.AppendLine("\t\ttry {");
            sb.AppendLine("\t\t\tString typeName = TableNameToClassName(name);");
            sb.AppendLine("\t\t\tString containerTypeName = name + \"Container\";");
            sb.AppendLine(string.Format("\t\t\tClass type = Class.forName(\"{0}.\" + typeName + \"$\" + containerTypeName);"
                , package));
            sb.AppendLine("\t\t\tString fileName = _protoPath + name + _protoExtension;");
            sb.AppendLine("\t\t\tFileInputStream fis = new FileInputStream(fileName);");
            sb.AppendLine("\t\t\tMethod method = type.getMethod(\"parseFrom\", InputStream.class);");
            sb.AppendLine("\t\t\tObject obj = method.invoke(null, fis);");
            sb.AppendLine("\t\t\treturn (T)obj;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\tcatch (Exception e) {");
            sb.AppendLine("\t\t\tSystem.out.print(e.getMessage());");
            sb.AppendLine("\t\t\treturn null;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");

            sb.AppendLine();
            //类名转换函数
            sb.AppendLine("\tprivate String TableNameToClassName(String tableName) {");
            sb.AppendLine("\t\tString sub = tableName.substring(2, 3).toUpperCase() + tableName.substring(3);");
            sb.AppendLine("\t\tString className = tableName.substring(0, 1).toUpperCase() + sub;");
            sb.AppendLine("\t\treturn className;");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            SaveFile(sb.ToString(), outputPath);
        }

        //表名转为java中的类名
        private static string TableNameToClassName(string tableName)
        {
            string sub = tableName.Substring(2, 1).ToUpper() + tableName.Substring(3);
            return tableName.Substring(0, 1).ToUpper() + sub;
        }

        //保存文件
        private static void SaveFile(string content, string outputPath)
        {
            string path = Path.Combine(outputPath, "DataManager.java");
            FileStream fs = File.Create(path);
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

    }
}
