using BundleARMACommands;

namespace BundleARMACommandsTests;

[TestFixture]
public class CommitTests
{
    [Test]
    public void PushToNppRepo()
    {
        Assert.DoesNotThrowAsync(() => Commit.PushToNppRepo("repo/location"));
    }

    [Test]
    public void PushToNppRepo_RepoNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => Commit.PushToNppRepo(""));
    }

    [Test]
    public void PushToNppRepo_DriveLetterNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => Commit.PushToNppRepo("repo/location", ""));
    }
}
