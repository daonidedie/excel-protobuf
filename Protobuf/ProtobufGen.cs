using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Protobuf;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Google.Protobuf.Collections;
using static ExcelData;
using static ExcelData.TableData;

class ProtobufGen : SingletonTemplate<ProtobufGen>
{

    //容器名扩展
    public const string CONTAINER_NAME_EXTEND = "Container";
    //第一列id的名字
    public const string ID_NAME = "id";
    //容器变量名
    public static readonly string CONTAINER_VARIABLE_NAME = CONTAINER_NAME_EXTEND.ToLower();
    //总配置表的名字
    public const string ALL_CONFIG_NAME = "allConfig";
    //proto协议后缀名
    private const string PROTO_SUFFIX = ".proto";
    //二进制文件后缀名
    private const string BINARY_SUFFIX = ".bytes";
    //枚举tag
    private const string ENUM_TAG = "enum";

    //C#脚本输出目录 选项
    private const string OPTION_CSHARP_OUT_DIR = "--csharp_out";
    //Java脚本输出目录 选项
    private const string OPTION_JAVA_OUT_DIR = "--java_out";

    //是否生成c#文件
    private bool _isOutputCSharp = false;
    //当前的tag
    private string _tag;
    //枚举类型字典
    private Dictionary<string, List<string>> _enumDict = new Dictionary<string, List<string>>();

