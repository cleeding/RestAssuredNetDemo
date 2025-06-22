using Xunit;
using static RestAssured.Dsl;
using FluentAssertions;
using System.Net;
using System.Text.Json;

public class JsonPlaceholderTests
{
    private const string BaseUrl = "https://jsonplaceholder.typicode.com";

    [Fact]
    public void GetPosts_ShouldReturn200()
    {
        Given()
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(200);
    }

    [Fact]
    public void GetPosts_ShouldReturn200_WithHttpStatusCode()
    {
        Given()
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public void GetPosts_ShouldContainJsonContentTypeHeader()
    {
        Given()
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(200)
            .Header("content-type", "application/json; charset=utf-8");
    }

    [Fact]
    public void GetNonexistentPost_ShouldReturn404()
    {
        Given()
            .When()
            .Get($"{BaseUrl}/posts/0")
            .Then()
            .StatusCode(404);
    }

    [Fact]
    public void CreatePost_ShouldReturnNewId()
    {
        var createResponse = Given()
            .ContentType("application/json")
            .Body(new { title = "test", body = "body", userId = 42 })
            .When()
            .Post($"{BaseUrl}/posts");

        createResponse
            .Then()
            .StatusCode(201);

        var bodyJson = createResponse
            .Extract()
            .BodyAsString();

        using var doc = JsonDocument.Parse(bodyJson);
        int newId = doc.RootElement.GetProperty("id").GetInt32();

        newId.Should().BeGreaterThanOrEqualTo(101);
    }

    [Fact]
    public void GetPosts_ShouldContainPostWithUserId1()
    {
        var response = Given()
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(HttpStatusCode.OK)
            .Extract()
            .BodyAsString();

        response.Should().Contain("\"userId\": 10");
    }

    [Fact]
    public void GetPosts_ShouldReturn100Posts()
    {
        var posts = Given()
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(HttpStatusCode.OK)
            .Extract()
            .BodyAsString();

        using var jsonDoc = JsonDocument.Parse(posts);
        var root = jsonDoc.RootElement;

        root.GetArrayLength().Should().Be(100, because: "the JSONPlaceholder API returns exactly 100 posts");
    }

    [Fact]
    public void UpdatePost_ShouldReturnUpdatedPost()
    {
        var updateBody = new
        {
            id = 1,
            title = "updated title",
            body = "updated body",
            userId = 1
        };

        var jsonBody = JsonSerializer.Serialize(updateBody);

        var response = Given()
            .ContentType("application/json")
            .Body(jsonBody)
            .When()
            .Put($"{BaseUrl}/posts/1")
            .Then()
            .StatusCode(HttpStatusCode.OK)
            .Extract()
            .BodyAsString();

        using var jsonDoc = JsonDocument.Parse(response);
        var root = jsonDoc.RootElement;

        root.GetProperty("title").GetString().Should().Be("updated title");
        root.GetProperty("body").GetString().Should().Be("updated body");
    }

    [Fact]
    public void DeletePost_ShouldReturnStatus200()
    {
        Given()
            .When()
            .Delete($"{BaseUrl}/posts/1")
            .Then()
            .StatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public void GetPostByUserIdAndId_ShouldReturnSinglePost()
    {
        var responseBody = Given()
            .QueryParam("userId", "10")
            .QueryParam("id", "95")
            .When()
            .Get($"{BaseUrl}/posts")
            .Then()
            .StatusCode(HttpStatusCode.OK)
            .Extract()
            .BodyAsString();

        responseBody.Should().Contain("\"id\": 95");
        responseBody.Should().Contain("\"userId\": 10");
    }
}
