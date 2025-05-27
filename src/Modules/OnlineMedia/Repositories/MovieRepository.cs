using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Forms;
using NetDream.Modules.OnlineMedia.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class MovieRepository(MediaContext db, IClientContext client)
    {
        public IPage<MovieEntity> GetList(QueryForm form)
        {
            return db.Movies.Search(form.Keywords, "Title").ToPage(form);
        }

        public IOperationResult<MovieEntity> Get(int id)
        {
            var model = db.Movies.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MovieEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<MovieEntity> Save(MovieForm data)
        {
            var model = data.Id > 0 ? db.Movies.Where(i => i.Id == data.Id)
                 .Single() :
                 new MovieEntity();
            if (model is null)
            {
                return OperationResult.Fail<MovieEntity>("id error");
            }
            model.Title = data.Title;
            db.Movies.Save(model);
            db.SaveChanges();
            if (data.Files?.Length > 0)
            {
                foreach (var item in data.Files)
                {
                    item.MovieId = model.Id;
                    FileSave(item);
                }
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Areas.Where(i => i.Id == id).ExecuteDelete();
        }

        public IPage<MovieSeriesEntity> SeriesList(int movie, QueryForm form)
        {
            return db.MovieSeries.Where(i => i.MovieId == movie)
                .Search(form.Keywords, "title")
                .OrderBy(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<MovieSeriesEntity> SeriesSave(MovieSeriesForm data)
        {
            var model = data.Id > 0 ? db.MovieSeries.Where(i => i.Id == data.Id)
                 .Single() :
                 new MovieSeriesEntity();
            if (model is null)
            {
                return OperationResult.Fail<MovieSeriesEntity>("id error");
            }
            model.Episode = data.Episode;
            model.MovieId = data.MovieId;
            model.Title = data.Title;
            db.MovieSeries.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void SeriesRemove(int id)
        {
            db.MovieSeries.Where(i => i.Id == id).ExecuteDelete();
        }

        public IPage<MovieFileEntity> FileList(QueryForm form, int movie, int series = 0)
        {
            return db.MovieFiles.Where(i => i.MovieId == movie && i.SeriesId == series)
                .OrderBy(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<MovieFileEntity> FileSave(MovieFileForm data)
        {
            var model = data.Id > 0 ? db.MovieFiles.Where(i => i.Id == data.Id)
                 .Single() :
                 new MovieFileEntity();
            if (model is null)
            {
                return OperationResult.Fail<MovieFileEntity>("id error");
            }
            model.FileType = data.FileType;
            model.File = data.File;
            model.MovieId = data.MovieId;
            db.MovieFiles.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void FileRemove(int id)
        {
            db.MovieFiles.Where(i => i.Id == id).ExecuteDelete();
        }

        public MovieScoreEntity[] ScoreList(int movie)
        {
            return db.MovieScores.Where(i => i.MovieId == movie).ToArray();
        }

        public IOperationResult<MovieScoreEntity> ScoreSave(MovieScoreForm data)
        {
            var model = data.Id > 0 ? db.MovieScores.Where(i => i.Id == data.Id)
                 .Single() :
                 new MovieScoreEntity();
            if (model is null)
            {
                return OperationResult.Fail<MovieScoreEntity>("id error");
            }
            model.Name = data.Name;
            model.Score = data.Score;
            model.Url = data.Url;
            model.Amount = data.Amount;
            model.MovieId = data.MovieId;
            db.MovieScores.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void ScoreRemove(int id)
        {
            db.MovieScores.Where(i => i.Id == id).ExecuteDelete();
        }

        public AreaEntity[] AreaList()
        {
            return db.Areas.ToArray();
        }

        public IOperationResult<MovieModel> GetEdit(int id)
        {
            var model = db.Movies.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MovieModel>.Fail("数据有误");
            }
            var res = model.CopyTo<MovieModel>();
            res.Tags = new TVRepository(db, client).Tag().GetTags(id);
            return OperationResult.Ok(res);
        }

        public IPage<MovieListItem> Search(QueryForm form, int category = 0, 
            int area = 0, int age = 0)
        {
            var res = db.Movies
                .Search(form.Keywords, "title")
                .When(category > 0, i => i.CatId == category)
                .When(area > 0, i => i.AreaId == area)
                .When(age > 0, i => i.Age == age.ToString())
                .Select(i => new MovieListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    FilmTitle = i.FilmTitle,
                    Cover = i.Cover,
                    Director = i.Director,
                    Leader = i.Leader,
                    CatId = i.CatId,
                    AreaId  = i.AreaId,
                    Age = i.Age,
                    Language = i.Language,
                    ReleaseDate = i.ReleaseDate,
                    Duration = i.Duration,
                    Description = i.Description,
                    SeriesCount = i.SeriesCount,
                    Status =i.Status,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                }).ToPage(form);
            AreaRepository.Include(db, res.Items);
            CategoryRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<MovieModel> GetFull(int id)
        {
            var model = db.Movies.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MovieModel>.Fail("数据有误");
            }
            var res = model.CopyTo<MovieModel>();
            res.Category = db.Categories.Where(i => i.Id == model.CatId).SingleOrDefault();
            res.Area = db.Areas.Where(i => i.Id == model.AreaId).SingleOrDefault();
            res.Tags = new TVRepository(db, client).Tag().GetTags(id);
            if (model.SeriesCount > 1)
            {
                res.Series = db.MovieSeries.Where(i => i.MovieId == model.Id).ToArray();
            }
            else
            {
                res.Files = db.MovieFiles.Where(i => i.MovieId == id).ToArray();
            }
            return OperationResult.Ok(res);
        }
    }
}
