using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExcelData;

public static class CSharpGenerator
{
    public static void GenDataManager(ExcelData excelData, string outputPath)
    {
        StringBuilder sb = new StringBuilder();
        //引用
        sb.AppendLine("/*Auto create");
        sb.AppendLine("Don't Edit it*/");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Runtime.Serialization;");
        sb.AppendLine("using System.Runtime.Serialization.Formatters.Binary;");
        sb.AppendLine("using System.IO;");
        sb.AppendLine("using Google.Protobuf;");
        sb.AppendLine("using System.Reflection;");

        //类
        sb.AppendLine("public class DataManager");
        sb.AppendLine("{");

        //inst
        sb.AppendLine("\tpublic static DataManager inst");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tget");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\tif (_inst == null) _inst = new DataManager();");
        sb.AppendLine("\t\t\treturn _inst;");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("\tprivate static DataManager _inst;");

        //变量
        for (int i = 0; i < excelData.tableList.Count; ++i)
        {
            TableData data = excelData.tableList[i];
            sb.AppendLine(string.Format("\tpublic {0} {1};"
                , data.tableName + ProtobufGen.CONTAINER_NAME_EXTEND
                , data.tableName.Substring(2) + ProtobufGen.CONTAINER_NAME_EXTEND));
        }
        sb.AppendLine("\tprivate Func<string, byte[]> _loadFunc;");

        //初始化函数
        sb.AppendLine();
        sb.AppendLine("\tpublic void Init(Func<string, byte[]> loadFunc)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\t_loadFunc = loadFunc;");
        for (int i = 0; i < excelData.tableList.Count; ++i)
        {
            TableData data = excelData.tableList[i];
            string name = data.tableName.Substring(2);
            string containerName = name + ProtobufGen.CONTAINER_NAME_EXTEND;
            string containerTypeName = data.tableName + ProtobufGen.CONTAINER_NAME_EXTEND;

            sb.AppendLine(string.Format("\t\t{0} = Load<{2}>(\"{1}\");"
                , containerName
                , data.tableName
                , containerTypeName));
        }
        sb.AppendLine("\t}");

        //函数
        for (int i = 0; i < excelData.tableList.Count; ++i)
        {
            sb.AppendLine();
            TableData data = excelData.tableList[i];
            string name = data.tableName.Substring(2);
            string containerName = name + ProtobufGen.CONTAINER_NAME_EXTEND;
            sb.AppendLine(string.Format("\tpublic {0} {1}({2} {3})"
                , data.tableName
                , "Get" + name.Substring(0, 1).ToUpper() + name.Substring(1)
                , ConvertIdTypeName(data.idTypeName)
                , ProtobufGen.ID_NAME));
            sb.AppendLine("\t{");

            sb.AppendLine(string.Format("\t\t{0} t = null;", data.tableName));
            sb.AppendLine(string.Format("\t\t{0}.{1}.TryGetValue({2}, out t);"
                , containerName
                , ProtobufGen.CONTAINER_NAME_EXTEND
                , ProtobufGen.ID_NAME));

            sb.AppendLine(string.Format("\t\tif (t == null) throw new Exception(\"can't find the id \" + {0} + \" in {1}\");"
                , ProtobufGen.ID_NAME
                , containerName));
            sb.AppendLine("\t\treturn t;");
            sb.AppendLine("\t}");
        }

        //加载函数
        sb.AppendLine();
        sb.AppendLine("\tprivate T Load<T>(string name) where T : IMessage");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tbyte[] bytes = _loadFunc.Invoke(name);");
        sb.AppendLine("\t\tType type = typeof(T);");
        sb.AppendLine("\t\tPropertyInfo info = type.GetProperty(\"Parser\");");
        sb.AppendLine("\t\tobject o = info.GetGetMethod().Invoke(null, null);");
        sb.AppendLine("\t\tobject msg = o.GetType().GetMethod(\"ParseFrom\", new Type[] { typeof(byte[]), typeof(int), typeof(int) }).Invoke(o, new object[] { bytes, 0, bytes.Length });");
        sb.AppendLine("\t\treturn (T)msg;");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        SaveFile(sb.ToString(), outputPath);
    }

    //保存文件
    private static void SaveFile(string content, string outputPath)
    {
        string path = Path.Combine(outputPath, "DataManager.cs");
        FileStream fs = File.Create(path);
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    private static string ConvertIdTypeName(string idTypeName)
    {
        switch (idTypeName)
        {
            case "int32": return "int";
        }
        return idTypeName;
    }
}
