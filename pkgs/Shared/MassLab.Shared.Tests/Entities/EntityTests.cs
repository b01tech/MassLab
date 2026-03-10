using MassLab.Shared.Entities;

namespace MassLab.Shared.Tests.Entities;

public class EntityTests
{
    private class TestEntity : Entity
    {
        public TestEntity() : base() { }
        public new void SetUpdatedAt() => base.SetUpdatedAt();
    }

    [Fact]
    public void Constructor_ShouldInitializeIdAndCreatedAt()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.NotEqual(default, entity.CreatedAt);
        Assert.Null(entity.UpdatedAt);
    }

    [Fact]
    public void SetUpdatedAt_ShouldUpdateTimestamp()
    {
        // Arrange
        var entity = new TestEntity();
        var initialUpdatedAt = entity.UpdatedAt;

        // Act
        entity.SetUpdatedAt();

        // Assert
        Assert.Null(initialUpdatedAt);
        Assert.NotNull(entity.UpdatedAt);
        Assert.True(entity.UpdatedAt > entity.CreatedAt);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
    {
        // Arrange
        var entity1 = new TestEntity();
        var entity2 = new TestEntity { Id = entity1.Id };

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenIdsAreDifferent()
    {
        // Arrange
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenObjectIsNotEntity()
    {
        // Arrange
        var entity = new TestEntity();
        var otherObject = new object();

        // Act
        var result = entity.Equals(otherObject);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenObjectIsNull()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        var result = entity.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_ShouldReturnIdHashCode()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        var hashCode = entity.GetHashCode();

        // Assert
        Assert.Equal(entity.Id.GetHashCode(), hashCode);
    }
}