    public void Gen(ExcelData data, string protoPath, string outputPath, string binaryPath, string package)
    {
        ClearProto(protoPath);
        GenEnumProto(data, protoPath, package);
        CheckTag(data, outputPath);
        for (int i = 0; i < data.tableList.Count; ++i)
        {
            TableData table = data.tableList[i];
            string code = GenMessageProto(table, protoPath, outputPath, package);
            byte[] bytes = Encoding.UTF8.GetBytes(code);
            string fileName = Path.Combine(protoPath, table.tableName + PROTO_SUFFIX);
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        string[] temp = outputPath.Split('=');
        string type = temp[0];
        string path = temp[1];

        GenAllConfigProto(data, protoPath, package);
        GenScript(protoPath, type, path);
        GenBinary(data, path, binaryPath, GetNamespaceByPackage(package));
        GenDataManager(data, path, package);
        ClearTemp(path);
    }

    //清理proto文件
    private void ClearProto(string protoPath)
    {
        string[] protos = Directory.GetFiles(protoPath, "*.proto");
        for (int i = 0; i < protos.Length; ++i)
            File.Delete(protos[i]);
    }

    //生成Message .proto文件
    private string GenMessageProto(TableData table, string protoPath, string outputPath, string package)
    {
        //获取id索引的类型
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("syntax = \"proto3\";");
        sb.AppendLine();
        //包名
        if (!string.IsNullOrEmpty(package))
        {
            sb.AppendLine("package " + package + ";");
            sb.AppendLine();
        }

        //枚举需要插入的位置
        int enumIndex = sb.Length;
      
        //Container
        sb.AppendLine(string.Format(@"message {0} {1}", table.tableName + CONTAINER_NAME_EXTEND, "{"));
        sb.AppendLine(string.Format("\tmap<{0},{1}> {2} = 1;", table.idTypeName, table.tableName, CONTAINER_VARIABLE_NAME));
        sb.AppendLine("}");
        sb.AppendLine();

        //message
        sb.AppendLine(string.Format(@"message {0} {1}", table.tableName, "{"));

        //引用的枚举类型列表
        List<string> listEnum = new List<string>();
        int index = 1;
        for (int i = 0; i < table.tableDeclare.Count; ++i)
        {
            TableData.VariableDeclare declare = table.tableDeclare[i];

            //判定字段
            if (string.IsNullOrEmpty(declare.tag) 
                || !declare.tag.Contains(_tag)
                || string.IsNullOrEmpty(declare.type)
                || string.IsNullOrEmpty(declare.name)) continue;

            sb.AppendLine(string.Format("\t{0} {1} = {2};", declare.type, declare.name, index++));
            foreach (KeyValuePair<string, List<string>> each in _enumDict)
            {
                foreach (string item in each.Value)
                {
                    if (declare.type.Equals(item) && !listEnum.Contains(each.Key))
                    {
                        listEnum.Add(each.Key);
                        break;
                    }
                }
            }
        }

        //引用枚举
        for (int i = 0; i < listEnum.Count; ++i)
        {
            string import = string.Format("import \"{0}{1}\";\r\n", listEnum[i], ProtobufGen.PROTO_SUFFIX);
            sb.Insert(enumIndex, import);
            index += import.Length;
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    //生成枚举 .proto文件
    private void GenEnumProto(ExcelData excelData, string protoPath, string package)
    {
        //筛选出所有枚举类型的表
        List<TableData> _enumTableList = new List<TableData>();
        for (int i = excelData.tableList.Count - 1; i >= 0; --i)
        {
            TableData table = excelData.tableList[i];
            string tag = table.tableDeclare.Count > 0 ? table.tableDeclare[0].tag : string.Empty;
            if (string.IsNullOrEmpty(tag) || !tag.Equals(ENUM_TAG)) continue;

            _enumTableList.Add(table);
            excelData.tableList.RemoveAt(i);
        }

        if (_enumTableList.Count == 0) return;

        for (int i = 0; i < _enumTableList.Count; ++i)
        {
            TableData table = _enumTableList[i];

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("syntax = \"proto3\";");
            sb.AppendLine();
            //包名
            if (!string.IsNullOrEmpty(package))
            {
                sb.AppendLine("package " + package + ";");
                sb.AppendLine();
            }
            for (int j = 0; j < table.rowDatas.Count; ++j)
            {
                FieldData[] fields = table.rowDatas[j];
                string enumName = fields[0].data;
                sb.AppendLine(string.Format("enum {0} {1}", enumName, "{"));
                for (int k = 1; k < fields.Length; ++k)
                {
                    string field = fields[k].data;
                    if (string.IsNullOrEmpty(field)) continue;
                    sb.AppendLine(string.Format("\t{0}_{1} = {2};"
                        , enumName, field, k - 1));
                }
                sb.AppendLine("}");

                List<string> list = null;
                if (!_enumDict.TryGetValue(table.tableName, out list))
                {
                    list = new List<string>();
                    _enumDict.Add(table.tableName, list);
                }
                list.Add(enumName);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            string fileName = Path.Combine(protoPath, table.tableName + PROTO_SUFFIX);
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }

    //生成一个汇总的proto文件
    private void GenAllConfigProto(ExcelData excelData, string protoPath, string package)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("syntax = \"proto3\";");
        sb.AppendLine();

        //包名
        if (!string.IsNullOrEmpty(package))
        {
            sb.AppendLine("package " + package + ";");
            sb.AppendLine();
        }

        //引用
        for (int i = 0; i < excelData.tableList.Count; ++i)
        {
            TableData table = excelData.tableList[i];
            sb.AppendLine(string.Format("import \"{0}{1}\";", table.tableName, PROTO_SUFFIX));
        }
        sb.AppendLine();
        //message
        sb.AppendLine(string.Format(@"message {0} {1}", ALL_CONFIG_NAME, "{"));
        int index = 1;
        for (int i = 0; i < excelData.tableList.Count; ++i)
        {
            TableData table = excelData.tableList[i];
            sb.AppendLine(string.Format("\t{0} {1} = {2};"
                , table.tableName + CONTAINER_NAME_EXTEND
                , table.tableName.Substring(2) + CONTAINER_NAME_EXTEND
                , index++));
        }
        sb.AppendLine("}");

        string path = Path.Combine(protoPath, ALL_CONFIG_NAME + PROTO_SUFFIX);
        FileStream file = File.Create(path);
        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        file.Write(bytes, 0, bytes.Length);
        file.Close();
    }

    //生成脚本
    private void GenScript(string protoPath, string type, string path)
    {
        //清理文件
        string[] files = Directory.GetFiles(path, "*.cs");
        for (int i = 0; i < files.Length; ++i)
            File.Delete(files[i]);
        ProcessStartInfo pStartInfo = new ProcessStartInfo("protoc.exe")//设置进程
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardError = false,
            RedirectStandardInput = false,
            WorkingDirectory = Directory.GetCurrentDirectory(),
        };

        string format = @"{0} -I={1} {2}={3}";
        files = Directory.GetFiles(protoPath, "*.proto");
        for (int i = 0; i < files.Length; ++i)
        {
            string file = files[i];
            string proto = Path.GetFileName(file);
            pStartInfo.Arguments = string.Format(format, proto, protoPath, type, path);
            //开始进程
            Process process = Process.Start(pStartInfo);
            while (!process.HasExited)
            {
                Thread.Sleep(10);
            }
        }

        _isOutputCSharp = type.Equals(OPTION_CSHARP_OUT_DIR);
        //如果不是生成的c#脚本. 则需要再生成一次c#. 用于序列化二进制文件
        if (!_isOutputCSharp)
        {
            for (int i = 0; i < files.Length; ++i)
            {
                string file = files[i];
                string proto = Path.GetFileName(file);
                pStartInfo.Arguments = string.Format(format, proto, protoPath, OPTION_CSHARP_OUT_DIR, path);
                //开始进程
                Process process = Process.Start(pStartInfo);
                while (!process.HasExited)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }

    //生成二进制文件
    private void GenBinary(ExcelData excelData, string outputPath, string binaryPath, string nsp)
    {
        Assembly assembly = GetAssembly(outputPath);
        if (assembly == null) throw new Exception("assembly is null");

        foreach (TableData tableData in excelData.tableList)
        {
            //创建配置对象容器
            string containerName = nsp + tableData.tableName + CONTAINER_NAME_EXTEND;
            Type containerType = assembly.GetType(containerName);
            object container = Activator.CreateInstance(containerType);

            try
            {
                //创建数据对象,并添加入容器
                Type configType = assembly.GetType(nsp + tableData.tableName);
                for (int i = 0; i < tableData.rowDatas.Count; ++i)
                {
                    //每行的数据
                    FieldData[] datas = tableData.rowDatas[i];
                    object config = Activator.CreateInstance(configType);
                    object id = null;
                    for (int j = 0; j < datas.Length; ++j)
                    {
                        FieldData data = datas[j];
                        //首字母大写
                        FieldInfo field = configType.GetField(GetFieldName(data.name), BindingFlags.Instance | BindingFlags.NonPublic);
                        if (field == null) continue;

                        object value = GetFieldData(data, assembly, nsp);
                        if (data.name.Equals("id")) id = value;
                        field.SetValue(config, value);
                    }
                    //添加进入容器
                    FieldInfo fieldContainer = containerType.GetField(CONTAINER_VARIABLE_NAME + "_", BindingFlags.Instance | BindingFlags.NonPublic);
                    object map = fieldContainer.GetValue(container);
                    fieldContainer.FieldType.GetMethod("Add", new Type[] { id.GetType(), configType }).Invoke(map, new object[] { id, config });
                }

                //序列化成二进制文件
                string path = Path.Combine(binaryPath, tableData.tableName + BINARY_SUFFIX);
                FileStream file = File.Create(path);
                using (CodedOutputStream cos = new CodedOutputStream(file))
                {
                    containerType.GetMethod("WriteTo").Invoke(container, new object[] { cos });
                    cos.Flush();
                }
                file.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e + "  in " + tableData.tableName);
            }
        }
    }

    //获取字段名,根据Protobuf的规则
    //protobuf生成的字段名是没有下划线的
    private string GetFieldName(string name)
    {
        if (string.IsNullOrEmpty(name)) return string.Empty;
        string[] splitName = name.Split('_');
        string fieldName = splitName[0];
        for (int i = 1; i < splitName.Length; i++)
        {
            fieldName += Util.InitialToUpper(splitName[i]);
        }
        fieldName += "_";
        return fieldName;
    }

    //获取每个类型的值
    private object GetFieldData(FieldData data, Assembly assembly, string namespaceName)
    {
        try
        {
            switch (data.type)
            {
                case SupportType.INT: return int.Parse(data.data);
                case SupportType.FLOAT: return float.Parse(data.data);
                case SupportType.STRING: return string.IsNullOrEmpty(data.data) ?  string.Empty : data.data;
                case SupportType.BOOLEAN: return bool.Parse(data.data);
                case SupportType.LIST_INT:
                    RepeatedField<int> repeatedInt = new RepeatedField<int>();
                    string[] intStr = data.data.Split(';');
                    for (int i = 0; i < intStr.Length; ++i)
                        repeatedInt.Add(int.Parse(intStr[i]));
                    return repeatedInt;
                case SupportType.LIST_FLOAT:
                    RepeatedField<float> repeatedFloat = new RepeatedField<float>();
                    string[] floatStr = data.data.Split(';');
                    for (int i = 0; i < floatStr.Length; ++i)
                        repeatedFloat.Add(float.Parse(floatStr[i]));
                    return repeatedFloat;
                case SupportType.LIST_STRING:
                    RepeatedField<string> repeatedString = new RepeatedField<string>();
                    repeatedString.AddRange(data.data.Split(';'));
                    return repeatedString;
                default:
                    string typeName = namespaceName + data.type;
                    Type enumType = assembly.GetType(typeName);
                    if (enumType != null) return Enum.Parse(enumType, data.data);
                    throw new Exception("不匹配的类型 : " + typeName);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    private Assembly GetAssembly(string outputPath)
    {
        //直接动态编译生成的proto脚本
        CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
        CompilerParameters objCompilerParameters = new CompilerParameters();
        objCompilerParameters.GenerateInMemory = true;
        objCompilerParameters.GenerateExecutable = false;
        objCompilerParameters.ReferencedAssemblies.Add("System.dll");
        objCompilerParameters.ReferencedAssemblies.Add("Google.Protobuf.dll");
        List<string> srcList = GetSources(outputPath);
        CompilerResults result = objCSharpCodePrivoder.CompileAssemblyFromFile(objCompilerParameters, srcList.ToArray());
        //编译错误
        if (result.Errors.HasErrors)
        {
            for (int i = 0; i < result.Errors.Count; ++i)
                Console.WriteLine(result.Errors[i]);
            return null;
        }
        else
            return result.CompiledAssembly;
    }

    //生成数据管理器
    private void GenDataManager(ExcelData excelData, string outputPath, string package)
    {
        if (_isOutputCSharp)
            CSharpGenerator.GenDataManager(excelData, outputPath);
        else if (!string.IsNullOrEmpty(package))
            JavaGenerator.GenDataManager(excelData, outputPath, package);
    }

    //清理临时文件
    private void ClearTemp(string dir)
    {
        if (_isOutputCSharp) return;
        string[] files = Directory.GetFiles(dir, "*.cs");
        for (int i = 0; i < files.Length; ++i)
            File.Delete(files[i]);
    }

    /// <summary>
    /// 获取所有热更源码
    /// </summary>
    /// <returns></returns>
    private static List<string> GetSources(string dir)
    {
        List<string> list = new List<string>();
        list.AddRange(Directory.GetFiles(dir, "*.cs"));
        string[] directories = Directory.GetDirectories(dir);
        for (int i = 0; i < directories.Length; ++i)
            list.AddRange(GetSources(directories[i]));
        return list;
    }

    private string GetNamespaceByPackage(string package)
    {
        if (string.IsNullOrEmpty(package)) return null;
        string nsp = package.Substring(0, 1).ToUpper() + package.Substring(1);
        int index = nsp.IndexOf('.');
        while (index > 0)
        {
            index += 1;
            string c = nsp.Substring(index, 1).ToUpper();
            nsp = nsp.Remove(index, 1);
            nsp = nsp.Insert(index, c);
            index = nsp.IndexOf('.', index);
        }
        return nsp + ".";
    }

    //检测是否包含tag,全不包含的直接移除
    private void CheckTag(ExcelData excelData, string outputPath)
    {
        if (outputPath.Contains(OPTION_CSHARP_OUT_DIR)) _tag = "c#";
        else if (outputPath.Contains(OPTION_JAVA_OUT_DIR)) _tag = "java";

        for (int i = excelData.tableList.Count - 1; i >= 0; --i)
        {
            TableData table = excelData.tableList[i];
            bool isExist = false;
            for (int j = 0; j < table.tableDeclare.Count; ++j)
            {
                VariableDeclare declare = table.tableDeclare[j];
                if (!string.IsNullOrEmpty(declare.tag) && declare.tag.Contains(_tag))
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist) excelData.tableList.RemoveAt(i);
        }
    }
}
