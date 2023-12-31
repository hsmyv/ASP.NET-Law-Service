﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;

namespace Aspİntro.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPosts(int take);
    }
}
