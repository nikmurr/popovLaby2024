# Winnovative Word to PDF Library for .NET

[![Winnovative Logo Image](https://raw.githubusercontent.com/Winnovative/winnovative-files/main/readme/winnovative-logo-banner.jpg)](http://www.winnovative-software.com)

[Word to PDF for .NET](http://www.winnovative-software.com/word-to-pdf-converter.aspx) | [C# PDF Library for .NET](http://www.winnovative-software.com) | [Free Trial](http://www.winnovative-software.com/download.aspx) | [Licensing](http://www.winnovative-software.com/buy.aspx) | [Support](http://www.winnovative-software.com/contact.aspx)

**Winnovative Word to PDF Library for .NET** can be easily integrated in your applications targeting the .NET Framework to create PDF documents from Word documents.
The library can also be used to create, edit and merge PDF documents.

This version of the library is compatible with .NET Framework on Windows platforms.

For .NET Core and .NET Standard applications on Windows you can use the library from [Winnovative.WordToPdf.NetCore](https://www.nuget.org/packages/Winnovative.WordToPdf.NetCore/) NuGet package.

In any .NET application for Linux, macOS, Windows, Azure App Service, Xamarin, UWP and other platforms you can use the cross-platform library from [Winnovative.Client](https://www.nuget.org/packages/Winnovative.Client/) NuGet package.

## Main Features

* Convert Word DOC and DOCX, RTF documents to PDF
* Does not require Microsoft Word or other third party tools
* Convert to memory buffer, file, stream or to a PDF object for further processing
* Convert all the pages or select the pages in document to convert
* Add headers and footers with page numbering to PDF pages
* Append or prepend external PDF files to conversion result
* Password protect and set permissions of the PDF document
* Add a digital signature to generated PDF document
* Add graphic elements to generated PDF document
* Generate PDF/A and PDF/X compliant documents
* Generate CMYK and Gray Scale PDF documents
* Edit existing PDF documents
* Merge multiple PDF documents in a single PDF document
* Split a PDF document in multiple PDF documents

## Compatibility

Winnovative Word to PDF Library for .NET is compatible with Windows platforms which support .NET Framework 4.0 and above, including:

* .NET Framework 4.8.1, 4.7.2, 4.6.1, 4.0 (and above)
* Windows 32-bit (x86) and 64-bit (x64)
* Azure Cloud Services and Azure Virtual Machines
* Web, Console and Desktop applications

## Getting Started

After the reference to library was added to your project you are now ready to start writing code to convert Word to PDF in your .NET application.
You can copy the C# code lines from the section below to create a PDF document from a Word document and save the resulted PDF to a memory buffer for further processing, to a PDF file or send it to browser for download in ASP.NET applications.

### C# Code Samples

At the top of your C# source file add the ```using WnvWordToPdf;``` statement to make available the Winnovative Word to PDF API for your .NET application.

```csharp
// add this using statement at the top of your C# file
using WnvWordToPdf;
```

To convert a Word file to a PDF file you can use the C# code below.

```csharp
// create the converter object in your code where you want to run conversion
WordToPdfConverter converter = new WordToPdfConverter();

// convert the Word file to a PDF file
converter.ConvertWordFileToFile("my_word_file_path", "WordToFile.pdf");
```

To convert a Word file to a PDF document in a memory buffer and then save it to a file you can use the C# code below.

```csharp
// create the converter object in your code where you want to run conversion
WordToPdfConverter converter = new WordToPdfConverter();

// convert a Word file to a memory buffer
byte[] wordToPdfBuffer = converter.ConvertWordFile("my_word_file_path");

// write the memory buffer to a PDF file
System.IO.File.WriteAllBytes("WordToMemory.pdf", wordToPdfBuffer);
```

To convert in your ASP.NET MVC applications a Word file to a PDF document in a memory buffer and then send it for download to browser you can use the C# code below.

```csharp
// create the converter object in your code where you want to run conversion
WordToPdfConverter converter = new WordToPdfConverter();

// convert a Word file to a memory buffer
byte[] wordToPdfBuffer = converter.ConvertWordFile("my_word_file_path");

FileResult fileResult = new FileContentResult(wordToPdfBuffer, "application/pdf");
fileResult.FileDownloadName = "WordToPdf.pdf";
return fileResult;
```

To convert in your ASP.NET Web Forms application a Word file to a PDF document in a memory buffer and then send it for download to browser you can use the C# code below.

```csharp
// create the converter object in your code where you want to run conversion
WordToPdfConverter converter = new WordToPdfConverter();

// convert a Word file to a memory buffer
byte[] wordToPdfBuffer = converter.ConvertWordFile("my_word_file_path");

HttpResponse httpResponse = HttpContext.Current.Response;
httpResponse.AddHeader("Content-Type", "application/pdf");
httpResponse.AddHeader("Content-Disposition",
    String.Format("attachment; filename=WordToPdf.pdf; size={0}",
    wordToPdfBuffer.Length.ToString()));
httpResponse.BinaryWrite(wordToPdfBuffer);
httpResponse.End();
```

## Free Trial

You can download the full Winnovative Word to PDF Converter for .NET Framework package from [Winnovative Software Downloads](http://www.winnovative-software.com/download.aspx) page of the website.

The package for .NET Framework contains the product binaries, a demo Visual Studio project with full C# code for Windows Forms targeting .NET Framework 4 and later versions, the library documentation in CHM format.

You can evaluate the library for free as long as it is needed to ensure that the solution fits your application needs.

## Licensing

The Winnovative Software licenses are perpetual which means they never expire for a version of the product and include free maintenance for the first year. You can find [more details about licensing](http://www.winnovative-software.com/buy.aspx) on website.

## Support

For technical and sales questions or for general inquiries about our software and company you can contact us using the email addresses from the [contact page](http://www.winnovative-software.com/contact.aspx) of the website. 
