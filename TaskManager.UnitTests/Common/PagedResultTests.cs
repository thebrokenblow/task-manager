using TaskManager.Application.Common;

namespace TaskManager.UnitTests.Common;

public class PagedResultTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };
        int totalCount = 10;
        int pageNumber = 2;
        int pageSize = 5;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        Assert.Equal(items, result.Items);
        Assert.Equal(totalCount, result.TotalCount);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
    }

    [Theory]
    [InlineData(10, 5, 2)]    // 10 элементов, по 5 на страницу = 2 страницы
    [InlineData(25, 10, 3)]   // 25 элементов, по 10 на страницу = 3 страницы
    [InlineData(0, 10, 0)]    // 0 элементов = 0 страниц
    [InlineData(5, 10, 1)]    // 5 элементов, по 10 на страницу = 1 страница
    [InlineData(10, 3, 4)]    // 10 элементов, по 3 на страницу = 4 страницы (округление вверх)
    public void TotalPages_ShouldCalculateCorrectly(int totalCount, int pageSize, int expectedTotalPages)
    {
        // Arrange
        var items = new List<string>();
        var pagedResult = new PagedResult<string>(items, totalCount, 1, pageSize);

        // Act
        var totalPages = pagedResult.TotalPages;

        // Assert
        Assert.Equal(expectedTotalPages, totalPages);
    }

    [Theory]
    [InlineData(1, false)] // Первая страница - нет предыдущей
    [InlineData(2, true)]  // Вторая страница - есть предыдущая
    [InlineData(3, true)]  // Третья страница - есть предыдущая
    public void HasPreviousPage_ShouldReturnCorrectValue(int pageNumber, bool expectedHasPreviousPage)
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedResult = new PagedResult<string>(items, 10, pageNumber, 5);

        // Act
        var hasPreviousPage = pagedResult.HasPreviousPage;

        // Assert
        Assert.Equal(expectedHasPreviousPage, hasPreviousPage);
    }

    [Theory]
    [InlineData(2, 2, false)] // Последняя страница - нет следующей
    [InlineData(1, 3, true)]  // Не последняя страница - есть следующая
    [InlineData(1, 2, true)]  // Не последняя страница - есть следующая
    public void HasNextPage_ShouldReturnCorrectValue(int pageNumber, int totalPages, bool expectedHasNextPage)
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var totalCount = totalPages * 5; // Для вычисления TotalPages
        var pagedResult = new PagedResult<string>(items, totalCount, pageNumber, 5);

        // Act
        var hasNextPage = pagedResult.HasNextPage;

        // Assert
        Assert.Equal(expectedHasNextPage, hasNextPage);
    }

    [Fact]
    public void HasNextPage_WhenPageNumberLessThanTotalPages_ShouldReturnTrue()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedResult = new PagedResult<string>(items, 15, 1, 5); // TotalPages = 3

        // Act & Assert
        Assert.True(pagedResult.HasNextPage);
    }

    [Fact]
    public void HasNextPage_WhenPageNumberEqualsTotalPages_ShouldReturnFalse()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedResult = new PagedResult<string>(items, 15, 3, 5); // TotalPages = 3

        // Act & Assert
        Assert.False(pagedResult.HasNextPage);
    }

    [Fact]
    public void PagedResult_WithEmptyItems_ShouldWorkCorrectly()
    {
        // Arrange
        var emptyItems = new List<string>();
        int totalCount = 0;
        int pageNumber = 1;
        int pageSize = 10;

        // Act
        var pagedResult = new PagedResult<string>(emptyItems, totalCount, pageNumber, pageSize);

        // Assert
        Assert.Empty(pagedResult.Items);
        Assert.Equal(0, pagedResult.TotalCount);
        Assert.Equal(0, pagedResult.TotalPages);
        Assert.False(pagedResult.HasPreviousPage);
        Assert.False(pagedResult.HasNextPage);
    }

    [Theory]
    [InlineData(0, 10, 1, false)]    // Первая страница, но нет элементов
    [InlineData(1, 10, 1, false)]    // Первая страница с элементами
    [InlineData(2, 10, 1, false)]    // Первая страница с элементами
    [InlineData(2, 10, 2, true)]     // Вторая страница с элементами
    public void HasPreviousPage_EdgeCases(int totalCount, int pageSize, int pageNumber, bool expected)
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedResult = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Act & Assert
        Assert.Equal(expected, pagedResult.HasPreviousPage);
    }
}