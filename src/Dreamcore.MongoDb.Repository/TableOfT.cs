using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dreamcore.MongoDb.Repository
{
    public class Table<TEntity> : IQueryable<TEntity>
    {
        private readonly IQueryable<TEntity> collection;

        public Table(IQueryable<TEntity> collection)
        {
            this.collection = collection;
        }

        public Type ElementType => this.collection.ElementType;

        public Expression Expression => this.collection.Expression;

        public IQueryProvider Provider => this.collection.Provider;

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
