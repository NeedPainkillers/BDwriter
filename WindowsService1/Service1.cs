using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Data.SqlClient;


namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        Database1Entities db;
        private static string connectionLocation = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\apcxo\source\repos\WindowsService1\WindowsService1\Database1.mdf;Integrated Security=True";
        private string folderPath = "C:\\Users\\apcxo\\source\\repos\\WindowsFormsApp1\\WindowsFormsApp1\\bin\\Debug\\";
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "logService.txt", "Service is started at" + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000;
            db = new Database1Entities();
            db.Database.Connection.ConnectionString = connectionLocation;
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            db.Database.Connection.Close();
        }

        private List<WindowsFormsApp1.CNews> FilterPosts(List<WindowsFormsApp1.CNews> data, List<PostText> writtenData)
        {
            return data
                    .Except(writtenData
                    .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();
        }
        private List<WindowsFormsApp1.CNews> FilterPosts(List<WindowsFormsApp1.CNews> data, List<PostLink> writtenData)
        {
            return data
                    .Except(writtenData
                    .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();
        }
        private List<WindowsFormsApp1.CNews> FilterPosts(List<WindowsFormsApp1.CNews> data, List<PostImgLink> writtenData)
        {
            return data
                    .Except(writtenData
                    .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            var test = AppDomain.CurrentDomain.BaseDirectory;
            string[] fileNames = new string[3] {folderPath + "JStext.txt", folderPath + "JSlink.txt", folderPath + "JSimageLink.txt" };
            List<WindowsFormsApp1.CNews>[] readedJS = new List<WindowsFormsApp1.CNews>[3];

            readedJS[0] = new List<WindowsFormsApp1.CNews>();
            readedJS[1] = new List<WindowsFormsApp1.CNews>();
            readedJS[2] = new List<WindowsFormsApp1.CNews>();

            for (int i = 0; i < 3; i++)
            {
                var r = WindowsFormsApp1.FileOperation.IsOpen(fileNames[i]);
                if (r)
                    continue;
                WindowsFormsApp1.FileOperation.CreateLockMarker(fileNames[i]);
                
                readedJS[i] = WindowsFormsApp1.FileOperation.ReadFromFile(fileNames[i]);

                WindowsFormsApp1.FileOperation.DeleteLockMarker(fileNames[i]);
            }
            db.PostTexts.ToList();

            //List<PostText> toBeAddedPostsT;
            db.Database.Connection.Open();
            if (readedJS[0].Any())
            {
                //toBeAddedPostsT = new List<PostText>();
                //toBeAddedPostsT = readedJS[0].Select(x => new PostText() { id_post = x.id, text = x.text }).ToList();
                //db.PostTexts.Union(toBeAddedPostsT, new PostTextEqualityComparer());


                //var qr = toBeAddedPostsT.Union(db.PostTexts.ToList())/*, new PostImgLinkEqualityComparer()*//*).Distinct(new PostImgLinkEqualityComparer()).ToList()*/;

                //var distinctTest = (from item in qr
                //                    group item by item.id_post into grouped
                //                    where grouped.Count() == 1
                //                    select new PostText { id_post = grouped.Key, text = grouped.ToList()[0].text }).ToList();

                //db.PostTexts.AddRange(distinctTest);
                //var toAdd = readedJS[0].Except(db.PostTexts.ToList()
                //    .Select(x => new WindowsFormsApp1.cNews() { id = x.idPost })
                //    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();

                var addPostTexts = (from item in readedJS[0].Except(db.PostTexts
                                                            .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                                                            , new WindowsFormsApp1.cNewsEqualityComparer())
                                    select new PostText { idPost = item.id, text = item.text }).ToList();

                db.PostTexts.AddRange(addPostTexts);

            } 

            if (readedJS[1].Any())
            {
                //var qe = readedJS[1].Union(db.PostLinks.ToList().Select(x => new WindowsFormsApp1.cNews() { id = x.idPost }).ToList());
                //var GroupedId1 = (from item in qe
                //                 group item by item.id into grouped
                //                 where grouped.Count() == 1
                //                 from links in grouped
                //                 from link in links.link
                //                 select new PostLink { idPost = grouped.Key, Link = link }).ToList();
                //db.PostLinks.AddRange(GroupedId1); 
                //var toAdd = readedJS[1]
                //    .Except(db.PostLinks.ToList()
                //    .Select(x => new WindowsFormsApp1.cNews() { id = x.idPost })
                //    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();

                var PostLinks = (from item in readedJS[1].Except(db.PostLinks
                                                         .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                                                         , new WindowsFormsApp1.cNewsEqualityComparer()).ToList()
                                 from link in item.link
                                 select new PostLink { idPost = item.id, Link = link }).ToList();

                db.PostLinks.AddRange(PostLinks);
            }

            if (readedJS[2].Any())
            {
                //var qe = readedJS[2].Union(db.PostImgLinks.ToList().Select(x => new WindowsFormsApp1.cNews() { id = x.idPost }).ToList());
                //var GroupedId1 = (from item in qe
                //                  group item by item.id into grouped
                //                  where grouped.Count() == 1
                //                  from imgLinks in grouped
                //                  from imgLink in imgLinks.imageLink
                //                  select new PostImgLink { idPost = grouped.Key, imgLink = imgLink }).ToList(); //победа

                //db.PostImgLinks.AddRange(GroupedId1);
                //List<WindowsFormsApp1.cNews> toAdd = readedJS[2]
                //    .Except(db.PostImgLinks.ToList()
                //    .Select(x => new WindowsFormsApp1.cNews() { id = x.idPost })
                //    , new WindowsFormsApp1.cNewsEqualityComparer()).ToList();

                List<PostImgLink> PostImgLinks = (from item in readedJS[2].Except(db.PostImgLinks
                                                         .Select(x => new WindowsFormsApp1.CNews() { id = x.idPost })
                                                         , new WindowsFormsApp1.cNewsEqualityComparer()).ToList()
                                                  from imgLink in item.imageLink
                                                  select new PostImgLink { idPost = item.id, imgLink = imgLink }).ToList();

                db.PostImgLinks.AddRange(PostImgLinks);
            }

            db.SaveChanges();
            db.Database.Connection.Close();


        }
    }
}
