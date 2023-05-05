﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess
{
    public class IdentityAccessRepository : IIdentityAccessRepository
    {
        private readonly ApplicationDbContext _ctx;

        public IdentityAccessRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        //userAccessToken table methods
        public async Task AddUserAccessTokenAsync(UserAccessToken userAccessToken)
        {
            await _ctx.UsersAccessToken.AddAsync(userAccessToken);
        }
        public async Task<UserAccessToken?> GetUserAccessTokenByIdAsync(AccessTokenID id)
        {
            return await _ctx.UsersAccessToken.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateUserAccessToken(UserAccessToken userAccessToken)
        {
            var data = await GetUserAccessTokenByIdAsync(userAccessToken.Id);
            
            if (data is not null)
            {
                data = userAccessToken;
                _ctx.UsersAccessToken.Update(data);
            }
        }
        //userRefreshToken table methods
        public async Task AddUserRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            await _ctx.UsersRefreshToken.AddAsync(userRefreshToken);
        }
        public async Task DeleteRefreshTokenByIdAsync(RefreshTokenID refreshTokenID)
        {
            var userRefreshToken=await _ctx.UsersRefreshToken.FirstOrDefaultAsync(x=>x.Id== refreshTokenID);
            if (userRefreshToken is not null)
            {
                _ctx.UsersRefreshToken.Remove(userRefreshToken);
            }
        }
        public async Task<UserRefreshToken?> GetUserRefreshTokenByIdAsync(RefreshTokenID id)
        {
            return await _ctx.UsersRefreshToken.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateUserRefreshToken(UserRefreshToken userRefreshToken)
        {
            var data=await GetUserRefreshTokenByIdAsync(userRefreshToken.Id);
            if (data is not null)
            {
                data = userRefreshToken;
                _ctx.UsersRefreshToken.Update(data);
            }            
        }
        //userAccess table methods
        public async Task AddUserAccessAsync(UserAccess userAccess)
        {
            await _ctx.UsersAccess.AddAsync(userAccess);
        }
        public async Task<UserAccess?> GetUserAccessByUserId(UserID userID)
        {
            return await _ctx.UsersAccess.FirstOrDefaultAsync(x => x.UserID == userID);
        }
        //userAccessBlacklist methods
        public async Task AddUserAccessBlacklistAsync(UserAccessBlackList userAccessBlackList)
        {
            await _ctx.UsersAccessBlackLists.AddAsync(userAccessBlackList);
        }
        public async Task<UserAccessBlackList?> GetUserAccessBlacklistAsync(RefreshTokenID refreshTokenID)
        {
            return await _ctx.UsersAccessBlackLists.FirstOrDefaultAsync(x => x.RefreshTokenID == refreshTokenID);
        }

    }
}