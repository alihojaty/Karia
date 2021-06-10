﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Karia.Api.Helpers
{
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }


        public PagedList(List<T> items,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages =(int) Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count =await source.CountAsync();
            var items =await source.Skip((pageNumber - 1)*pageSize)
                .Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}