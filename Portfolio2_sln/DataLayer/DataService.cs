﻿using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DomainModels.NameModels;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DomainModels.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        public IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20)
        {
            var result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();
            result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();

            return result;
        }

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);

            return temp;
        }

        public BasicTitleDTO GetBasicTitle(string tconst)
        {
            using var db = new ImdbContext();
            //var basicTitle = _db.TitleBasicss
            var basicTitle = db.TitleBasicss
                .FirstOrDefault(x => x.Tconst.Trim() == tconst.Trim());
            var basic = new BasicTitleDTO
            {
                Tconst = tconst,
                TitleType = basicTitle.TitleType,
                PrimaryTitle = basicTitle.PrimaryTitle,
                StartYear = basicTitle.StartYear,
            };

            return basic;
        }

        public IList<BasicTitleDTO> GetBasicTitles(int page = 0, int pageSize = 20)
        {

            Console.WriteLine(_db.TitleBasicss.First().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleDTO
                {
                    Tconst = t.Tconst,
                    TitleType = t.TitleType,
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicTitles;
        }


        public IList<TitleForListDTO> GetListTitles(int page = 0, int pageSize = 1)
        {

            var titles = _db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
                {

                    BasicTitle = new BasicTitleDTO
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType
                        //TitleType = x.TitleType,
                    },
                    //Runtime = x.Runtime,
                    //Rating = x.Rating,
                    //Genres = x.genre,


                }).Skip(page * pageSize).Take(pageSize).ToList();

            //return titles;
            return titles;
        }




        public IList<DetailedTitleDTO>? GetDetailedTitles(int page, int pageSize)
        {



            var titles = _db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleDTO
                {
                    PrimaryTitle = model.First().PrimaryTitle,
                    StartYear = model.First().StartYear,
                    TitleType = model.First().TitleType,
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Plot = model.First().Plot,
                    Poster = model.First().Poster,
                    Tconst = key,
                    //Tconst = obj.Tconst,
                    Genres = model.Select(m => m.Genre).Distinct()
                    .Skip(page * pageSize).Take(pageSize).ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();



            var temp = new List<DetailedTitleDTO>();
            return titles;
        }




        public TitleForListDTO GetListTitle(string tconst)
        {
            return null;
        }

        public DetailedTitleDTO GetDetailedTitle(string tconst)
        {

            return null;
        }




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


        public IList<BasicNameDTO> GetBasicNames(int page = 0, int pageSize = 20)
        {
            var basicnames = _db.NameBasicss
                .Select(n => new BasicNameDTO
                {
                    Nconst = n.Nconst,
                    PrimaryName = n.PrimaryName
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicnames;
        }

        public BasicNameDTO GetBasicName(string nconst)
        {
            var namebasic = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);

            var basicname = new BasicNameDTO()
            {
                Nconst = nconst,
                PrimaryName = namebasic.PrimaryName
            };

            return basicname;
        }



        //public IList<NameForListDTO> GetListNames(int page = 0, int pageSize = 20)
        //{
        //    var names = GetDetailedNames()
        //        .Select(x => new NameForListDTO()
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





        public IList<NameForListDTO> GetListNames(int page = 0, int pageSize = 20)
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
                        // .GroupJoin(_db.TitleBasicss,
                        //basics => basics.KnownForTitle,
                        //knownFor => knownFor.Tconst,
                        //(basics, knownFors) =>
                        //new
                        //{
                        //    Nconst = basics.basics.Nconst,
                        //    PrimaryName = basics.basics.PrimaryName,
                        //    KnownForTitle = knownFors.Select(x => x.Tconst)
                        //})
                        ;
            Console.WriteLine("before names2");

            var names2 = query.ToList()
                .GroupBy(t => t.Nconst, (key, model) => new NameForListDTO
                {
                    BasicName = new BasicNameDTO
                    { 
                    Nconst = key,
                    PrimaryName = model.First().PrimaryName,
                    },

                    KnownForTitleBasics = model.First().KnownForTitle.Any() ? GetBasicTitle(model.First().KnownForTitle.First()) : null
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();

            Console.WriteLine("after names2");
            return names2;




            //    var names = _db.FullViewNames

            //        .ToList()
            //        //.GroupBy(t => t.Tconst,t => t.genre, (key, genre) => new DetailedTitleDTO
            //        .GroupBy(t => t.Nconst, (key, model) => new NameForListDTO
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


        public IList<DetailedNameDTO>? GetDetailedNames(int page = 0, int pageSize = 20)
        {
            var names = _db.FullViewNames

                .ToList()
                .GroupBy(t => t.Nconst, (key, model) => new DetailedNameDTO
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




        /* ----------------------------------------------------------------------------------------------------------
                        USER 
        ---------------------------------------------------------------------------------------------------------- */

        public User GetUser(string username)
        {
            return _db.Users.FirstOrDefault(x => x.Username == username);
        }

        public IList<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public void CreateUser(string username, string password, string email)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password,
                Email = email
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

        }

        public bool DeleteUser(string username)
        {

            var product = GetUser(username);
            if (product == null)
            {
                return false;
            }
            _db.Users.Remove(GetUser(username));
            _db.SaveChanges();
            return true;
        }

        /*
         
         BOOKMARK TITLES
         
         */

        public BookmarkTitle GetBookmarkTitle(string username, string tconst)
        {
            return _db.BookmarkTitles.FirstOrDefault(x => x.Username == username && x.Tconst.Trim() == tconst.Trim());
        }

        public IList<BookmarkTitle> GetBookmarkTitles()
        {
            return _db.BookmarkTitles.ToList();
        }

        public IList<BookmarkTitle> GetBookmarkTitlesByUser(string username)
        {
            return _db.BookmarkTitles
                .Where(x => x.Username == username)
                .ToList();
        }

        public void CreateBookmarkTitle(string username, string tconst, string annotation)
        {
            BookmarkTitle newBookmark = new BookmarkTitle()
            {
                Username = username,
                Tconst = tconst,
            };

            _db.BookmarkTitles.Add(newBookmark);
            _db.SaveChanges();

        }

        public bool DeleteBookmarkTitle(string username, string tconst)
        {

            var product = GetBookmarkTitle(username, tconst);
            if (product == null)
            {
                return false;
            }
            _db.BookmarkTitles.Remove(GetBookmarkTitle(username, tconst));
            _db.SaveChanges();
            return true;
        }



        public bool CreateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            Console.WriteLine(username + " " + tconst + " " + rating);
            var result = db.Database.ExecuteSqlInterpolated($"select * from user_rate({username}, {tconst}, {rating})");
            //Console.WriteLine(result);

            return true; // shouldn't return this - fix

        }

        public SearchResultDTO CreateUserSearch(string username, string searchContent, string searchCategory = null)
        {
            using var db = new ImdbContext();
            var searchResults = db.UserSearches.FromSqlInterpolated($"SELECT * FROM save_string_search({username}, {searchContent}, {searchCategory})");
            
            Console.WriteLine(searchResults.FirstOrDefault().SearchId);
            
            var searchResult = new SearchResultDTO();
            var titles = db.SearchTitleResults.FromSqlInterpolated($"select * from string_search_titles({searchContent})").ToList();
            var names  = db.SearchNameResults.FromSqlInterpolated($"select * from string_search_names({searchContent})").ToList();

            var listTitles = GetTitlesForSearch(titles);

            searchResult.TitleResults = listTitles;


            return searchResult; 

        }


        public IList<TitleForListDTO> GetTitlesForSearch (List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            //var filteredTitles = _db.FullViewTitles.ToList()
            var filteredTitles = db.FullViewTitles.ToList()
                .Join(searchedTitles,  //inner sequence
                    fullView => fullView.Tconst, //outerKeySelector 
                    searchResults => searchResults.Tconst,     //innerKeySelector
                    (fullView, searchResults)
                                  => fullView
                    )
                ;



            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
                {

                    BasicTitle = new BasicTitleDTO
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType,
                    },
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Genres = model.Select(m => m.Genre).Distinct().ToList(),
                    //ParentTitle = GetBasicTitle(model.FirstOrDefault().ParentTconst)
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst) ? null : GetBasicTitle(model.FirstOrDefault().ParentTconst)

                })




                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            var listTitles2 = groupedTitles
                   //.GroupJoin(_db.TitleEpisodes,  //inner sequence
                   .GroupJoin(db.TitleEpisodes,  //inner sequence
                       std => std.BasicTitle.Tconst, //outerKeySelector 
                       s => s.Tconst,     //innerKeySelector
                       (std, s) => 
                       //std
                       new TitleForListDTO
                       {

                           BasicTitle = new BasicTitleDTO
                           {
                               Tconst = std.BasicTitle.Tconst,
                               PrimaryTitle = std.BasicTitle.PrimaryTitle,
                               StartYear = std.BasicTitle.StartYear,
                               TitleType = std.BasicTitle.TitleType,
                               //TitleType = x.TitleType,
                           },
                           Runtime = std.Runtime,
                           Rating = std.Rating,
                           Genres= std.Genres,
                       }
                       );




            //var listTitles3 = listTitles2.ToList().Select(
            //        x => x.ParentTitle = x.
            //    );


            return groupedTitles;
        }




















    }
}