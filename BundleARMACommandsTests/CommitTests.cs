using BundleARMACommands.Actions;

namespace BundleARMACommandsTests;

[TestFixture]
public class CommitTests
{
    [Test]
    public void PushToNpp()
    {
        Assert.DoesNotThrowAsync(() => Commit.PushToRepo("F:\\GitHub\\npp-sqf", CancellationToken.None));
    }

    [Test]
    public void PushToNpp_RepoNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => Commit.PushToRepo("", CancellationToken.None));
    }

    [Test]
    public void PushToNpp_RepoNotExists()
    {
        Assert.ThrowsAsync<DirectoryNotFoundException>(() => Commit.PushToRepo("repo/location", CancellationToken.None));
    }
}
