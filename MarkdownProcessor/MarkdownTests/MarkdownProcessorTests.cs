using Markdown.MarkdownProcessor;

namespace MarkdownTests;

[TestFixture]
public class MarkdownProcessorTests
{
    private MarkdownProcessor _markdownProcessor;
    
    [SetUp]
    public void Setup()
    {
        _markdownProcessor = new MarkdownProcessor();
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Header_1()
    {
        var markdownText = "#Headline";
        var expectedHtml = "<h1>Headline</h1>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
    
    [Test]
    public void ConvertToHtml_Should_Handle_Header_2()
    {
        var markdownText = "##Headline";
        var expectedHtml = "<h2>Headline</h2>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Header_3()
    {
        var markdownText = "###Headline";
        var expectedHtml = "<h3>Headline</h3>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Header_4()
    {
        var markdownText = "####Headline";
        var expectedHtml = "<h4>Headline</h4>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Header_5()
    {

        var markdownText = "#####Headline";
        var expectedHtml = "<h5>Headline</h5>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Header_6()
    {
        var markdownText = "#######Headline";
        var expectedHtml = "<h6>#Headline</h6>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Bold_Text()
    {
        var markdownText = "__Bold text__";
        var expectedHtml = "<strong>Bold text</strong>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
    
    [Test]
    public void ConvertToHtml_Should_Handle_Incomplete_Bold_Text()
    {
        var markdownText = "incomplete __bold text";
        var expectedHtml = "incomplete __bold text";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Italic_Text()
    {
        var markdownText = "_Italic text_";
        var expectedHtml = "<em>Italic text</em>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }

    [Test]
    public void ConvertToHtml_Should_Handle_Incomplete_Italic_Text()
    {
        var markdownText = "incomplete _italic text";
        var expectedHtml = "incomplete _italic text";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
    
    [Test]
    public void ConvertToHtml_Should_Handle_Italic_And_Bold_Text()
    {
        var markdownText = "_italic_ and __bold__ text";
        var expectedHtml = "<em>italic</em> and <strong>bold</strong> text";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
        
    [Test]
    public void ConvertToHtml_Should_Handle_Italic_In_Bold_Text()
    {
        var markdownText = "__bold with _italic_ and _italic_ text__";
        var expectedHtml = "<strong>bold with <em>italic</em> and <em>italic</em> text</strong>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
            
    [Test]
    public void ConvertToHtml_Should_Handle_Bold_In_Italic_Text()
    {
        var markdownText = "_italic with __bold__ and __bold__ text_";
        var expectedHtml = "<em>italic with __bold__ and __bold__ text</em>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
                
    [Test]
    public void ConvertToHtml_Should_Handle_Escaped_Header()
    {
        var markdownText = "\\## Check \\escap\\ing";
        var expectedHtml = "## Check \\escap\\ing";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
                    
    [Test]
    public void ConvertToHtml_Should_Handle_Escaped_Italic_And_Bold()
    {
        var markdownText = "\\_italic text\\_, and \\__bold text\\__";
        var expectedHtml = "_italic text_, and __bold text__";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
                        
    [Test]
    public void ConvertToHtml_Should_Handle_Escaped_Backslash()
    {
        var markdownText = "\\\\_italic text\\\\_ and \\\\__bold text\\\\__";
        var expectedHtml = "\\<em>italic text\\</em> and \\<strong>bold text\\</strong>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
                            
    [Test]
    public void ConvertToHtml_Should_Handle_Header_With_Italic_And_Bold()
    {
        var markdownText = "# heading with _italics_ and __bold__";
        var expectedHtml = "<h1>heading with <em>italics</em> and <strong>bold</strong></h1>";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
                                
    [Test]
    public void ConvertToHtml_Should_Handle_Four_Underscores()
    {
        var markdownText = "____";
        var expectedHtml = "____";
        var actualHtml = _markdownProcessor.ConvertToHtml(markdownText);

        Assert.That(actualHtml, Is.EqualTo(expectedHtml));
    }
    
}