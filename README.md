# PDFjsGrey
PDF js solution using Xamarin forms with an issue rendering PDF's as grey/disabled on multiple loads until finally rendering correctly

I've added a screenshot once opening the app and downloading the PDF it opens and is greyed out and looks disabled and is very hard to read
Reloading the same PDF multiple times eventually renders as expected

This seems to be happening frequently on various devices and Android emulators, some devices render correctly immediately others seem to always render grey first

Once the PDF renders correctly it stays rendering correctly regardless of how many loads are repeated

I am currently using;
* Android 9 API 28 emulator with Chrome 66.0.3359.158
* Xamarin Forms 4.1.0.555618
* PDF.js 2.1.266

I have tried the v2.2.228 PDF.js version as well it seemed to render slower on each load and did not solve the issue unfortunately

Chrome remote debugging shows only one line each time for the MVVM Book Preview pdf document, the line doesnt change whether the document is rendered in grey text or normal
PDF a542b7ae69a71147a88b29253c4b493c [1.5 Microsoft® Word 2013 / Microsoft® Word 2013] (PDF.js: 2.1.266)
