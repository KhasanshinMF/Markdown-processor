using Markdown.Classes;

namespace MarkdownTest;

public class MarkdownProcessorTest
{
    [Fact]
    public void ConvertToHtml_Should_Handle_Headers()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#���������";
        var expectedHtml = "<h1>���������</h1>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Bold_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "**������ �����**";
        var expectedHtml = "<strong>������ �����</strong>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Italic_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "*��������� �����*";
        var expectedHtml = "<em>��������� �����</em>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Line_Breaks()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "������ 1\n ������ 2";
        var expectedHtml = "������ 1<br> ������ 2";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }
}