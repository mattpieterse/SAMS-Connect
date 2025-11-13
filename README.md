# Evidence Portfolio

> An application to bridge the gap between Municipal Services and their Communities.

## Getting started

**GitHub:** https://github.com/mattpieterse/IIEVC-SAMSConnect

Watch the [video walkthrough](https://youtu.be/hLbq6MNjTWg) to see the application in action and
learn how to use the issue reporting feature (5min). For markers discretion, an
optional [technical demonstration](https://youtu.be/lBYYz2pvQjU) and walkthrough of the codebase is
available (10min). This demonstration is very brief, but covers all core aspects of the system if
needed.

### Compiling the source code

See
the [official documentation](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository)
for more information.

If you do not have Git installed and set up on your local machine, you can download a ZIP archive of
the repository under the <kbd><> Code</kbd> dropdown menu. To compile the source code, run Visual
Studio 2022 (or later versions) and open the uncompressed folder, then compile the project from the
IDE. To do so, click the green run icon at the top of the screen.

#### Clone using a Command Line Interface (CLI)

1. Create/open an empty folder on your local machine to store the project files.
2. Open a terminal window and navigate to the directory of the folder.
  - **Windows:** To do this in an efficient manner, enter the folder in the default file explorer
    and click on the address bar. Then, type <kbd>C</kbd> <kbd>M</kbd> <kbd>D</kbd> followed
    by <kbd>Enter</kbd> to directly initialise a Windows terminal at the directory of your folder.
    This works with PowerShell in the same way. Watch a
    short [video guide](https://youtu.be/N7IqS3PX3YA?t=80) here.

3. Clone this repository using the following command:

``` bash
git clone https://github.com/mattpieterse/IIEVC-SAMSConnect
```

### Creating your first report

- From the homescreen menu, select <kbd>Create an Issue Report</kbd>.
- Fill in all of the required fields marked by an asterisk (*), ensuring that all validation rules
  are satisfied.
- Attach any relevant files in the interactive builder.
- Submit your claim.

## Changelog

### Submission 2

> [!WARNING]
> From this version of the application onwards, it is advisable to run the application on a Windows
> 11 device. Icons are rendered using a pre-packaged font that may behave unexpectedly on earlier
> Windows versions.

#### Using the application

If you navigate to Local Events & Announcements, the second sidebar tab, you'll see all of the posts
send out by the South African Municipal Services in your area. A building icon shows that it is an
event, a message icon shows that is an announcement, and you'll find all of the relevant information
adjacent to this.

We have provided helpful search and filtering methods on the side of this list. You can use all of
these filters at the same time, too! This helps you focus on just what you need to hear. Use the
dropdown menu to filter to updates from one department, and use the clear button to view all again.
The date sliders, both inclusive, provide a powerful way to hone in on something you're looking for.
The top one is the earliest day (inclusive), and the bottom one is the latest day (inclusive). Play
with all of the options to build powerful custom feeds.

As soon as you use the filters, we use your usage patterns to understand what you want to see. In
the main list, you'll notice a star appear next to updates from a specific department. This is your
new recommendation helper! We learn which department you look for the most, and help you spot them
in the crowd for your convenience.

Don't forget that you can use the sidebar to navigate around at any time!

#### Screens

**HomeScreen**

![Screen](.github/assets/P2-HOME.png)

**TicketUpsertScreen (previous)**

![Screen](.github/assets/P2-TICKETS.png)

**EventScreen**

![Screen](.github/assets/P2-EVENTS.png)

## Contribution

This repository is private and has been released into the public domain with the intention of code
submission. All issues and pull requests will be ignored, do not attempt to modify and/or contribute
to this repository in any way as it is the work of an individual.