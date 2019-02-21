# Sample Application using stonehenge & Chromely libraries

## Chromely
Chromely addresses the problem of creating platform independent user interfaces. 

It is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps based on embedded Chromium (CEF) without WinForms or WPF.

For more information see: https://github.com/chromelyapps/Chromely

## stonehenge
stonehenge addresses the problem of creating HTML5 based applications without the need to write JavaScript code. Internally it is using **Vue.js** as client framework.

It handles as a MVVM framework and all code can be written in C#. The complexity using a JavaScript client framework is abstracted, the still necessary code is generated automatically.

For more information see: https://github.com/FrankPfattheicher/stonehenge3


## ![Stonehenge Chromely](icon64.png) This Sample
The goal is bringing together both frameworks to be able to write cross platform applications only using C# and HTML5/CSS, no JavaScript required.

It is also a playground to improve the user experience for application writers using the libraries. There is still a lot of missing documentation.

#### Notes
* This code is work in progress!    
  To see what's going on look at the [ReleaseNotes...](ReleaseNotes.md)
* I will focus on .NET Core to be platform independent
* Running Windows .NET Framework 4.7.2 is also targeted
* Still not everything working as expected
* There will be breaking changes all the time
* Is is provided as a starting point to learn using the libraries
* Please feel free to add comments, issues and improvements

#### Development Environment
Tested using **Rider**, a fast & powerful cross-platform .NET IDE from JetBrains.
It is used first class for this project.

For more information see https://www.jetbrains.com/rider/

On Windows the sample is also tested using **VisualStudio 2017**.

#### Contribution
If you have an idea what should be included or clarified in the sample
please feel free to contact me or open an issue on github.

All questions are welcome !

Regards Frank
