using ARMACommands.Actions;

namespace BundleARMACommandsTests;

[TestFixture]
public class CommitTests
{
    [Test, Ignore("Only works locally")]
    public void PushToNpp()
    {
        Assert.DoesNotThrowAsync(() => Commit.PushToRepo("E:\\GitHub\\npp-sqf"));
    }

    [Test]
    public void PushToNpp_RepoNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => Commit.PushToRepo(""));
    }

    [Test]
    public void PushToNpp_RepoNotExists()
    {
        Assert.ThrowsAsync<DirectoryNotFoundException>(() => Commit.PushToRepo("repo/location"));
    }
}
