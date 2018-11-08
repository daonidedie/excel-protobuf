﻿//#author:qiuyukun

using Excel;
using System;
using System.Collections.Generic;
using System.IO;
using static ExcelData;
using static ExcelData.TableData;

/// <summary>
/// 配置表数据内容
/// </summary>
public class FieldData
{
    /// <summary>
    /// 变量类型
    /// </summary>
    public string Type;
    /// <summary>
    /// 变量名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 数据内容
    /// </summary>
    public string Data;
}

/// <summary>
/// excel数据,包含所有表的数据
/// </summary>
public class ExcelData
{
    /// <summary>
    /// 表的数据内容
    /// </summary>
    public class TableData
    {
        /// <summary>
        /// 变量声明
        /// </summary>
        public class VariableDeclare
        {
            //类型
            public string type;
            //名字
            public string name;
            //导出标签
            public string tag;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName;

        /// <summary>
        /// id索引类型名称
        /// </summary>
        public string idTypeName;

        /// <summary>
        /// 类的声明数据
        /// </summary>
        public List<VariableDeclare> tableDeclare = new List<VariableDeclare>();

        /// <summary>
        /// 行数据
        /// </summary>
        public List<FieldData[]> rowDatas = new List<FieldData[]>();
    }

    /// <summary>
    /// 所有表的数据
    /// </summary>
    public List<TableData> tableList = new List<TableData>();
}

/// <summary>
/// 支持类型
/// </summary>
public class SupportType
{
    public const string INT = "int32";
    public const string FLOAT = "float";
    public const string STRING = "string";
    public const string LIST_INT = "repeated int32";
    public const string LIST_FLOAT = "repeated float";
    public const string LIST_STRING = "repeated string";
}

/// <summary>
/// Excel读取类
/// </summary>
public static class ExcelReader
{
    private const int EXPORT_TAG_ROW = 3; //导出类型行数
    private const int FIELD_TYPE_ROW = 4; //类型行数
    private const int FIELD_NAME_ROW = 5; //名字行数 

    /// <summary>
    /// 读取路径下所有Excel文件
    /// </summary>
    /// <param name="excelPath">路径</param>
    /// <returns></returns>
    public static ExcelData ReadAll(string excelPath)
    {
        ExcelData excelData = new ExcelData();
        string[] files = Directory.GetFiles(excelPath, "*.xlsx");
        for (int i = 0; i < files.Length; ++i)
        {
            //打开excel
            excelData.tableList.Add(ReadTable(files[i]));
        }
        return excelData;
    }

    /// <summary>
    /// 读取excel表
    /// </summary>
    /// <param name="tablePath">表路径</param>
    /// <returns></returns>
    private static TableData ReadTable(string tablePath)
    {
        try
        {
            TableData tableData = new TableData();
            //打开文件
            FileStream stream = File.Open(tablePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            if (!excelReader.IsValid) throw new Exception("无法打开文件");

            //读取每行数据
            Dictionary<int, string[]> dataDict = ReadRowData(excelReader);
            //获取变量声明
            tableData.tableDeclare = GetVariableDeclare(dataDict[EXPORT_TAG_ROW], dataDict[FIELD_TYPE_ROW], dataDict[FIELD_NAME_ROW]);
            tableData.tableName = excelReader.Name;
            tableData.idTypeName = GetIdTypeName(tableData.tableDeclare);

            foreach (KeyValuePair<int, string[]> item in dataDict)
            {
                if (item.Key <= FIELD_NAME_ROW) continue;

                //整合每一行的数据
                string[] values = item.Value;
                FieldData[] datas = new FieldData[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    VariableDeclare declare = tableData.tableDeclare[i];
                    FieldData data = new FieldData()
                    {
                        Type = declare.type,
                        Name = declare.name,
                        Data = values[i],
                    };
                    datas[i] = data;
                }
                tableData.rowDatas.Add(datas);
            }
            excelReader.Close();
            return tableData;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message + "  " + tablePath);
        }
    }

    /// <summary>
    /// 获取变量声明
    /// </summary>
    /// <returns></returns>
    private static List<VariableDeclare> GetVariableDeclare(string[] tags, string[] types, string[] names)
    {
        if (types.Length != names.Length)
            throw new Exception("类型和变量名列数不匹配");
        List<VariableDeclare> list = new List<VariableDeclare>();
        for (int i = 0; i < types.Length; ++i)
        {
            VariableDeclare declare = new VariableDeclare()
            {
                type = types[i],
                name = names[i],
                tag = tags[i],
            };
            list.Add(declare);
        }
        return list;
    }

    private static string GetIdTypeName(List<VariableDeclare> declareList)
    {
        for (int i = 0; i < declareList.Count; ++i)
        {
            VariableDeclare declare = declareList[i];
            if (declare.name.Equals(ProtobufGen.ID_NAME))
                return declare.type;
        }
        return null;
    }

    /// <summary>
    /// 读取表中每一行的数据
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    private static Dictionary<int, string[]> ReadRowData(IExcelDataReader reader)
    {
        //数据字典,储存每一行的数据
        Dictionary<int, string[]> dataDict = new Dictionary<int, string[]>();

        int index = 0;
        //开始读取
        while (reader.Read())
        {
            ++index;
            //行数据
            string[] rowDatas = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                rowDatas[i] = reader.GetString(i);
            }

            //不读取空行
            if (IsNullRow(rowDatas)) continue;
            dataDict.Add(index, rowDatas);
        }
        return dataDict;
    }

    /// <summary>
    /// 是否为空行
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private static bool IsNullRow(string[] row)
    {
        for (int i = 0; i < row.Length; ++i)
            if (!string.IsNullOrEmpty(row[i])) return false;
        return true;
    }
}