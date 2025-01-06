using Markdown.Classes;

namespace MarkdownTest;

public class MarkdownProcessorTest
{
    [Fact]
    public void ConvertToHtml_Should_Handle_Header_1()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#Headline";
        var expectedHtml = "<h1>Headline</h1>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Header_2()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "##Headline";
        var expectedHtml = "<h2>Headline</h2>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Header_3()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "###Headline";
        var expectedHtml = "<h3>Headline</h3>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Header_4()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "####Headline";
        var expectedHtml = "<h4>Headline</h4>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Header_5()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#####Headline";
        var expectedHtml = "<h5>Headline</h5>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Header_6()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#######Headline";
        var expectedHtml = "<h6>#Headline</h6>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Bold_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "__Bold text__";
        var expectedHtml = "<strong>Bold text</strong>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Italic_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "_Italic text_";
        var expectedHtml = "<em>Italic text</em>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }
}