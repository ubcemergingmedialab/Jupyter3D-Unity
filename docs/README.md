{# Generating Documentation

This folder contains a sample doxyfile to autogenerate documentation from the comments on files in the scripts folder of the project.

To see a sample of this form of documentation, see [this](https://unoctium1.github.io/RubberHandVR/) or [this](https://ubcemergingmedialab.github.io/ARDesign/namespaces.html)

## Setup

In order to use this, both [doxygen](http://www.doxygen.nl/) and [Graphviz](https://graphviz.gitlab.io/download/) must be installed on your computer. To use the doxyfile, open the doxygen wizard and load the doxyfile. Fill out the project name, description, and any other template values you wish to change. Then, just click run, and you should be left with an output html folder where you can preview your documentation.

Note: graphviz is optional. It is required if you wish to have doxygen autogenerate UML diagrams. 

Note: Doxygen expects a file called emllogo.jpg. Some logo file should be added to this folder before running doxygen.


## Documentation

Doxygen generates your documentation from [XML comments](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/) in your classes. In general, it's good practice to use XML comments anyway, as VisualStudio parses these for it's tooltips.
To create an XML comment for a method, class, struct, property, etc, just right click on the element in question in Visual Studio, select "snippets" and select "insert comment". Try to keep comments as informative and descriptive as possible. 

## Github Pages

The last step once you have your documentation is to deploy it to a github pages branch. This can be done either before or after generating the documentation. 

First, we need to create a bare branch. To do this, clone your workspace in a new location. From your master branch, in the terminal run the following commands:

```
git checkout --orphan gh-pages
git rm -rf .
echo "My gh-pages branch" > README.md
git add .
git commit -a -m "Clean gh-pages branch"
git push origin gh-pages
```

You can delete this copy of your workspace if you like. Now, in your main workspace, go into this docs folder and run `git clone --single-branch --branch gh-pages <git url>`.
You should end up with a folder containing an empty readme named after your project. Rename this folder to html and (if it exists) merge it with your documentation output. Then, go into your documentation, make sure you're on the gh-pages branch, and run `git add .` Follow the normal steps to commit and push the repository. The branch should be autodeployed to the github pages for the repo.}
