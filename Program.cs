using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Reflection;

namespace ConfigTool
{
    class Program
    {
        //proto文件输出目录 选项
        private const string OPTION_PROTO_PATH = "--proto_path";
        //excel文件目录 选项
        private const string OPTION_EXCEL_PATH = "--excel_path";
        //数据二进制文件目录 选项
        private const string OPTION_BINARY_PATH = "--binary_path";
        //包名
        private const string OPTION_PACHAGE = "--package";

        static void Main(string[] args)
        {
            string excelPath = null;
            string protoPath = null;
            string outputPath = null;
            string binaryPath = null;
            string package = null;

            for (int i = 0; i < args.Length; ++i)
            {
                string[] option = args[i].Split('=');
                if (option.Length == 2)
                {
                    switch (option[0])
                    {
                        case OPTION_PROTO_PATH:protoPath = option[1];break;
                        case OPTION_EXCEL_PATH: excelPath = option[1]; break;
                        case OPTION_BINARY_PATH: binaryPath = option[1]; break;
                        case OPTION_PACHAGE: package = option[1]; break;
                        default:
                            outputPath = args[i];
                            break;
                    }
                }
            }

            try
            {
                ExcelData data = ExcelReader.ReadAll(excelPath);
                ProtobufGen.inst.Gen(data, protoPath , outputPath, binaryPath, package);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Console.Read();
            }
            Console.WriteLine("ok");
            Console.Read();
        }
    }
}
