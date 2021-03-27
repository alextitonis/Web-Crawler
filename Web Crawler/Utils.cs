using System;
using System.Collections.Generic;
using System.Text;
using static Web_Crawler.Crawler;

namespace Web_Crawler
{
    public class Utils
    {
        public static Dictionary<string, ObjectType> stringToDictionary(string str)
        {
            Dictionary<string, ObjectType> res = new Dictionary<string, ObjectType>();

            string[] data = str.Split('|');
            for (int i = 0; i < data.Length; i++)
            {
                string[] _data = data[i].Split(';');
                if (_data.Length != 2)
                    continue;
            
                string key = _data[0];
                ObjectType value = (ObjectType)int.Parse(_data[1]);
                res.Add(key, value);
            }

            return res;
        }
    }
}
