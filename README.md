# PDFjsGrey
PDF js solution using Xamarin forms with an issue rendering PDF's as grey/disabled on multiple loads until finally rendering correctly

I've added a screenshot once opening the app and downloading the PDF it opens and is greyed out and looks disabled and is very hard to read
Reloading the same PDF multiple times eventually renders as expected

This seems to be happening frequently on various devices and Android emulators, some devices render correctly immediately others seem to always render grey first

I am currently using;
* Android 9 API 28 emulator
* Xamarin Forms 4.1.0.555618
* PDF.js 2.1.266

I have tried the v2.2.228 PDF.js version as well it seemed to render slower on each load and did not solve the issue unfortunately
