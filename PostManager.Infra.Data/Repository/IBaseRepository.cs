﻿using PostManager.Domain.Model;
using System.Collections.Generic;

namespace PostManager.Infra.Data.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(int id);

        IList<TEntity> Select();

        TEntity Select(int id);
    }
}
