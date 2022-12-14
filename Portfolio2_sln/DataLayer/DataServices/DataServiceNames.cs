using DataLayer.DataTransferObjects;
using DataLayer.DomainModels.NameModels;
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

        public int GetNumberOfPeople()
        {
            return _db.NameBasicss.Count();
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
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return basicnames;
        }

        public BasicNameDTO GetBasicName(string nconst)
        {
            if (string.IsNullOrEmpty(nconst))
                return new BasicNameDTO();

            var namebasic = _db.NameBasicss.FirstOrDefault(x => x.Nconst.Trim() == nconst.Trim());
            Console.WriteLine(namebasic.Nconst);
            var basicname = new BasicNameDTO
            {
                Nconst = nconst,
                PrimaryName = namebasic.PrimaryName
            };

            return basicname;
        }

        public NameForListDTO GetListName(string nconst)
        {
            // Joins the filtered name_basics with known_for to get list form of matching names

            using var db = new ImdbContext();

            var names = db.NameBasicss.ToList()
                .Where(n => n.Nconst.Equals(nconst))
                .GroupJoin(db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) => new NameForListDTO
                       {
                           BasicName =
                           new BasicNameDTO
                           {
                               Nconst = basics.Nconst,
                               PrimaryName = basics.PrimaryName,
                           },
                           KnownForTitleBasics = knownFor.Any() ?
                                    new DataServiceTitles().
                                    GetBasicTitle(knownFor.FirstOrDefault().Tconst)
                                    : null
                       }
                    )
                .FirstOrDefault();


            return names;
        }

        public IList<NameForListDTO> GetListNames(int page = 0, int pageSize = 20)
        {
            // Joins the filtered name_basics with known_for to get list form of matching names

            using var db = new ImdbContext();

            var names = db.NameBasicss.ToList()
                .Skip((page - 1) * pageSize).Take(pageSize)
                .GroupJoin(db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) => new NameForListDTO
                       {
                           BasicName =
                           new BasicNameDTO
                           {
                               Nconst = basics.Nconst,
                               PrimaryName = basics.PrimaryName,
                           },
                           KnownForTitleBasics = knownFor.Any() ?
                                    new DataServiceTitles().
                                    GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                       }
                    )
                .ToList();


            return names;
        }


        //----------------------------------------------------------------------------------------------
        //         NAME HELPERS
        //----------------------------------------------------------------------------------------------


        public IList<NameForListDTO> GetFilteredNames(List<NconstObject> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();



            // Filters names so only contain those that matched the search
            var filtered = db.NameBasicss.ToList()
                .Join(searchedNames,
                    fullView => fullView.Nconst,
                    searchResults => searchResults.Nconst,
                    (fullView, searchResults)
                                  => fullView
                    );




            // Joins the filtered name_basics with known_for to get list form of matching names
            var searchedTitleResults =
                filtered.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) => new NameForListDTO
                       {
                           BasicName =
                               new DataServiceNames().
                                    GetBasicName(basics.Nconst),
                           KnownForTitleBasics = knownFor.Any() ?
                                    new DataServiceTitles().
                                    GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                       }
                    )
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();

            return searchedTitleResults;
        }


        public (int, IList<TitleCrewDTO>) GetRelatedTitles(string nconst, int page, int pageSize)
        {
            using var db = new ImdbContext();

            var cast1 = db.TitlePrincipals
                .Where(c => c.Nconst.Equals(nconst))
                .Join(db.NameBasicss,
                     crew => crew.Nconst,
                     nameBasics => nameBasics.Nconst,
                     (crew, nameBasics)
                     => new
                     {
                         Tconst = crew.Tconst,
                         Nconst = nameBasics.Nconst,
                         PrimaryName = nameBasics.PrimaryName,
                         Category = crew.Category
                     }
             );

            var cast = cast1
                .GroupJoin(db.Jobs,
                     crew => new { crew.Nconst, crew.Tconst },
                     job => new { job.Nconst, job.Tconst },
                     (crew, job)
                     => new { Outer = crew, Inner = job })
                .SelectMany(
                x => x.Inner.DefaultIfEmpty(),
                (crew, job)
                             => new
                             {
                                 Tconst = crew.Outer.Tconst,
                                 PrimaryName = crew.Outer.PrimaryName,
                                 Category = crew.Outer.Category,
                                 JobName = job.JobName,
                                 Nconst = nconst,
                             }
             )
            //    .ToList();

            //var cast2 = cast
                .GroupJoin(db.Characters,
                     crew => new { crew.Nconst, crew.Tconst },
                     chars => new { chars.Nconst, chars.Tconst },
                     (crew, chars)
                     => new { Outer = crew, Inner = chars })
                .SelectMany(
                x => x.Inner.DefaultIfEmpty(),
                (crew, chars)
                             => new
                             TitleCrewDTO
                             {
                                 Tconst = crew.Outer.Tconst,
                                 //Tconst = test(crew.Outer.Tconst),
                                 //Tconst = "jj",
                                 //BasicTitle = new BasicTitleDTO(),
                                 BasicTitle = new DataServiceTitles().GetBasicTitle(crew.Outer.Tconst),
                                 Nconst = nconst,
                                 PrimaryName = crew.Outer.PrimaryName,
                                 Category = crew.Outer.Category,
                                 JobName = crew.Outer.JobName,
                                 CharacterName = chars.CharacterName
                             }

             )
            ;
            Console.WriteLine("-----------------------------------------");


            var totalItems = cast.Count();
            Console.WriteLine("data layer total " + cast.Count());
            var result = cast
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return (cast.Count(), result);
            //return (cast.Count(), null);
        }

        private string? test(string? tconst)
        {
            Console.WriteLine("--------------------------------------------------------");
            return "sdd";
            return tconst;
        }

        /*
         
        public IList<NameTitleRelationDTO> GetNameTitleRelations(string nconst)
        {
            using var db = new ImdbContext();
            var jobsJoin = db.Jobs
                 .Where(n => n.Nconst.Trim().Contains(nconst.Trim()))
                 .Join(db.TitleBasicss,
                     job => job.Tconst,
                     title => title.Tconst,
                     (job, title) => new NameTitleRelationDTO
                     {
                         Nconst = nconst,
                         Relation = job.JobName,
                         Title = new BasicTitleDTO
                         {
                             Tconst = title.Tconst,
                             TitleType = title.TitleType,
                             PrimaryTitle = title.PrimaryTitle,
                             StartYear = title.StartYear,
                         }
                        }
                     )
                 .ToList();


            var characterJoin = db.Characters 
                 .Where(n => n.Nconst.Trim() == nconst.Trim())
              .Join(db.TitleBasicss,
                  character => character.Tconst,
                  title => title.Tconst,
                  (character, title) => new NameTitleRelationDTO
                  {
                      Nconst = nconst,
                      Relation = character.CharacterName,
                      Title = new BasicTitleDTO
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


         */







        //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------



        /*


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
                        ;

            var names2 = query.ToList()
                .GroupBy(t => t.Nconst, (key, model) => new NameForListDTO
                {
                    BasicName = new BasicNameDTO
                    {
                        Nconst = key,
                        PrimaryName = model.First().PrimaryName,
                    },

                    KnownForTitleBasics = model.First().KnownForTitle.Any() ? new DataServiceTitles().GetBasicTitle(model.First().KnownForTitle.First()) : null
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();

            return names2;

        }

         
         */


        public DetailedNameDTO GetDetailedName(string nconst)
        {
            var names = _db.FullViewNames
                .Where(n => n.Nconst.Trim() == nconst.Trim())
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
                ).FirstOrDefault();

            return names;
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
                ).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return names;
        }






















    }
}
