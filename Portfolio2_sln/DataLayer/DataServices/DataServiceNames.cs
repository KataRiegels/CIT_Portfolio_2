using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServices
{
    public class DataServiceNames : IDataServiceNames
    {
        private static ImdbContext _db = new ImdbContext();






        public NameBasics GetName(string nconst)
        {
            var temp = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);
            return temp;
        }

        public IList<NameBasics> GetNames(int page = 0, int pageSize = 20)
        {
            return _db.NameBasicss.Skip(page * pageSize).Take(pageSize).ToList();
        }




        //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------


        public IList<BasicNameModelDL> GetBasicNames(int page = 0, int pageSize = 20)
        {
            var basicnames = _db.NameBasicss
                .Select(n => new BasicNameModelDL
                {
                    Nconst = n.Nconst,
                    PrimaryName = n.PrimaryName
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicnames;
        }

        public BasicNameModelDL GetBasicName(string nconst)
        {
            if (string.IsNullOrEmpty(nconst))
            {
                return new BasicNameModelDL();
            }

            var namebasic = _db.NameBasicss.FirstOrDefault(x => x.Nconst.Trim() == nconst.Trim());
            Console.WriteLine(namebasic.Nconst);
            var basicname = new BasicNameModelDL
            {
                Nconst = nconst,
                PrimaryName = namebasic.PrimaryName
            };

            return basicname;
        }



        //public IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20)
        //{
        //    var names = GetDetailedNames()
        //        .Select(x => new ListNameModelDL()
        //        {
        //            Nconst = x.Nconst,
        //            PrimaryName = x.PrimaryName,
        //            //Profession = x.Profession,
        //            KnownForTitle = x.KnownForTitle,
        //            StartYear = x.StartYear,
        //            TitleType = x.TitleType,
        //            Tconst = x.Tconst
        //        }).Skip(page * pageSize).Take(pageSize).ToList();

        //    return names;
        //}


        //----------------------------------------------------------------------------------------------
        //         NAME HELPERS
        //----------------------------------------------------------------------------------------------












        //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------





        public IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20)
        {
            Console.WriteLine("before KnownFor");
            var query =
                _db.NameBasicss.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFors) =>
                       new
                       {
                           //basics,
                           Nconst = basics.Nconst,
                           PrimaryName = basics.PrimaryName,
                           //PrimaryName = basics.PrimaryName,
                           KnownForTitle = knownFors.Select(x => x.Tconst)
                       }
                           )
                        ;

            var names2 = query.ToList()
                .GroupBy(t => t.Nconst, (key, model) => new ListNameModelDL
                {
                    BasicName = new BasicNameModelDL
                    {
                        Nconst = key,
                        PrimaryName = model.First().PrimaryName,
                    },

                    KnownForTitleBasics = model.First().KnownForTitle.Any() ? new DataServiceTitles().GetBasicTitle(model.First().KnownForTitle.First()) : null
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();

            return names2;




            //    var names = _db.FullViewNames

            //        .ToList()
            //        //.GroupBy(t => t.Tconst,t => t.genre, (key, genre) => new DetailedTitleModelDL
            //        .GroupBy(t => t.Nconst, (key, model) => new ListNameModelDL
            //        {
            //            Nconst = key,
            //            PrimaryName = model.First().PrimaryName,
            //            KnownForTitleBasics = model.First().KnwonForTconst != null ? GetBasicTitle(model.First().KnwonForTconst) : null
            //            //KnownForTitle = model.First().KnwonForTconst,
            //            //TitleType = model.First().KnwonForTconst != null ? GetTitle(model.First().KnwonForTconst).TitleType : null
            //            //TitleType = model.First().KnwonForTconst != null ? model.First().KnwonForTconst : null
            //            //StartYear = model.First().KnwonForTconst,

            //            //DeathYear = model.First().DeathYear,
            //            //Professions = model.Select(p => p.Profession).Distinct().ToList(),
            //            //KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
            //            //Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
            //            //Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
            //            //plot = model.First().plot,
            //            //poster = model.First().poster,
            //            ////Tconst = obj.Tconst,
            //            //genre = model.Select(m => m.genre).Distinct().ToList()
            //        }
            //        ).Take(21).ToList();
            //return names;



            //return null;
        }


        public IList<DetailedNameModelDL>? GetDetailedNames(int page = 0, int pageSize = 20)
        {
            var names = _db.FullViewNames

                .ToList()
                .GroupBy(t => t.Nconst, (key, model) => new DetailedNameModelDL
                {
                    Nconst = key,
                    PrimaryName = model.First().PrimaryName,
                    BirthYear = model.First().BirthYear,
                    DeathYear = model.First().DeathYear,
                    Professions = model.Select(p => p.Profession).Distinct().ToList(),
                    KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
                    Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
                    Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
                    //plot = model.First().plot,
                    //poster = model.First().poster,
                    ////Tconst = obj.Tconst,
                    //genre = model.Select(m => m.genre).Distinct().ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();
            return names;
        }






















    }
}
