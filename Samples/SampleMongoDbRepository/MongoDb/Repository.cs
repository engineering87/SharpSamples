﻿// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleMongoDbRepository.MongoDb
{
    public class Repository<T> : IRepository<T, string> where T : MongoDbEntity, new()
    {
        protected readonly IMongoCollection<T> Collection;

        public Repository(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.Database);
            this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public virtual IMongoCollection<T> GetCollection()
        {
            return Collection;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            return await Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            return await Collection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await Collection.FindOneAndDeleteAsync(filter);
        }
    }
}
