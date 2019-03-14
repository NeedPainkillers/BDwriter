using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WindowsFormsApp1
{
    static public class FileOperation
    {
        public static bool IsOpen(string fileName)
        {
            if (File.Exists(fileName + "Locked"))
                return true;
            return false;
        }
        public static void CreateLockMarker(string fileName)
        { 
            File.Create(fileName + "Locked").Close();
        }
        public static void DeleteLockMarker(string fileName)
        {
            File.Delete(fileName + "Locked");
        }
        public static List<CNews> ReadFromFile(string fileName)
        {
            List<CNews> item = new List<CNews>();
            string serializedWrittenPosts = File.ReadAllText(fileName);
            item = JsonConvert.DeserializeObject<List<CNews>>(serializedWrittenPosts);
            return item;
        }
    }
    

    

    public class CNews
    {
        public string id;
        public string text;
        public List<string> link;
        public List<string> imageLink;
        public CNews()
        {
            link = new List<string>();
            imageLink = new List<string>();
        }      
    }
    class cNewsEqualityComparer : IEqualityComparer<CNews>
    {
        public bool Equals(CNews b1, CNews b2)
        {
            return b1.id.Equals(b2.id);
        }

        public int GetHashCode(CNews bx)
        {
            int hCode = bx.id.GetHashCode();
            return hCode.GetHashCode();
        }
    }


}
