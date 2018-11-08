using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Util
{
    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <returns></returns>
    public static string InitialToUpper(string str)
    {
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }
}
