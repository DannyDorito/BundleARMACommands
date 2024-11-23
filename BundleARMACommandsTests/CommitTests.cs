using BundleARMACommands.Actions;

namespace BundleARMACommandsTests;

[TestFixture]
public class CommitTests
{
    [Test, Ignore("Only works locally")]
    public void PushToNpp()
    {
        Assert.DoesNotThrowAsync(() => Commit.PushToRepo("E:\\GitHub\\npp-sqf", CancellationToken.None));
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
