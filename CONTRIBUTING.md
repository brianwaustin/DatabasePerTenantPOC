# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue,
email, or any other method with the owners of this repository before making a change.

All changes to this codebase should be done via [git feature branching](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow) and submitted for merge to master via [a pull request](https://www.atlassian.com/git/tutorials/making-a-pull-request).

## Pull Request Process

1. Ensure any install or build dependencies are removed before the end of the layer when doing a build.
2. Update the README.md with details of changes to the interface, this includes new environment variables, exposed ports, useful file locations and container parameters.
3. Increase the version numbers in any examples files and the README.md to the new version that this
   Pull Request would represent. The versioning scheme we use is [SemVer](http://semver.org/).
4. You may merge the Pull Request in once you have the sign-off of two other developers, or if you do not have permission to do that, you may request the second reviewer to merge it for you.

## Architectural Patterns

### Our Pledge

We are uncovering better ways of developing software by doing it and helping others do it.

Through this work we have come to value:

* **Individuals and interactions** over processes and tools
* **Working software** over comprehensive documentation
* **Customer collaboration** over contract negotiation
* **Responding to change** over following a plan

That is, while there is value in the items on the right, we value the items on the left more. - [Manifesto for Agile Software Development](http://agilemanifesto.org/)

### Our Standards

We strive to build systems that are:

1. **Independent of Frameworks**. The architecture does not depend on the existence of some library of feature laden software. This allows you to use such frameworks as tools, rather than having to cram your system into their limited constraints.

2. **Testable**. The business rules can be tested without the UI, Database, Web Server, or any other external element.

3. **Independent of UI**. The UI can change easily, without changing the rest of the system. A Web UI could be replaced with a console UI, for example, without changing the business rules.

4. **Independent of Database**. You can swap out Oracle or SQL Server, for Mongo, BigTable, CouchDB, or something else. Your business rules are not bound to the database.

5. **Independent of any external agency**. In fact your business rules simply don’t know anything at all about the outside world.

### Clean Architecture

![alt text](https://8thlight.com/blog/assets/posts/2012-08-13-the-clean-architecture/CleanArchitecture-8d1fe066e8f7fa9c7d8e84c1a6b0e2b74b2c670ff8052828f4a7e73fcbbc698c.jpg "Clean Architecture Diagram")

### Scope

TBD

### Enforcement

TBD

### Sample Project

See Steve Smith's sample project: https://github.com/ardalis/CleanArchitecture

### Attribution & References

* [The Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html) by Uncle Bob
* [Better Software Design with Clean Architecture](https://fullstackmark.com/post/11/better-software-design-with-clean-architecture) by Mark Macneil
* [Dependency Injection (DI) vs. Inversion of Control (IOC)](https://www.codeproject.com/Articles/592372/Dependency-Injection-DI-vs-Inversion-of-Control-IO) by Shivprasad koirala
