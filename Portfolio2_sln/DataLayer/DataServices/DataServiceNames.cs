using DataLayer.DataTransferObjects;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using DataLayer.DataServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;

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


        public IList<ListNameModelDL> GetFilteredNames(List<NconstObject> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();


            Console.WriteLine("before join");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Filters names so only contain those that matched the search
            var filtered = db.NameBasicss.ToList()
                .Join(searchedNames,
                    fullView => fullView.Nconst,
                    searchResults => searchResults.Nconst,
                    (fullView, searchResults)
                                  => fullView
                    );


            stopwatch.Stop();
            var elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("joining name basics with searched list: ms: " + elapsed_time);

            Console.WriteLine("after filtered");

            stopwatch.Start();

            // Joins the filtered name_basics with known_for to get list form of matching names
            var searchedTitleResults =
                filtered.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) => new ListNameModelDL
                       {
                           BasicName =
                               new DataServiceNames().
                                    GetBasicName(basics.Nconst),
                           KnownForTitleBasics = knownFor.Any() ?
                                    new DataServiceTitles().
                                    GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                       }
                    )
                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            stopwatch.Stop();
            elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(elapsed_time);

            Console.WriteLine("after join");

            return searchedTitleResults;
        }


        public IList<NameTitleRelationDTO> GetNameTitleRelations(string nconst)
        {
            using var db = new ImdbContext();
            Console.WriteLine(nconst);
            var jobsJoin = db.Jobs
                 .Where(n => n.Nconst.Trim().Contains(nconst.Trim()))
                 .Join(db.TitleBasicss,
                     job => job.Tconst,
                     title => title.Tconst,
                     (job, title) => new NameTitleRelationDTO
                     {
                         Nconst = nconst,
                         Relation = job.JobName,
                         Title = new BasicTitleModelDL
                         {
                             Tconst = title.Tconst,
                             TitleType = title.TitleType,
                             PrimaryTitle = title.PrimaryTitle,
                             StartYear = title.StartYear,
                         }
                        }
                     )
                 .ToList();

            Console.WriteLine(jobsJoin.Count());

            var characterJoin = db.Characters 
              //.Where(t => t.)
                 .Where(n => n.Nconst.Trim() == nconst.Trim())
              .Join(db.TitleBasicss,
                  character => character.Tconst,
                  title => title.Tconst,
                  (character, title) => new NameTitleRelationDTO
                  {
                      Nconst = nconst,
                      Relation = character.CharacterName,
                      Title = new BasicTitleModelDL
                      {
                          Tconst = title.Tconst,
                          TitleType = title.TitleType,
                          PrimaryTitle = title.PrimaryTitle,
                          StartYear = title.StartYear,
                      }
                  }

                  )
              .ToList();
            return jobsJoin.Concat(characterJoin).ToList();

        }









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

        public DetailedNameModelDL GetDetailedName(string nconst)
        {
            var names = _db.FullViewNames
                .Where(n => n.Nconst.Trim() == nconst.Trim())
                .ToList()
                .GroupBy(t => t.Nconst, (key, model) => new DetailedNameModelDL
                {
                    Nconst = key,
                    PrimaryName = model.First().PrimaryName,
                    BirthYear = model.First().BirthYear,
                    DeathYear = model.First().DeathYear,
                    //Professions = model.Select(p => p.Profession).Distinct().ToList(),
                    //KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
                    //Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
                    //Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
                    //plot = model.First().plot,
                    //poster = model.First().poster,
                    ////Tconst = obj.Tconst,
                    //genre = model.Select(m => m.genre).Distinct().ToList()
                }
                ).FirstOrDefault();

            return names;
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
                    //Professions = model.Select(p => p.Profession).Distinct().ToList(),
                    //KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
                    //Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
                    //Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
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
