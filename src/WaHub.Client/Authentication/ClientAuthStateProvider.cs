﻿using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using WaHub.Shared.Models;

namespace WaHub.Client.Authentication;

public class ClientAuthStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> _defaultUnauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private readonly Task<AuthenticationState> _authenticationStateTask = _defaultUnauthenticatedTask;

    public ClientAuthStateProvider(PersistentComponentState state)
    {
        if (!state.TryTakeFromJson<UserSession>(nameof(UserSession), out var userInfo) || userInfo is null)
        {
            return;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.Name, userInfo.FullName),
            new Claim(ClaimTypes.Email, userInfo.Email)
            ];

        _authenticationStateTask = 
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims, authenticationType: nameof(ClientAuthStateProvider)))));

    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => _defaultUnauthenticatedTask;
   
}
