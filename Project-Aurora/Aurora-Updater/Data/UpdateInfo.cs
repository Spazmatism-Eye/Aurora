﻿using System.Linq;
using System.Threading.Tasks;
using Octokit;
using SemanticVersioning;

namespace Aurora_Updater.Data;

public class UpdateInfo(Version currentVersion, string author, string repoName, bool getPreReleases)
{
    private readonly GitHubClient _gClient = new(new ProductHeaderValue("aurora-updater", currentVersion.ToString()));
    private readonly Version _currentVersion = new(199, 0, 0);

    public async Task<Release> FetchData()
    {
        if (!getPreReleases && string.IsNullOrWhiteSpace(_currentVersion.PreRelease) && !await IsCurrentlyPreRelease())
        {
            return await _gClient.Repository.Release.GetLatest(author, repoName);
        }

        var releases = await _gClient.Repository.Release.GetAll(author, repoName, new ApiOptions { PageCount = 1, PageSize = 1 });
        return releases.OrderByDescending(r => r.PublishedAt).First();
    }

    private async Task<bool> IsCurrentlyPreRelease()
    {
        var release = await _gClient.Repository.Release.Get(author, repoName, $"v{_currentVersion.Major}");
        return release.Prerelease;
    }
}